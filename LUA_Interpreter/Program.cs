#define DEBUG

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
            Parser parser = new Parser();
            
            System.IO.FileStream fs;
            if (args.Length > 0)                
                fs = new System.IO.FileStream(args[0], System.IO.FileMode.Open);
            else
                return;
            
            parser.Scanner = new Scanner(fs);
#if DEBUG            
            Console.WriteLine("Тест {0} прошел:{1}", args[0], parser.Parse());
#endif
        }
    }
}
