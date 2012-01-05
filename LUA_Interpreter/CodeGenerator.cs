using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LUA_Interpreter
{
    public struct FuncObject
    {
        public string fname;
        public int args_count;
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
        private List<string> global_vars;
        private bool in_func;
        private Dictionary<string, int> function_list;

        public CodeGenerator(ASTree tree)
        {
            m_tree = tree;
            global_vars = new List<string>();
            function_list = new Dictionary<string, int>();
            in_func = false;
            InitStandardFunc();
        }

        private void InitStandardFunc()
        {
            function_list.Add("printf", -1);
            function_list.Add("write", 1);
        }

        public void Generate(string path)
        {
            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine("#include \"Var.h\"\n");
            global_vars.Clear();
            short stage = 0;
            for (stage = 0; stage < LAST_STAGE+1; stage++)
            {
                if(stage == 1) sw.WriteLine("\nint main() {");

                Root = m_tree.Root;
                TransformTree(sw, ref stage);

                if (stage == 0) PrintGlobalVars(sw);
                if (stage == LAST_STAGE) sw.WriteLine("return 1;}");
            }
            sw.Close();
        }

        private void PrintGlobalVars(StreamWriter sw)
        {
            for (int c = 0; c < global_vars.Count; c++)
            {
                sw.WriteLine("Var {0};", global_vars[c]);
            }
        }

        public void TransformTree(StreamWriter sw, ref short stage)
        {
            int level = 1;
            TransformChild(Root, ref sw, ref stage);
        }

        public void TransformChild(TreeNode<Node> par, ref StreamWriter sw, ref short stage)
        {
            if(PrintStringValue(par, ref sw, ref stage))
                for (int i = 0; i < par.Children.Count; i++)
                {
                    TransformChild(par.Children[i], ref sw, ref stage);
                }
        }

        private string IntToStrC(double num)
        {
            string str = String.Format("{0}", num);
            str = str.Replace(',', '.');
            return str;
        }

        public void GetAssignElem(TreeNode<Node> tn, ref StreamWriter sw, ref short stage)
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
                    if (en.Children[c].Value.type == FUNCTION_CALL)
                    {
                        BuildFunction(en.Children[c], ref sw);
                    }
                    else if ((en.Children[c].Value.type == STRING) || (en.Children[c].Value.type == Id))
                        m_exp.rightArg = en.Children[c].Value.dataS;
                    else if (en.Children[c].Value.type == NUMBER)
                        m_exp.rightArg = IntToStrC(en.Children[c].Value.dataN);
                    else
                    {
                        //System.Diagnostics.Debug.WriteLine(String.Format("{0}", en.Children[c].Value.type));
                        int level = 0;
                        int arg_type = -1;
                        m_exp.rightArg = BuildExpressionString(en.Children[0], level, ref arg_type);

                    }
                }
            }
        }

        public string BuildExpressionString(TreeNode<Node> tn, int level, ref int arg_type)
        {
            level++;
            string str = "";
            string sign = GetSign(tn.Value.type);

            if (sign != "\0")
            {
                if (tn.Children.Count > 1)
                {
                    if(level > 1) str += '(';
                    str += CheckOperNode(tn.Children[0], level, ref arg_type);
                    str += sign;
                    str += CheckOperNode(tn.Children[1], level, ref arg_type);
                    if (level > 1) str += ')';
                }
                else if (tn.Children.Count > 0)
                {
                    str += sign;
                    str += CheckOperNode(tn.Children[0], level, ref arg_type);
                }
                else
                {
                    str += sign;
                }
            }

            return str;
        }

        public string CheckOperNode(TreeNode<Node> tn, int level, ref int arg_type)
        {
            if (tn.Value.type == NUMBER)
            {
                if (arg_type == -1) arg_type = NUMBER;
                return IntToStrC(tn.Value.dataN);
            }
            else if (tn.Value.type == FUNCTION_CALL)
            {
                StreamWriter sw = new StreamWriter("expression.log");
                return BuildFunction(tn, ref sw);
            }
            else if ((tn.Value.type == STRING) || (tn.Value.type == Id))
            {
                if (arg_type == -1) arg_type = STRING;
                return tn.Value.dataS;
            }

            return BuildExpressionString(tn, level, ref arg_type);
        }

        public string GetSign(int op_code)
        {
            switch (op_code)
            {
                case OperatorDiv:
                    return "/";
                    break;
                case OperatorMinus:
                    return "-";
                    break;
                case OperatorPlus:
                    return "+";
                    break;
                case OperatorMod:
                    return "%";
                    break;
                case OperatorStepen:
                    return "^";
                    break;
                case OperatorConcatenacii:
                    return "|";
                    break;
                case OperatorMul:
                    return "*";
                    break;
                case EQUAL:
                    return "==";
                    break;
                case LESS:
                    return "<";
                    break;
                case GREATER:
                    return ">";
                    break;
                case LESS_EQUAL:
                    return "<=";
                    break;
                case GREATER_EQUAL:
                    return ">=";
                    break;
            }
            System.Diagnostics.Debug.WriteLine(String.Format("{0}", op_code));
            return "\0";
        }

        private FuncCallMask ReplaceFuncName(string lua_fn)
        {
            FuncCallMask fc_mask = new FuncCallMask();

            // Стандартные функции
            if (lua_fn == "print") return new FuncCallMask("printf", "*", true);
            if (lua_fn == "write") return new FuncCallMask("printf", "u", true);
            //-----------------------------------------------------------------

            return new FuncCallMask(lua_fn, "*", false);
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

        public string BuildFunction(TreeNode<Node> tn, ref StreamWriter sw, bool print = false)
        {
            FuncCallMask fc_mask = ReplaceFuncName(tn.Children[0].Value.dataS);
            string expr = "";
            expr += String.Format("{0}(", fc_mask.funcname);
            string arg_str = "";

            int cur_type = -1; // Ожидаемый тип аргумента. -1 - любой тип
            TreeNode<Node> argn = null;
            List<int> func_arg_types = new List<int>();
            if (tn.Children[1].Children.Count > 0)
            {
                argn = tn.Children[1].Children[0];

                if (!function_list.ContainsKey(fc_mask.funcname))
                {
                    Console.WriteLine("Function {0} is undefined!", fc_mask.funcname);
                    return "";
                    //throw new Exception();
                }

                if ((function_list[fc_mask.funcname] != -1) 
                   && (argn.Children.Count < function_list[fc_mask.funcname]))
                {
                    Console.WriteLine("Function {0} requires {1} arguments,", fc_mask.funcname
                                        , function_list[fc_mask.funcname]);
                    Console.WriteLine("but {0} arguments taken!", argn.Children.Count);
                    throw new Exception();
                }

                for (int c = 0, mask_pos = 0; c < argn.Children.Count; c++)
                {
                    /*cur_type = GetCurTypeID(fc_mask.mask, ref mask_pos);

                    if (((cur_type != -1) && (cur_type != argn.Children[c].Value.type))
                        || (mask_pos >= fc_mask.mask.Length))
                        throw new Exception("Invalid function argument!");*/

                    if (arg_str != "")
                    {
                        arg_str += ", ";
                    }
                    if (argn.Children[c].Value.type == NUMBER)
                    {
                        arg_str += IntToStrC(argn.Children[c].Value.dataN);
                        func_arg_types.Add(NUMBER);
                    }
                    else if (argn.Children[c].Value.type == FUNCTION_CALL)
                    {
                        arg_str += BuildFunction(argn.Children[c], ref sw);
                    }
                    else if ((argn.Children[c].Value.type == STRING) || (argn.Children[c].Value.type == Id))
                    {
                        arg_str += argn.Children[c].Value.dataS;

                        if (argn.Children[c].Value.type == Id)
                            arg_str += ".toString()";

                        func_arg_types.Add(STRING);
                    }
                    else
                    {
                        int level = 0;
                        int arg_type = -1;
                        arg_str += BuildExpressionString(argn.Children[c], level, ref arg_type);
                        func_arg_types.Add(arg_type);
                    }
                }
                if (fc_mask.mask_string_req)
                    arg_str = BuildMaskString(func_arg_types) + arg_str;

                expr += arg_str;
            }

            expr += ")";

            if(print)
                sw.Write("{0}", expr);

            return expr;
        }

        private string BuildMaskString(List<int> func_arg_types)
        {
            string str = "\"";
            for (int i = 0; i < func_arg_types.Count; i++)
            {
                if (func_arg_types[i] == NUMBER)
                    str += "%d";
                if (func_arg_types[i] == STRING)
                    str += "%s";
            }
            str += "\", ";

            return str;
        }

        /*       
        public const int OperatorDiv = 8;
        public const int OperatorMinus = 9;
        public const int OperatorPlus = 10;
        public const int OperatorMod = 11;
        public const int OperatorStepen = 12;
        public const int OperatorConcatenacii = 13;
        public const int OperatorMul = 14; */

        private void PrintChunk(TreeNode<Node> tn, ref StreamWriter sw)
        {
            sw.WriteLine("{");
            for (short stage = 0; stage < LAST_STAGE + 1; stage++ )
                TransformChild(tn, ref sw, ref stage);

            sw.WriteLine("}");
        }

        private void PrintWhile(TreeNode<Node> tn, ref StreamWriter sw)
        {
            TreeNode<Node> en = tn.Children[0];
            TreeNode<Node> chn = tn.Children[1];

            string expr = "while(";

            if (en.Children[0].Value.type == FUNCTION_CALL)
            {
                BuildFunction(en.Children[0], ref sw);
            }
            else if ((en.Children[0].Value.type == STRING) || (en.Children[0].Value.type == Id))
                if (en.Children[0].Value.dataS != "nil") expr += "true";
                else expr += "false";
            else if (en.Children[0].Value.type == NUMBER)
                expr += "true";
            else
            {
                //System.Diagnostics.Debug.WriteLine(String.Format("{0}", en.Children[c].Value.type));
                int level = 0;
                int arg_type = -1;
                expr += BuildExpressionString(en, level, ref arg_type);

            }
            expr += ")";
            sw.WriteLine(expr);

            PrintChunk(chn, ref sw);
        }

        private void PrintIf(TreeNode<Node> tn, ref StreamWriter sw)
        {
            TreeNode<Node> en = tn.Children[0].Children[0];
            TreeNode<Node> chn = tn.Children[1];

            string expr = "if(";

            if (en.Children[0].Value.type == FUNCTION_CALL)
            {
                BuildFunction(en.Children[0], ref sw);
            }
            else if ((en.Children[0].Value.type == STRING) || (en.Children[0].Value.type == Id))
                if (en.Children[0].Value.dataS != "nil") expr += "true";
                else expr += "false";
            else if (en.Children[0].Value.type == NUMBER)
                expr += "true";
            else
            {
                int level = 0;
                int arg_type = -1;
                expr += BuildExpressionString(en, level, ref arg_type);

            }
            expr += ")";
            sw.WriteLine(expr);

            PrintChunk(chn, ref sw);

            if (tn.Children.Count > 3)
            {
                for (int ch_id = 3; ch_id < tn.Children.Count; ch_id++)
                {
                    if (tn.Children[ch_id].Value.type == ELSE)
                        sw.WriteLine("else");
                    else
                        sw.WriteLine("elseif");
                    PrintChunk(tn.Children[ch_id].Children[0], ref sw);
                }
            }
        }

        private void PrintFor(TreeNode<Node> tn, ref StreamWriter sw)
        {
            string expr = String.Format(@"for({0}.setValue({1}); ", tn.Children[0].Value.dataS
                                        , tn.Children[1].Value.dataN);

            if (tn.Children[2].Value.type == FUNCTION_CALL)
            {
                BuildFunction(tn.Children[2], ref sw);
            }
            else if ((tn.Children[2].Value.type == STRING) || (tn.Children[2].Value.type == Id))
                if (tn.Children[2].Value.dataS != "nil") expr += "true";
                else expr += "false";
            else if (tn.Children[2].Value.type == NUMBER)
                expr += "true";
            else
            {
                int level = 0;
                int arg_type = -1;
                expr += BuildExpressionString(tn.Children[2], level, ref arg_type);

            }

            expr += String.Format("; {0} = {0} + {1}", tn.Children[0].Value.dataS
                                    , tn.Children[3].Children[0].Value.dataN);
            expr += ")";
            sw.WriteLine(expr);

            PrintChunk(tn.Children[4], ref sw);
        }

        public void PrintRep(TreeNode<Node> tn, ref StreamWriter sw)
        {
            sw.WriteLine("do");
            PrintChunk(tn.Children[0], ref sw);

            string expr = "";
            if (tn.Children[1].Children[0].Value.type == STRING)
                if (tn.Children[1].Children[0].Value.dataS != "nil") expr += "true";
                else expr += "false";
            else if (tn.Children[1].Children[0].Value.type == NUMBER)
                expr += "true";
            else
            {
                int level = 0;
                int arg_type = -1;
                expr += BuildExpressionString(tn.Children[1].Children[0], level, ref arg_type);

            }
            sw.WriteLine("while({0});", expr);
        }

        public void BuildFunctionInit(TreeNode<Node> tn, ref StreamWriter sw)
        {
            in_func = true;
            string fname = tn.Children[0].Children[0].Value.dataS;
            int args_count = tn.Children[1].Children.Count;
            function_list.Add(fname, args_count);

            string expr = "Var ";
            expr += tn.Children[0].Children[0].Value.dataS + "(";

            string args = "";
            if (tn.Children[1].Value.type == PARLIST)
            {
                for (int c = 0; c < tn.Children[1].Children.Count; c++)
                {
                    if (args != "")
                        args += ", ";

                    args += "Var " + tn.Children[1].Children[c].Value.dataS;
                }
            }
            expr += args + ")";
            sw.WriteLine("{0}", expr);

            if ((tn.Children.Count >= 2))
                if (tn.Children[1].Value.type == CHUNK)
                    PrintChunk(tn.Children[1], ref sw);
                else if ((tn.Children.Count > 2) && (tn.Children[2].Value.type == CHUNK))
                    PrintChunk(tn.Children[2], ref sw);
            else
                sw.WriteLine("{}");
            sw.WriteLine();
            in_func = false;
        }

        private void PrintReturn(TreeNode<Node> tn, ref StreamWriter sw)
        {
            sw.Write("return ");
            if ((tn.Children[0].Children[0].Value.type == STRING) || (tn.Children[0].Children[0].Value.type == Id))
                sw.Write(tn.Children[0].Children[0].Value.dataS);
            else if(tn.Children[0].Children[0].Value.type == NUMBER)
                sw.Write(String.Format("{0}", tn.Children[0].Children[0].Value.dataN));
            else
            {
                int level = 0;
                int arg_type = -1;
                sw.Write(BuildExpressionString(tn.Children[1].Children[0], level, ref arg_type));
            }
            sw.WriteLine(";");
        }

        public bool PrintStringValue(TreeNode<Node> tn, ref StreamWriter sw, ref short stage)
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
                        GetAssignElem(tn, ref sw, ref stage);
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
                    {
                        if (stage == 0)
                        {
                            BuildFunctionInit(tn, ref sw);
                        }
                        return false;
                    }
                    break;
                case FUNCNAME:
                    break;
                case PARLIST:
                    break;
                case DO:
                    break;
                case WHILE:
                    {
                        if (stage == 1)
                        {
                            PrintWhile(tn, ref sw);
                            return false;
                        }
                    }
                    break;
                case FUNCTION_CALL:
                    {
                        if (stage == 1)
                        {
                            //sw.WriteLine("FUNCTION_CALL");
                            BuildFunction(tn, ref sw, true);
                            sw.WriteLine(";");
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
                    {
                        if (stage == 1)
                        {
                            PrintRep(tn, ref sw);
                            return false;
                        }
                    }
                    break;
                case UNTIL_COND:
                    break;
                case IF:
                    {
                        if (stage == 1)
                        {
                            PrintIf(tn, ref sw);
                            return false;
                        }
                    }
                    break;
                case IF_COND:
                    break;
                case FOR:
                    {
                        if (stage == 1)
                        {
                            PrintFor(tn, ref sw);
                            return false;
                        }
                    }
                    break;
                case STEP:
                    break;
                case ELSEIF_LIST:
                    break;
                case ELSE:
                    break;
                case RETURN:
                    {
                        if ((in_func) && (stage == 1))
                        {
                            PrintReturn(tn, ref sw);
                        }
                        return false;
                    }
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
