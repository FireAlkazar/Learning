using System;

namespace Algorithms.Sorts
{
    public class InsertionSort
    {
        public void Sort(int[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            if(array.Length == 0)
            {
                return;
            }

            for(int i = 1; i < array.Length; i++)
            {
                for (int j = i; j > 0; j--)
                {
                    if(array[j] < array[j - 1])
                    {
                        var tmp = array[j];
                        array[j] = array[j - 1];
                        array[j - 1] = tmp;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}