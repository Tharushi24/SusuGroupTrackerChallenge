# Susu Group Tracker

## Project Overview

Susu Group Tracker is a C# console application that manages a rotating savings group. Members contribute a fixed amount each week, and the total collected amount is paid to one member in joining order.

## Features

- Create a savings group with a fixed weekly contribution.
- Add members with unique names.
- Record full or partial contributions.
- View each member's payments and outstanding balance.
- Prevent week closure when any member has an outstanding balance.
- Distribute the collected pot in joining order.
- View payout history.
- Validate duplicate members, unknown members, invalid payments, and overpayments.
- Use `decimal` for accurate monetary calculations.

## Technologies Used

- C#
- .NET 10
- xUnit
- Visual Studio Code
- Git

## Prerequisites

Install the following before running the application:

- .NET 10 SDK
- Git (optional, for version control)

## How to Run

1. Clone or download this repository.
2. Open a terminal in the project root folder.
3. Build the solution:

   ```bash
   dotnet build SusuGroupTracker.sln
   ```

4. Run the application:

   ```bash
   dotnet run --project SusuGroupTracker/SusuGroupTracker.csproj
   ```

5. Enter the weekly contribution amount.
6. Add each member separately, typing `done` when finished.
7. Use the menu to record contributions, view reports, close weeks, and view payout history.

## How to Run Tests

Run the automated tests from the project root folder:

```bash
dotnet test SusuGroupTracker.sln
```

The test suite covers group creation, duplicate members, partial payments, unpaid members, payout calculations, invalid payments, overpayments, payout order, and membership restrictions.

## Business Rules

- The weekly contribution must be greater than zero.
- Member names must be unique, ignoring differences in letter case.
- Contributions may be paid in multiple instalments.
- A member cannot pay more than the amount they owe for the current week.
- A week can close only when every member has paid the full contribution.
- The payout amount equals the weekly contribution multiplied by the number of members.
- Payouts follow the order in which members joined.
- New members cannot be added after contributions begin.

## Project Structure

```text
SusuGroupTrackerChallenge/
├── SusuGroupTracker.sln
├── README.md
├── NOTES.md
├── SusuGroupTracker/
│   ├── Models/
│   │   ├── Member.cs
│   │   ├── Contribution.cs
│   │   └── Payout.cs
│   ├── Services/
│   │   └── SusuGroup.cs
│   ├── Program.cs
│   └── SusuGroupTracker.csproj
└── SusuGroupTracker.Tests/
    ├── UnitTest1.cs
    └── SusuGroupTracker.Tests.csproj
```

## Author

Tharushi Weerasekara
