using System;
using System.Collections.Generic;
using System.Text;
using Tristan;
using Tristan.inproc;
using fitlibrary;
using fit;
namespace Tristan.Test.Settlement
{
//START:accafter
  internal class BalanceCheckFixture : ColumnFixture
  {
    private IPlayerManager _playerManager;
    public BalanceCheckFixture(IPlayerManager pm)
    {
      _playerManager = pm;
    }
    public String player;
    public decimal Balance
    {
      get
      {
        return _playerManager.GetPlayer(player).Balance;
      }
    }
  }
//END:accafter
//START:accbef    
  internal class CreatePlayerFixture : SetUpFixture
  {
    private IPlayerManager _playerManager;
    public CreatePlayerFixture(IPlayerManager pm)
    {
      _playerManager = pm;
    }
    public void PlayerBalance(String player, decimal balance)
    {
      PlayerRegistrationInfo p = new PlayerRegistrationInfo();
      p.Username = player; p.Name = player;
      p.Password = "XXXXXX";
      // define other mandatory properties
      int playerId = _playerManager.RegisterPlayer(p);
      _playerManager.AdjustBalance(playerId, balance);
    }
  }
//END:accbef    
//START:tickets
  internal class TicketPurchaseFixture: SetUpFixture
  {
    private IDrawManager _drawManager;
    private DateTime _drawDate;
    private IPlayerManager _playerManager;

    public TicketPurchaseFixture(IPlayerManager pm, IDrawManager dm, 
    	DateTime drawDate)
    {
      _drawManager = dm;
      _playerManager = pm;
      _drawDate = drawDate;
    }
    public void PlayerNumbersValue(String player, int[] numbers, decimal value)
    {
      _drawManager.PurchaseTicket(_drawDate, 
        _playerManager.GetPlayer(player).PlayerId, numbers, value);
    }
  }
//END:tickets
//START:dofstart
  public class SettlementTest:DoFixture
  {
    private IDrawManager drawManager;
    private IPlayerManager playerManager;
    private DateTime drawDate;   
    public SettlementTest()
    {
      playerManager = new PlayerManager();
      drawManager = new DrawManager(playerManager);
      drawDate = DateTime.Now;
      drawManager.CreateDraw(drawDate);
    }
//END:dofstart
//START:doftickets
    public Fixture TicketsInTheDraw()
    {
      return new TicketPurchaseFixture(playerManager, drawManager, drawDate);
    }
//END:doftickets
//START:dofdraw
    public void DrawResultsAre(int[] numbers)
    {
      drawManager.SettleDraw(drawDate, numbers);
    }
//END:dofdraw
//START:dofaccafter
    public Fixture AccountsAfterTheDraw()
    {
      return new BalanceCheckFixture(playerManager);
    }
//END:dofaccafter
//START:dofaccbef    
    public Fixture AccountsBeforeTheDraw()
    {
      return new CreatePlayerFixture(playerManager);
    }
//END:dofaccbef    
//START:dofstart
  }
//END:dofstart
}
