# Sample Humble Object

This project demonstrates the Humble Object pattern in .NET. It utilizes [Spectre.Console](https://spectreconsole.net/)
for console UI and [Microsoft.Extensions.DependencyInjection](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)
for dependency injection.

## Table of Contents

- [Introduction](#introduction)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [Project Structure](#project-structure)
- [Contributing](#contributing)

## Introduction

The Humble Object pattern is used to separate complex logic from infrastructure code, making the logic easier to test.
This project provides a simple example of how to implement this pattern in a .NET application.

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Installation

1. Clone the repository:
   ```sh
   git clone https://github.com/singhgarima/sample-humble-object.git
   cd sample-humble-object
    ```
1. Restore the dependencies:
   ```sh
   dotnet restore
   ```

## Usage

### Running the Application

To run the application, use the following command:

```sh
dotnet run
```

### Running the tests

```sh
dotnet test --settings test.runsettings
```

#### Test Coverage

You would need to install `dotnet-reportgenerator-globaltool`

> dotnet tool install -g dotnet-reportgenerator-globaltool

```sh
reportgenerator -reports:"./tests/**/TestResults/**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
```

## Project Structure

The project is organized as follows:

* `src/`: Contains the main application code.
    * `Program.cs`: Entry point of the application.
    * `Services/`: Contains service classes encapsulating the API calls.
    * `Commands/`: Contains classes related to the CLI user interface and execution logic.
    * `CommandManager`: Manages the command initialisation, configuration and their execution.
* `tests/`: Contains unit tests for the application

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request or open an Issue if you have any improvements or
suggestions.

### Steps to Contribute

* Fork the repository.
* Create a feature branch (`git checkout -b feature-branch`).
* Commit your changes (`git commit -m 'Add some feature'`).
* Push to the branch (`git push origin feature-branch`).
* Create a new Pull Request.
