#include "Var.h"

Var lol(str)
{}

Var b;
Var c;
Var d;
Var f;
Var t;

int main() {
b.setValue(2);
c.setValue(123.456);
d.setValue(12);
f.setValue(0.789);
b.setValue("12");
t.setValue((5+(5*5))-10);
printf("%s", t.toString());
return 1;}
