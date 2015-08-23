using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Sicp.BookExercises.Chapter2
{
    public class MultiplyNegative_2_1
    {
        [Fact]
        public void MultiplyTest()
        {
            var result = new RationalNumber(-3, 1)
               * new RationalNumber(2, -4);

            Assert.Equal(new RationalNumber(3, 2), result);
        }

        public struct RationalNumber : IEquatable<RationalNumber>
        {
            private int numer;
            private int denom;

            public RationalNumber(int numer, int denom)
            {
                this.numer = numer;
                this.denom = denom;
            }

            public static RationalNumber operator +(RationalNumber left, RationalNumber right)
            {
                return new RationalNumber(left.numer*right.denom + right.denom*left.numer, left.denom*right.denom);
            }

            public static RationalNumber operator *(RationalNumber left, RationalNumber right)
            {
                return new RationalNumber(left.numer * right.numer, left.denom * right.denom);
            }

            public override bool Equals(object obj)
            {
                if (obj is RationalNumber == false)
                {
                    return false;
                }

                return Equals((RationalNumber)obj);
            }

            public bool Equals(RationalNumber other)
            {
                return numer * other.denom == denom * other.numer;
            }

            public static bool operator ==(RationalNumber left, RationalNumber right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(RationalNumber left, RationalNumber right)
            {
                return (left == right) == false;
            }

            public override int GetHashCode()
            {
                return ((double)numer/denom).GetHashCode();
            }
        }

    }
}
