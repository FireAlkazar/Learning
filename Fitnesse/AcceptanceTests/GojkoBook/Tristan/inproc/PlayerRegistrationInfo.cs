using System;
using System.Collections.Generic;
using System.Text;
namespace Tristan.inproc
{
  public class PlayerRegistrationInfo: IPlayerRegistrationInfo
  {
    private string _name;
    public string Name { get { return _name; } set { _name = value; } }

    private string _address;
    public string Address { get { return _address; } set { _address = value; } }

    private string _city;
    public string City { get { return _city; } set { _city = value; } }

    private string _postCode;
    public string PostCode { get { return _postCode; } set { _postCode = value; } }

    private string _country;
    public string Country { get { return _country; } set { _country = value; } }

    private string _username;
    public string Username { get { return _username; } set { _username = value; } }

    private string _password;
    public string Password { get { return _password; } set { _password = value; } }
  }
}
