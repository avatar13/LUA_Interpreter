using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUA_Interpreter
{
    public class TreeNode<T>
    {
        private TreeNode<T> m_parent;
        public TreeNode<T> Parent
        {
            get
            {
                return m_parent;
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                if (m_parent != null)
                {
                    m_parent.Children.Remove(this);
                }
                if (value != null && !value.Children.Contains(this))
                {
                    value.Children.Add(this);
                }

                m_parent = value;
            }
        }

        public TreeNode<T> Root
        {
            get
            {
                TreeNode<T> node = this;
                while (node.Parent != null)
                {
                    node = node.Parent;
                }
                return node;
            }
        }

        private TreeNodeList<T> m_children;
        public TreeNodeList<T> Children
        {
            get
            {
                return m_children;
            }
            private set
            {
                m_children = value;
            }
        }

        private T m_value;
        public T Value
        {
            get
            {
                return m_value;
            }
            set
            {
                m_value = value;
                /*if (m_value != null)
                {
                    m_valu
                }*/
            }
        }

        public TreeNode(T Value)
        {
            this.Value = Value;
            Parent = null;
            Children = new TreeNodeList<T>(this);
        }

        public TreeNode(T Value, TreeNode<T> Parent)
        {
            this.Value = Value;
            this.Parent = Parent;
            Children = new TreeNodeList<T>(this);
        }
    }
}
