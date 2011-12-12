using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public Tree<Node> Root;

        private List<TreeNode> m_stack = new List<TreeNode>();

        public ASTree()
        {
            Root = new Tree<Node>(new Node());
        }
        /*
        public void AddStatNode(string expr)
        {
            Node n = new Node();
            n.type = STAT;
            n.par = expr;
            TreeNode<Node> node = new TreeNode<Node>(n, m_root);
        }
        */
        /*public void CreateNode(int type, TreeNode<Node> parent)
        {
            Node n = new Node();
            n.type = type;           
            TreeNode<Node> node = new TreeNode<Node>(n, parent);
        }
        */
        /*
        public TreeNode<Node> CreateNode(int type, TreeNodeList<Node> childs)
        {
            Node n = new Node();
            n.type = type;
            TreeNode<Node> node = new TreeNode<Node>(n);            
            return node;
        }
        */
        public TreeNode<Node> CreateNode(int type, TreeNode<Node> child)
        {
            Node n = new Node();
            n.type = type;
            TreeNode<Node> node = new TreeNode<Node>(n);
            m_stack.Add(node);
            node.Children.Add(child);
            return node;
        }

        public TreeNode<Node> CreateNode(int type, TreeNode ch1, TreeNode ch2)
        {
            Node n = new Node();
            n.type = type;            
            TreeNode node = new TreeNode(n);
            m_stack.Add(node);
            node.Children.Add(ch1);
            node.Children.Add(ch2);
            return node;
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

        public TreeNodeList<Node> CreateNodeList(TreeNode<Node> child)
        {
            TreeNodeList<Node> nodelist = new TreeNodeList<Node>();
            if (child != null)
            {
                nodelist.Add(child);
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

        public TreeNodeList<Node> AppendNodeList(TreeNodeList<Node> nl, TreeNode<Node> node)
        {
            if (nl == null)
            {
                return CreateNodeList(node);
            }
            if (node != null)
            {
                nl.Add(node);
            }
            return nl;
        }

        public TreeNode<Node> AppendChild(int type, TreeNode<Node> n, TreeNode<Node> ch)
        {
            if (n == null)
            {
                n = CreateNode(type, ch);                
            }
            else
            {
                n.Children.Add(ch);
            }
            return n;
        }

        /*
        private string ParseExpr(string expr)
        {
            //expr = ;
            return new string("123");
        }*/
    }
}
