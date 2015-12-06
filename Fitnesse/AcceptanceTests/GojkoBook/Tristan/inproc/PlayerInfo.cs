using System;
using System.Collections.Generic;
using System.Text;
using Tristan;
namespace Tristan.inproc
{
  public class PlayerInfo:IPlayerInfo
  {
    private static int nextId=1;
    public PlayerInfo(IPlayerRegistrationInfo reg)
    {
      this._address = reg.Address;
      this._balance = 0;
      this._city = reg.City;
      this._country = reg.Country;
      this._name = reg.Name;
      this._postcode = reg.PostCode;
      this._playerId = nextId++;
      this._username = reg.Username;
      this._password = reg.Password;
    }
    
    private string _password;
    internal string Password { get { return _password; } }

    private string _name;
    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    private string _address;
    public string Address
    {
      get { return _address; }
      set { _address = value; }
    }

    private string _city;
    public string City
    {
      get { return _city; }
      set { _city = value; }
    }

    private string _postcode;
    public string PostCode
    {
      get { return _postcode; }
      set { _postcode = value; }
    }
    private string _country;
    public string Country
    {
      get { return _country;  }
      set { _country = value; }
    }

    private string _username;
    public string Username
    {
      get { return _username; }
      set { _username = value; }
    }

    private decimal _balance;
    public decimal Balance
    {
      get { return _balance; }
      set { _balance = value; }
    }
    private int _playerId;
    public int PlayerId
    {
      get { return _playerId; }
      set { _playerId = value; }
    }
    private bool _verified;
    public bool IsVerified { 
      get { return _verified; } 
      set { _verified = value; } 
    }
  }
}