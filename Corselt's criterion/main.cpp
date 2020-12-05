#include <iostream>

using namespace std;

bool sqr_check(int n){
    for( long long i=2; i <= sqrt(n); i++){
        if (n % (i*i) == 0) return false;
    }
    return true;
}

bool prime_factors_check(int n){
    int n1=n;
    for(int i=2; i<=(n/2); i++){
        while((n1>1) && (n1%i==0)){
            if ( ((n-1)%(i-1)) != 0) return false;
            n1/=i;
        }
    }

    return (n1!=n);
}
int main() {
    int n;
    do{
        cout<<"Enter number for check (0 to exit):\n";
        cin>>n;
        cout<<n<<" is ";
        if ((sqr_check(n) && prime_factors_check(n))) {
            cout << "NOT ";
			cout << "a Carmichael number\n";
        }
        cout<< "a Carmichael number\n";
		n++;
    }while(n!=0);

    return 0;
}
