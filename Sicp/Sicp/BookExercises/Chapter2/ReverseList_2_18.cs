using System;
using Sicp.BookExercises.Chapter2.Models;
using Xunit;

namespace Sicp.BookExercises.Chapter2
{
    public sealed class ReverseList_2_18
    {
        [Fact]
        public void AppendTest()
        {
            Pair list = Pair.GetList(1, 4);

            Pair result = Pair.Append(list, 9);

            Assert.Equal(9, result.Next.Next.Value);
        }

        [Fact]
        public void GetListTest()
        {
            Pair list = Pair.GetList(1, 4, 9, 16, 25);

            Assert.Equal(25, list.Next.Next.Next.Next.Value);
        }

        [Fact]
        public void ReverseTest()
        {
            Pair list = Pair.GetList(1, 4, 9, 16, 25);

            Pair result = Pair.Reverse(list);

            Assert.Equal(4, result.Next.Next.Next.Value);
            Assert.Equal(1, result.Next.Next.Next.Next.Value);
        }
    }
}
