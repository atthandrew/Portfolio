#include <iostream>
#include <map>
#include <algorithm>
using namespace std;

#define MAX_PEN 30000*30000

//Finds penalty(i), the min penalty of driving to Emerald City from city i
int penalty(int distance[], int i, int n, int cache[]){

  if(cache[i] != -1){
    return cache[i];
  }

  int min_penalty = MAX_PEN;
  for(int k = i+1; k <= n; k++){

    //Finds [400-(distance[k] - distance[i])]^2 + penalty(k)
    int diff = 400 - (distance[k] - distance[i]);
    int cur_penalty = diff * diff + penalty(distance, k, n, cache);

    min_penalty = min(min_penalty, cur_penalty);
  }

  cache[i] = min_penalty; //cache the min_penalty, penalty(i)
  return min_penalty;
}

//Finds penalty(), the min penalty of driving to Emerald City from Munchkinland
int penalty(int distance[], int n){
  int cache[n+1];

  for(int i = 0; i < n+1; i++){
    cache[i] = -1;
  }

  cache[n] = 0; //penalty(n) = 0, so enter it immediately

  //Finds our answer, penalty(0)
  return penalty(distance, 0, n, cache);
}

int main(int argc, char** argv){
  int n;
  cin >> n;

  int dist[n+1];

  for(int i = 0; i < n+1; i++){
    cin >> dist[i];
  }

  int result = penalty(dist, n);
  cout << result;
}
