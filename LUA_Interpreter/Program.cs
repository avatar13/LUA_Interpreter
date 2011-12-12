﻿#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUA_Interpreter;
using LuaLex;
using LuaSyntax;

namespace LUA_Interpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            IdentiferTable table = new IdentiferTable();
            ASTree tree = new ASTree();
            Parser parser = new Parser(table, tree);
            
            
            System.IO.FileStream fs;
            if (args.Length > 0)                
                fs = new System.IO.FileStream(args[0], System.IO.FileMode.Open);
            else
                return;
            
            parser.Scanner = new Scanner(fs);
#if DEBUG            
            Console.WriteLine("Тест {0} прошел:{1}", args[0], parser.Parse());
            table.PrintTable();
            Console.WriteLine("---------------------------------------------------");
#endif
        }
    }
}
