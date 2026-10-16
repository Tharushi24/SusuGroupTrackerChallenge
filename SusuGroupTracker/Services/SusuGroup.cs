
using SusuGroupTracker.Models;

namespace SusuGroupTracker.Services;

public class SusuGroup
{
	private readonly List<Member> _members = new();
	private readonly List<Contribution> _contributions = new();
	private readonly List<Payout> _payoutHistory = new();

	private int _nextPayoutIndex;
	private bool _membershipLocked;

	public decimal WeeklyContribution { get; }

	public int CurrentWeek { get; private set; } = 1;

	public IReadOnlyList<Member> Members => _members.AsReadOnly();

	public IReadOnlyList<Payout> PayoutHistory =>
		_payoutHistory.AsReadOnly();

	public SusuGroup(decimal weeklyContribution)
	{
		if (weeklyContribution <= 0)
		{
			throw new ArgumentException(
				"Weekly contribution must be greater than zero.");
		}

		WeeklyContribution = weeklyContribution;
	}

	public void AddMember(string name)
	{
		if (_membershipLocked)
		{
			throw new InvalidOperationException(
				"Members cannot be added after contributions begin.");
		}

		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException(
				"Member name cannot be empty.");
		}

		string normalizedName = name.Trim();

		bool alreadyExists = _members.Any(member =>
			string.Equals(
				member.Name,
				normalizedName,
				StringComparison.OrdinalIgnoreCase));

		if (alreadyExists)
		{
			throw new InvalidOperationException(
				$"Member '{normalizedName}' already exists.");
		}

		_members.Add(new Member(normalizedName));
	}

	public void RecordContribution(string memberName, decimal amount)
	{
		if (string.IsNullOrWhiteSpace(memberName))
		{
			throw new ArgumentException(
				"Member name cannot be empty.");
		}

		if (amount <= 0)
		{
			throw new ArgumentException(
				"Payment amount must be greater than zero.");
		}

		Member? member = _members.FirstOrDefault(m =>
			string.Equals(
				m.Name,
				memberName.Trim(),
				StringComparison.OrdinalIgnoreCase));

		if (member is null)
		{
			throw new ArgumentException(
				$"Unknown member: '{memberName}'.");
		}

		decimal alreadyPaid = _contributions
			.Where(c =>
				c.WeekNumber == CurrentWeek &&
				c.MemberName == member.Name)
			.Sum(c => c.Amount);

		decimal amountOwed = WeeklyContribution - alreadyPaid;

		if (amount > amountOwed)
		{
			throw new InvalidOperationException(
				$"{member.Name} owes only GHS {amountOwed:F2}.");
		}

		_contributions.Add(
			new Contribution(member.Name, CurrentWeek, amount));

		_membershipLocked = true;
	}

	public IReadOnlyList<(
		string Name,
		decimal Paid,
		decimal Owes,
		bool PaidInFull)> GetCurrentWeekStatus()
	{
		return _members.Select(member =>
		{
			decimal paid = _contributions
				.Where(c =>
					c.WeekNumber == CurrentWeek &&
					c.MemberName == member.Name)
				.Sum(c => c.Amount);

			decimal owes = WeeklyContribution - paid;

			return (
				Name: member.Name,
				Paid: paid,
				Owes: owes,
				PaidInFull: owes == 0
			);
		}).ToList();
	}

	public Payout CloseWeek()
	{
		if (_members.Count == 0)
		{
			throw new InvalidOperationException(
				"Add at least one member before closing a week.");
		}

		var unpaidMembers = GetCurrentWeekStatus()
			.Where(member => !member.PaidInFull)
			.ToList();

		if (unpaidMembers.Count > 0)
		{
			string details = string.Join(
				Environment.NewLine,
				unpaidMembers.Select(member =>
					$"{member.Name} owes GHS {member.Owes:F2}"));

			throw new InvalidOperationException(
				$"Week {CurrentWeek} cannot close. " +
				$"Outstanding payments:{Environment.NewLine}{details}");
		}

		decimal pot = WeeklyContribution * _members.Count;

		Member recipient = _members[_nextPayoutIndex];

		var payout = new Payout(
			CurrentWeek,
			recipient.Name,
			pot);

		_payoutHistory.Add(payout);

		_nextPayoutIndex =
			(_nextPayoutIndex + 1) % _members.Count;

		CurrentWeek++;

		// Membership remains locked for all future weeks.
		return payout;
	}
}
