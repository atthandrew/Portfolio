#ifndef DEPENDENCY_GRAPH_H 
 
#define DEPENDENCY_GRAPH_H

#include<map>
#include<vector>
#include<string>

class dependency_graph {
 private:
  std::map<std::string, std::vector<std::string> > dependents;
  std::map<std::string, std::vector<std::string> > dependees;
  int size;

 public:
  dependency_graph();

  int get_size();
  int get_num_dependees(std::string);
  bool has_dependents(std::string);
  bool has_dependees(std::string);
  std::vector<std::string> get_dependents(std::string);
  std::vector<std::string> get_dependees(std::string);
  void add_dependency(std::string, std::string);
  void remove_dependency(std::string, std::string);
  void replace_dependents(std::string, std::vector<std::string>);
  void replace_dependees(std::string, std::vector<std::string>);
  bool check_circular(std::string s, std::vector<std::string>);
  void visit(std::string, std::string, std::vector<std::string> &);
};


#endif
