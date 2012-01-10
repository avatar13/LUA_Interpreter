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

    public struct LocalVars
    {
        public int lvl;
        public string id; // Пример: len_sort_if_if
        public List<string> varlist;
    };

    public struct Expression
    {
        public int oper_code;
        public List<string> leftArg;
        public string rightArg;
        public string prefix, sub_params;
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
        private Dictionary<string, int> function_list, array_list;

        public CodeGenerator(ASTree tree)
        {
            m_tree = tree;
            global_vars = new List<string>();
            function_list = new Dictionary<string, int>();
            array_list = new Dictionary<string, int>();
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
            sw.WriteLine("#include \"iofunc.h\"\n");
            global_vars.Clear();
            short stage = 0;
            for (stage = 0; stage < LAST_STAGE+1; stage++)
            {
                if(stage == 1) sw.WriteLine("\nint main() {");

                Root = m_tree.Root;
                TransformTree(sw, ref stage);

                if (stage == LAST_STAGE) sw.WriteLine("return 1;}");
            }
            sw.Close();
        }

        private void PrintVars(StreamWriter sw, ref LocalVars nestinglvl)
        {
            for (int c = 0; c < nestinglvl.varlist.Count; c++)
            {
                sw.WriteLine("Var {0};", nestinglvl.varlist[c]);
            }
            nestinglvl.varlist.Clear();
        }

        public void TransformTree(StreamWriter sw, ref short stage)
        {
            int level = 1;
            LocalVars nestinglvl = new LocalVars();
            nestinglvl.id = ""; nestinglvl.lvl = 0; nestinglvl.varlist = new List<string>();
            TransformChild(Root, ref sw, ref stage, ref nestinglvl);
            PrintVars(sw, ref nestinglvl);
        }

        public void TransformSubTree(TreeNode<Node> par, ref StreamWriter sw, ref short stage, ref LocalVars nestinglvl)
        {
            nestinglvl.lvl = 0;
            nestinglvl.varlist = new List<string>();
            TransformChild(par, ref sw, ref stage, ref nestinglvl);
            PrintVars(sw, ref nestinglvl);
        }

        public void TransformChild(TreeNode<Node> par, ref StreamWriter sw, ref short stage, ref LocalVars nestinglvl)
        {
            if(PrintStringValue(par, ref sw, ref nestinglvl, ref stage))
                for (int i = 0; i < par.Children.Count; i++)
                {
                    TransformChild(par.Children[i], ref sw, ref stage, ref nestinglvl);
                }
        }

        private string IntToStrC(double num)
        {
            string str = String.Format("{0}", num);
            str = str.Replace(',', '.');
            return str;
        }

        private string BuildTableBlock(TreeNode<Node> tn, ref LocalVars nestinglvl)
        {
            string expr = "{";
            for (int i = 0; i < tn.Children.Count; i++ )
            {
                if (expr != "{")
                {
                    expr += ", ";
                }
                if (tn.Children[i].Value.type == NUMBER)
                    expr += String.Format("{0}", tn.Children[i].Value.dataN);
                else if (tn.Children[i].Value.type == STRING)
                    expr += tn.Children[i].Value.dataS;
                else if (tn.Children[i].Value.type == TABLE_CONSTRUCTOR)
                    expr += BuildTableBlock(tn.Children[i], ref nestinglvl);
                else
                {
                    int level = 0;
                    int arg_type = -1;
                    BuildExpressionString(tn.Children[i], level, ref nestinglvl, ref arg_type);
                }

            }
            string tmp = String.Format("{0}", expr + '}');
            return tmp;
        }

        public void GetAssignElem(TreeNode<Node> tn, ref StreamWriter sw, ref LocalVars nestinglvl, ref short stage)
        {
            TreeNode<Node> vn = tn.Children[0];
            TreeNode<Node> en = tn.Children[1];
            if (stage == 0)
            {
                for (int c = 0; c < vn.Children.Count; c++)
                {
                    string expr = vn.Children[c].Value.dataS + nestinglvl.id;
                    if (!nestinglvl.varlist.Contains(expr))
                    {
                        nestinglvl.varlist.Add(expr);
                    }
                }
            }
            else if (stage == 1)
            {
                for (int c = 0; c < vn.Children.Count; c++)
                {
                    if (vn.Children[c].Value.type == Id_Up)
                        m_exp.leftArg.Add(vn.Children[c].Children[0].Value.dataS);
                    else
                        m_exp.leftArg.Add(vn.Children[c].Value.dataS);
                }
                for (int c = 0; c < en.Children.Count; c++)
                {
                    if (en.Children[c].Value.type == FUNCTION_CALL)
                    {
                        BuildFunction(en.Children[c], ref sw, ref nestinglvl);
                    }
                    else if ((en.Children[c].Value.type == STRING) || (en.Children[c].Value.type == Id))
                        m_exp.rightArg = en.Children[c].Value.dataS;
                    else if (en.Children[c].Value.type == NUMBER)
                        m_exp.rightArg = IntToStrC(en.Children[c].Value.dataN);
                    else
                    {
                        TreeNode<Node> expr_node = en.Children[0];
                        if (expr_node.Value.type == TABLE_CONSTRUCTOR)
                        {
                            int elem_count = expr_node.Children.Count;
                            m_exp.prefix = String.Format("Var lua_temp[]={0};",
                                BuildTableBlock(expr_node, ref nestinglvl));
                            m_exp.rightArg = String.Format("lua_temp, {0}", elem_count);
                        }
                        else
                        {
                            //System.Diagnostics.Debug.WriteLine(String.Format("{0}", en.Children[c].Value.type));
                            int level = 0;
                            int arg_type = -1;
                            m_exp.rightArg = BuildExpressionString(en.Children[0], level, ref nestinglvl, ref arg_type);
                        }
                    }
                }
            }
        }

        public string BuildExpressionString(TreeNode<Node> tn, int level, ref LocalVars nestinglvl, ref int arg_type,
                                            string sub_appx = "")
        {
            level++;
            string str = "";
            string sign = GetSign(tn.Value.type);

            if (sign != "\0")
            {
                if (tn.Children.Count > 1)
                {
                    if(level > 1) str += '(';
                    str += CheckOperNode(tn.Children[0], level, ref nestinglvl, ref arg_type, sub_appx);
                    str += sign;
                    str += CheckOperNode(tn.Children[1], level, ref nestinglvl, ref arg_type);
                    if (level > 1) str += ')';
                }
                else if (tn.Children.Count > 0)
                {
                    str += sign;
                    str += CheckOperNode(tn.Children[0], level, ref nestinglvl, ref arg_type, sub_appx);
                }
                else
                {
                    str += sign;
                }
            }

            return str;
        }

        public string CheckOperNode(TreeNode<Node> tn, int level, ref LocalVars nestinglvl, ref int arg_type, 
                                    string sub_appx = "")
        {
            if (tn.Value.type == NUMBER)
            {
                if (arg_type == -1) arg_type = NUMBER;
                return IntToStrC(tn.Value.dataN);
            }
            else if (tn.Value.type == FUNCTION_CALL)
            {
                StreamWriter sw = new StreamWriter("expression.log");
                return BuildFunction(tn, ref sw, ref nestinglvl);
            }
            else if ((tn.Value.type == STRING) || (tn.Value.type == Id))
            {
                if (arg_type == -1) arg_type = STRING;
                if (tn.Value.type == Id)
                {
                    string expr = tn.Value.dataS;
                    expr += String.Format("{0}{1}", nestinglvl.id, sub_appx);
                    return expr + ".toInt()";
                }
                else
                    return tn.Value.dataS;
            }

            return BuildExpressionString(tn, level, ref nestinglvl, ref arg_type);
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
            if (lua_fn == "write") return new FuncCallMask("write", "u", false);
            if (lua_fn == "writeln") return new FuncCallMask("writeln", "u", false);
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

        public string BuildFunction(TreeNode<Node> tn, ref StreamWriter sw, ref LocalVars nestinglvl, bool print = false)
        {
            FuncCallMask fc_mask = ReplaceFuncName(tn.Children[0].Value.dataS);
            string expr = "";
            TreeNode<Node> argn = null;
            bool is_table = false;
            if (tn.Children[1].Value.type == TABLE_CONSTRUCTOR)
            {
                argn = tn.Children[1];
                string varname = fc_mask.funcname + nestinglvl.id;
                expr += String.Format("{0}", varname);
                is_table = true;
            }
            else
            {
                if (tn.Children[1].Children.Count > 0)
                    argn = tn.Children[1].Children[0];
                expr += String.Format("{0}(", fc_mask.funcname);
            }

            string arg_str = "";

            int cur_type = -1; // Ожидаемый тип аргумента. -1 - любой тип
            
            List<int> func_arg_types = new List<int>();
            if (tn.Children[1].Children.Count > 0)
            {

                if (!function_list.ContainsKey(fc_mask.funcname))
                {
                    if (!array_list.ContainsKey(fc_mask.funcname))
                    {
                        Console.WriteLine("Array or function {0} is undefined!", fc_mask.funcname);
                        return "";
                        //throw new Exception();
                    }
                }
                else
                {
                    if ((function_list[fc_mask.funcname] != -1)
                       && (argn.Children.Count < function_list[fc_mask.funcname]))
                    {
                        Console.WriteLine("Function {0} requires {1} arguments,", fc_mask.funcname
                                            , function_list[fc_mask.funcname]);
                        Console.WriteLine("but {0} arguments taken!", argn.Children.Count);
                        throw new Exception();
                    }
                }

                for (int c = 0, mask_pos = 0; c < argn.Children.Count; c++)
                {
                    /*cur_type = GetCurTypeID(fc_mask.mask, ref mask_pos);

                    if (((cur_type != -1) && (cur_type != argn.Children[c].Value.type))
                        || (mask_pos >= fc_mask.mask.Length))
                        throw new Exception("Invalid function argument!");*/

                    if (is_table)
                        arg_str += "[";
                    else
                        if (arg_str != "")
                        {
                            arg_str += ", ";
                        }

                    TreeNode<Node> tmp_node;
                    if (argn.Children[c].Value.type == Id_Up)
                        tmp_node = argn.Children[c];
                    else
                        tmp_node = argn;

                    if (tmp_node.Children[0].Value.type == NUMBER)
                    {
                        arg_str += IntToStrC(tmp_node.Value.dataN);
                        func_arg_types.Add(NUMBER);
                    }
                    else if (tmp_node.Children[0].Value.type == FUNCTION_CALL)
                    {
                        arg_str += BuildFunction(tmp_node, ref sw, ref nestinglvl);
                    }
                    else if ((tmp_node.Children[0].Value.type == STRING) || (tmp_node.Children[0].Value.type == Id))
                    {
                        arg_str += tmp_node.Children[0].Value.dataS + nestinglvl.id;

                        if (argn.Children[c].Children.Count > 1)
                        {
                            if (tmp_node.Children[1].Value.type == NUMBER)
                                arg_str += "[" + tmp_node.Children[1].Value.dataN + "]";
                            else
                                arg_str += "[\"" + tmp_node.Children[1].Value.dataS + "\"]";
                        }

                        func_arg_types.Add(STRING);
                    }
                    else
                    {
                        int level = 0;
                        int arg_type = -1;
                        arg_str += BuildExpressionString(tmp_node.Children[0], level, ref nestinglvl, ref arg_type);
                        func_arg_types.Add(arg_type);
                    }
                    if (is_table)
                        arg_str += "]";
                }
                if (fc_mask.mask_string_req)
                    arg_str = BuildMaskString(func_arg_types) + arg_str;

                expr += arg_str;
            }

            if (tn.Children[1].Value.type == TABLE_CONSTRUCTOR)
                expr += "";
            else
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

        private void PrintChunk(TreeNode<Node> tn, ref StreamWriter sw, ref LocalVars nestinglvl)
        {
            sw.WriteLine("{");
            for (short stage = 0; stage < LAST_STAGE + 1; stage++ )
                TransformSubTree(tn, ref sw, ref stage, ref nestinglvl);

            sw.WriteLine("}");
        }

        private void PrintWhile(TreeNode<Node> tn, ref StreamWriter sw, ref LocalVars nestinglvl)
        {
            TreeNode<Node> en = tn.Children[0];
            TreeNode<Node> chn = tn.Children[1];

            string expr = "while(";

            if (en.Children[0].Value.type == FUNCTION_CALL)
            {
                BuildFunction(en.Children[0], ref sw, ref nestinglvl);
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
                expr += BuildExpressionString(en, level, ref nestinglvl, ref arg_type);

            }
            expr += ")";
            sw.WriteLine(expr);

            LocalVars tmp = nestinglvl;
            tmp.id += "_while";
            PrintChunk(chn, ref sw, ref tmp);
        }

        private void PrintIf(TreeNode<Node> tn, ref StreamWriter sw, ref LocalVars nestinglvl)
        {
            TreeNode<Node> en = tn.Children[0].Children[0];
            TreeNode<Node> chn = tn.Children[1];

            string expr = "if(";

            if (en.Children[0].Value.type == FUNCTION_CALL)
            {
                BuildFunction(en.Children[0], ref sw, ref nestinglvl);
            }
            else if ((en.Value.type == STRING) || (en.Value.type == Id))
                    if (en.Value.dataS != "nil") expr += "true";
                    else expr += "false";

            else if (en.Children[0].Value.type == NUMBER)
                expr += "true";
            else
            {
                int level = 0;
                int arg_type = -1;
                expr += BuildExpressionString(en, level, ref nestinglvl, ref arg_type);

            }
            expr += ")";
            sw.WriteLine(expr);

            //LocalVars tmp = nestinglvl;
            //tmp.id += String.Format("_if{0}", tmp.lvl);
            PrintChunk(chn, ref sw, ref nestinglvl);

            if (tn.Children.Count > 3)
            {
                for (int ch_id = 3; ch_id < tn.Children.Count; ch_id++)
                {
                    //LocalVars tmp2 = nestinglvl;
                    //tmp2.id += String.Format("_if{0}", tmp.lvl);
                    if (tn.Children[ch_id].Value.type == ELSE)
                    {
                        //tmp2.id += String.Format("_else{0}", tmp.lvl);
                        sw.WriteLine("else");
                    }
                    else
                    {
                        //tmp2.id += String.Format("_elseif{0}", tmp.lvl);
                        sw.WriteLine("elseif");
                    }
                    PrintChunk(tn.Children[ch_id].Children[0], ref sw, ref nestinglvl);
                }
            }
        }

        private void PrintFor(TreeNode<Node> tn, ref StreamWriter sw, ref LocalVars nestinglvl, int stage)
        {
            string varname = tn.Children[0].Value.dataS + nestinglvl.id + "_for_" + nestinglvl.lvl;

            if (stage == 0)
            {
                if (!nestinglvl.varlist.Contains(varname))
                {
                    //sw.WriteLine("Var {0};", varname);
                    nestinglvl.varlist.Add(varname);
                }
            }
            else if (stage == 1)
            {
                string expr = String.Format(@"for({0}.setValue({1}); ", varname, tn.Children[1].Value.dataN);

                if (tn.Children[2].Value.type == FUNCTION_CALL)
                {
                    BuildFunction(tn.Children[2], ref sw, ref nestinglvl);
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
                    expr += BuildExpressionString(tn.Children[2], level, ref nestinglvl, ref arg_type, 
                                                    "_for_" + nestinglvl.lvl);

                }

                expr += String.Format("; {0}.setValue({0}.toInt() + {1})", varname, tn.Children[3].Children[0].Value.dataN);
                expr += ")";
                sw.WriteLine(expr);

                if (tn.Children.Count >= 5)
                {
                    LocalVars tmp = new LocalVars();
                    tmp.id = nestinglvl.id + String.Format("_for_{0}", nestinglvl.lvl);
                    PrintChunk(tn.Children[4], ref sw, ref tmp);
                }
            }
        }

        public void PrintRep(TreeNode<Node> tn, ref StreamWriter sw, ref LocalVars nestinglvl)
        {
            sw.WriteLine("do");
            LocalVars tmp = nestinglvl;
            tmp.id = nestinglvl.id + String.Format("_rep{0}", nestinglvl.lvl);
            PrintChunk(tn.Children[0], ref sw, ref nestinglvl);

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
                expr += BuildExpressionString(tn.Children[1].Children[0], level, ref nestinglvl, ref arg_type);

            }
            sw.WriteLine("while({0});", expr);
        }

        public void BuildFunctionInit(TreeNode<Node> tn, ref StreamWriter sw, ref LocalVars nestinglvl)
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
                {
                    LocalVars tmp = nestinglvl;
                    tmp.id += String.Format("_{0}{1}", fname, nestinglvl.lvl);
                    PrintChunk(tn.Children[1], ref sw, ref tmp);
                }
                else if ((tn.Children.Count > 2) && (tn.Children[2].Value.type == CHUNK))
                {
                    LocalVars tmp = nestinglvl;
                    tmp.id += String.Format("_{0}{1}", fname, nestinglvl.lvl);
                    PrintChunk(tn.Children[2], ref sw, ref tmp);
                }
            else
                sw.WriteLine("{}");
            sw.WriteLine();
            in_func = false;
        }

        private void PrintReturn(TreeNode<Node> tn, ref StreamWriter sw, ref LocalVars nestinglvl)
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
                sw.Write(BuildExpressionString(tn.Children[1].Children[0], level, ref nestinglvl, ref arg_type));
            }
            sw.WriteLine(";");
        }

        public bool PrintStringValue(TreeNode<Node> tn, ref StreamWriter sw, ref LocalVars nestinglvl, ref short stage)
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
                        GetAssignElem(tn, ref sw, ref nestinglvl, ref stage);
                        if (stage == 1)
                            for (int c = 0; c < m_exp.leftArg.Count; c++)
                            {
                                if (m_exp.prefix != "")
                                {
                                    if (!array_list.ContainsKey(m_exp.leftArg[c]))
                                        array_list.Add(m_exp.leftArg[c], -1);
                                    sw.WriteLine("{0}\n{1}.setValue({2});", m_exp.prefix,
                                                    m_exp.leftArg[c] + nestinglvl.id, m_exp.rightArg);
                                }
                                else
                                    sw.WriteLine("{0}.setValue({1});", m_exp.leftArg[c] + nestinglvl.id
                                                                 , m_exp.rightArg);
                            }

                        if (stage == 0)
                        {
                            
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
                            BuildFunctionInit(tn, ref sw, ref nestinglvl);
                            nestinglvl.lvl++;
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
                            PrintWhile(tn, ref sw, ref nestinglvl);
                            nestinglvl.lvl++;
                            return false;
                        }
                    }
                    break;
                case FUNCTION_CALL:
                    {
                        if (stage == 1)
                        {
                            //sw.WriteLine("FUNCTION_CALL");
                            BuildFunction(tn, ref sw, ref nestinglvl, true);
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
                            PrintRep(tn, ref sw, ref nestinglvl);
                            nestinglvl.lvl++;
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
                            PrintIf(tn, ref sw, ref nestinglvl);
                            nestinglvl.lvl++;
                            return false;
                        }
                    }
                    break;
                case IF_COND:
                    break;
                case FOR:
                    {
                        PrintFor(tn, ref sw, ref nestinglvl, stage);
                        nestinglvl.lvl++;
                        return false;
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
                            PrintReturn(tn, ref sw, ref nestinglvl);
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
