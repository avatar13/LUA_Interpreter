#include "Var.h"

Var b;
Var c;
Var d;
Var f;
Var t;
Var lol(Var str)
{}


int main() {
b.setValue(2);
c.setValue(123.456);
d.setValue(12);
f.setValue(0.789);
t.setValue((5+(5*5))-10);
printf("%s", t.toString());
return 1;}
