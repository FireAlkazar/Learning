using System;
using Xunit;

namespace Sicp.OriginalExercises
{
    /// <summary>
    /// Сколькими способами можно разбить 100 центов монетами по 1, 5, 10, 25, 50 центов.
    /// </summary>
    public sealed class CountChangeProblem_1_14
    {
        [Fact]
        public void CalculateTest()
        {
            int calculate = Calculate(100, 5);

            Assert.Equal(292, calculate);
        }

        [Fact]
        public void CalculateTest2()
        {
            int calculate = Calculate(11, 5);

            Assert.Equal(4, calculate);
        }

        private int Calculate(int total, int numberOfCoinKinds)
        {
            if(total == 0)
            {
                return 1;
            }

            if(total < 0)
            {
                return 0;
            }

            if(numberOfCoinKinds == 0)
            {
                return 0;
            }

            return Calculate(total - GetFirstKindAmount(numberOfCoinKinds), numberOfCoinKinds)
                + Calculate(total, numberOfCoinKinds - 1);
        }

        private int GetFirstKindAmount(int numberOfCoinKinds)
        {
            switch (numberOfCoinKinds)
            {
                case 5:
                    return 50;
                case 4:
                    return 25;
                case 3:
                    return 10;
                case 2:
                    return 5;
                case 1:
                    return 1;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}