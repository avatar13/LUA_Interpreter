#define DBG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LUA_Interpreter;
using LuaLex;
using LuaSyntax;
using System.Diagnostics;

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
            
            string programName = args[0].Substring(0, args[0].Length - 4);

            parser.Scanner = new Scanner(fs);
#if DBG
            Console.WriteLine("Тест {0} прошел:{1}", args[0], parser.Parse());
            table.PrintTable();
            Console.WriteLine("---------------------------------------------------");
            string str = args[0].Remove(args[0].Length - 4);
            tree.DrawTree(str.Insert(str.Length, "Tree.txt"));
#else
            if (!parser.Parse())
            {
                return;
            }
#endif
            //TODO : Сдесь сделать генерацию кода в файл bin/<programName>.cpp

            CodeGenerator codegen = new CodeGenerator(tree);
            codegen.Generate("prog.cpp");
            Process runner = new Process();
            //compiller.StartInfo.Arguments = "-emit-llvm -c n.c -o n.bc";            
            /*compiller.StartInfo.UseShellExecute = false;
            compiller.StartInfo.RedirectStandardInput = true;
            compiller.StartInfo.RedirectStandardError = true;
            compiller.StartInfo.RedirectStandardOutput = true;
            compiller.StartInfo.CreateNoWindow = true;*/
            runner.StartInfo.WorkingDirectory = "bin";
            runner.StartInfo.Arguments = programName;
            runner.StartInfo.FileName = "run.bat";
            runner.Start();
            runner.WaitForExit();
        }
    }
}
