using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUA_Interpreter
{
    public class TreeNodeList<T> : List<TreeNode<T>>
    {
        public TreeNode<T> Parent;

        public TreeNodeList()
        {
        }

        public TreeNodeList(TreeNode<T> Parent)
        {
            this.Parent = Parent;
        }

        public new TreeNode<T> Add(TreeNode<T> Node)
        {
            base.Add(Node);
            Node.Parent = Parent;
            return Node;
        }

        public TreeNode<T> Add(T Value)
        {
            return Add(new TreeNode<T>(Value));
        }
    }
}
