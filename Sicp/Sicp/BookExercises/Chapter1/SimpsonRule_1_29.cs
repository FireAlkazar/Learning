using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Sicp.BookExercises
{
    public class SimpsonRule_1_29
    {
        [Fact]
        public void StraightForwardTest()
        {
            double dx = 0.001;
            double result = Integral(x => x * x * x, 0, 1, dx);

            Assert.True(Math.Abs(0.249999875 - result) < 0.00001);
        }

        [Fact]
        public void SimpsonTest()
        {
            double result = IntegralSimpson(x => x * x * x, 0, 1, 1000, 0);
            Console.WriteLine(result);
            Assert.True(Math.Abs(0.250333333 - result ) < 0.00001);
        }

        private double Integral(Func<double, double> func, double start, double stop, double dx)
        {
            if(start > stop)
            {
                return 0.0;
            }

            return func(start + dx / 2) * dx + Integral(func, start + dx, stop, dx);
        }

        private double IntegralSimpson(Func<double, double> func, double start, double stop, int n, int k)
        {
            if (k > n)
            {
                return 0.0;
            }

            double dx = (stop - start) / n;
            double term = dx/3 * (((k % 2) == 0) ? 2 : 4) * func(start + k*dx);
            return term + IntegralSimpson(func, start, stop, n, k + 1);
        }
    }
}
