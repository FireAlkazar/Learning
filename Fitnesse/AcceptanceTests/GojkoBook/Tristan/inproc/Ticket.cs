using System;
using System.Collections.Generic;
using System.Text;

namespace Tristan.inproc
{
  class Ticket:ITicket
  {
    public Ticket(IPlayerInfo holder, DateTime draw, int[] numbers, decimal value)
    {
      _numbers = new int[numbers.Length];
      System.Array.Copy(numbers, _numbers, numbers.Length);
      _holder = holder;
      _value = value;
      _open = true;
      _winnings = 0;
      _drawDate = draw;
    }
    private int[] _numbers;
    public int[] Numbers
    {
      get { return _numbers; }
    }

    private IPlayerInfo _holder;
    public IPlayerInfo Holder
    {
      get { return _holder; }
    }
    private decimal _value;
    public decimal Value
    {
      get { return _value; }
    }

    private bool _open;
    public bool IsOpen
    {
      get { return _open; }
      set { _open = value; }
    }
    private decimal _winnings;
    public decimal Winnings
    {
      get { return _winnings; }
      set { _winnings = value; }
    }
    private DateTime _drawDate;
    public DateTime draw
    {
      get { return _drawDate; }
    }
  }
}
