# BankApp

## How to run 
  1. Change "ConnectionString:Default" in `appsettings.json` value to your valid connection string and ports in `./Properties/launchSettings.json`
  2. Database should be initiated in `DatabaseSeeder.cs` with default two Customers with usernames: `johnsmith` and `janedoe` with passwords `pass123` for both.
## Actions
  `POST api/customer/` - in order to login `LoginRequest` object in the request body is needed: 
  ```
  {
    "username": "johnsmith",
    "password": "pass123"
  }
  ```
  If the login succeeds, you will receive `{id}` for the next actions, and authorization token (for example in Postman - Authorization Type: Token Bearer). Sample response:
  ```
  {
    "success": true,
    "customerId": "0d95d5f4-59f6-463f-8e68-33ed9def289d",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE2OTg2NDkxMzAsImlzcyI6ImxvY2FsaG9zdCIsImF1ZCI6ImxvY2FsaG9zdCJ9.7QSHPn0CVyJpUDhnCxjZ5gAQgWX6uCyiRCT0M6ZX9TA"
  }
  ```
  
  `GET api/customer/{id}/balance` - checks balance
  `PATCH api/customer/{id}/deposit` - deposits funds, request body requires plain, positive decimal value
  `PATCH api/customer/{id}/withdraw` - withdraws fuunds, request body requires plain, positive decimal value and cannot exceed the customer's current balance
  In order to receive real-time notifications you can access it via url `https://localhost:7098/api/customer/notifications`
