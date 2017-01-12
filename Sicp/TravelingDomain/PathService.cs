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

            var firstCard = GetFirstCard(cards);
            var orderedCards = GetOrderedCardsByMovingFromFirstToLast(firstCard, cards);
            return orderedCards;
        }

        private TravelCard GetFirstCard(List<TravelCard> cards)
        {
            string[] orderedDestinationPoints = cards
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

            throw new InvalidOperationException("First card not found");
        }

        private List<TravelCard> GetOrderedCardsByMovingFromFirstToLast(TravelCard firstCard, List<TravelCard> cards)
        {
            var result = new List<TravelCard> {firstCard};
            var dictionary = cards.ToDictionary(x => x.DeparturePoint, x => x);
            var currentCard = firstCard;

            while (dictionary.ContainsKey(currentCard.DestinationPoint))
            {
                var nextCard = dictionary[currentCard.DestinationPoint];
                result.Add(nextCard);
                currentCard = nextCard;
            }

            return result;
        }

        private bool HasNoCardBefore(TravelCard travelCard, string[] orderedDestinationPoints)
        {
            int elementIndex = Array.BinarySearch(orderedDestinationPoints, travelCard.DeparturePoint);
            return elementIndex < 0;
        }
    }
}