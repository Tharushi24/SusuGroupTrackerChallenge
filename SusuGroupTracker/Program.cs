
using SusuGroupTracker.Services;

Console.WriteLine("==================================");
Console.WriteLine("       SUSU GROUP TRACKER");
Console.WriteLine("==================================");

decimal weeklyAmount;

while (true)
{
	Console.Write("Enter weekly contribution in GHS: ");

	if (decimal.TryParse(Console.ReadLine(), out weeklyAmount)
		&& weeklyAmount > 0)
	{
		break;
	}

	Console.WriteLine("Enter a valid amount greater than zero.");
}

var group = new SusuGroup(weeklyAmount);

Console.WriteLine("\nEnter member names. Type 'done' when finished.");

while (true)
{
	Console.Write("Member name: ");
	string name = Console.ReadLine() ?? "";

	if (name.Trim().Equals("done",
		StringComparison.OrdinalIgnoreCase))
	{
		break;
	}

	try
	{
		group.AddMember(name);
		Console.WriteLine("Member added successfully.");
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Error: {ex.Message}");
	}
}

if (group.Members.Count == 0)
{
	Console.WriteLine("No members added. Exiting program.");
	return;
}

bool running = true;

while (running)
{
	Console.WriteLine($"\n========== WEEK {group.CurrentWeek} ==========");
	Console.WriteLine("1. Record contribution");
	Console.WriteLine("2. View current week report");
	Console.WriteLine("3. Close week and pay pot");
	Console.WriteLine("4. View payout history");
	Console.WriteLine("5. Exit");
	Console.Write("Choose an option: ");

	string choice = Console.ReadLine() ?? "";

	try
	{
		switch (choice)
		{
			case "1":
				Console.Write("Member name: ");
				string memberName = Console.ReadLine() ?? "";

				Console.Write("Amount paid (GHS): ");

				if (!decimal.TryParse(
					Console.ReadLine(), out decimal amount))
				{
					Console.WriteLine("Invalid amount.");
					break;
				}

				group.RecordContribution(memberName, amount);

				Console.WriteLine(
					$"Payment of GHS {amount:F2} recorded.");
				break;

			case "2":
				Console.WriteLine(
					$"\nWeek {group.CurrentWeek} Report");

				foreach (var member in group.GetCurrentWeekStatus())
				{
					string status = member.PaidInFull
						? "PAID IN FULL"
						: "OUTSTANDING";

					Console.WriteLine(
						$"{member.Name}: Paid GHS {member.Paid:F2}, " +
						$"Owes GHS {member.Owes:F2} - {status}");
				}
				break;

			case "3":
				var payout = group.CloseWeek();

				Console.WriteLine(
					$"Week {payout.WeekNumber} closed successfully!");

				Console.WriteLine(
					$"{payout.RecipientName} received " +
					$"GHS {payout.Amount:F2}.");

				Console.WriteLine(
					$"Week {group.CurrentWeek} has started.");
				break;

			case "4":
				Console.WriteLine("\nPayout History");

				if (group.PayoutHistory.Count == 0)
				{
					Console.WriteLine("No payouts yet.");
				}
				else
				{
					foreach (var payoutRecord in group.PayoutHistory)
					{
						Console.WriteLine(
							$"Week {payoutRecord.WeekNumber}: " +
							$"{payoutRecord.RecipientName} - " +
							$"GHS {payoutRecord.Amount:F2}");
					}
				}
				break;

			case "5":
				running = false;
				Console.WriteLine("Thank you for using Susu Group Tracker.");
				break;

			default:
				Console.WriteLine("Invalid option. Choose 1 to 5.");
				break;
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Error: {ex.Message}");
	}
}
