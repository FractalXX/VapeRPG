using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VapeRPG.DataStructures
{
    internal class TreeNode<T>
    {
        private T value;
        private List<TreeNode<T>> children;

        public T Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        public List<TreeNode<T>> Children
        {
            get
            {
                return this.children;
            }
        }

        public int ChildrenCount
        {
            get
            {
                return this.Children.Count;
            }
        }

        public TreeNode<T> this[int index]
        {
            get
            {
                if(index < this.ChildrenCount && index >= 0)
                {
                    return this.children[index];
                }
                else
                {
                    return null;
                }
            }
        }

        public TreeNode(T value)
        {
            this.children = new List<TreeNode<T>>();
            this.Value = value;
        }

        public void AddChild(TreeNode<T> child)
        {
            this.children.Add(child);
        }
    }
}
