dotnet --version
cd Solinor.MonthlyWageCalculation
dotnet restore
dotnet build
cd ..
cd Solinor.MonthlyWageCalculation.UnitTests
dotnet restore
dotnet build
cd ..
cd Solinor.MonthlyWageCalculation.ConsoleApp
dotnet restore
dotnet build
cd ..  
cd Solinor.MonthlyWageCalculation.WebApp
dotnet restore
dotnet build
cd ..  