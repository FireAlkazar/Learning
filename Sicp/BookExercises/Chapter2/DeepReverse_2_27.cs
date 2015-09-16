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
        public void TreeDeepReverse()
        {
            var firstSubList = new Pair(new IntNode(1), new Pair(new IntNode(2), null));
            var secondSubList = new Pair(new IntNode(3), new Pair(new IntNode(4), null));

            var list = new Pair(firstSubList, new Pair(secondSubList, null));

            INode result = DeepReverse(list);

            Assert.Equal(4, ((IntNode)(((Pair)((Pair)result).Value).Value)).IntValue);
        }

        private INode DeepReverse(INode list)
        {
            return DeepReverseIter(list, null);
        }

        private INode DeepReverseIter(INode list, INode curResult)
        {
            if (list == null)
            {
                return curResult;
            }

            if (list.IsPair == false)
            {
                return ((IntNode)list).Clone();
            }

            var reversedCurrentNode = DeepReverseIter(((Pair)list).Value, null);

            return DeepReverseIter(((Pair)list).Next, new Pair(reversedCurrentNode, (Pair)curResult));
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

            public IntNode Clone()
            {
                return new IntNode(IntValue);
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
