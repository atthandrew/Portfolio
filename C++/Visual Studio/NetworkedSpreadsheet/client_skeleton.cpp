#include <iostream>
#include <cstring>
#include <stdio.h> 
#include <sys/socket.h> 
#include <stdlib.h> 
#include <netinet/in.h>
#include <arpa/inet.h>
#include <unistd.h>

using namespace std;

int main(int argc, char **argv)
{
  cout << "What is the address of the server you would like to connect to?" << endl;

  string host = "";
  cin >> host;
  char addr[host.size() + 1];
  host.copy(addr, host.size() + 1);
  addr[host.size()] = '\0';

  cout << "Username: ";
  string username = "";
  cin >> username;

  cout << "Password: ";
  string password = "";
  cin >> password;

  cout << "Filename to open: ";
  string filename = "";
  cin >> filename;

  string connectionstring = username + "," + password + "," + filename;
  cout << connectionstring << endl;


  int const PORT = 8080;
  struct sockaddr_in address;
  int sock = 0, valread;
  struct sockaddr_in serv_addr;

  char hello[connectionstring.size() + 1];
  connectionstring.copy(hello, connectionstring.size() + 1);
  hello[connectionstring.size()] = '\0';

  char buffer[1024] = {0};
if ((sock = socket(AF_INET, SOCK_STREAM, 0)) < 0) 
    { 
        printf("\n Socket creation error \n"); 
        return -1; 
    } 
   
    memset(&serv_addr, '0', sizeof(serv_addr)); 
   
    serv_addr.sin_family = AF_INET; 
    serv_addr.sin_port = htons(PORT); 
       
    // Convert IPv4 and IPv6 addresses from text to binary form 
    if(inet_pton(AF_INET, addr, &serv_addr.sin_addr)<=0)  
    { 
        printf("\nInvalid address/ Address not supported \n"); 
        return -1; 
    } 
   
    if (connect(sock, (struct sockaddr *)&serv_addr, sizeof(serv_addr)) < 0) 
    { 
        printf("\nConnection Failed \n"); 
        return -1; 
    } 
    send(sock , hello , strlen(hello) , 0 ); 
    std::cout << "Sent connectionstring to server\n";
    valread = read( sock , buffer, 1024); 
    std::cout << buffer << std::endl;

  return 0;
}

