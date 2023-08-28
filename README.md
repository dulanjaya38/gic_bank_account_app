# GIC_BANKACCOUNT_APP

Test Project
Developed by Sanjana Ratnayake

# Requirements

1. .NET 7.0 SDK
2. SQL Server 2019 0r higher
3. Visual Studio 2022

# How to Run

1. Change configurations.

    Change connection string values in the appSettings.json.
        File path: GIC_BANK_APP\GIC.BANKACCOUNT.APP\appSettings.json
        Section: ConnectionStrings -> AppDbConnectionString

    Change log file path.
        File path: GIC_BANK_APP\GIC.BANKACCOUNT.APP\appSettings.json
        Section: Serilog -> WriteTo -> Args -> path

2. Set project named "GIC.BANKACCOUNT.APP" as the startup project.

3.	Create Database

    Option 1: Using EF Migrations
             - Open Tools -> NuGet Package Manager -> Package Manager Console
             - Select Default project as "GIC.BANKACCOUNT.APP"
             - Run "update-database" command 
             - Database will be created in the SQL server.

    Option 2: Using backup file
             - Restore database using SSMS.
             - Backup files are given separately with the package. 
             - To restore fresh database please use below backup file:
               "GIC_BANKACCOUNT_APP_DB.bak"
             - To restore the database with sample data, then please use below backup file:
               "GIC_BANKACCOUNT_APP_DB_WithData.bak"

 3. Run application. 

