using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
//alias of Identifer to simple use
using Identifer = LUA_Interpreter.IdentiferTable.Identifer;
using Tokens = LuaSyntax.Tokens;

namespace LUA_Interpreter
{
    using TreeNode = LUA_Interpreter.TreeNode<Node>;
    public struct Node
    {
        public int type;
        public string par;
        //public Identifer idLink;        
        public string dataS;
        public double dataN;
    };  

    public class ASTree
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

        public Tree<Node> Root;

        private List<TreeNode> m_stack = new List<TreeNode>();

        public ASTree()
        {
            Root = new Tree<Node>(new Node());
        }

        public TreeNode<Node> CreateNode(int type, string val)
        {
            Node n = new Node();
            n.type = type;
            //n.idLink
            n.dataS = val;
            TreeNode<Node> node = new TreeNode<Node>(n);
            m_stack.Add(node);
            return node;
        }

        public TreeNode CreateNode(int type, double val)
        {
            Node n = new Node();
            n.type = type;
            n.dataN = val;
            TreeNode node = new TreeNode(n);
            m_stack.Add(node);
            return node;
        }

        public TreeNode CreateNode(int type, TreeNodeList<Node> tnl)
        {
            Node n = new Node();
            n.type = type;
            TreeNode node = new TreeNode(n);
            m_stack.Add(node);
            if (tnl != null)
            {
                foreach (TreeNode tn in tnl)
                {
                    if (tn != null)
                    {
                        node.Children.Add(tn);
                    }
                }
            }
            return node;
        }

        public TreeNode CreateNode(int type, params TreeNode[] ch)
        {
            Node n = new Node();
            n.type = type;
            TreeNode node = new TreeNode(n);
            m_stack.Add(node);
            foreach(TreeNode c in ch)
            {
                if (c != null)
                {
                    node.Children.Add(c);
                }
            }
            return node;
        }

        public TreeNodeList<Node> CreateNodeList(params TreeNode[] childs)
        {
            TreeNodeList<Node> nodelist = new TreeNodeList<Node>();
            foreach (TreeNode child in childs)
            {
                if (child != null)
                {
                    nodelist.Add(child);
                }
            }
            return nodelist;
        }

        public TreeNodeList<Node> CreateNodeList(TreeNode child, TreeNodeList<Node> nl)
        {
            TreeNodeList<Node> nodelist = new TreeNodeList<Node>();
            if (child != null)
            {
                nodelist.Add(child);
            }
            if (nl != null)
            {
                foreach (TreeNode node in nl)
                {
                    if (node != null)
                    {
                        nodelist.Add(node);
                    }
                }
            }
            return nodelist;
        }

        public TreeNode<Node> AppendChild(TreeNode par, TreeNodeList<Node> childs)
        {
            if (par != null && childs != null)
            {
                foreach(TreeNode child in childs)
                {
                    par.Children.Add(child);
                }
                childs.Clear();
            }
            return par;
        }

        public TreeNodeList<Node> AppendNodeList(TreeNodeList<Node> nl, params TreeNode[] nodes)
        {
            if (nl == null)
            {
                return CreateNodeList(nodes);
            }
            foreach (TreeNode node in nodes)
            {
                if (node != null)
                {
                    nl.Add(node);
                }
            }
            return nl;
        }

        public TreeNode<Node> AppendChild(int type, TreeNode n, TreeNode ch)
        {
            if (n == null)
            {
                n = CreateNode(type, ch);                
            }
            else
            {
                if (ch != null)
                {
                    n.Children.Add(ch);
                }
            }
            return n;
        }

        public void DrawTree(string path)
        {
            StreamWriter sw = new StreamWriter(path);
            int level = 1;
            DrawChild(Root, ref sw, level);
            sw.Close();
        }

        public void DrawChild(TreeNode par, ref StreamWriter sw, int level)
        {
            //sw.WriteLine(par.Value.type);
            PrintStringValue(par.Value, ref sw);
            for (int i = 0; i < par.Children.Count; i++)
            {
                for (int c = 0; c < level; c++)
                    sw.Write('-');

                DrawChild(par.Children[i], ref sw, level + 1);
            }
        }

        public void PrintStringValue(Node n, ref StreamWriter sw)
        {
            switch (n.type)
            {
                case STAT:
                    sw.WriteLine("STAT");
                    break;
                case VARLIST:
                    sw.WriteLine("VARLIST");
                    break;
                case Id:
                    sw.WriteLine("Id");
                    break;
                case NUMBER:
                    sw.WriteLine("NUMBER {0}", n.dataN);
                    break;
                case ASSIGN:
                    sw.WriteLine("ASSIGN");
                    break;
                case EXPLIST:
                    sw.WriteLine("EXPLIST");
                    break;
                case CHUNK:
                    sw.WriteLine("CHUNK");
                    break;
                case STRING:
                    sw.WriteLine("STRING", n.dataS);
                    break;
                case OperatorMinus:
                    sw.WriteLine("OperatorMinus");
                    break;
                case OperatorPlus:
                    sw.WriteLine("OperatorPlus");
                    break;
                case OperatorMod:
                    sw.WriteLine("OperatorMod");
                    break;
                case OperatorDiv:
                    sw.WriteLine("OperatorDiv");
                    break;
                case OperatorMul:
                    sw.WriteLine("OperatorMul");
                    break;
                case EXP:
                    sw.WriteLine("EXP");
                    break;
                case FUNCTION:
                    sw.WriteLine("FUNCTION");
                    break;
                case FUNCNAME:
                    sw.WriteLine("FUNCNAME");
                    break;
                case PARLIST:
                    sw.WriteLine("PARLIST");
                    break;
                case DO:
                    sw.WriteLine("DO");
                    break;
                case WHILE:
                    sw.WriteLine("WHILE");
                    break;
                case FUNCTION_CALL:
                    sw.WriteLine("FUNCTION_CALL");
                    break;
                case ARGS:
                    sw.WriteLine("ARGS");
                    break;
                case ASSIGN_LOCAL:
                    sw.WriteLine("ASSIGN_LOCAL");
                    break;
                case BREAK:
                    sw.WriteLine("BREAK");
                    break;
                case REPEAT:
                    sw.WriteLine("REPEAT");
                    break;
                case UNTIL_COND:
                    sw.WriteLine("UNTIL_COND");
                    break;
                case IF:
                    sw.WriteLine("IF");
                    break;
                case IF_COND:
                    sw.WriteLine("IF_COND");
                    break;
                case FOR:
                    sw.WriteLine("FOR");
                    break;
                case STEP:
                    sw.WriteLine("STEP");
                    break;
                case ELSEIF_LIST:
                    sw.WriteLine("ELSEIF_LIST");
                    break;
                case ELSE:
                    sw.WriteLine("ELSE");
                    break;
                case RETURN:
                    sw.WriteLine("RETURN");
                    break;
                case TABLE_CONSTRUCTOR:
                    sw.WriteLine("TABLE_CONSTRUCTOR");
                    break;
                case GREATER:
                    sw.WriteLine("GREATER");
                    break;
                case GREATER_EQUAL:
                    sw.WriteLine("GREATER_EQUAL");
                    break;
                case LESS:
                    sw.WriteLine("LESS");
                    break;
                case LESS_EQUAL:
                    sw.WriteLine("LESS_EQUAL");
                    break;
                case EQUAL:
                    sw.WriteLine("EQUAL");
                    break;
                case NOT_EQUAL:
                    sw.WriteLine("NOT_EQUAL");
                    break;
                case AND:
                    sw.WriteLine("AND");
                    break;
                case OR:
                    sw.WriteLine("OR");
                    break;
                case NOT:
                    sw.WriteLine("NOT");
                    break;
                case UPVALUE:
                    sw.WriteLine("UPVALUE");
                    break;
                case FOR_IN:
                    sw.WriteLine("FOR_IN");
                    break;
                case Id_Up:
                    sw.WriteLine("Id_Up");
                    break;
                case FUNCNAME_UP:
                    sw.WriteLine("FUNCNAME_UP");
                    break;
                case FUNCNAME_IMPLICIT:
                    sw.WriteLine("FUNCNAME_IMPLICIT");
                    break;
                case FUNCTION_CALL_IMPLICIT:
                    sw.WriteLine("FUNCTION_CALL_IMPLICIT");
                    break;
            }
        }
    }
}
