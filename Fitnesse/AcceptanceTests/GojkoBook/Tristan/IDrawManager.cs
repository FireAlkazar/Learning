using System;
using System.Collections.Generic;
using System.Text;

namespace Tristan
{
  public class DrawNotOpenException : ApplicationException
  {
    public DrawNotOpenException()
      : base("Draw is closed")
    {
    }
  }
//START:code
  interface IDrawManager
  {
    IDraw GetDraw(DateTime date);
    IDraw CreateDraw(DateTime drawDate);
    void PurchaseTicket(DateTime drawDate, int playerId, 
      int[] numbers, decimal value);
//END:code
//START:settledraw
    void SettleDraw(DateTime drawDate, int[] results);
    decimal OperatorDeductionFactor { get; } 
//END:settledraw
//START:review1
    List<ITicket> GetOpenTickets(int playerId);
//END:review1
//START:review2
    List<ITicket> GetTickets(DateTime drawDate, int playerId);
//END:review2
//START:code
  }
//END:code
}
