#include "Var.h"

Var i;
Var EvenAndOdd()
{
for(i.setValue(0); i<10; i.setValue(i + 1))
{
if((i%2)==0)
{
printf("%s", "Odd");
}
else
{
printf("%s", "Even");
}
}
}


int main() {
i.setValue(0);
EvenAndOdd();
return 1;}
