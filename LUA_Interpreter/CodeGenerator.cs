﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LUA_Interpreter
{
    struct FuncObject
    {
        string fname;
        int returnType;
        string[] args;
        object[] ops;
    };

    public struct Expression
    {
        public int oper_code;
        public List<string> leftArg;
        public string rightArg;
        public string result;
        public int lvl;
    };

    public struct SimpleOper
    {
        int id, lvl;
    };

    public struct FuncCallMask
    {
        /* Шаблон:
         s - строка
         n - число
         u - любой тип
         l - любое количество строк
         r - любое количество чисел
         * - любое количество произвольных аргументов
         */
        public string funcname;
        public bool mask_string_req; //mask_string_req - строка с подстановкой аргументов(например, в printf)
        public string mask;

        public FuncCallMask(string name, string templ, bool mstreq)
        {
            funcname = name;
            mask = templ;
            mask_string_req = mstreq;
        }
    }

    class CodeGenerator
    {
        public const int STAT = 0;
        public const int VARLIST = 1;
        public const int Id = 2;
        public const int NUMBER = 3;
        public const int ASSIGN = 4;
        public const int EXPLIST = 5;
        public const int CHUNK = 6;
        public const int EXP = 7;
        public const int OperatorDiv = 8;
        public const int OperatorMinus = 9;
        public const int OperatorPlus = 10;
        public const int OperatorMod = 11;
        public const int OperatorStepen = 12;
        public const int OperatorConcatenacii = 13;
        public const int OperatorMul = 14;
        public const int STRING = 15;
        public const int FUNCTION = 16;
        public const int FUNCNAME = 17;
        public const int PARLIST = 18;
        public const int DO = 19;
        public const int WHILE = 20;
        public const int FUNCTION_CALL = 21;
        public const int ARGS = 22;
        public const int ASSIGN_LOCAL = 23;
        public const int BREAK = 24;
        public const int REPEAT = 25;
        public const int UNTIL_COND = 26;
        public const int IF = 27;
        public const int IF_COND = 28;
        public const int FOR = 29;
        public const int STEP = 30;
        public const int ELSEIF_LIST = 31;
        public const int ELSE = 32;
        public const int RETURN = 33;
        public const int TABLE_CONSTRUCTOR = 34;
        public const int GREATER = 35;
        public const int GREATER_EQUAL = 36;
        public const int LESS = 37;
        public const int LESS_EQUAL = 38;
        public const int EQUAL = 39;
        public const int NOT_EQUAL = 40;
        public const int AND = 41;
        public const int OR = 42;
        public const int NOT = 43;
        public const int UPVALUE = 44;
        public const int FOR_IN = 45;
        public const int Id_Up = 46;
        public const int FUNCNAME_UP = 47;
        public const int FUNCNAME_IMPLICIT = 48;
        public const int FUNCTION_CALL_IMPLICIT = 49;
        public const int LAST_STAGE = 1;

        public Tree<Node> Root;

        private ASTree m_tree;
        private bool mainFunc; // Показыве
        private FuncObject curSubFunc;
        private Expression m_exp;
        TreeNode<Node> cur_node;
        private short stage;
        private List<string> global_vars;

        public CodeGenerator(ASTree tree)
        {
            m_tree = tree;
            stage = 0;
            global_vars = new List<string>();
        }

        public void Generate(string path)
        {
            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine("#include \"Var.h\"\n");
            global_vars.Clear();
            for (stage = 0; stage < LAST_STAGE+1; stage++)
            {
                if(stage == 1) sw.WriteLine("\nint main() {");

                Root = m_tree.Root;
                TransformTree(sw);

                if (stage == 0) PrintGlobalVars(sw);
                if (stage == LAST_STAGE) sw.WriteLine("return 1;}");
            }
            sw.Close();
        }

        private void PrintGlobalVars(StreamWriter sw)
        {
            for (int c = 0; c < global_vars.Count; c++)
            {
                sw.WriteLine("Var {0}();", global_vars[c]);
            }
        }

        public void TransformTree(StreamWriter sw)
        {
            int level = 1;
            TransformChild(Root, ref sw, level);
        }

        public void TransformChild(TreeNode<Node> par, ref StreamWriter sw, int level)
        {
            if(PrintStringValue(par, level, ref sw))
                for (int i = 0; i < par.Children.Count; i++)
                {
                    TransformChild(par.Children[i], ref sw, level + 1);
                }
        }

        private string IntToStrC(double num)
        {
            string str = String.Format("{0}", num);
            str = str.Replace(',', '.');
            return str;
        }

        public void GetAssignElem(TreeNode<Node> tn, ref StreamWriter sw)
        {
            TreeNode<Node> vn = tn.Children[0];
            TreeNode<Node> en = tn.Children[1];
            if (stage == 0)
            {
                for (int c = 0; c < vn.Children.Count; c++)
                {
                    if (!global_vars.Contains(vn.Children[c].Value.dataS))
                    {
                        global_vars.Add(vn.Children[c].Value.dataS);
                    }
                }
            }
            else if (stage == 1)
            {
                for (int c = 0; c < vn.Children.Count; c++)
                {
                    m_exp.leftArg.Add(vn.Children[c].Value.dataS);
                }
                for (int c = 0; c < en.Children.Count; c++)
                {
                    if (en.Children[c].Value.type == STRING)
                        m_exp.rightArg = en.Children[c].Value.dataS;
                    else if (en.Children[c].Value.type == NUMBER)
                        m_exp.rightArg = IntToStrC(en.Children[c].Value.dataN);
                    else
                    {
                        //System.Diagnostics.Debug.WriteLine(String.Format("{0}", en.Children[c].Value.type));
                        int level = 0;
                        m_exp.rightArg = BuildExpressionString(en.Children[0], level);
                    }
                }
            }
        }

        public string BuildExpressionString(TreeNode<Node> tn, int level)
        {
            level++;
            string str = "";
            char sign = GetSign(tn.Value.type);

            if (sign != '\0')
            {
                if (tn.Children.Count > 1)
                {
                    if(level > 1) str += '(';
                    str += CheckOperNode(tn.Children[0], level);
                    str += sign;
                    str += CheckOperNode(tn.Children[1], level);
                    if (level > 1) str += ')';
                }
                else if (tn.Children.Count > 0)
                {
                    str += sign;
                    str += CheckOperNode(tn.Children[0], level);
                }
                else
                {
                    str += sign;
                }
            }

            return str;
        }

        public string CheckOperNode(TreeNode<Node> tn, int level)
        {
            if(tn.Value.type == NUMBER)
                return IntToStrC(tn.Value.dataN);
            else if (tn.Value.type == STRING)
                return tn.Value.dataS;

            return BuildExpressionString(tn, level);
        }

        public char GetSign(int op_code)
        {
            switch (op_code)
            {
                case OperatorDiv:
                    return '/';
                    break;
                case OperatorMinus:
                    return '-';
                    break;
                case OperatorPlus:
                    return '+';
                    break;
                case OperatorMod:
                    return '%';
                    break;
                case OperatorStepen:
                    return '^';
                    break;
                case OperatorConcatenacii:
                    return '|';
                    break;
                case OperatorMul:
                    return '*';
                    break;
            }
            System.Diagnostics.Debug.WriteLine(String.Format("{0}", op_code));
            return '\0';
        }

        private FuncCallMask ReplaceFuncName(string lua_fn)
        {
            FuncCallMask fc_mask = new FuncCallMask();

            // Стандартные функции
            if (lua_fn == "print") return new FuncCallMask("printf", "*", true);
            //-----------------------------------------------------------------

            return new FuncCallMask(lua_fn, null, false);
        }

        private int GetCurTypeID(string mask, ref int pos)
        {
            char mask_el = mask[pos];
            switch (mask_el)
            {
                case 's':
                    {
                        return STRING;
                        pos++;
                    }
                    break;
                case 'n':
                    {
                        return NUMBER;
                        pos++;
                    }
                    break;
                case 'u':
                    return -1;
                    break;
                case 'l':
                    return STRING;
                    break;
                case 'r':
                    return NUMBER;
                    break;
                case '*':
                    return -1;
                    break;
            }

            return -1;
        }

        public void BuildFunction(TreeNode<Node> tn, ref StreamWriter sw)
        {
            FuncCallMask fc_mask = ReplaceFuncName(tn.Children[0].Value.dataS);
            sw.Write("{0}(", fc_mask.funcname);
            string arg_str = "";

            int cur_type = -1; // Ожидаемый тип аргумента. -1 - любой тип
            TreeNode<Node> argn = null;
            if (tn.Children[1].Children.Count > 0)
            {
                argn = tn.Children[1].Children[0];

                for (int c = 0, mask_pos = 0; c < argn.Children.Count; c++)
                {
                    //cur_type = GetCurTypeID(fc_mask.mask, ref mask_pos);

                    /*if (cur_type != argn.Children[c].Value.type)
                        throw new Exception("Invalid function argument!");*/

                    if (arg_str != "")
                    {
                        arg_str += ", ";
                    }
                    if (argn.Children[c].Value.type == NUMBER)
                    {
                        arg_str += IntToStrC(argn.Children[c].Value.dataN);
                    }
                    else if (argn.Children[c].Value.type == STRING)
                    {
                        arg_str += argn.Children[c].Value.dataS;
                    }
                    else
                    {
                        int level = 0;
                        arg_str += BuildExpressionString(argn.Children[c], level);
                    }
                }
            }
            sw.WriteLine("{0});", arg_str);
        }

        /*       
        public const int OperatorDiv = 8;
        public const int OperatorMinus = 9;
        public const int OperatorPlus = 10;
        public const int OperatorMod = 11;
        public const int OperatorStepen = 12;
        public const int OperatorConcatenacii = 13;
        public const int OperatorMul = 14; */

        public bool PrintStringValue(TreeNode<Node> tn, int lvl, ref StreamWriter sw)
        {
            Node n = tn.Value;
            switch (n.type)
            {
                case STAT:
                    break;
                case VARLIST:
                    //sw.WriteLine("VARLIST");
                    break;
                case Id:
                    //sw.WriteLine("Id");
                    break;
                case NUMBER:
                    //sw.WriteLine("NUMBER {0}", n.dataN);
                    break;
                case ASSIGN:
                    {
                        //sw.WriteLine("ASSIGN");
                        m_exp = new Expression();
                        m_exp.leftArg = new List<string>();
                        GetAssignElem(tn, ref sw);
                        if (stage == 1)
                            for (int c = 0; c < m_exp.leftArg.Count; c++)
                            {
                                sw.WriteLine("{0}.setValue({1});", m_exp.leftArg[c], m_exp.rightArg);
                            }
                        return false;
                    }
                    break;
                case EXPLIST:
                    break;
                case CHUNK:
                    break;
                case STRING:
                    break;
                case OperatorMinus:
                    break;
                case OperatorPlus:
                    break;
                case OperatorMod:
                    break;
                case OperatorDiv:
                    break;
                case OperatorMul:
                    break;
                case EXP:
                    break;
                case FUNCTION:
                    break;
                case FUNCNAME:
                    break;
                case PARLIST:
                    break;
                case DO:
                    break;
                case WHILE:
                    break;
                case FUNCTION_CALL:
                    {
                        if (stage == 1)
                        {
                            //sw.WriteLine("FUNCTION_CALL");
                            BuildFunction(tn, ref sw);
                            return false;
                        }
                    }
                    break;
                case ARGS:
                    break;
                case ASSIGN_LOCAL:
                    break;
                case BREAK:
                    break;
                case REPEAT:
                    break;
                case UNTIL_COND:
                    break;
                case IF:
                    break;
                case IF_COND:
                    break;
                case FOR:
                    break;
                case STEP:
                    break;
                case ELSEIF_LIST:
                    break;
                case ELSE:
                    break;
                case RETURN:
                    break;
                case TABLE_CONSTRUCTOR:
                    break;
                case GREATER:
                    break;
                case GREATER_EQUAL:
                    break;
                case LESS:
                    break;
                case LESS_EQUAL:
                    break;
                case EQUAL:
                    break;
                case NOT_EQUAL:
                    break;
                case AND:
                    break;
                case OR:
                    break;
                case NOT:
                    break;
                case UPVALUE:
                    break;
                case FOR_IN:
                    break;
                case Id_Up:
                    break;
                case FUNCNAME_UP:
                    break;
                case FUNCNAME_IMPLICIT:
                    break;
                case FUNCTION_CALL_IMPLICIT:
                    break;
            }
            return true;
        }
    }
}
