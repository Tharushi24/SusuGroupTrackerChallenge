
namespace SusuGroupTracker.Models;

public class Payout
{
	public int WeekNumber { get; }
	public string RecipientName { get; }
	public decimal Amount { get; }

	public Payout(int weekNumber, string recipientName, decimal amount)
	{
		WeekNumber = weekNumber;
		RecipientName = recipientName;
		Amount = amount;
	}
}
