#include <stdio.h>
#include "Var.h" 

Var b;
Var m;
Var str;

int main() 
{
b.setValue(22);
m.setValue(33.92);
str.setValue("hello world");
b.printValue();
m.printValue();
str.printValue();
//printf(b);
	printf("not alone\n");
return 1;
}
