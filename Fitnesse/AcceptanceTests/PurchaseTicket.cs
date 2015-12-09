using fitlibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tristan;

namespace AcceptanceTests
{
	public class PurchaseTicket : DoFixture
	{
		public void PlayerDepositsDollarsWithCardAndExpiryDate(string username, decimal amount, string card, string expiry)
		{
			int pid = SetUpTestEnvironment.playerManager.GetPlayer(username).PlayerId;
			SetUpTestEnvironment.playerManager.DepositWithCard(pid, card, expiry, amount);
		}

		public decimal PlayerHasDollars(String username, decimal amount)
		{
			return (SetUpTestEnvironment.playerManager.GetPlayer(username).Balance);
		}

		public void PlayerBuysATicketWithNumbersForDrawOn(string username, int[] numbers, DateTime date)
		{
			PlayerBuysTicketsWithNumbersForDrawOn(username, 1, numbers, date);
		}

		public void PlayerBuysTicketsWithNumbersForDrawOn(string username, int tickets, int[] numbers, DateTime date)
		{
			int pid = SetUpTestEnvironment.playerManager.
			GetPlayer(username).PlayerId;
			SetUpTestEnvironment.drawManager.PurchaseTicket(date, pid, numbers, 10 * tickets);
		}

		public bool PoolValueForDrawOnIsDollars(DateTime date,decimal amount)
		{
			return SetUpTestEnvironment.drawManager.GetDraw(date).TotalPoolSize == amount;
		}

		private static bool CompareArrays(int[] sorted1, int[] unsorted2)
		{
			if (sorted1.Length != unsorted2.Length) return false;
			Array.Sort(unsorted2);
			for (int i = 0; i < sorted1.Length; i++)
			{
				if (sorted1[i] != unsorted2[i]) return false;
			}
			return true;
		}

		public bool TicketWithNumbersForDollarsIsRegisteredForPlayerForDrawOn(int[] numbers, decimal amount, string username, DateTime draw)
		{
			ITicket[] tck = SetUpTestEnvironment.drawManager.GetDraw(draw).Tickets;
			Array.Sort(numbers);
			foreach (ITicket ticket in tck)
			{
				if (CompareArrays(numbers, ticket.Numbers) 
					&& amount == ticket.Value 
					&& username.Equals(ticket.Holder.Username))
					return true;
			}
			return false;
		}
	}
}
