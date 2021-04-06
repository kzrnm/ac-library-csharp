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
	int n = 1 << 24;;

	n >>= 3;
	long ans = 0;
	auto graph = scc_graph(n);
	for (int i = 0; i < n; i++)
	{
		graph.add_edge(i, (int)((1000000007L * i) % n));
	}
	ans = graph.scc().size();
	printf("%lld\n", ans);
	return 0;
}
