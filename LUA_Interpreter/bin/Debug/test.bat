@echo off
LUA_Interpreter.exe test.txt 2>report.txt
LUA_Interpreter.exe tests/array.txt 2>tests/arrayReport.txt
rem tests/buble_sort.txt ������ � ����, ����������� ������� �� ����� ���� ��� ��������� (������ ������) 4.0 version

rem (������ �� ��� ������ ������ �����=) � ��� 4.0, ��� ��� �� �������� ���������� 5.1. ������ ����� ��� 4.0 ������ ���������� �����.)
LUA_Interpreter.exe tests/cycles.txt 2>tests/cyclesReport.txt
rem LUA_Interpreter.exe tests/if.txt 2>report.txt ������ ���� ����� if ..., ������ ���� then ...
LUA_Interpreter.exe tests/empty_func.txt 2>tests/empty_funcReport.txt
LUA_Interpreter.exe tests/math.txt 2>tests/mathReport.txt
LUA_Interpreter.exe tests/math_priority.txt 2>tests/math_priorityReport.txt
LUA_Interpreter.exe tests/print.txt 2>tests/printReport.txt
LUA_Interpreter.exe tests/simp_func.txt 2>tests/simp_funcReport.txt
LUA_Interpreter.exe tests/space_in_code.txt 2>tests/space_in_codeReport.txt
rem LUA_Interpreter.exe tests/string_oper.txt 2>report.txt ��� ����� � ������������ ����� �������� �� � ����� ����� ��� ������������ ����� ".." ����������
rem � ���� � ����� ���� �� �� str1 and str2 ����������� � ����� � ����������=)))
rem LUA_Interpreter.exe tests/table.txt 2>report.txt ��� �� ������� '' - ��� �������� ��� �����, 
rem ������ ��� � �� ����� ����� ������ ��������=) � ��� ���� �� ����� ""??? ��� �� ���� ��� ��� � c# ��� 'abc' and "abc" - ������????
rem �� ������� ������ � 1 ��� ���������, ��� ������� ��������� ��� ���� ������


