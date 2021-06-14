#ifndef CELLMANIPULATOR_H
#define CELLMANIPULATOR_H

#include <string>
#include <vector>


  class cellmanipulator
  {
    public:
    cellmanipulator(std::string spreadIN);  
   
    void edit_cell(std::string cell, std::string value);
    void edit_func_cell(std::string cell, std::string value, std::vector<std::string> dependencies);
    std::string revert_cell(std::string cell);
    std::string undo();

    private:
    void saveChanges(std::string new_cell, std::string new_value, std::string type);
  };


#endif