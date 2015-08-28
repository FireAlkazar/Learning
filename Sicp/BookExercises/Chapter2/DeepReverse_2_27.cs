using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Sicp.BookExercises.Chapter2
{
    public class DeepReverse_2_27
    {
        [Fact]
        public void Test()
        {
            var firstSubList = new Pair(new IntNode(1), new Pair(new IntNode(2), null));
            var secondSubList = new Pair(new IntNode(3), new Pair(new IntNode(4), null));

            var list = new Pair(firstSubList, new Pair(secondSubList, null));

            INode result = DeepReverse(list);
        }

        private INode DeepReverse(INode list)
        {
            if (list == null)
            {
                return null;
            }

            if (list.IsPair == false)
            {
                return list;
            }

            var pair = (Pair)list;

            if (pair.Next == null)
            {
                return new Pair(DeepReverse(pair.Value), null);
            }

            return new Pair(DeepReverse(pair.Next), pair);
        }

        private INode DeepReverseIter(INode list, INode curResult)
        {
            if (list == null)
            {
                return curResult;
            }

            if (list.IsPair == false)
            { 
                return new Pair()???
            }
        }

        interface INode
        {
            bool IsPair { get; }
        }

        class IntNode : INode
        {
            public IntNode(int value)
            {
                IntValue = value;
            }

            public int IntValue { get; set; }

            public bool IsPair
            {
                get { return false; }
            }
        }

        class Pair : INode
        {
            public Pair(INode value, Pair next)
            {
                Value = value;
                Next = next;
            }

            public Pair Next { get; set; }

            public INode Value { get; set; }

            public bool IsPair
            {
                get { return true; }
            }
        }
    }
}
