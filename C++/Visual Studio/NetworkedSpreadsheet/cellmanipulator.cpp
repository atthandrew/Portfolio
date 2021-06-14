
#include <stdio.h>
#include <iostream>
#include <string.h>
#include <vector>
#include <pthread.h>
#include <dirent.h>
#include "include/document.h"
#include "include/rapidxml.hpp"
#include "include/rapidxml_utils.hpp"
#include "include/rapidxml_print.hpp"
#include "string_set.h"
#include "node.h"
#include "cellmanipulator.h"

using namespace std;

cs3505::string_set cell_manipulate = cs3505::string_set();
string masterChange;
pthread_mutex_t changeLock = PTHREAD_MUTEX_INITIALIZER;

void loadChanges(string filePath)
{
  cout<<filePath<<endl;
  ifstream load(filePath.c_str());
  if (!load.good())
  {
    std::ofstream newSprd;
    newSprd.open(filePath.c_str()); 
    newSprd.close();
  } 
  
  string line;  

  if (load.is_open())
  {
    while ( getline (load,line) )
    {   
      cell_manipulate.add(line.substr(0, line.find(",")), line.substr(line.find(",") + 1, line.length()));     
    }    
  }

  load.close();
};

cellmanipulator::cellmanipulator(string spreadIN)
{
  masterChange = spreadIN;

  //Check for file to load
  string filePath = "ChangeSaves/";
  filePath.append(masterChange);
  ifstream does_it_exist(filePath.c_str());
  if (does_it_exist.good())
  {
    loadChanges(filePath);
    return;
  } 

};  

void cellmanipulator::edit_cell(string cell, string value)
{
  string type = "edit";
  cell_manipulate.add(cell, value);
  vector<string> myGraph = cell_manipulate.get_elements();
  saveChanges(cell, value, type);
};


void cellmanipulator::edit_func_cell(string cell, string value, vector<string> dependencies)
{
  string type = "edit";
  //Check for circular dependencies
  cell_manipulate.add(cell, value);
  vector<string> myGraph = cell_manipulate.get_elements();
  for (std::vector<std::string>::iterator it = myGraph.begin(); it != myGraph.end(); it++)
   {
     string word = *it;
     cout << word << endl;
   }
   saveChanges(cell, value, type);
  //save the .sprd and the xml for the map
};

string cellmanipulator::revert_cell(string cell)
{
  string reverted;
  string type = "revert";
  reverted = cell_manipulate.remove(cell);

  if (reverted != "crap")
  {
    saveChanges(cell, reverted, type);
    return reverted;
  }
  return ":-:empty:-:";
};

string cellmanipulator::undo()
{
  string undone;
  string type = "Undo";
  undone = cell_manipulate.removeLast();

  if (undone != "crap")
  {
    string cell = undone.substr(0, undone.find(","));
    string contents = undone.substr(undone.find(",") + 1, undone.length());
    saveChanges(cell, contents, type);
    return undone;
  }

  return ":-:empty:-:";
};

void cellmanipulator::saveChanges(string new_cell, string new_value, string type)
{
  pthread_mutex_lock(&changeLock);
  string filePath = "ChangeSaves/";
  filePath.append(masterChange);
  string temp_filepath = filePath;
  temp_filepath.append("temp");
  cout<<filePath<<endl;
  ifstream load(filePath.c_str());
  if (!load.good())
  {
    std::ofstream newSprd;
    newSprd.open(filePath.c_str()); 
    newSprd.close();
  } 
  string toCompare = new_cell + "," + new_value;
  
  string line;  
  ofstream temp(temp_filepath.c_str());

  if (load.is_open())
  {
    while ( getline (load,line) )
    {   
      if (line != toCompare)
      {
        temp << line + "\n"; 
      }       
    }    
  }
  if (type == "edit")
  {
    cell_manipulate.add(new_cell, new_value);
    temp << new_cell + "," + new_value + "\n";
  }

  load.close();
  temp.close();
  
  // trade file contents
  ifstream load2 (temp_filepath.c_str());
  ofstream temp2(filePath.c_str());
  while ( getline (load2,line) )
  {
    temp2 << line + "\n";
  }
  temp2.close();
  load2.close();
  
  remove(temp_filepath.c_str());

  //unlock
  pthread_mutex_unlock(&changeLock);
};



