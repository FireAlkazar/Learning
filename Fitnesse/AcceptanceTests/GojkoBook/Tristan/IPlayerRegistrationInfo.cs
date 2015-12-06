using System;
using System.Collections.Generic;
using System.Text;

namespace Tristan
{
  public interface IPlayerRegistrationInfo
  {
    string Name { get;}
    string Address { get;}
    string City { get;}
    string PostCode { get;}
    string Country { get;}
    string Username { get;}
    string Password { get;}
  }
}
