#include <iostream>
#include <vector>
#include <iomanip>
#include <queue>
using namespace std;

/*
 * See https://utah.kattis.com/problems/getshorty for more details.
 * Author: Andrew Thompson
 */
double dijkstra(vector<pair<int, double> > G[], int s, int size){
  double scale[size];
  bool popped[size];

  for(int u = 0; u < size; u++){
    scale[u] = -1.0;
    popped[u] = false;
  }

  priority_queue<pair<double, int>, vector<pair <double, int> >, 
		 less<pair<double, int> > > PQ;
  PQ.push(make_pair(1.0, s));
  scale[s] = 1.0;

  while(!PQ.empty()){
    int u = PQ.top().second;
    PQ.pop();

    if(popped[u])
      continue;
    /*if(u == size - 1)
      break;*/

    vector<pair<int, double> >:: iterator iter;
    for(iter = G[u].begin(); iter != G[u].end(); ++iter){
      int v = (*iter).first;
      double weight = (*iter).second;

      if(scale[v] < scale[u] * weight){
	scale[v] = scale[u] * weight;
	PQ.push(make_pair(scale[v], v));
	//cout << scale[v] << endl;
      }
    }
    popped[u] = true;
  }

  return scale[size-1];
}

int main(int argc, char **argv){
  int inter;
  int corr;
  int inter1;
  int inter2;
  double factor;

  cin >> inter;
  cin >> corr;

  while(inter != 0 && corr != 0){
    double result;
    vector<pair<int, double> > graph[inter];

    for(int i = 0; i < corr; i++){
      cin >> inter1;
      cin >> inter2;
      cin >> factor;

      graph[inter1].push_back(make_pair(inter2, factor));
      graph[inter2].push_back(make_pair(inter1, factor));
    }

    result = dijkstra(graph, 0, inter);
    cout << fixed << setprecision(4) << result << endl;

    cin >> inter;
    cin >> corr;
  }

}
