@echo off
rem 2>report.txt означает что 2 - поток stderr перенаправлен в report.txt
rem LUA_Interpreter.exe test.txt 2>report.txt OK
rem tests/array.txt пока не тестил из за комментов, с ними отдельный гемор в лексическом анализаторе чисто, синтаксиса они не касаются
rem tests/buble_sort.txt ошибка в коде, Определение функции не может быть без аргуметов (хотябы пустых) 4.0 version

rem (видимо мы для разных версий пишем=) я для 4.0, там где мы смотрели грамматику 5.1. Честно скажу для 4.0 писать грамматику проще.)
rem LUA_Interpreter.exe tests/cycles.txt 2>report.txt OK
rem LUA_Interpreter.exe tests/if.txt 2>report.txt ошибка твоя после if ..., должен быть then ...
rem LUA_Interpreter.exe tests/empty_func.txt 2>report.txt OK
rem LUA_Interpreter.exe tests/math.txt 2>report.txt комменты
rem LUA_Interpreter.exe tests/math_priority.txt комменты
rem LUA_Interpreter.exe tests/print.txt 2>report.txt OK
rem LUA_Interpreter.exe tests/simp_func.txt 2>report.txt OK
rem LUA_Interpreter.exe tests/space_in_code.txt 2>report.txt OK
rem LUA_Interpreter.exe tests/string_oper.txt 2>report.txt мой косяк с определением строк исправлю но в целом вроде для конкатенации строк ".." используют
rem а если в таком виде то мы str1 and str2 преобразуем в числа и складываем=)))
rem LUA_Interpreter.exe tests/table.txt 2>report.txt тут не понятно '' - для символов или строк, 
rem кароче так и не понял пошел сериал смотреть=) а для чего же тогда ""??? сам не знаю тут как в c# или 'abc' and "abc" - строки????
rem На будущее сделай в 1 бат тестовике, так удобнее проверять или сюда добавь


