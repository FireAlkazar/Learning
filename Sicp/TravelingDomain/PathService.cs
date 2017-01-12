using System;
using System.Collections.Generic;
using System.Linq;

namespace TravelingDomain
{
    public class PathService
    {
        public List<TravelCard> GetOrderedCards(List<TravelCard> cards)
        {
            if (cards == null || cards.Count == 0)
            {
                throw new ArgumentException(nameof(cards));
            }

            TravelCard firstCard = GetFirstCard(cards);
            List<TravelCard> orderedCards = GetOrderCardsByMovingFromFirstToLast(firstCard, cards);
            return orderedCards;
        }

        private List<TravelCard> GetOrderCardsByMovingFromFirstToLast(TravelCard firstCard, List<TravelCard> cards)
        {
            var result = new List<TravelCard> { firstCard };
            var dictionary = cards.ToDictionary(x => x.DeparturePoint, x => x);
            TravelCard currentCard = firstCard;

            while (dictionary.ContainsKey(currentCard.DestinationPoint))
            {
                var nextCard = dictionary[currentCard.DestinationPoint];
                result.Add(nextCard);
                currentCard = nextCard;
            }

            return result;
        }

        private TravelCard GetFirstCard(List<TravelCard> cards)
        {
            var orderedDestinationPoints = cards
                .Select(x => x.DestinationPoint)
                .OrderBy(x => x)
                .ToArray();

            foreach (var travelCard in cards)
            {
                if (HasNoCardBefore(travelCard, orderedDestinationPoints))
                {
                    return travelCard;
                }
            }

            throw new IndexOutOfRangeException("First card not found");
        }

        private bool HasNoCardBefore(TravelCard travelCard, string[] orderedDestinationPoints)
        {
            var elementIndex = Array.BinarySearch(orderedDestinationPoints, travelCard.DeparturePoint);
            return elementIndex < 0;
        }
    }
}