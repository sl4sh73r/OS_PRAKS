using namespace std;

int main(){

    unsigned int a = 0, b = 2, c = 3;

    for (int i = 0; i <100000000; i++){
       a += 2*b + c - i; 
    }
    cout<<a;
    return 0; 
}
