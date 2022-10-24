#include <stdio.h>
#include <chrono>

#define a 0;
#define b 2;
#define c 3;
#define iter 100000000;
char printFormat1[] = "%u";

int main() {
	auto start = std::chrono::high_resolution_clock::now();
	
	__asm {

		start:
			xor edx, edx	//using as iterator, fast set i = 0
			mov ebx, b		//set ebx as b
			shl ebx, 1		//fast b*2
			add ebx, c
			xor eax, eax	//using eax as a
			mov ecx, iter	//using as loop counter

		addition:
			add eax, ebx	//a += b*2 + c
			sub eax, edx	//a -= i
			inc edx			//i++
			loop addition	//decrements ecx, jumps addition

		result:
			push eax
			mov eax, offset printFormat1
			push eax
			call printf;
			pop eax
			pop eax
	}

	auto elapsed = std::chrono::high_resolution_clock::now() - start;

	long long microseconds = std::chrono::duration_cast<std::chrono::microseconds>(elapsed).count();

	printf("\nExecution time: 0.%llums\n", microseconds);
	
	return 0;
}


