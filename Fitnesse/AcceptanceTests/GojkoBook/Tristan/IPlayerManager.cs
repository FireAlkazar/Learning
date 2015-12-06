using System;
using System.Collections.Generic;
using System.Text;

namespace Tristan
{
  public class UnknownPlayerException : ApplicationException
  {
    public UnknownPlayerException() : base("Unknown user") { }
  }
  public class InvalidPasswordException: ApplicationException
  {
    public InvalidPasswordException():base("Invalid password"){}
  }
  public class DuplicateUsernameException : ApplicationException
  {
    public DuplicateUsernameException() : base("Duplicate username") { }
  }
  public class NotEnoughFundsException : ApplicationException
  {
    public NotEnoughFundsException() : base("Not enough funds") { }
  }
  public class TransactionDeclinedException : ApplicationException
  {
    public TransactionDeclinedException() : base("Transaction declined") { }
  }

  public interface IPlayerManager
  {
//START:registration
    int RegisterPlayer(IPlayerRegistrationInfo p);
    IPlayerInfo GetPlayer(int id);
    IPlayerInfo GetPlayer(String username);
    int LogIn(String username, String password);
//END:registration
//START:trading
    void AdjustBalance(int playerId, decimal amount);
    void DepositWithCard(int playerId, String cardNumber, 
      String expiryDate, decimal amount);
//END:trading
  }
}
