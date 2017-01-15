using System;

namespace Algorithms.Sorts
{
    public class SelectionSort
    {
        public void Sort(int[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            if (array.Length == 0)
            {
                return;
            }


            for (int i = 0; i < array.Length - 1; i++)
            {
                int smallestElementIndex = i;
                for(int j = i + 1; j < array.Length; j++)
                {
                    if(array[j] < array[smallestElementIndex])
                    {
                        smallestElementIndex = j;
                    }
                }

                int tmp = array[i];
                array[i] = array[smallestElementIndex];
                array[smallestElementIndex] = tmp;
            }
        }
    }
}