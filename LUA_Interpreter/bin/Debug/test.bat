@echo off
rem 2>report.txt �������� ��� 2 - ����� stderr ������������� � report.txt
rem LUA_Interpreter.exe test.txt 2>report.txt OK
rem tests/array.txt ���� �� ������ �� �� ���������, � ���� ��������� ����� � ����������� ����������� �����, ���������� ��� �� ��������
rem tests/buble_sort.txt ������ � ����, ����������� ������� �� ����� ���� ��� ��������� (������ ������) 4.0 version

rem (������ �� ��� ������ ������ �����=) � ��� 4.0, ��� ��� �� �������� ���������� 5.1. ������ ����� ��� 4.0 ������ ���������� �����.)
rem LUA_Interpreter.exe tests/cycles.txt 2>report.txt OK
rem LUA_Interpreter.exe tests/if.txt 2>report.txt ������ ���� ����� if ..., ������ ���� then ...
rem LUA_Interpreter.exe tests/empty_func.txt 2>report.txt OK
rem LUA_Interpreter.exe tests/math.txt 2>report.txt ��������
rem LUA_Interpreter.exe tests/math_priority.txt ��������
rem LUA_Interpreter.exe tests/print.txt 2>report.txt OK
rem LUA_Interpreter.exe tests/simp_func.txt 2>report.txt OK
rem LUA_Interpreter.exe tests/space_in_code.txt 2>report.txt OK
rem LUA_Interpreter.exe tests/string_oper.txt 2>report.txt ��� ����� � ������������ ����� �������� �� � ����� ����� ��� ������������ ����� ".." ����������
rem � ���� � ����� ���� �� �� str1 and str2 ����������� � ����� � ����������=)))
rem LUA_Interpreter.exe tests/table.txt 2>report.txt ��� �� ������� '' - ��� �������� ��� �����, 
rem ������ ��� � �� ����� ����� ������ ��������=) � ��� ���� �� ����� ""??? ��� �� ���� ��� ��� � c# ��� 'abc' and "abc" - ������????
rem �� ������� ������ � 1 ��� ���������, ��� ������� ��������� ��� ���� ������


