# Solinor Monthly Wage Calculation System

- Requirements are located in folder "Requirements". CSV File is also included there for testing purposes.

[![Build status](https://ci.appveyor.com/api/projects/status/7psw8kuajowhup23?svg=true)](https://ci.appveyor.com/project/miikkij/solinor-monthly-wage-calculation-system)

### WebApp Demo currently running at [http://solinor.pause.games/](http://solinor.pause.games/)

### WebApi can be test by using PostMan at [http://solinor.pause.games/api](http://solinor.pause.games/api) address
- [monthlywage/person](http://solinor.pause.games/api/monthlywage/person) - get wages and hours for each person
- [monthlywage/person/](http://solinor.pause.games/api/monthlywage/person/1){id} - get persons wage slips by person id



Things you'll need:

* [.NET Core (a.k.a ASP.NET core)](https://www.microsoft.com/net/core#windowsvs2015)

## Section Title

> This block quote is here for your information.



# Windows installation
Download [1.0.0-preview4-004233](https://github.com/dotnet/core/blob/master/release-notes/preview4-download.md) installer

# Linux Installation

Note when following instructions for Linux environment USE dotnet-dev-1.0.0-preview4-004233 version instead of dotnet-dev-1.0.0-preview2.1-003177. Applications require preview4 and for Windows and OSX there are installation packages for this. You can download those from https://github.com/dotnet/core/blob/master/release-notes/preview4-download.md 
- Follow installation instructions for different distributions at https://www.microsoft.com/net/core#linuxubuntu 


# OSX Installation 

Note when following instruction USE dotnet-dev-1.0.0-preview4-004233 version instead of dotnet-dev-1.0.0-preview2.1-003177. Applications require preview4 and for Windows and OSX there are installation packages for this. You can download those from [https://github.com/dotnet/core/blob/master/release-notes/preview4-download.md](https://github.com/dotnet/core/blob/master/release-notes/preview4-download.md)
- Install Dotnet Core to OSX by following [these instructions](https://www.microsoft.com/net/core#macos)


# Optional tools
- [Visual studio community 2015](https://www.visualstudio.com/downloads/)
- [Visual studio code](https://code.visualstudio.com/docs/?dv=osx)
    - For helpful extensions go to: View > Extensions
        - C# for Visual Studio Code (powered by OmniSharp)
        - C# XML Documentation Comments

# Source code

> git clone https://github.com/miikkij/Solinor-Monthly-Wage-Calculation-System

> cd Solinor-Monthly-Wage-Calculation-System

## Structure

- Library (Solinor.MonthlyWageCalculation)
- ConsoleApp (Solinor.MonthlyWageCalculation.ConsoleApp)
- UnitTests (Solinor.MonthlyWageCalculation.UnitTests)
- WebApp (Solinor.MonthlyWageCalculation.WebApp)

## Library

This library calculates monthly wages for personnel.

Little bit about the library functionality:

WageService is used to load and serve information of personnels wages. This service uses TinyCsvParser to parse CSV-file to get the source data. WageService inherits IWageService, which then again gives us possibility to populate data from another sources if necessary.

HourCalculation and WageCalculation is used as separate functionalities and those use specific interfaces IHourCalculation and IWageCalculation, just in case that there might be different type of calculations later on in the future and then it would be easier just to change calculations. Or in case that there might be different type of persons when need arises to calculate something differently. 

To build the library which is then used in Unit tests, Console App and Web App run next commands, you can also skip this and go straight to console app or web app and this will be build as it is included in project files as a dependency.

> cd Solinor.MonthlyWageCalculation

> dotnet restore

> dotnet build

> cd ..

## Unit tests

Hour and Wage calculations are tested here for this case and CSV data file importer that date information is parsed correctly. There are over 40 tests to make sure that personnel would get correct paycheck and that their overtime and evening compensations are calculated correctly for each month. 

To run the unit tests execute next commands.

> cd Solinor.MonthlyWageCalculation.UnitTests

> dotnet restore

> dotnet build

> dotnet test

> cd ..


## Console app

All the personnel hours are printed out and each work hours entry for each day is divided into their own rows for each month.

Monthly wage slip will show Persons name, which year and month included with information like regular, evening, overtime and total hours worked in that month and compensation.

Hour list is shown for each wage slip. Hours types are separated for each day showing if work has been evening work with different compensation (this can be also early morning work in case that person has come to work before 0600 hours). Regular working hours are from 0600 hours to 1800 hours, and after that it is considered as evening hours with different compensation. If person works more than 8 hours in single workday then rest of the hours are compensated as overtime hours. First two overtime hours are compensated 25% extra salary, two more hours after this are compensated with 50% extra salary, and rest of the hours are compensated with 100% extra salary. 

Currency used in these examples is dollar '$'.

To run the console app execute next commands.

> cd Solinor.MonthlyWageCalculation.ConsoleApp

> dotnet restore

> dotnet build

> dotnet run

Application can be also run with parameter to test other CSV-files. for example:

> dotnet run HourList201403_broken.csv

There should be handled exceptions and then application is terminated. To test that application will pass the errors just remove "return;" end of inside the exception catch and run the previous command again to see list with little bit different values and some errors printed out before that.

> cd ..


## WebApp

WebApp also provides WebApi, however the main page uses only Razor currently and basic ASP.Net (ViewMode etc). API is available and can be tested with for e.g. PostMan.

All the personnel hours are printed out and each work hours entry for each day is divided into their own rows for each month.

Monthly wage slip will show Persons name, which year and month included with information like regular, evening, overtime and total hours worked in that month and compensation.

Hour list is shown for each wage slip. Hours types are separated for each day showing if work has been evening work with different compensation (this can be also early morning work in case that person has come to work before 0600 hours). Regular working hours are from 0600 hours to 1800 hours, and after that it is considered as evening hours with different compensation. If person works more than 8 hours in single workday then rest of the hours are compensated as overtime hours. First two overtime hours are compensated 25% extra salary, two more hours after this are compensated with 50% extra salary, and rest of the hours are compensated with 100% extra salary. 

Currency used in these examples is dollar '$'.

To run the console app execute next commands.

> cd Solinor.MonthlyWageCalculation.WebApp

> dotnet restore

> dotnet build

> dotnet run

Open address http://localhost:5000 in browser

# Deployment to linux environment (Production)

- [https://docs.microsoft.com/en-us/aspnet/core/publishing/linuxproduction](https://docs.microsoft.com/en-us/aspnet/core/publishing/linuxproduction)

# Notes

- [Google sheet notes to double check my calculations](https://docs.google.com/spreadsheets/d/1ShHTJHLhGodPnhTxZZcO7WBFpfQuls4gF_B5Os0YRYM/edit?usp=sharing)