using System;
using System.Collections.Generic;
using System.Text;

namespace Tristan
{
//START:basic
  public interface IDraw
  {
    DateTime DrawDate { get; }
    bool IsOpen { get; }
    decimal TotalPoolSize { get;}

    ITicket[] Tickets { get;}
    void AddTicket(ITicket ticket);
  }
//END:basic
}
