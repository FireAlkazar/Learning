using System;
using Sicp.BookExercises.Chapter2.Models;
using Xunit;

namespace Sicp.BookExercises.Chapter2
{
    public sealed class CountChange_2_19
    {
        [Fact]
        public void Test()
        {
            Pair coinsTypes = Pair.GetList(50, 25, 10, 5, 1);

            int calculate = Calculate(100, coinsTypes);

            Assert.Equal(292, calculate);
        }

        private int Calculate(int total, Pair coinsTypes)
        {
            if (total == 0)
            {
                return 1;
            }

            if (total < 0)
            {
                return 0;
            }

            if (coinsTypes == null)
            {
                return 0;
            }

            return Calculate(total - GetLastKindAmount(coinsTypes), coinsTypes)
                + Calculate(total, Pair.RemoveLast(coinsTypes));
        }

        private int GetLastKindAmount(Pair coinsTypes)
        {
            if (coinsTypes.Next == null)
            {
                return coinsTypes.Value;
            }

            return GetLastKindAmount(coinsTypes.Next);
        }
    }
}
