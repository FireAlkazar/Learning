using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelingDomain
{
    public sealed class TravelCard
    {
        public TravelCard(string departurePoint, string destinationPoint)
        {
            DeparturePoint = departurePoint;
            DestinationPoint = destinationPoint;
        }

        public string DeparturePoint { get; }

        public string DestinationPoint { get;  }
    }
}
