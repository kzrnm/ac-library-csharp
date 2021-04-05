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


ll op(ll a, ll b) { return a + b; }
ll e() { return 0; }
ll target;
bool f(ll v) { return v * 3 / 2 <= target; }

int main() {
	int n;
	scanf("%d", &n);

	long ans = 0;
	auto seg = segtree<ll, op, e>(n);
	for (int i = 0; i < n; i++)
	{
		seg.set(i, i);
	}
	for (int i = 0; 2 * i <= n; i++)
	{
		ans ^= seg.prod(i, 2 * i);
	}
	printf("%lld\n", ans);
	return 0;
}
