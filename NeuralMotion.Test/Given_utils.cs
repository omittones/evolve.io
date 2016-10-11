using System.Diagnostics;
using Util;
using Xunit;


namespace NeuralMotion.Test
{
    public class Given_utils
    {
        [Fact]
        public void Set_linear_works()
        {
            var array = new[,] {{1, 2, 3}, {4, 5, 6}};
            Debug.Assert(array[0, 0] == 1);
            Debug.Assert(array[0, 1] == 2);
            Debug.Assert(array[1, 0] == 4);

            Debug.Assert(array.GetLinearLength() == 6);
            Debug.Assert(array.GetLinear(0) == 1);
            Debug.Assert(array.GetLinear(1) == 2);
            Debug.Assert(array.GetLinear(2) == 3);
            Debug.Assert(array.GetLinear(3) == 4);
            Debug.Assert(array.GetLinear(4) == 5);
            Debug.Assert(array.GetLinear(5) == 6);

            for (var i = 0; i < 6; i++)
                array.SetLinear(i, 6 - i);

            Debug.Assert(array.GetLinear(0) == 6);
            Debug.Assert(array.GetLinear(1) == 5);
            Debug.Assert(array.GetLinear(2) == 4);
            Debug.Assert(array.GetLinear(3) == 3);
            Debug.Assert(array.GetLinear(4) == 2);
            Debug.Assert(array.GetLinear(5) == 1);
        }
    }
}
