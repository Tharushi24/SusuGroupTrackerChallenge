# Project Notes

## Assumptions

1. Each member contributes a fixed amount every week.
2. Members receive the payout in the order they joined the group.
3. Every member must pay their full weekly contribution before the week can close.
4. Partial payments are allowed, and multiple payments are added together.
5. Member names are unique, regardless of uppercase or lowercase letters.
6. New members cannot be added after contributions begin, ensuring the payout amount and order remain consistent.
7. The application starts with Week 1 whenever it is launched.

## Known Limitations

1. Data is stored in memory and is lost when the application closes.
2. The application is designed for one savings group at a time.
3. The application does not include user accounts, authentication, or access control.
4. Contributions and payouts are recorded within the application; no real bank or mobile payment integration is provided.
5. The application uses a console interface rather than a graphical user interface.
6. The application does not save a permanent transaction history between sessions.

## Future Improvements

1. Add a database such as SQLite or SQL Server to store members, contributions, and payouts permanently.
2. Add data export to CSV or PDF for reports.
3. Develop a web-based or desktop graphical user interface.
4. Add transaction dates, payment references, and audit logs.
5. Support multiple savings groups.
6. Add authentication and role-based access control.
7. Improve input handling and provide clearer reports.

## Design Decisions

- Used `decimal` for monetary calculations to avoid floating-point precision issues.
- Separated member, contribution, and payout models from the group management service.
- Used validation to prevent invalid payments, duplicate members, unknown members, and overpayments.
- Added automated unit tests using xUnit to verify important business rules.
