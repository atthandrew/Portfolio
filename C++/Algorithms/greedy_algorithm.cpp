#include <iostream>
#include <map>
#include <set>
using namespace std;

typedef map<int, set<int, greater<int> >, greater<int> > mymap;
typedef set<int, greater<int> > myset;

int main(int argc, char **argv){
  int N;
  int T;
  int cash;
  int wait_time;
  int max_cash;
  int max_cashwait; //keeps track of the wait time (key) of the largest cash value
  int total_cash = 0;
  mymap people;

  cin >> N;
  cin >> T;

  for(int i = 0; i < N; i++){
    cin >> cash;
    cin >> wait_time;

    people[wait_time].insert(cash);
  }

  //Look at each possible time slot from closest to closing time to farthest
  for(int cur_time = T-1; cur_time >= 0; cur_time--){
    max_cash = 0;

    //Look through the map, sorted from greatest wait time to smallest wait time
    for(mymap::iterator it = people.begin(); it != people.end(); ++it){
      //Stop going through the map once we've examined all wait times >= the time slot in question
      if(it->first < cur_time)
	break;

      //Look at the first element in the set, which will be the largest cash value for that wait time
      myset::iterator it_set = it->second.begin();

      //If bigger than the current max cash value, make it the new max cash value
      if(*it_set > max_cash){
	max_cash = *it_set;
	max_cashwait = it->first;
      }
    }

    total_cash += max_cash; //add the max cash for the time slot to the total

    //Remove the person (cash value) from the appropriate set
    if(max_cash != 0){
      people[max_cashwait].erase(max_cash);
    }
  }

  cout << total_cash;
}
