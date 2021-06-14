#include "dependency_graph.h"

#include <algorithm>

using namespace std;

dependency_graph::dependency_graph(){
  this->dependents = map<string, vector<string> >();
  this->dependees = map<string, vector<string> >();

  this->size = 0;
}

int dependency_graph::get_size(){
  return this->size;
}

int dependency_graph::get_num_dependees(string s){
  if(this->dependees.find(s) == this->dependees.end()){
    return 0;
  }
  else{
    return this->dependees[s].size();
  }
}

bool dependency_graph::has_dependents(string s){
  if(this->dependents.find(s) == this->dependents.end()){
    return false;
  }
  else{
    return this->dependents[s].size() > 0;
  }
}

bool dependency_graph::has_dependees(string s){
  if(this->dependees.find(s) == this->dependees.end()){
    return false;
  }
  else{
    return this->dependees[s].size() > 0;
  }
}

vector<string> dependency_graph::get_dependents(string s){
  if(this->dependents.find(s) == this->dependents.end()){
    return vector<string>();
  }
  else{
    return this->dependents[s];
  }
}

vector<string> dependency_graph::get_dependees(string s){
  if(this->dependees.find(s) == this->dependees.end()){
    return vector<string>();
  }
  else{
    return this->dependees[s];
  }
}

void dependency_graph::add_dependency(string s, string t){
  if(this->dependents.find(s) == this->dependents.end()){
    this->dependents.insert(pair<string, vector<string> >(s, vector<string>()));
  }

  if(this->dependees.find(t) == this->dependees.end()){
    this->dependees.insert(pair<string, vector<string> >(t, vector<string>()));
  }

  if(find(this->dependents[s].begin(), this->dependents[s].end(), t) == this->dependents[s].end() &&
     find(this->dependees[t].begin(), this->dependees[t].end(), s) == this->dependees[t].end()){
    this->dependents[s].push_back(t);
    this->dependees[t].push_back(s);
    this->size++;
  }
}

void dependency_graph::remove_dependency(string s, string t){
  if(this->dependents.find(s) == this->dependents.end() && 
     this->dependees.find(s) == this->dependees.end()){
    return;
  }

  else{
    if(find(this->dependents[s].begin(), this->dependents[s].end(), t) == this->dependents[s].end() &&
       find(this->dependees[t].begin(), this->dependees[t].end(), s) == this->dependees[t].end()){
      return;
    }

    else{
      this->dependents[s].erase(find(this->dependents[s].begin(), this->dependents[s].end(), t));
      this->dependees[t].erase(find(this->dependees[t].begin(), this->dependees[t].end(), s));
    }
  }
}

void dependency_graph::replace_dependents(string s, vector<string> new_dependents){
  if (this->dependents.find(s) != this->dependents.end()) {
    vector<string> old_dependents = this->get_dependents(s);
    for (int i = 0; i < old_dependents.size(); i++) {
      this->remove_dependency(s, old_dependents[i]);
    }
  }
  for (int i = 0; i < new_dependents.size(); i++) {
      this->add_dependency(s, new_dependents[i]);
    }
}

void dependency_graph::replace_dependees(string s, vector<string> new_dependees){
  if (this->dependees.find(s) != this->dependees.end()) {
    vector<string> old_dependees = this->get_dependees(s);
    for (int i = 0; i < old_dependees.size(); i++) {
      this->remove_dependency(old_dependees[i], s);
    }
  }
  for (int i = 0; i < new_dependees.size(); i++) {
    this->add_dependency(new_dependees[i], s);
  }
}

bool dependency_graph::check_circular(string s, vector<string> direct_dependencies) {

  vector<string> old_dependees = this->get_dependees(s);
  this->replace_dependees(s, direct_dependencies);

  vector<string> visited;
  
  //getcellstorecalculate
  try {
    for (int i = 0; i < direct_dependencies.size(); i++) {
      if (find(visited.begin(), visited.end(), direct_dependencies[i]) == visited.end()) {
        this->visit(direct_dependencies[i], direct_dependencies[i], visited);
      }
    }
    return false;
  }
  catch(int e) {
    this->replace_dependees(s, old_dependees);
    return true;
  }
}

void dependency_graph::visit(string start, string name, vector<string> &visited) {
  vector<string> sub_dependents = this->get_dependents(name);
  visited.push_back(name);
  for (int i = 0; i < sub_dependents.size(); i++) {
    if (sub_dependents[i] == start) {
      throw 2;
    }
    else if (find(visited.begin(), visited.end(), sub_dependents[i]) == visited.end()) {
      this->visit(start, sub_dependents[i], visited);
    }
  }
}
