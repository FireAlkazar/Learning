using System.Collections.Generic;
using Xunit;

namespace Sicp.OtherExercises
{
    /// <summary>
    /// Задан массив положительных чисел, распечатать все суммы, которые можно составить из этих чисел, равные заданному числу.
    /// </summary>
    public sealed class ComposeNumberBySummingOtherNumbers
    {
        private bool[] flags;
        private int[] globalNumbers;
        private int curIndex;
        private int curSum;
        private int globalResultNumber;
        private readonly List<string> globalResult = new List<string>();
            
        [Fact]
        public void Test()
        {
            List<string> sums = GetSums(new int[] { 1, 1 }, 2);

            Assert.Equal(1, sums.Count);
            Assert.Equal("1 + 1", sums[0]);
        }

        [Fact]
        public void Test2()
        {
            List<string> sums = GetSums(new int[] { 1, 2, 3, 4 }, 4);

            Assert.Equal(2, sums.Count);
            Assert.Equal("1 + 3", sums[0]);
            Assert.Equal("4", sums[1]);
        }

        private List<string> GetSums(int[] numbers, int resultNumber)
        {
            flags = new bool[numbers.Length];
            curIndex = -1;
            globalResultNumber = resultNumber;
            globalNumbers = numbers;

            UpToTopAndProcess();

            while (CanDown())
            {
                if(CanRight())
                {
                    Right();
                    UpToTopAndProcess();
                }

                Down();
            }

            return globalResult;
        }

        private void Down()
        {
            if(flags[curIndex])
            {
                curSum -= globalNumbers[curIndex];
                flags[curIndex] = false;
            }

            curIndex--;
        }

        private void Right()
        {
            flags[curIndex] = false;
            curSum -= globalNumbers[curIndex];
        }

        private bool CanRight()
        {
            return flags[curIndex];
        }

        private bool CanDown()
        {
            return curIndex > -1;
        }

        private void UpToTopAndProcess()
        {
            while (CanUp())
            {
                Up();
            }

            Process();
        }

        private void Process()
        {
            if(curSum == globalResultNumber)
            {
                globalResult.Add(GetCurrentSum());
            }
        }

        private string GetCurrentSum()
        {
            List<string> components = new List<string>();
            for (int i = 0; i < flags.Length; i++)
            {
                if(flags[i])
                {
                    components.Add(globalNumbers[i].ToString());
                }
            }

            return string.Join(" + ", components.ToArray());
        }

        private void Up()
        {
            curIndex++;
            flags[curIndex] = true;
            curSum += globalNumbers[curIndex];
        }

        private bool CanUp()
        {
            return curIndex < flags.Length - 1 &&
                curSum < globalResultNumber;
        }
    }
}