#include <iostream>
#include <string>
#define mod(X,Y) ((X%Y) + ((X%Y<0)?Y:0))
using namespace std;

typedef long long ll;


struct RSA{
  ll N;
  ll e;
  ll d;
};

struct EEresult{
  ll x;
  ll y;
  ll d;
};

ll gcd(ll a, ll b){
  if(b == 0)
    return a;
  else
    return gcd(b, mod(a,b));
}

ll exp(ll x, ll y, ll N){
  if(y == 0)
    return 1;
  else{
    ll z = exp(x, y/2, N);

    if(mod(y,2) == 0)
      return mod(mod(z,N)*mod(z,N), N);
    else
      return mod(mod(x,N)*mod(mod(z,N)*mod(z,N), N), N);
  }
}

struct EEresult ee(ll a, ll b){
  if(b == 0){
    struct EEresult result = {1, 0, a};
    return result;
  }
  else{
    struct EEresult xyd = ee(b, mod(a,b));
    struct EEresult result = {xyd.y, (xyd.x-((a/b)*xyd.y)), xyd.d};
    return result;
  }
}

//Returns N if the inverse does not exist
ll inverse(ll a, ll N){
  struct EEresult xyd = ee(a, N);

  if(xyd.d == 1)
    return mod(xyd.x, N);
  else
    return N;
}

bool isprime(ll p){
  ll a = 2;
  if(exp(a, p-1, p) != 1)
    return false;

  a = 3;
  if(exp(a, p-1, p) != 1)
    return false;

  a = 5;
  if(exp(a, p-1, p) != 1)
    return false;

  return true;
}

struct RSA key(ll p, ll q){
  struct RSA result = {0, 0, 0};

  //Calculate N
  result.N = p * q;

  //Calculate phi
  ll phi = (p-1) * (q-1);

  //Calculate e
  int e = 2;
  while(result.e == 0){
    if(gcd(e, phi) == 1)
      result.e = e;
    else
      e++;
  }

  //Calculate d
  result.d = inverse(result.e, phi);

  return result;
}

int main(int argc, char **argv){
  string cat;
  ll arg1;
  ll arg2;
  ll arg3;

  while(cin >> cat){
    if(cat.compare("gcd") == 0){
      cin >> arg1;
      cin >> arg2;

      cout << gcd(arg1, arg2) << endl;
    }

    else if(cat.compare("exp") == 0){
      cin >> arg1;
      cin >> arg2;
      cin >> arg3;

      cout << exp(arg1, arg2, arg3) << endl;
    }

    else if(cat.compare("inverse") == 0){
      cin >> arg1;
      cin >> arg2;

      ll result = inverse(arg1, arg2);

      if(result == arg2)
	cout << "none" << endl;
      else
	cout << result << endl;
    }

    else if(cat.compare("isprime") == 0){
      cin >> arg1;

      bool result = isprime(arg1);

      if(result)
	cout << "yes" << endl;
      else
	cout << "no" << endl;
    }

    else if(cat.compare("key") == 0){
      cin >> arg1;
      cin >> arg2;

      struct RSA result = key(arg1, arg2);
      cout << result.N << " " << result.e << " " << result.d << endl;
    }

    else
      break;
  }
}
