using Xunit;

namespace Algorithms.Sorts.UnitTests
{
    public class SortTests
    {
        [Theory]
        [InlineData(new int[0], new int[0])]
        [InlineData(new[] { 1 }, new[] { 1})]
        [InlineData(new[] { 1,2 }, new[] { 1,2})]
        [InlineData(new[] { 3, 2, 1 }, new[] { 1, 2, 3})]
        [InlineData(new[] { 4, 3, 2, 1 }, new[] { 1, 2, 3, 4})]
        public void Sort_Array_Ok(int[] array, int[] expectedResult)
        {
            var selectionSort = new SelectionSort();

            selectionSort.Sort(array);

            Assert.Equal(expectedResult, array);
        }
    }
}