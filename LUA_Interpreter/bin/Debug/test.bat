@echo off
LUA_Interpreter.exe test.txt 2>report.txt
LUA_Interpreter.exe tests/array.txt 2>tests/arrayReport.txt
rem tests/buble_sort.txt ошибка в коде, Определение функции не может быть без аргуметов (хотябы пустых) 4.0 version

rem (видимо мы для разных версий пишем=) я для 4.0, там где мы смотрели грамматику 5.1. Честно скажу для 4.0 писать грамматику проще.)
LUA_Interpreter.exe tests/cycles.txt 2>tests/cyclesReport.txt
rem LUA_Interpreter.exe tests/if.txt 2>report.txt ошибка твоя после if ..., должен быть then ...
LUA_Interpreter.exe tests/empty_func.txt 2>tests/empty_funcReport.txt
LUA_Interpreter.exe tests/math.txt 2>tests/mathReport.txt
LUA_Interpreter.exe tests/math_priority.txt 2>tests/math_priorityReport.txt
LUA_Interpreter.exe tests/print.txt 2>tests/printReport.txt
LUA_Interpreter.exe tests/simp_func.txt 2>tests/simp_funcReport.txt
LUA_Interpreter.exe tests/space_in_code.txt 2>tests/space_in_codeReport.txt
rem LUA_Interpreter.exe tests/string_oper.txt 2>report.txt мой косяк с определением строк исправлю но в целом вроде для конкатенации строк ".." используют
rem а если в таком виде то мы str1 and str2 преобразуем в числа и складываем=)))
rem LUA_Interpreter.exe tests/table.txt 2>report.txt тут не понятно '' - для символов или строк, 
rem кароче так и не понял пошел сериал смотреть=) а для чего же тогда ""??? сам не знаю тут как в c# или 'abc' and "abc" - строки????
rem На будущее сделай в 1 бат тестовике, так удобнее проверять или сюда добавь

rem LUA_Interpreter.exe tests/elseif.txt 2>tests/elseif.txt там был файл с отчетом по ошибке
LUA_Interpreter.exe tests/few_ops_in_line.txt 2>tests/few_ops_in_lineReport.txt
LUA_Interpreter.exe tests/function_as_arg.txt 2>tests/function_as_argReport.txt
LUA_Interpreter.exe tests/single_oper_block.txt 2>tests/single_oper_blockReport.txt
LUA_Interpreter.exe tests/value_in_args.txt 2>tests/value_in_argsReport.txt
LUA_Interpreter.exe tests/exp.txt 2>tests/expReport.txt

