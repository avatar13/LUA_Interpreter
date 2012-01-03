@echo off
llvm-gcc.exe -emit-llvm -c %1.c -o %1.bc
lli.exe %1.bc
del %1.bc
Pause