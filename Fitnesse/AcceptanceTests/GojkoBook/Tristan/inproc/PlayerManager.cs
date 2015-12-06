using System;
using System.Collections.Generic;
using System.Text;
using Tristan;
namespace Tristan.inproc
{
  public class PlayerManager:IPlayerManager
  {
    private Dictionary<int,PlayerInfo> _players = new Dictionary<int,PlayerInfo>();
    private Dictionary<string, PlayerInfo> _playersByName = new Dictionary<string, PlayerInfo>();
    public PlayerManager() { }
    public int RegisterPlayer(IPlayerRegistrationInfo p)
    {
      if (_playersByName.ContainsKey(p.Username)) throw new DuplicateUsernameException();
      PlayerInfo np = new PlayerInfo(p);
      _players.Add(np.PlayerId, np);
      _playersByName.Add(np.Username, np);
      return np.PlayerId;
    }

    public IPlayerInfo GetPlayer(int id)
    {
      return _players[id];
    }
    public IPlayerInfo GetPlayer(String username)
    {
      if (!_playersByName.ContainsKey(username)) throw new UnknownPlayerException();
      PlayerInfo pi = _playersByName[username];
      return pi;
    }

    public int LogIn(String username, String password)
    {
      if (!_playersByName.ContainsKey(username)) throw new UnknownPlayerException();
      PlayerInfo pi = _playersByName[username];
      if (password.Equals(pi.Password)) return pi.PlayerId;
      throw new InvalidPasswordException();
    }

    public void AdjustBalance(int playerId, decimal amount)
    {
      PlayerInfo pi = _players[playerId];
      if (amount < 0 && pi.Balance < (-1 * amount))
        throw new NotEnoughFundsException();
      pi.Balance += amount;
    }

    public void DepositWithCard(int playerId, string cardNumber, string expiryDate, decimal amount)
    {
      if (cardNumber.EndsWith("2"))
        throw new TransactionDeclinedException();
      PlayerInfo pi = _players[playerId];
      pi.Balance += amount;      
    }    
  }
}
