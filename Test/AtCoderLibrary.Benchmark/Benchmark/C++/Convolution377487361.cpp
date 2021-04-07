#include <atcoder/all>
#include <iostream>
#include <random>
#include <iostream>
#include <algorithm>
#include <vector>
#include <queue>
#include <set>
#include <map>
#include <math.h>
#include <unistd.h>
#include <unordered_set>
#include <unordered_map>
#define rep(i,n) for(int i=0;i<n;i++)
#define rep2(i,s,n) for(int i=s;i<n;i++)
#define repd(i,s,n,d) for(int i=s;i<n;i+=d)
#define repit(col) for(auto it = std::begin(col); it != std::end(col); ++it)

using namespace std;
using namespace atcoder;

using mint = static_modint<377487361>;
typedef long long ll;

int main() {
	int n = 1 << 24;;

	n >>= 2;
	vector<mint> a(n), b(n);
	for (int i = 0; i < n; i++) {
		a[i] = i + 1234;
		b[i] = i + 5678;
	}

	auto c = convolution(a, b);
	printf("%d\n", c[0].val());
	return 0;
}
