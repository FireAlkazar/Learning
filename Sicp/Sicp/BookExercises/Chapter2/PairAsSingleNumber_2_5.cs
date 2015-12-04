using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Sicp.BookExercises.Chapter2
{
    public class PairAsSingleNumber_2_5
    {
        [Fact]
        public void Test()
        {
            var pair = Cons(4, 8);

            Assert.Equal(4, Car(pair));
            Assert.Equal(8, Cdr(pair));
        }

        /// <summary>
        /// Get second pair element;
        /// </summary>
        private int Cdr(int pair)
        {
            return CdrIter(pair, 0);
        }

        private int CdrIter(int pair, int val)
        {
            if (pair % 3 != 0)
            {
                return val;
            }

            return CdrIter(pair / 3, val + 1);
        }

        /// <summary>
        /// Get first element in pair
        /// </summary>
        private int Car(int pair)
        {
            return CarIter(pair, 0);
        }

        private int CarIter(int pair, int val)
        {
            if (pair % 2 != 0)
            {
                return val;
            }

            return CarIter(pair / 2, val + 1);
        }

        /// <summary>
        /// Make-pair
        /// </summary>
        private int Cons(int p1, int p2)
        {
            return (int)(Math.Pow(2, p1) * Math.Pow(3, p2));
        }


    }
}
