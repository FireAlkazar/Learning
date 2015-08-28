namespace Sicp.BookExercises.Chapter2.Models
{
    public sealed class Pair
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public Pair(int value, Pair next)
        {
            Next = next;
            Value = value;
        }

        public Pair Next { get; set; }

        public int Value { get; set; }

        public static Pair Append(Pair list, int value)
        {
            if (list == null)
            {
                return new Pair(value, null);
            }

            return new Pair(list.Value, Append(list.Next, value));
        }

        public static Pair GetList(params int[] values)
        {
            return GetListIter(0, values);
        }

        public static Pair Reverse(Pair list)
        {
            //if (list.Next == null)
            //{
            //    return list;
            //}

            //return Append(Reverse(list.Next), list.Value);
            return ReverseIter(list, null);
        }

        private static Pair ReverseIter(Pair list, Pair result)
        { 
            if(list == null)
            {
                return result;
            }

            return ReverseIter(list.Next, new Pair(list.Value, result));
        }

        private static Pair GetListIter(int startIndex, int[] values)
        {
            if (startIndex >= values.Length)
            {
                return null;
            }

            return new Pair(values[startIndex], GetListIter(startIndex + 1, values));
        }

        public static Pair RemoveLast(Pair pair)
        {
            if (pair.Next == null)
            {
                return null;
            }

            return new Pair(pair.Value, RemoveLast(pair.Next));
        }
    }
}