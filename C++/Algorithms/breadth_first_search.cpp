#include <iostream>
#include <string>
#include <map>
#include <queue>
#include <list>
#include <algorithm>
using namespace std;

map<string, int> dist;
map<string, list<string> > graph;

void bfs(map<string, list<string> > G, string s){
  int day;
  string u;

  for(map<string, list<string> >::iterator iter = G.begin(); iter != G.end(); ++iter){
    dist[iter->first] = -1;
  }

  dist[s] = 0;

  queue<string> Q;
  Q.push(s);

  while(!Q.empty()){
    u = Q.front();
    Q.pop();
    for(list<string>::iterator iter = G[u].begin(); iter != G[u].end(); ++iter){
      if(dist[(*iter)] == -1){
	Q.push((*iter));
	day = dist[u] + 1;
	dist[(*iter)] = day;
      }
    }
  }

}   

int main(int argc, char **argv){
  int num_students;
  int num_friends;
  int num_rumors;
  string student1;
  string student2;
  list<string> unaware_students;

  cin >> num_students;
  for(int i = 0; i < num_students; i++){
    cin >> student1;

    list<string> friends;
    graph[student1] = friends;
  }

  cin >> num_friends;
  for(int i = 0; i < num_friends; i++){
    cin >> student1;
    cin >> student2;

    graph[student1].push_back(student2);
    graph[student2].push_back(student1);
  }

  cin >> num_rumors;
  for(int i = 0; i < num_rumors; i++){
    cin >> student1;
    map <int, list<string> > students_of_day;
    list<string> unaware_students;

    bfs(graph, student1);

    //Group the students by day they heard the rumor
    for(map<string, int>::iterator iter = dist.begin(); iter != dist.end(); ++iter){
      if(iter->second != -1)
	students_of_day[iter->second].push_back(iter->first);
      else
	unaware_students.push_back(iter->first);
    }

    for(map<int, list<string> >::iterator iter = students_of_day.begin(); iter != students_of_day.end(); ++iter){
      (iter->second).sort();
      for(list<string>::iterator iter2 = (iter->second).begin(); iter2 != (iter->second).end(); ++iter2){
	cout << (*iter2) << " ";
      }
    }

    unaware_students.sort();
    for(list<string>::iterator iter = unaware_students.begin(); iter != unaware_students.end(); ++iter){
      cout << (*iter) << " ";
    }
    cout << endl;
  }
}

