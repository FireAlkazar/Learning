using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Sicp.BookExercises.Chapter2
{
    public class PairsAsLambda_2_4
    {
        [Fact]
        public void PairTest()
        { 
            var pair = Cons(3,4);

            Assert.Equal(3, Car(pair));
            Assert.Equal(4, Cdr(pair));
        }

        /// <summary>
        /// Get second pair element;
        /// </summary>
        private int Cdr(Func<Func<int, int, int>, int> pair)
        {
            return pair((x, y) => y);
        }

        /// <summary>
        /// Get first element in pair
        /// </summary>
        private int Car(Func<Func<int, int, int>, int> pair)
        {
            return pair((x, y) => x);
        }

        /// <summary>
        /// Make-pair
        /// </summary>
        private Func<Func<int, int, int>, int> Cons(int p1, int p2)
        {
            Func<Func<int, int, int>, int> result = m => m(p1, p2);
            return result;
        }
    }
}
