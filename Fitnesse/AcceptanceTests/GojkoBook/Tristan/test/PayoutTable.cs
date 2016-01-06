namespace Tristan.Test 
{
  public class PayoutTable:fit.ColumnFixture 
  {
    private WinningsCalculator wc=new WinningsCalculator();    
    public int winningCombination;
    public decimal payoutPool;
    public int PoolPercentage() 
    {
      return wc.GetPoolPercentage(winningCombination);
    }
    public decimal PrizePool() 
    {
      return wc.GetPrizePool(winningCombination, payoutPool);
    }
  }
}
