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

using mint = modint998244353;
typedef long long ll;

int main() {
	int n;
	scanf("%d", &n);

	n <<= 1;
	long ans = 0;
	fenwick_tree<ll> fw(n);
	for (int i = 0; i < n; i++)
	{
		fw.add(i, i + 1234);
	}
	for (int i = 0; 2 * i <= n; i++)
	{
		ans ^= fw.sum(i, 2 * i);
	}
	printf("%lld\n", ans);
	return 0;
}
