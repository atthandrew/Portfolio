#include <iostream>

using namespace std;

int main() {
  int num = 1;
  cout << num << endl;
  try {
      num++;
      throw 20;
  }
  catch(int e) {
      cout << "inside catch: " << num << endl;
  }
  cout << num << endl;
  return 0;
}