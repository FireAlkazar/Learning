using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Sicp.BookExercises
{
    public class ContinuedFraction_1_37
    {
        [Fact]
        public void ContinuedFractionTest()
        {
            var fi = 1.6180339887;
            var result = CalcContinuedFraction(x => 1.0, x => 1.0, 100);

            Assert.True(Math.Abs(1 / fi - result) < 0.0001);
        }

        private double CalcContinuedFraction(Func<int, double> nf, Func<int, double> df, int k)
        {
            return CalcContinuedFractionIter(nf, df, k, 1);
        }


        private double CalcContinuedFractionIter(Func<int, double> nf, Func<int, double> df, int k, int cur)
        {
            Func<double, double> term = x => nf(cur) / (df(cur) + x);
            if (k == cur)
            {
                return term(0);
            }

            return term(CalcContinuedFractionIter(nf, df, k, cur + 1));
        }
    }
}
