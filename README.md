
# 📦 PaymentGateway Challenge

A payment gateway, an API based application that will allow a merchant to offer a way for their shoppers to pay for their product.


## 🧰 Tech Stack

- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [FluentValidation](https://docs.fluentvalidation.net/en/latest/)
- [NUnit](https://nunit.org/)
- [NSubstitute](https://nsubstitute.github.io/)

## 📁 Project Structure
```
src/
    PaymentGateway.Api
        Controllers - API Controllers
        Enums - Enums used in the project
        Models - Different models such as requests, responses and database equivalent
        Repositories - The data access layer
        Services - Business logic
        Validation - FluentValidation validators
test/
    PaymentGateway.Api.Tests
	    Controllers - tests for controllers
	    Services - tests for services
	    Validation - tests for validation layer
imposters/ - contains the bank simulator configuration. Don't change this

.editorconfig - don't change this. It ensures a consistent set of rules for submissions when reformatting code
docker-compose.yml - configures the bank simulator
PaymentGateway.sln
```

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- IDE like [Visual Studio 2022 version 17.8 or higher.](https://visualstudio.microsoft.com/)
- [Docker](https://www.docker.com/)

### Clone and Build

```bash
git clone https://github.com/JakeLRL/payment-gateway-challenge-jacoblittle.git
cd payment-gateway-challenge-jacoblittle
dotnet build
```

Or

```
Clone through Visual Studio
Build (Ctrl + Shift + B)
```

### Run the App

```
dotnet run --project src/PaymentGateway.Api
```
[Swagger Url](https://localhost:7092/swagger/index.html)

Or

Run directly through Visual Studio with "PaymentGateway.Api" as the startup project

Then

Start the Simulator using the command
```
docker-compose up
```

## 🧪 Testing

Tests are located in the tests/ProjectName.Tests directory. The test suite uses:

    NUnit for assertions and test structure

    NSubstitute for mocking dependencies

### Run Tests

```
dotnet test
```

Or

Run directly in the test explorer

Common issue
    To run the tests in Visual Studio you will need "NUnit Test Generator VS2022" extension installed.

## ✅ Validation

We use FluentValidation to validate Requests.
[FluentValidation Docs](https://docs.fluentvalidation.net/en/latest/)