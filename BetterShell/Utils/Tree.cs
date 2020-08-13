using System;
using System.Collections.Generic;

namespace BetterShell.Utils
{
    public class Tree<T> : Branch<T>
    {
    }

    public class Branch<T>
    {
        public T Data { get; }

        public List<Branch<T>> Children { get; }

        public Branch(T data = default)
        {
            Data = data;
            Children = new List<Branch<T>>();
        }

        public virtual void AddChild(T data)
        {
            Children.Add(new Leaf<T>(data));
        }

        public virtual void AddChildHierarchy(Branch<T> data)
        {
            Children.Add(data);
        }
    }
    
    class Leaf<T> : Branch<T>
    {
        public Leaf(T data = default) : base(data)
        {
        }

        public override void AddChild(T data)
        {
            throw new NotImplementedException();
        }

        public override void AddChildHierarchy(Branch<T> data)
        {
            throw new NotImplementedException();
        }
    }
}