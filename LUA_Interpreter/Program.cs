using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUA_Interpreter;

namespace LUA_Interpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser parser = new Parser();

            System.IO.TextReader reader;
            if (args.Length > 0)
                reader = new System.IO.StreamReader(args[0]);
            else
                reader = System.Console.In;
            
            parser.Scanner = new Lexer(reader);
                        
            parser.Parse();
        }
    }
}
