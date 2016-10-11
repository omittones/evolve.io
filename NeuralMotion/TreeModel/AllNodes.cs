using System;
using System.Collections.Generic;
using System.Reflection;

namespace NeuralMotion.TreeModel
{
    public static class AllNodes
    {
        private static Random _rndGen = new Random(DateTime.Now.Millisecond);
        private static List<Func<BaseNode>> _aNonLeafCreators = new List<Func<BaseNode>>();
        private static List<Func<BaseNode>> _aLeafCreators = new List<Func<BaseNode>>();

        static AllNodes()
        {
            var asmModule = Assembly.GetExecutingAssembly();
            var aTypes = asmModule.GetTypes();
            for (var i = 0; i < aTypes.Length; i++)
            {
                if (aTypes[i].IsSubclassOf(typeof(BaseNode)))
                {
                    var node = (BaseNode)asmModule.CreateInstance(aTypes[i].FullName);
                }
            }

            _aLeafCreators.Add(Constant.MakeNew);
            _aLeafCreators.Add(State.MakeNew);
            _aNonLeafCreators.Add(Linear.MakeNew);
            _aNonLeafCreators.Add(Plus.MakeNew);
            _aNonLeafCreators.Add(Minus.MakeNew);
            _aNonLeafCreators.Add(Multi.MakeNew);
            _aNonLeafCreators.Add(Div.MakeNew);
            _aNonLeafCreators.Add(Sin.MakeNew);
            _aNonLeafCreators.Add(Cos.MakeNew);
        }

        public static KeyValuePair<string, Func<BaseNode>>[] GetCreators()
        {
            var aCreators = new KeyValuePair<string, Func<BaseNode>>[_aLeafCreators.Count + _aNonLeafCreators.Count];
            for (var cCrtr = 0; cCrtr < aCreators.Length; cCrtr++)
            {
                Func<BaseNode> delCreator;
                if (cCrtr >= _aLeafCreators.Count)
                    delCreator = _aNonLeafCreators[cCrtr - _aLeafCreators.Count];
                else
                    delCreator = _aLeafCreators[cCrtr];

                var creator = new KeyValuePair<string, Func<BaseNode>>(delCreator.Method.DeclaringType.Name, delCreator);
                aCreators[cCrtr] = creator;
            }
            return aCreators;
        }

        public static BaseNode MakeRandomNode(bool inForceLeaf)
        {
            BaseNode objNode;
            int nVar;
            if (inForceLeaf)
            {
                nVar = _rndGen.Next(_aLeafCreators.Count);
                objNode = _aLeafCreators[nVar]();                
            }
            else
            {
                nVar = _rndGen.Next(_aLeafCreators.Count + _aNonLeafCreators.Count);
                if (nVar >= _aLeafCreators.Count)
                    objNode = _aNonLeafCreators[nVar - _aLeafCreators.Count]();
                else
                    objNode = _aLeafCreators[nVar]();
            }

            for (var i = 0; i < objNode.NumberOfParameters; i++)
                objNode.Parameters[i] = 1;

            return objNode;
        }

        public static BaseNode MakeRandomTree(int inMaxNonLeafCount, out int outNonLeafCount)
        {
            outNonLeafCount = 0;
            BaseNode root = null;

            if (false)
            {
                var sin = new Sin();                
                sin.Parameters[0] = 100;
                sin.Parameters[1] = 20;
                sin.Parameters[2] = 10;

                var lin = new Linear();
                lin.Parameters[0] = 100;
                lin.Parameters[1] = 20;                
                lin.SetChild(0, new State());
                sin.SetChild(0, lin);

                //Minus min = new Minus();
                //Constant a = new Constant(); a.Value = 5;
                //Constant b = new Constant(); b.Value = 7;
                //min.AddChild(a);
                //min.AddChild(b);

                //Multi mul = new Multi();
                //Constant d = new Constant(); d.Value = 2;
                //mul.AddChild(d);
                //mul.AddChild(min);

                //Plus pls = new Plus();
                //Constant c = new Constant(); c.Value = 90;

                //pls.AddChild(mul);
                //pls.AddChild(c);

                root = sin;
            }
            root = MakeRandomNode(inMaxNonLeafCount <= 0);
            var lUnsolved = new List<BaseNode>();
            lUnsolved.Add(root);

            while (lUnsolved.Count > 0)
            {
                for (var cNode = 0; cNode < lUnsolved[0].NumberOfChildren; cNode++)
                {
                    var kid = MakeRandomNode(inMaxNonLeafCount <= 0);
                    if (kid.NumberOfChildren > 0)
                    {
                        inMaxNonLeafCount--;
                        lUnsolved.Add(kid);
                    }
                    lUnsolved[0].SetChild(cNode, kid);
                }
                lUnsolved.RemoveAt(0);
            }

            Minimize(root);

            var aNodes = root.AllChildren;
            for (var i = 0; i < aNodes.Length; i++)
                if (aNodes[i].NumberOfChildren > 0) outNonLeafCount++;

            return root;
        }

        public static bool IsStatic(BaseNode inNode)
        {
            if (inNode is Minus)
                if (inNode.Children[0] is State && inNode.Children[1] is State) return true;

            var aChildren = inNode.AllChildren;
            for (var i = 0; i < aChildren.Length; i++)
                if (aChildren[i] is State) return false;
            return true;
        }

        public static void Minimize(BaseNode inRoot)
        {
            var aAllNodes = inRoot.AllChildren;

            while (true)
            {
                var bMinimized = false;
                for (var i = 0; i < aAllNodes.Length; i++)
                {
                    var bnNode = aAllNodes[i];
                    if (bnNode.NumberOfChildren > 0 && IsStatic(bnNode))
                    {
                        var value = bnNode.Evaluate(0);
                        if (double.IsNegativeInfinity(value)) value = -100000;
                        if (double.IsPositiveInfinity(value)) value = 100000;

                        var bnNew = new Constant();
                        bnNew.Parameters[0] = value;
                        bnNode.Parent.ReplaceChild(bnNode, bnNew);
                        bMinimized = true;
                        break;
                    }
                }
                aAllNodes = inRoot.AllChildren;
                if (!bMinimized) return;
            }
        }
    }
}