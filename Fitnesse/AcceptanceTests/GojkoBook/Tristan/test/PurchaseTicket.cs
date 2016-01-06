using System;
using System.Collections.Generic;
using System.Text;
using Tristan.inproc;
using fit;
namespace Tristan.Test.PurchaseTicket
{
//START:setup
  public class SetUpTestEnvironment : ColumnFixture
  {
    internal static IPlayerManager playerManager;
    internal static IDrawManager drawManager;
    public SetUpTestEnvironment()
    {
      playerManager = new PlayerManager();
      drawManager = new DrawManager(playerManager);
    }
    public DateTime CreateDraw { 
      set 
      { 
      	drawManager.CreateDraw(value); 
      } 
    }
  }
//END:setup
  public class PlayerRegisters : ColumnFixture
  {
    public class ExtendedPlayerRegistrationInfo: 
      PlayerRegistrationInfo
    {
      public int PlayerId()
      {
        return SetUpTestEnvironment.playerManager.
          RegisterPlayer(this);
      }
    }
    private ExtendedPlayerRegistrationInfo to = 
    	new ExtendedPlayerRegistrationInfo();
    public override object GetTargetObject()
    {
      return to;
    }
  }
//START:purchasetick
  public class PurchaseTicket : fitlibrary.DoFixture 
  {
    public void PlayerDepositsDollarsWithCardAndExpiryDate(
      string username, decimal amount, string card, string expiry)
    {
      int pid = SetUpTestEnvironment.playerManager.
                        GetPlayer(username).PlayerId;
      SetUpTestEnvironment.playerManager.DepositWithCard(
        pid, card, expiry, amount);
    }
    public bool PlayerHasDollars(String username, decimal amount)
    {
      return (SetUpTestEnvironment.playerManager.
          GetPlayer(username).Balance == amount);
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
      int pid = SetUpTestEnvironment.playerManager.
                         GetPlayer(username).PlayerId;
      SetUpTestEnvironment.drawManager.PurchaseTicket(
          date, pid, numbers, 10*tickets);
    }
    public bool PoolValueForDrawOnIsDollars(DateTime date, 
                          decimal amount)
    {
      return SetUpTestEnvironment.drawManager.GetDraw(date).
                     TotalPoolSize == amount;
    }
    private static bool CompareArrays(int[] sorted1, int[] unsorted2)
    {
      if (sorted1.Length != unsorted2.Length) return false;
      Array.Sort(unsorted2);
      for (int i = 0; i < sorted1.Length; i++)
      {
        if (sorted1[i] != unsorted2[i]) return false;
      }
      return true;
    }
    public bool 
         TicketWithNumbersForDollarsIsRegisteredForPlayerForDrawOn(
      int[] numbers, decimal amount, string username, DateTime draw)
    {
      ITicket[] tck = SetUpTestEnvironment.
        drawManager.GetDraw(draw).Tickets;
      Array.Sort(numbers);
      foreach (ITicket ticket in tck)
      {
        if (CompareArrays(numbers, ticket.Numbers) && 
             amount == ticket.Value && 
             username.Equals(ticket.Holder.Username))
          return true;
      }
      return false;
    }
//END:purchasetick
//START:usingcheck
  public int TicketsInDrawOn(DateTime date)
  {
      return SetUpTestEnvironment.drawManager.
        GetDraw(date).Tickets.Length;
  }
  public decimal PoolValueForDrawOnIs(DateTime date)
  {
      return SetUpTestEnvironment.drawManager.
        GetDraw(date).TotalPoolSize;
  }
  public decimal AccountBalanceFor(String username)
  {
    return SetUpTestEnvironment.playerManager.
      GetPlayer(username).Balance;
  }
//END:usingcheck
//START:purchasetick
  }
//END:purchasetick
}