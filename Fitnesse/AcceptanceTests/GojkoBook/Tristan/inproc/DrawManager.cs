using System;
using System.Collections.Generic;
using System.Text;
using Tristan;
namespace Tristan.inproc
{
  public class DrawManager : IDrawManager
  {
    private Dictionary<DateTime, Draw> _draws=new Dictionary<DateTime,Draw>();
    private IPlayerManager _playerManager;
    public DrawManager(IPlayerManager playerMgr)
    {
      this._playerManager = playerMgr;
    }
    public IDraw GetDraw(DateTime date)
    {
      return _draws[date];
    }    
    public IDraw CreateDraw(DateTime drawDate)
    {
      Draw d = new Draw(drawDate);
      _draws[drawDate] = d;
      return d;
    }
    public void PurchaseTicket(DateTime drawDate, int playerId, int[] numbers, decimal value)
    {
      if (!_draws.ContainsKey(drawDate))
        throw new DrawNotOpenException();
      Draw d = _draws[drawDate];
      IPlayerInfo player=_playerManager.GetPlayer(playerId);
      _playerManager.AdjustBalance(playerId, -1 * value);
      d.AddTicket(new Ticket(player,drawDate, numbers,value));
    }
    private int CountCommonElements(int[] array1, int[] array2)
    {
      int common = 0;
      foreach (int i in array1)
        foreach (int j in array2)
          if (i == j) common++;
      return common;
    }
    public void SettleDraw(DateTime drawDate, int[] results)
    {
      WinningsCalculator wc = new WinningsCalculator();
      Draw d = _draws[drawDate];
      d.IsOpen = false;
      Dictionary<int, List<Ticket>> ticketCategories=SplitTicketsIntoCategories(results, d);
      for (int i = 0; i <= results.Length; i++)
      {
        decimal prizePool=wc.GetPrizePool(i, d.TotalPoolSize * (1-OperatorDeductionFactor));
        foreach (Ticket t in ticketCategories[i])
        {
          t.IsOpen = false;
          if (prizePool > 0)
          {
            decimal totalTicketValue = GetTotalTicketValue(ticketCategories[i]);
            t.Winnings = t.Value * prizePool / totalTicketValue;
            _playerManager.AdjustBalance(t.Holder.PlayerId,
              t.Winnings);
          }
        }
      }
    }

    private static decimal GetTotalTicketValue(List<Ticket> tickets)
    {
      decimal totalTicketValue = 0;
      foreach (Ticket t in tickets)
        totalTicketValue += t.Value;
      return totalTicketValue;
    }
    public decimal OperatorDeductionFactor { get { return 0.5m; } }
    private Dictionary<int, List<Ticket>> SplitTicketsIntoCategories(int[] results, Draw d)
    {
      Dictionary<int, List<Ticket>> ticketcategories = new Dictionary<int, List<Ticket>>();
      for (int i = 0; i <= results.Length; i++)
        ticketcategories[i] = new List<Ticket>();
      foreach (Ticket t in d.Tickets)
      {
        int c = CountCommonElements(t.Numbers, results);
        ticketcategories[c].Add(t);
      }
      return ticketcategories;
    }

     
    public List<ITicket> GetOpenTickets(int playerId)
    {
      List<ITicket> tickets = new List<ITicket>();
      foreach (Draw d in _draws.Values)
      {
        if (d.IsOpen){
          tickets.AddRange(GetTickets(d.DrawDate,playerId));
        }
      }
      return tickets;
    }
    public List<ITicket> GetTickets(DateTime drawDate, int playerId)
    {
      List<ITicket> tickets = new List<ITicket>();
      foreach (Ticket t in _draws[drawDate].Tickets)
      {
        if (t.Holder.PlayerId == playerId)
          tickets.Add(t);
      }
      return tickets;
    }
  }
}
