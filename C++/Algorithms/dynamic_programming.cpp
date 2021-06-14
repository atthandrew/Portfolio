#include <iostream>
#include <vector>
#include <algorithm>
#define mod(X,Y) ((X%Y) + ((X%Y<0)?Y:0))
using namespace std;

/*
 * See https://utah.kattis.com/problems/utah.gobble for problem details.
 * Author: Andrew Thompson
 */

typedef vector<vector<int> > myvec;

int r;
int c;

int maxScore(myvec &grid, pair<int, int> cell, myvec &cache){
  int cellval = grid[cell.first][cell.second];
  int cellrow = cell.first;
  int cellcol = cell.second;

  if(cache[cellrow][cellcol] != -1){
    return cache[cellrow][cellcol];
  }

  int max_score = 0;
  for(int i = cellrow; i > 0; i--){
    int up_row = cellrow-1;
    int uplft_col = mod((cellcol-1),(c));
    int uprt_col = mod((cellcol+1),(c));

    int uplft_max = maxScore(grid, make_pair(up_row,uplft_col), cache)-grid[up_row][uplft_col];
    int uprt_max = maxScore(grid, make_pair(up_row,uprt_col), cache)-grid[up_row][uprt_col];
    int up_max = maxScore(grid, make_pair(up_row,cellcol), cache)+grid[up_row][cellcol];

    max_score = max(uplft_max, uprt_max);
    max_score = max(max_score, up_max);
  }

  cache[cellrow][cellcol] = max_score;
  return max_score;
}

int maxScore(myvec &grid){
  myvec cache (r, vector<int> (c, -1));
  int max_score = -1;

  for(int k = 0; k < c; k++){
    int start_score = maxScore(grid, make_pair(r-1, k), cache) + grid[r-1][k];
    max_score = max(max_score, start_score);
  }

  /*
  for(int i = 0; i < r; i++){
    for(int j = 0; j < c; j++){
      cout << cache[i][j] << " ";
    }
    cout << endl;
  }
  */

  return max_score;
}

int main(int argc, char** argv){
  //These were made global
  //int r;
  //int c;

  cin >> r;
  cin >> c;

  myvec grid (r, vector<int> (c, 0));

  for(int i = 0; i < r; i++){
    for(int j = 0; j < c; j++){
      cin >> grid[i][j];
    }
  }

  int result = maxScore(grid);
  cout << result << endl;
}
