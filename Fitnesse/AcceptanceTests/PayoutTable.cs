using fit;
using System;
using Tristan;

namespace AcceptanceTests
{
	public class PayoutTable : ColumnFixture
	{
		private WinningsCalculator wc = new WinningsCalculator();
		public int winningCombination;
		public decimal? payoutPool;
		public int PoolPercentage()
		{
			return wc.GetPoolPercentage(winningCombination);
		}
		public decimal PrizePool()
		{
			if (payoutPool == null)
			{
				payoutPool = Decimal.Parse(Args[0]);
			}

			return wc.GetPrizePool(winningCombination, payoutPool.Value);

		}
	}
}
