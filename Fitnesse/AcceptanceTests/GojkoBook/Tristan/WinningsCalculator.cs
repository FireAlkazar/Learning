namespace Tristan 
{
  public class WinningsCalculator 
  {
    public int GetPoolPercentage(int combination) 
    {
      switch(combination) {
        case 6: return 68;
        case 5: return 10;
        case 4: return 10;
        case 3: return 12;
        default: return 0;
      }
    }
    public decimal GetPrizePool(int combination, decimal payoutPool) 
    {
      return payoutPool * GetPoolPercentage(combination) / 100;
    }
  }
}