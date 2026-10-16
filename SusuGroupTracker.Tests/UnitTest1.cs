
using SusuGroupTracker.Services;

namespace SusuGroupTracker.Tests;

public class SusuGroupTests
{
	[Fact]
	public void CreateGroup_StoresWeeklyContribution()
	{
		var group = new SusuGroup(50m);

		Assert.Equal(50m, group.WeeklyContribution);
		Assert.Equal(1, group.CurrentWeek);
	}

	[Fact]
	public void AddMember_RejectsDuplicateName()
	{
		var group = new SusuGroup(50m);
		group.AddMember("Ama");

		Assert.Throws<InvalidOperationException>(
			() => group.AddMember("ama"));
	}

	[Fact]
	public void RecordContribution_AcceptsPartialPayment()
	{
		var group = new SusuGroup(50m);
		group.AddMember("Esi");

		group.RecordContribution("Esi", 30m);

		var status = Assert.Single(group.GetCurrentWeekStatus());

		Assert.Equal(30m, status.Paid);
		Assert.Equal(20m, status.Owes);
		Assert.False(status.PaidInFull);
	}

	[Fact]
	public void CloseWeek_WithUnpaidMember_DoesNotAdvanceWeek()
	{
		var group = new SusuGroup(50m);
		group.AddMember("Ama");
		group.AddMember("Kofi");

		group.RecordContribution("Ama", 50m);

		Assert.Throws<InvalidOperationException>(
			() => group.CloseWeek());

		Assert.Equal(1, group.CurrentWeek);
		Assert.Empty(group.PayoutHistory);
	}

	[Fact]
	public void CloseWeek_WhenFullyPaid_PaysCorrectPot()
	{
		var group = new SusuGroup(50m);
		group.AddMember("Ama");
		group.AddMember("Kofi");
		group.AddMember("Esi");

		group.RecordContribution("Ama", 50m);
		group.RecordContribution("Kofi", 50m);
		group.RecordContribution("Esi", 30m);
		group.RecordContribution("Esi", 20m);

		var payout = group.CloseWeek();

		Assert.Equal("Ama", payout.RecipientName);
		Assert.Equal(150m, payout.Amount);
		Assert.Equal(2, group.CurrentWeek);
	}

	[Fact]
	public void RecordContribution_RejectsUnknownMember()
	{
		var group = new SusuGroup(50m);
		group.AddMember("Ama");

		Assert.Throws<ArgumentException>(
			() => group.RecordContribution("Unknown", 20m));
	}

	[Fact]
	public void RecordContribution_RejectsInvalidAmounts()
	{
		var group = new SusuGroup(50m);
		group.AddMember("Ama");

		Assert.Throws<ArgumentException>(
			() => group.RecordContribution("Ama", 0m));

		Assert.Throws<ArgumentException>(
			() => group.RecordContribution("Ama", -10m));
	}

	[Fact]
	public void RecordContribution_RejectsOverpayment()
	{
		var group = new SusuGroup(50m);
		group.AddMember("Ama");

		group.RecordContribution("Ama", 30m);

		Assert.Throws<InvalidOperationException>(
			() => group.RecordContribution("Ama", 21m));
	}

	[Fact]
	public void Payouts_FollowJoiningOrder()
	{
		var group = new SusuGroup(50m);
		group.AddMember("Ama");
		group.AddMember("Kofi");

		group.RecordContribution("Ama", 50m);
		group.RecordContribution("Kofi", 50m);

		var firstPayout = group.CloseWeek();

		group.RecordContribution("Ama", 50m);
		group.RecordContribution("Kofi", 50m);

		var secondPayout = group.CloseWeek();

		Assert.Equal("Ama", firstPayout.RecipientName);
		Assert.Equal("Kofi", secondPayout.RecipientName);
		Assert.Equal(2, group.PayoutHistory.Count);
	}

	[Fact]
	public void AddMember_AfterPaymentsBegin_IsRejected()
	{
		var group = new SusuGroup(50m);
		group.AddMember("Ama");

		group.RecordContribution("Ama", 10m);

		Assert.Throws<InvalidOperationException>(
			() => group.AddMember("Kofi"));
	}
}
