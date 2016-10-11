using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NeuralMotion.TreeModel
{
    public abstract class BaseNode
    {
        private List<BaseNode> _Children;

        public double[] Parameters { private set; get; }

        public int NumberOfChildren { private set; get; }
        public int NumberOfParameters { private set; get; }

        public BaseNode(int inNumberOfChildren, int inNumberOfParameters)
        {
            this._Children = new List<BaseNode>(inNumberOfChildren);
            for (var i = 0; i < inNumberOfChildren; i++)
                this._Children.Add(null);

            this.Parameters = new double[inNumberOfParameters];

            this.NumberOfChildren = inNumberOfChildren;
            this.NumberOfParameters = inNumberOfParameters;
        }

        public BaseNode Parent
        {
            private set;
            get;
        }

        public void SetChild(int inIndex, BaseNode inNode)
        {
            _Children[inIndex] = inNode;
            inNode.Parent = this;
        }

        public BaseNode GetChild(int inIndex)
        {
            return _Children[inIndex];
        }

        public void ReplaceChild(BaseNode inChild, BaseNode inNewChild)
        {
            var nIndex = this._Children.IndexOf(inChild);
            this._Children[nIndex] = inNewChild;
            inNewChild.Parent = this;
            inChild.Parent = null;
        }

        public ReadOnlyCollection<BaseNode> Children
        {
            get { return _Children.AsReadOnly(); }
        }

        public BaseNode[] AllChildren
        {
            get
            {
                var lNodes = new List<BaseNode>();
                lNodes.AddRange(this._Children);
                for (var i = 0; i < this._Children.Count; i++)
                    lNodes.AddRange(this._Children[i].AllChildren);
                return lNodes.ToArray();
            }
        }

        public abstract double Evaluate(double inState);
    }
}
