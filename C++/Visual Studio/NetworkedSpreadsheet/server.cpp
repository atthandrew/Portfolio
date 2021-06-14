#include <stdio.h>
#include <stdlib.h>
#include <sys/socket.h> 
#include <arpa/inet.h>
#include <unistd.h>
#include <netdb.h>
#include <netinet/in.h>
#include <string.h>
#include <pthread.h>
#include <iostream>
#include <vector>
#include <dirent.h>
#include <algorithm>
#include <fstream>
#include <map>
#include "include/document.h"
#include "include/rapidxml.hpp"
#include "include/rapidxml_utils.hpp"
#include "include/rapidxml_print.hpp"
#include <sstream>
#include "cellmanipulator.cpp"
#include "dependency_graph.h"


std::map<int,std::string> currentUsers;
std::map<int,std::string> userSpreadsheet;
std::map<std::string,cellmanipulator> changes;
std::map<std::string,dependency_graph> spreadsheet_graphs;
std::string replyForServer;
pthread_mutex_t serverLock = PTHREAD_MUTEX_INITIALIZER;

void shutDownServer()
{
    for(std::map<int, std::string>::iterator x = currentUsers.begin(); x != currentUsers.end(); ++x)
    { 
      close(x->first);
    }
    exit(1);
};

std::vector<std::string> get_spreadsheet_list(){

  //lock
  pthread_mutex_lock(&serverLock);
  std::vector<std::string> spreadsheet_list;
  DIR *dir;
  struct dirent *ent;

  //Gets Saved Files 
  if ((dir = opendir ("spreadsheets//")) != NULL) 
    {
    //Looks at all files in the saved folder
      while ((ent = readdir (dir)) != NULL) 
    {
	  
    std::string temps = (std::string) ent->d_name;
    
    //Pulls only .xml files
	  if(temps.find(".sprd") != std::string::npos && temps.find(".sprd~") == std::string::npos)
	    {	    
	      spreadsheet_list.push_back(temps);
        //printf ("%s\n", ent->d_name);
	    }
    }
      closedir (dir);
    }	
  else 
  {
    //No such files :(
    //unlock
    pthread_mutex_unlock(&serverLock);
    return spreadsheet_list;
  }
  //unlock
  pthread_mutex_unlock(&serverLock);
  return spreadsheet_list;

}

bool user_is_valid(std::string username, std::string  password) {
  //lock
  pthread_mutex_lock(&serverLock);
  std::string line,name,pass;
  std::ifstream userfile ("users.txt");
  if (userfile.is_open())
  {
    while ( getline (userfile,line) )
    {
      name = line.substr(0, line.find(","));
      if (username == name)
      {
        pass = line.substr(line.find(",")+1,strlen(line.c_str()));
        if (password == pass)
        {
          userfile.close();
          //unlock
          pthread_mutex_unlock(&serverLock);
          return true;
        }
      }
    }
    userfile.close();
    //unlock
    pthread_mutex_unlock(&serverLock);
    return false;
  }
};

void send_error(int cli, int errorCode, std::string source)
{
  std::string fullerrorMessage;
  fullerrorMessage.clear();
  fullerrorMessage = "{\"type\":\"error\",\"code\":\"";
  std::string temp;
  std::ostringstream convert;
  convert << errorCode;   
  temp = convert.str(); 
  fullerrorMessage.append(temp);
  fullerrorMessage.append("\",\"source\":\"");
  fullerrorMessage.append(source);
  fullerrorMessage.append("\"}\n\n");
  //returns Json that is ready to go
  send(cli , fullerrorMessage.c_str() , strlen(fullerrorMessage.c_str()) , 0 ); 
};
//who shot ya

void package_cells(int client, std::string fullSprd)
{
  //makes json to send
  std::string fullJsonMessage = "{\"type\":\"full send\",\"spreadsheet\":{";
  fullJsonMessage.append(fullSprd);
  fullJsonMessage.append("}}\n\n");
  //returns Json that is ready to go
  send(client , fullJsonMessage.c_str() , strlen(fullJsonMessage.c_str()) , 0 ); 
};

std::string read_sprd(std::string fileIN)
{
  //lock
  pthread_mutex_lock(&serverLock);
  rapidxml::xml_document<> doc;
  std::string filePath = "spreadsheets/";
  filePath.append(fileIN);
  std::string cellJson, fullJsonSprd;

  rapidxml::file<> xmlFile(filePath.c_str()); // Default template is char
  doc.parse<0>(xmlFile.data());

  rapidxml::xml_node<> *rootNode = doc.first_node("spreadsheet");
  //iterate over cells 
  for (rapidxml::xml_node<> * cell = rootNode->first_node("cell"); cell; cell = cell->next_sibling())
	{
      cellJson = "";
      //Find the name and contents
      rapidxml::xml_node<> * name = cell->first_node("name");
      rapidxml::xml_node<> * contents = name->next_sibling("contents");
      //abuse name and contents
      cellJson.append("\"");
      cellJson.append(name->value());
      cellJson.append("\":\"");
      cellJson.append(contents->value()); //A1: =C4+B2
      cellJson.append("\",");

      //populate dependency graph data
      std::string contents_str = contents->value();
      std::vector<std::string> dependees;
      if (contents_str[0] == '=')
      {
        //do a regex check to get all variables/dependees
        std::string var;
        for (int i=0; i < contents_str.size(); i++)
        {
          if (isalpha(contents_str[i]))
          {
            var = contents_str[i];
            if (isdigit(contents_str[i+1]))
            {
              var = var+contents_str[i+1];
              if (isdigit(contents_str[i+2]))
              {
                var = var+contents_str[i+2];
              }
              dependees.push_back(var);
            }
          }
        }
        for (int i=0; i<dependees.size(); i++)
        {
          spreadsheet_graphs[fileIN].add_dependency(dependees[i], name->value());
        }
      }

      //makes our full spreadsheet
      fullJsonSprd.append(cellJson);
	}
  //removes last comma
  fullJsonSprd = fullJsonSprd.substr(0, fullJsonSprd.length()-1);
  //unlock
  pthread_mutex_unlock(&serverLock);
  return fullJsonSprd;
};

std::string listen_for_reply(int clientIN)
{
    char buffer[1024] = {0};
    std::string message_from_client = buffer;
    std::string newTemp;
    while (message_from_client.find("\n\n") == std::string::npos)
    {
      //Checks to see if our Client is still with us
      int valread = recv(clientIN, buffer, 1024, 0);
      if (valread == -1)
      {
        //client hast left
        std::cout << "Client " << clientIN << " has logged off" << std::endl;
        currentUsers.erase(clientIN);
        userSpreadsheet.erase(clientIN);
        close(clientIN);
        return "close";
      }
      else
      {
        newTemp = buffer;
        message_from_client.append(newTemp);
      }
    }
    message_from_client = message_from_client.substr(0, message_from_client.length()-2);
    return message_from_client;
};

void * listen_for_reply_to_Server(void * clientIN)
{
    char buffer[1024] = {0};
    int client = *((int*)&clientIN);
    std::string message_from_client = buffer;
    std::string newTemp;
    replyForServer.clear();
    bool listeningTo = true;

    while (listeningTo == true)
    {
      int valread = recv(client, buffer, 1024, 0); 
      newTemp = buffer;
      message_from_client.append(newTemp);
      //std::cout << message_from_client << std::endl;
      replyForServer = message_from_client;
      message_from_client.clear();
      if (recv(client, buffer, sizeof(buffer), MSG_PEEK | MSG_DONTWAIT) == 0)
      {
        //Manangement server is gone
        break;
      }
    }
};

int authenticate_user(int clientIN, std::string message_from_client)
{
    rapidjson::Document user_request;
    std::string type, spreadsheet, username, password;
    bool changesLoaded = false;
    bool graphLoaded = false;
    try
    {
      //parse out the username, password and type
      user_request.Parse(message_from_client.c_str());
      type =  user_request["type"].GetString();
      spreadsheet =  user_request["name"].GetString();
      username =  user_request["username"].GetString();
      password =  user_request["password"].GetString();
    }
    catch(...)
    {
      send_error(clientIN, 1, "");
      message_from_client = listen_for_reply(clientIN);
      return authenticate_user(clientIN, message_from_client);
    }

    //checks to see if this is the managment portal
    if(type.find("Managment")!= std::string::npos)
    {
      //Authenticates
      if(user_is_valid(username, password))
      {
        //Calls the function to run the Management loop
        std::string approved = "Approved";
        send(clientIN , approved.c_str() , strlen(approved.c_str()) , 0 );
        return 2;
      }
      //Sends authentication error
      std::cout << "Invalid Management user" << std::endl;
      std::string invalid = "Invalid";
      send(clientIN , invalid.c_str() , strlen(invalid.c_str()) , 0 );
      //message_from_client = listen_for_reply(clientIN);
      //return authenticate_user(clientIN, message_from_client);
      return 3;
    }
    // do a check of the password to ensure its valid
    if (user_is_valid(username, password))
    {
      //Records users and spreadsheets
      currentUsers.insert(std::pair<int,std::string>(clientIN, username));
        
      std::string filePath = "spreadsheets/";
      filePath.append(spreadsheet);
      ifstream does_it_exist(filePath.c_str());
      if (!does_it_exist.good())
      {
        std::ofstream newSprd;
        newSprd.open(filePath.c_str()); 
        newSprd << "<spreadsheet version=\"ps6\">\n";
        newSprd << "</spreadsheet>";
        newSprd.close();
      }  
      userSpreadsheet.insert(std::pair<int,std::string>(clientIN, spreadsheet));
      //spreadsheet change list
      for(std::map<std::string,cellmanipulator>::iterator y = changes.begin(); y != changes.end(); ++y)
      { 
        if (y->first == spreadsheet)
        {
          changesLoaded = true;
        }
      }
      if (changesLoaded == false)
      {
        cellmanipulator changeLog = cellmanipulator(spreadsheet);
        changes.insert(std::pair<std::string,cellmanipulator>(spreadsheet, changeLog));
      }

      //dependency graph
      for(std::map<std::string,dependency_graph>::iterator y = spreadsheet_graphs.begin(); y != spreadsheet_graphs.end(); ++y)
      { 
        if (y->first == spreadsheet)
        {
          graphLoaded = true;
        }
      }
      if (graphLoaded == false)
      {
        dependency_graph DP_graph = dependency_graph();
        spreadsheet_graphs.insert(std::pair<std::string,dependency_graph>(spreadsheet, DP_graph));
      }
      return 1; 
    } 
    else
    {
      std::cout << "invalid user" << std::endl;
      send_error(clientIN, 1, "");
      message_from_client = listen_for_reply(clientIN);
      return authenticate_user(clientIN, message_from_client);
    }
    return 0;
};

void updateUser(std::string userData)
{ 
  //lock
  pthread_mutex_lock(&serverLock);
  bool isAdded = false;
  std::string line;
  std::ifstream userfile ("users.txt");
  std::string tempUser = userData.substr(0, userData.find(","));
  std::string tempPass = userData.substr(userData.find(",")+1, userData.length());
  std::ofstream outFile("replaced.txt");

  if (userfile.is_open())
  {
    while ( getline (userfile,line) )
    {
      if (tempUser == line.substr(0, line.find(",")))
      {
        if (tempPass != tempUser)
        {
          outFile << userData + "\n";
          isAdded = true;
        }
        else
        {
          std::cout << "Deleting User " << tempPass << std::endl;
          isAdded = true;
        }
      }
      else
      {
        outFile << line + "\n";
      }
    }
    if (isAdded == false)
    {
      outFile << userData + "\n";
    }
  }
  userfile.close();
  outFile.close();
  
  // trade file contents
  std::ifstream userin ("replaced.txt");
  std::ofstream Userout("users.txt");
  while ( getline (userin,line) )
  {
    Userout << line + "\n";
  }
  userfile.close();
  outFile.close();
  //unlock
  pthread_mutex_unlock(&serverLock);
  
};


std::string save_edit(std::string new_cell, std::string new_value, std::string spreadsheet)
{
  //lock
  pthread_mutex_lock(&serverLock);
  rapidxml::xml_document<> doc;
  std::string filePath = "spreadsheets/";
  filePath.append(spreadsheet);
  rapidxml::file<> xmlFile(filePath.c_str());
  doc.parse<0>(xmlFile.data());
  std::string oldContents = "";
  bool found = false;

  rapidxml::xml_node<> *rootNode = doc.first_node("spreadsheet");
  //iterate over cells 
  for (rapidxml::xml_node<> * current_cell = rootNode->first_node("cell"); current_cell; current_cell = current_cell->next_sibling())
	{
      //Find the name and contents
      rapidxml::xml_node<> * current_name = current_cell->first_node("name");
      rapidxml::xml_node<> * current_contents = current_name->next_sibling("contents");
      
	  //if we find the cell in the xml
    if (current_name->value() == new_cell)
	  {
      found = true;
		  oldContents = current_contents->value();
      if (new_value == "")
      {
        rootNode->remove_node(current_cell);
      }
      else
      {
        rapidxml::xml_node<> * new_contents = doc.allocate_node(rapidxml::node_element, "contents", new_value.c_str());
		    current_cell->remove_node(current_contents);
		    current_cell->append_node(new_contents);
      }
      break;
	  }
	}
  //there are no cells or the cell doesnt exist yet
  if (found == false)
  {
    rapidxml::xml_node<> * new_cell_xml = doc.allocate_node(rapidxml::node_element, "cell");
	  rootNode->append_node(new_cell_xml);
	  rapidxml::xml_node<> * new_name = doc.allocate_node(rapidxml::node_element, "name", new_cell.c_str());
	  new_cell_xml->append_node(new_name);
	  rapidxml::xml_node<> * new_contents = doc.allocate_node(rapidxml::node_element, "contents", new_value.c_str());
	  new_cell_xml->append_node(new_contents);
  }

  std::ofstream spreadsheet_file (filePath.c_str());
  spreadsheet_file << doc;
  spreadsheet_file.close();
  //unlock
  pthread_mutex_unlock(&serverLock);
  return oldContents;
};

void updateSheets(std::string sheetIN)
{
  pthread_mutex_lock(&serverLock);
  std::string filePath = "spreadsheets/";
  filePath.append(sheetIN);
  if( remove(filePath.c_str()) != 0 )
    std::cout << "Error deleting file " << filePath << std::endl;
  else
    std::cout << filePath << " File successfully deleted" << std::endl;
  pthread_mutex_unlock(&serverLock);
}

void runManagmentLoop(int clientIN)
{
  pthread_t replyThread;
  char buffer[1024] = {0};
  std::string userList;
  std::string spreadList;
  std::string reply;
  int count = 1;
  int running = 1;
  std::vector<std::string> list_to_send;
  std::string line,name,pass;
  std::string allSpreads;
  std::string allUsers;
  std::cout << "Managment has logged in" << std::endl;
  //boost::thread bt(listen_for_reply_to_Server, clientIN);
  pthread_create (&replyThread, NULL, listen_for_reply_to_Server, (void *)clientIN);
  pthread_detach(replyThread);

  //Run this while the Client is alive
  while (running == 1)
  {
    std::ifstream userfile ("users.txt");
    userList.clear();
    spreadList.clear();
    allSpreads.clear();
    allUsers.clear();
    userList.append("U,");
    spreadList.append("S,");     

    //Grabs message

    list_to_send = get_spreadsheet_list();;

    //Spreadsheets
    allSpreads = "Q,";
    std::string temp;

    pthread_mutex_lock(&serverLock);
    //Iterates the Spreadsheets 
    for(int it = 0; it < list_to_send.size(); it++)
    {
    temp.append((std::string)(list_to_send[it]));
    if (it != list_to_send.size() -1)
      {
        temp.append(","); 
      }
    }
    allSpreads.append(temp);
    allSpreads += ",;\n";

    //Itterates to get all the Users
    allUsers.append("A,");
    if (userfile.is_open())
    {
      while ( getline (userfile,line) )
      {
        name = line.substr(0, line.find(","));
        allUsers.append(name + ",");       
      }
    }
    userfile.close();
    allUsers.append(";\n");
    //unlock thread
    pthread_mutex_unlock(&serverLock);

    //Grabs the lists of active users and spreadsheets
    for(std::map<int, std::string>::iterator x = currentUsers.begin(); x != currentUsers.end(); ++x)
    { 
      userList.append(x->second);
      userList.append(",");
    }
    for(std::map<int, std::string>::iterator y = userSpreadsheet.begin(); y != userSpreadsheet.end(); ++y)
    { 
      spreadList.append(y->second);
      spreadList.append(",");
    }
    userList.append(";\n");
    spreadList.append(";\n");

    //Checks to see if our Manager is still with us
    if (recv(clientIN, buffer, sizeof(buffer), MSG_PEEK | MSG_DONTWAIT) == 0)
    {
      //Manangement server is gone
      std::cout << "Managment has logged off" << std::endl;
      break;
    }

    //Sends the Lists
    if(count == 200)
    { 
      if (userList.length() > 0 && spreadList.length())
      {
        send(clientIN , userList.c_str() , strlen(userList.c_str()) , 0 );
        send(clientIN , spreadList.c_str() , strlen(spreadList.c_str()) , 0 );
        send(clientIN , allSpreads.c_str(), strlen(allSpreads.c_str()), 0);
        send(clientIN , allUsers.c_str(), strlen(allUsers.c_str()), 0);
        //allUsers.clear();
      }
      count = 0;
    }
    count++;

    //Handles all commands
    if(replyForServer == "TurnOffServer\n\n")
    {
      std::cout << "shuttingdown" << std::endl;
      replyForServer.clear();
      shutDownServer();
      break;
    }
    else if(replyForServer[0] == 'U')
    {
      reply = replyForServer.substr(1, replyForServer.length() - 3);
      updateUser(reply);
      std::cout << "Modifying or Adding User" << std::endl;
      replyForServer.clear();
    }
    else if(replyForServer[0] == 'S')
    {
      reply = replyForServer.substr(1, replyForServer.length());
      std::cout << "Adding Spreadsheet" << std::endl;
      //open and save a new spreadsheet
      std::ofstream newSprd;
      std::string file_location = "spreadsheets/";
      file_location.append(reply.substr(0, replyForServer.find("sprd") + 3));
      newSprd.open(file_location.c_str());
      newSprd << "<spreadsheet version=\"ps6\">\n";
      newSprd << "</spreadsheet>";
      newSprd.close();
      replyForServer.clear();
    }
    else if(replyForServer[0] == 'D')
    {
      //remove the user
      reply = replyForServer.substr(1, replyForServer.length() - 3);
      updateUser(reply);
      replyForServer.clear();
    }
    else if(replyForServer[0] == 'X')
    {
      //remove the sheet
      reply = replyForServer.substr(1, replyForServer.length() - 3);
      //std::cout << reply << std::endl;
      updateSheets(reply);
      replyForServer.clear();
    }
    
  }
};

void sendAll(std::string spreadsheet, std::string update)
{
  //loop through clients and send to ones in this same sprd
  for(std::map<int, std::string>::iterator y = userSpreadsheet.begin(); y != userSpreadsheet.end(); ++y)
  { 
  if (y->second == (spreadsheet))
    {
      package_cells(y->first, update);
    }
  }
};

void client_loop(int client)
{
  rapidjson::Document request;
  char buffer[1024] = {0};
  bool run = true;
  std::string client_spreadsheet = userSpreadsheet.at(client);
  cellmanipulator spreadChanges = changes.at(client_spreadsheet);

  while (run)
  {
    std::string reply;
    reply = listen_for_reply(client);
    if (reply == "close")
    {
      run = false;
      break;
    }
    request.Parse(reply.c_str());
    std::string type =  request["type"].GetString();
    //std::cout << reply << std::endl;
    
    if(type == "edit")
    {
      std::string value = request["value"].GetString();
      std::string cell = request["cell"].GetString();
      if(value[0] ==  '=')
      {
        const rapidjson::Value& a = request["dependencies"];
        std::vector<std::string> dpend;
        assert(a.IsArray());
        
        for (rapidjson::SizeType i = 0; i < a.Size(); i++) // Uses SizeType instead of size_t
        {
          dpend.push_back(a[i].GetString());
        }
        
        if(!spreadsheet_graphs[client_spreadsheet].check_circular(cell, dpend))
        {
          std::string oldContents = save_edit(cell, value, client_spreadsheet);
          spreadChanges.edit_func_cell(cell, oldContents, dpend);

          std::string update = "\"" + cell + "\":\"" + value + "\"";
          //loop through clients and send to ones in this same sprd
          sendAll(client_spreadsheet, update);
        }
        else 
        {
          send_error(client, 2, cell);
        }
      }
      else
      {
        std::string oldContents = save_edit(cell, value, client_spreadsheet);
        spreadChanges.edit_cell(cell, oldContents);
        std::string update = "\"" + cell + "\":\"" + value + "\"";
        //loop through clients and send to ones in this same sprd
        sendAll(client_spreadsheet, update);
      }
      
    }
    else if(type == "undo")
    {
      std::string undo = spreadChanges.undo();

      if (undo != ":-:empty:-:")
      {
        std::string cellUndone = undo.substr(0, undo.find(","));
        std::string contentsUndone = undo.substr(undo.find(",") + 1, undo.length());
        save_edit(cellUndone, contentsUndone, client_spreadsheet);
        std::string update = "\"" + cellUndone + "\":\"" + contentsUndone + "\"";
        //loop through clients and send to ones in this same sprd
        sendAll(client_spreadsheet, update);
      }
    }
    else if(type == "revert")
    {
      std::string cell = request["cell"].GetString();
      std::string revert = spreadChanges.revert_cell(cell);

      if (revert != ":-:empty:-:")
      {
        //need to readd the cell with the correct contents func or normal edit
        save_edit(cell, revert, client_spreadsheet);
        std::string update = "\"" + cell + "\":\"" + revert + "\"";
        //loop through clients and send to ones in this same sprd
        sendAll(client_spreadsheet, update);
      }
    }
    reply.clear();
  }
  //this closes the thread the client was on
  pthread_exit(NULL);
};

void * receive_messages (void * client_socket) {
    
    rapidjson::Document user_request;
    int client;
    char buffer[1024] = {0}; 
    client = *((int*)&client_socket);
    std::cout<< "Client " << client << " has logged on" << std::endl;

    // Grabs a list of Servers and puts it into a Json 
    std::vector<std::string> list_to_send = get_spreadsheet_list();;

    //Begins our Glorious Json File
    std::string message_of_servers = "{\"type\":\"list\",\"spreadsheets\":[";
    std::string temp;

    //Iterates the vector of strings and adds each file to an "array" of strings 
    for(int it = 0; it < list_to_send.size(); it++)
    {
    temp.append("\"" + (std::string)(list_to_send[it]) + "\"");
    if (it != list_to_send.size() -1)
      {
        temp.append(","); 
      }
    }
    message_of_servers.append(temp);
    message_of_servers += "]}\n\n";

    //std::cout << message_of_servers << std::endl;

    //Sends list of Spreadhsheets
    send(client , message_of_servers.c_str() , strlen(message_of_servers.c_str()) , 0 ); 

    //Gets User Information
    std::string message_from_client = listen_for_reply(client);
    
    //parse out the spreadsheet
    user_request.Parse(message_from_client.c_str());
    std::string spreadsheet =  user_request["name"].GetString();

    //Determine if this is a Client (1) or a Management Device (2)
    int userType = authenticate_user(client, message_from_client);
    if (userType == 1)
    {
      //Client loop

      //open and save a new spreadsheet
      package_cells(client, read_sprd(spreadsheet));
      client_loop(client);
    }
    else if (userType == 2)
    {
      runManagmentLoop(client);
    }
};



int main( int argc, char *argv[] ) {
    pthread_t clientThread;
    int serverSocket, clientSocket, portno;
    struct sockaddr_in serv_addr, cli_addr;
    socklen_t clilen;
    
    /* First call to socket() function */
    serverSocket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
    
    if (serverSocket < 0) {
       perror("ERROR opening socket for the server");
       exit(1);
    }
    
    /* Initialize socket structure */
    bzero((char *) &serv_addr, sizeof(serv_addr));
    portno = 2112;
    
    serv_addr.sin_family = AF_INET;
    serv_addr.sin_addr.s_addr = INADDR_ANY;
    serv_addr.sin_port = htons(portno);
    
    /* Now bind the host address using bind() call.*/
    if (bind(serverSocket, (struct sockaddr *) &serv_addr, sizeof(serv_addr)) < 0) {
       perror("ERROR on binding");
       exit(1);
    }
   
    /* Now start listening for the clients, here
       * process will go in sleep mode and will wait
       * for the incoming connection
    */
   
    listen(serverSocket,5);
    clilen = sizeof(cli_addr);
   
    while (1) {
        clientSocket = accept(serverSocket, (struct sockaddr *) &cli_addr, &clilen);
		
        if (clientSocket < 0) {
           perror("ERROR on accept");
        }
        //start multithreading here accordingly
        pthread_create (&clientThread, NULL, receive_messages, (void *)clientSocket);
        pthread_detach(clientThread);
    }

    close(clientSocket);
    close(serverSocket);
    return 0;
}
