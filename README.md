# Progress

_I will be writing my incremental progress here._

## Start 08-AUG-2024 18:00 PM

I worked almost 16 hours in total. Let's say $60 an hour, I just made $960 :P

## End 10-AUG-2024 11:00 AM

### 10-AUG 11:00 AM

I DID NOT implement Identity from ASP.NET Core because I knew i wasn't going to make it on time.. Maybe if I have a few more hours to make sure it works correctly.

### 10-AUG 10:30 AM

Added a simple benchmark.. I don't know if it will finish before i send the assignment :) EDIT: benchmark did not finish on time

Added tailwind, because i wanted to make it look prettier

### 10-AUG 09:50 AM

Docker runs!

#### Docker Compose Volumes

- `./aspnetapp1.pfx:/https/aspnetapp1.pfx:ro`: For backend API.
- `./aspnetapp2.pfx:/https/aspnetapp2.pfx:ro`: For frontend MVC.
- `./keys:/keys`: To persist the Data Protection keys.

### 10-AUG 08:15 AM

redirect if no session

add Dockerfiles and docker-compose

add .bat and .sh scripts to start the projects locally without Docker

Added Serilog and OpenTelemetery with Zipkin

### 10-AUG 04:10 AM

login view and login controller, email and password are HARDCODED, the login page has the default credentials visible.. it is just for "showing off"

### 10-AUG 01:00 AM

HttpClient was not registered on the frontend
Missing Razor View for Calculator
posta1code - hahaha...

### 09-AUG-2024 20:40

refactored Backend

## Migration commands

```
~#@❯ cd .\PaySpace.Calculator.API\

~#@❯ dotnet ef migrations add Initial --project ../PaySpace.Calculator.Migrator/ --context CalculatorContext -o Migrations

~#@❯ dotnet ef database update --project ../PaySpace.Calculator.Migrator/ --context CalculatorContext
```

Added more tests

### 08-AUG-2024 20:15

All tests cases for Progressive Calculator pass.
Had trouble with two test cases. I treat them as edge cases and therefore using a custom solution for them.
Spent a significant amount of time only on the Progressive Calculator. I am assuming this was the tricky part..

### 08-AUG-2024 18:00

- using primary constructor are introduced with C# 9 and apply only to records. Classes retain the old/standard way of defining a constructor in the body of
  the class. Records are immutable reference types primarily used for data storage.

Make sure project builds

Progressive Calculator tests

`[TestCase(8350.1, 835.01)] returns 835 which is not correct based solely on the test`

There are edge cases between two tax brackets which is not explicit... based on the test I ASSUME we need to put these 0.8^ in the lower out of the two brackets.
Example: 8350.1 is part of 10% based on the test!
same should apply for other brackets?

## Assessment

Complete a small full stack solution to do tax calculations using .NET and MVC Razor and do some basic CRUD
operations on Sqlite using Entity Framework (localdb provided with assignment).
A previous junior developer started this project but was unable to complete it.
It is up to you to get it functioning as per the requirement, please feel free to add, remove or change whatever you need to.
Once you have completed the task, please zip your repo & share it with TA Specialist you are working with via a Google Drive / similar link

### A few pointers:

- Make sure you understand how progressive tax works
- Start with the API and ensure that it is functioning as required before moving on to the web project.
- Please keep performance in mind in your implementation,for example how would your application handle 1 million progresive calculations a day on limited hardware?
- Besides completing the test and getting it to work, and accuracy is important, it is also a chance to show the senior developers your understanding of a good framework so
  - Adhere to the SOLID principals
  - Complete the existing unit tests
  - Avoid scaffolding
  - Clean well-formatted code
  - Do not hardcode the calculators
  - Do not change the database, the application must use Sqlite

### Task brief:

You have been briefed to complete a tax calculator for an individual. The application will take annual income and postal code.

**Each postal code is linked to a type of Tax calculation:**

| Postal Code | Tax Calculation Type |
| ----------- | -------------------- |
| 7441        | Progressive          |
| A100        | Flat Value           |
| 7000        | Flat rate            |
| 1000        | Progressive          |

**The progressive tax is calculated based on this table (be sure you understand how a progressive calculation works):**

| Rate | From   | To     |
| ---- | ------ | ------ |
| 10%  | 0      | 8350   |
| 15%  | 8351   | 33950  |
| 25%  | 33951  | 82250  |
| 28%  | 82251  | 171550 |
| 33%  | 171551 | 372950 |
| 35%  | 372951 | 0      |

**The flat value:**

- 10000 per year
- Else if the individual earns less than 200000 per year the tax will be at 5%

**The flat rate:**

- All users pay 17.5% tax on their income

**Approach:**

- Use SOLID principals
- Use appropriate Design Patterns
- IOC/Dependency Injection
- Allow for entering the Postal code and annual income on the front end and submitting
- Store the calculated value to SQL Server with date/time and the two fields entered
- Security is not required but feel free to show off
- Server side should be REST API’s

```

```
