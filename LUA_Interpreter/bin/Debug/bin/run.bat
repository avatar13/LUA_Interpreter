@echo off
llvm-c++.exe -emit-llvm -c %1.cpp -o %1.bc
lli.exe %1.bc
del %1.bc
Pause