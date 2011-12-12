using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUA_Interpreter
{
    //specially for root
    public class Tree<T> : TreeNode<T>
    {
        public Tree(T RootValue) : base (RootValue)
        {
        }
    }
}
