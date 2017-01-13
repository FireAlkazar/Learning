using System.Collections.Generic;
using Xunit;

namespace TravelingDomain.UnitTests
{
    public sealed class PathServiceTests
    {
        private readonly PathService _pathService = new PathService();

        [Fact]
        public void GetOrderedCards_ThreeCardsWithLastTwoDisordered_Ok()
        {
            var card1 = new TravelCard("Мельбурн", "Кельн");
            var card2 = new TravelCard("Москва", "Париж");
            var card3 = new TravelCard("Кельн", "Москва");
            var disorderedCards = new List<TravelCard> {card1, card2, card3};

            List<TravelCard> orderedCards = _pathService.GetOrderedCards(disorderedCards);

            Assert.Equal(card1, orderedCards[0]);
            Assert.Equal(card3, orderedCards[1]);
            Assert.Equal(card2, orderedCards[2]);
        }

        [Fact]
        public void GetOrderedCards_ThreeCardsAllDisordered_Ok()
        {
            var card1 = new TravelCard("C", "D");
            var card2 = new TravelCard("B", "C");
            var card3 = new TravelCard("A", "B");
            var disorderedCards = new List<TravelCard> { card1, card2, card3 };

            List<TravelCard> orderedCards = _pathService.GetOrderedCards(disorderedCards);

            Assert.Equal(card3, orderedCards[0]);
            Assert.Equal(card2, orderedCards[1]);
            Assert.Equal(card1, orderedCards[2]);
        }

        [Fact]
        public void GetOrderedCards_SingleCard_Ok()
        {
            var singleCardList = new List<TravelCard> { new TravelCard("Мельбурн", "Кельн") };

            List<TravelCard> orderedCards = _pathService.GetOrderedCards(singleCardList);

            Assert.Equal(singleCardList[0], orderedCards[0]);
        }
    }
}