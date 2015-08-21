using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Sicp.BookExercises
{
    public class LogarithmicMultiplicationProblem_1_18
    {
        [Fact]
        public void MultiplyTest()
        {
            int result = Multiply(6, 7);

            Assert.Equal(42, result);
        }

        private int Multiply(int p1, int p2)
        {
            int rests = 0;
            while (p1 != 1)
            {
                if (p1 % 2 == 0)
                {
                    p1 /= 2;
                    p2 *= 2;
                }
                else 
                {
                    rests += p2;
                    p1 -= 1;
                }
            }

            return p2 + rests;
        }
    }
}
