using System;

namespace Tristan.Test 
{
  public class PrizeDistributionForPayoutPool:fit.ColumnFixture {
    private WinningsCalculator wc = new WinningsCalculator();
    public int winningCombination;
    public int PoolPercentage() 
    {
      return wc.GetPoolPercentage(winningCombination);
    }
//START:readarg
    public decimal? payoutPool;
    public decimal PrizePool()
    {
      if (payoutPool == null) payoutPool = Decimal.Parse(Args[0]);
      return wc.GetPrizePool(winningCombination, payoutPool.Value);
    }
//END:readarg
  }
}
