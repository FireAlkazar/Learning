using fit;
using Tristan.inproc;
using System;
namespace Tristan.Test
{
//START:setup
  public class SetUpTestEnvironment : Fixture
  {
    internal static IPlayerManager playerManager;
    public SetUpTestEnvironment()
    {
      playerManager = new PlayerManager();
    }
  }
//END:setup
}
namespace Tristan.Test.FirstTry
{
//START:firsttrycheck
  public class PlayerRegisters : ColumnFixture
  {
    public string Username;
    public string Password;
    public int PlayerId()
    {
      PlayerRegistrationInfo reg = new PlayerRegistrationInfo();
      reg.Username = Username;
      reg.Password = Password;
      return SetUpTestEnvironment.playerManager.RegisterPlayer(reg);
    }
  }
  public class CheckStoredDetails : ColumnFixture
  {
    public int PlayerId;
    public string Username 
    { 
      get 
      { 
        return SetUpTestEnvironment.playerManager.
          GetPlayer(PlayerId).Username; 
      } 
    }
    public decimal Balance 
    { 
      get 
      { 
        return SetUpTestEnvironment.playerManager.
          GetPlayer(PlayerId).Balance; 
      } 
    }
  }
//END:firsttrycheck
//START:firsttrylogin
  public class CheckLogIn:ColumnFixture{
    public string Username;
    public string Password;
    public bool CanLogIn()
    { 
      try 
      {
        SetUpTestEnvironment.playerManager.LogIn(Username, Password);
        return true;
      }
      catch (ApplicationException)
      {
        return false;
      }
    }
  }
//END:firsttrylogin
//END:firsttry
}
namespace Tristan.Test.SecondTry
{
//START:secondreg
  public class PlayerRegisters : ColumnFixture
  {
    public class ExtendedPlayerRegistrationInfo: PlayerRegistrationInfo 
    {
      public int PlayerId() 
      {
        return SetUpTestEnvironment.playerManager.RegisterPlayer(this);
      }  
    }
    private ExtendedPlayerRegistrationInfo to = 
      new ExtendedPlayerRegistrationInfo();
    public override object  GetTargetObject() 
    {
        return to;
    }
  }
//END:secondreg
//START:secondcheck
  public class CheckStoredDetailsFor : ColumnFixture
  {
    public override object GetTargetObject() 
    {
      int newid=(int)Fixture.Recall(Args[0]);
      return SetUpTestEnvironment.playerManager.GetPlayer(newid);    
    }      
  }
//END:secondcheck
//START:secondlogin
  public class CheckLogIn : ColumnFixture 
  {
    public string Username;
    public string Password;
    public int LoggedInAsPlayerId() 
    {
      return SetUpTestEnvironment.playerManager.
        LogIn(Username, Password);
    }
  }
//END:secondlogin
}
