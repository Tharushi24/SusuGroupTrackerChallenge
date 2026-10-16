
namespace SusuGroupTracker.Models;

public class Contribution
{
	public string MemberName { get; }
	public int WeekNumber { get; }
	public decimal Amount { get; }

	public Contribution(string memberName, int weekNumber, decimal amount)
	{
		MemberName = memberName;
		WeekNumber = weekNumber;
		Amount = amount;
	}
}
