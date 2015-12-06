using System;
using System.Collections.Generic;
using System.Text;

namespace Tristan
{
 //START:basic
  public interface ITicket
  {
    int[] Numbers { get;}
    IPlayerInfo Holder { get;}
    decimal Value {get;}
//END:basic
//START:review
    bool IsOpen { get;}
    decimal Winnings { get; }
    DateTime draw { get; }
//END:review
//START:basic
  }
 //END:basic
}
