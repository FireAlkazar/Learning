using System;
using System.Collections.Generic;
using System.Text;
using Tristan;
using Tristan.inproc;
using fit;
namespace Tristan.Test
{
//START:review1
  public class ReviewTickets:fitlibrary.DoFixture
  {    
    private IDrawManager _drawManager;
    private IPlayerManager _playerManager;
    public ReviewTickets()
    {
      _playerManager = new PlayerManager();
      _drawManager = new DrawManager(_playerManager);
    }
    public void DrawOnIsOpen(DateTime drawDate)
    {
      _drawManager.CreateDraw(drawDate);
    }
    public void PlayerOpensAccountWithDollars(String player, decimal balance)
    {
      PlayerRegistrationInfo p = new PlayerRegistrationInfo();
      p.Username = player; p.Name = player;
      p.Password = "XXXXXX";
      // define other mandatory properties
      int playerId = _playerManager.RegisterPlayer(p);
      _playerManager.AdjustBalance(playerId, balance);
    }
    public void PlayerBuysATicketWithNumbersForDrawOn(
      string username, int[] numbers, DateTime date)
    {
      PlayerBuysTicketsWithNumbersForDrawOn(username, 1, numbers, date);
    }

    public void PlayerBuysTicketsWithNumbersForDrawOn(
      string username, int tickets, int[] numbers, DateTime date)
    {
      int pid = _playerManager.GetPlayer(username).PlayerId;
      _drawManager.PurchaseTicket(date, pid, numbers, 10 * tickets);
    }
    public IList<ITicket> PlayerListsOpenTickets(String player)
    {
      return _drawManager.GetOpenTickets(
        _playerManager.GetPlayer(player).PlayerId);
    }
//END:review1
//START:review2
    public IList<ITicket> PlayerListsTicketsForDrawOn(
      String player, DateTime date)
    {
      return _drawManager.GetTickets(
        date,_playerManager.GetPlayer(player).PlayerId);
    }
//END:review2
//START:review3
    public void NumbersAreDrawnOn(int[] numbers, DateTime date)
    {
      _drawManager.SettleDraw(date, numbers);
    }
//END:review3
//START:review1
  }
//END:review1
  public class ReviewTicketsWithRowFixture : fitlibrary.DoFixture
  {
    private IDrawManager _drawManager;
    private IPlayerManager _playerManager;
    public ReviewTicketsWithRowFixture()
    {
      _playerManager = new PlayerManager();
      _drawManager = new DrawManager(_playerManager);
    }
    public void DrawOnIsOpen(DateTime drawDate)
    {
      _drawManager.CreateDraw(drawDate);
    }
    public void PlayerOpensAccountWithDollars(
      String player, decimal balance){
      PlayerRegistrationInfo p = new PlayerRegistrationInfo();
      p.Username = player; p.Name = player;
      p.Password = "XXXXXX";
      // define other mandatory properties
      int playerId = _playerManager.RegisterPlayer(p);
      _playerManager.AdjustBalance(playerId, balance);
    }
    public void PlayerBuysATicketWithNumbersForDrawOn(
      string username, int[] numbers, DateTime date)
    {
      PlayerBuysTicketsWithNumbersForDrawOn(
        username, 1, numbers, date);
    }

    public void PlayerBuysTicketsWithNumbersForDrawOn(
      string username, int tickets, int[] numbers, DateTime date)
    {
      int pid = _playerManager.GetPlayer(username).PlayerId;
      _drawManager.PurchaseTicket(date, pid, numbers, 10 * tickets);
    }
//START:rowfix
    public RowFixture PlayerListsOpenTickets(String player)
    {
      return new TicketRowFixture(
      	_drawManager.GetOpenTickets(
      	  _playerManager.GetPlayer(player).PlayerId));
    }
    public RowFixture PlayerListsTicketsForDrawOn(
      String player, DateTime date)
    {
      return new TicketRowFixture(
      	_drawManager.GetTickets(date, 
      	  _playerManager.GetPlayer(player).PlayerId));
    }
//END:rowfix
    public void NumbersAreDrawnOn(int[] numbers, DateTime date)
    {
      _drawManager.SettleDraw(date, numbers);
    }
  }
//START:rowfixclass
  public class TicketRowFixture : fit.RowFixture
  {
    private List<ITicket> _internalList;
    public TicketRowFixture(List<ITicket> tickets)
    {
      _internalList = tickets;
    }
    public override Type GetTargetClass()
    {
      return typeof(ITicket);
    }

    public override object[] Query()
    {
      return _internalList.ToArray();
    }
  }
//END:rowfixclass
}