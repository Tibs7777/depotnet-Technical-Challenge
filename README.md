
# Refactoring Challenge
 
## Introduction
This project requires Visual Studio, .NET, C#, SQL Server Management Studio (SSMS), and includes Swagger UI for API documentation.

## Tools Installation

### 1. Visual Studio

1. Navigate to the Visual Studio downloads page: [Visual Studio Downloads](https://visualstudio.microsoft.com/downloads/)
2. Download the Community edition.
3. Run the installer and select the following workloads: 
    - .NET desktop development
    - ASP.NET and web development
4. Click "Install" and wait for the process to complete.

### 2. .NET

1. Navigate to the .NET downloads page: [.NET Downloads](https://dotnet.microsoft.com/download)
2. Download the .NET SDK (Software Development Kit).
3. Run the installer and follow the prompts.

### 3. SQL Server Management Studio (SSMS)

1. Navigate to the SSMS download page: [SSMS Download](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
2. Download the latest stable version of SSMS.
3. Run the installer and follow the prompts.

### Database setup
1. Please follow the instruction to download and setup Northwind DB for SQL Server here: [Northwind DB for Sql Server](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/linq/downloading-sample-databases)
2. Once you have the sql script downloaded, press CTRL + O or navigate to File => Open => File and select the script.
3. Create a database called Northwind and run the script against it. Your Northwind DB should now be seeded with values

## Clone the Repository

Clone the repository from github by selecting the green code button, and using git clone on the url in the directory you wish to create this project in.

## Run the project

1. Open the project solution with Visual Studio
2. Press the green play button likely labelled 'IIS Express'

## Accessing Swagger UI

Once the service is running, you can access Swagger UI by navigating to `http://localhost:<port>/swagger` in your web browser, replacing `<port>` with the port number where your service is running.

With Swagger UI, you can view the service's API documentation and test its endpoints.

## What I would do differently for production:
 - Add a rate limiter
 - Use CORS
 - Add logging
 - Ensure the prod db connection string is behind any secret storing method, like env variables, an uncomitted secrets file, or Azure Key Vault
 - Add Authentication (But this is more of a feature, and may not be needed in all apps)

I have added several more comments inside of things I would do given more time on the project
