#include "Var.h"
#include "iofunc.h"
Var b;
Var c;
Var d;
Var f;
Var t;
Var lol(Var str)
{}


int main() {
Var temp[] = {0, 1, 2, 3, 4};
b.setValue(temp, 5);
writeln(b);
b[1].setValue(Var("hel"));
b[2].setValue(9.8);
b[3].setValue(999);
Var temp3[] = {1, 1, 1};
Var temp2[] = {8, "9", 7.0};
f.setValue(temp2, 3);
f[2].setValue(temp3 , 3);
b[4].setValue(f);
writeln(b);

return 1;}
