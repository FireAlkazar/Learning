using System;
using System.Collections.Generic;
using System.Text;
using Tristan;
namespace Tristan.inproc
{
  class Draw:IDraw
  {
    public Draw(DateTime drawDate)
    {
      this._totalSize = 0;
      this._drawDate = drawDate;
      this._isOpen = true;
      this._tickets = new List<ITicket>();
    }
    private DateTime _drawDate;
    public DateTime DrawDate
    {
      get { return _drawDate; }
    }
    private bool _isOpen;
    public bool IsOpen
    {
      get { return _isOpen; }
      set { _isOpen = value; }
    }
    private decimal _totalSize;
    public decimal TotalPoolSize
    {
      get { return _totalSize; }
    }
    private List<ITicket> _tickets;
    public ITicket[] Tickets
    {
      get { return _tickets.ToArray();}
    }
    public void AddTicket(ITicket ticket)
    {
      _tickets.Add(ticket);
      _totalSize += ticket.Value;
    }
  }
}
