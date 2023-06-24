# Assistants: Budget API. Integration Tests

This project houses a collection of integration tests specifically designed to validate the functionality and reliability of the Budget API solution. Tests performed on top of API controllers to verify E2E scenarious. This project is not aimed to perform Unit testing.

- [Assistants: Budget API. Integration Tests](#assistants-budget-api-integration-tests)
  - [Architecture](#architecture)
  - [Prerequisites](#prerequisites)
  - [Getting Started](#getting-started)
  - [Tests coverage](#tests-coverage)
  - [Useful links:](#useful-links)


## Architecture
- **Test engine**: [xUnit](https://www.nuget.org/packages/xunit);
- **Mocks**: [Moq](https://www.nuget.org/packages/Moq);
- **Tests Coverage Analyzer**: [coverlet.collector](https://github.com/coverlet-coverage/coverlet)
- **Tests Coverage Visualizer**: [ReportGenerator](https://github.com/danielpalme/ReportGenerator)

## Prerequisites
1. **Docker**: This project relies on Docker to set up and manage the testing environment. Make sure you have Docker installed and properly configured. You can download Docker from the official website: https://www.docker.com.
2. **ReportGenerator**: The Assistants.Budget.BE.API.Tests project generates test reports using ReportGenerator. Install the ReportGenerator tool by following the instructions provided on the GitHub repository: https://github.com/danielpalme/ReportGenerator.
   1. Detailed installation instructions: https://reportgenerator.io/usage


> **Note: If you want to add `ReportGenerator` into `path`**:
> ```bash
> cat << \EOF >> ~/.zprofile
> export PATH="$PATH:/Users/{YOUR_USER}/.dotnet/tools"
> EOF
> ```
> And run `zsh -l` to make it available for current session.
> You can only add it to the current session by running the following command:
> ```bash
> export PATH="$PATH:/Users/{YOUR_USER}/.dotnet/tools"
> ```
> And run `zsh -l` to make it available for current session.
> You can only add it to the current session by running the following command:
> ```bash
> export PATH="$PATH:/Users/eugene/.dotnet/tools"
> ```
> ___

## Getting Started

To get started with tests, follow the steps below:
1. Go to folder `./assistants-budget-be/Assistants.Budget.BE.API.Tests/`
1. Make sure that `.env` file exists and configured properly;
2. Run docker compose test environment:
   ```bash
    docker compose -f docker-compose.yml up -d
   ```
3. Execute tests:
   ```bash
    dotnet test \
         --results-directory ./TestReports \
         --collect:"XPlat Code Coverage;Format=json,cobertura" \
         /p:DeterministicSourcePaths=true -- \
         DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.DeterministicReport=true
   ```
4. After tests execution a file `./TestReports/{GUID}/coverage.cobertura.xml` should be created. Use a following command to generate UI report based on tests execution results:
   ```bash
    reportgenerator -reports:"./TestReports/{GUID}/coverage.cobertura.xml" -targetdir:"./CoverageReport" -reporttypes:Html;Badges
   ```
5. Open newly created folder `./CoverageReport`, open `index.html` and review reports;

> Steps above can be executed with a helper script:
> ```bash
>  bash generate-code-coverage.sh
> ```
>___

## Tests coverage 
Tests coverage can be reviewed by [link](https://htmlpreview.github.io/?https://github.com/lednova-irina/BudgetAssistant/tree/develop/assistants-budget-be/Assistants.Budget.BE.API.Tests/CoverageReport/index.htm) 
| Name            | Badge                                                         |
| --------------- | ------------------------------------------------------------- |
| Combined        | ![Combined](./CoverageReport/badge_combined.svg)              |
| Branch Coverage | ![Branch Coverage](./CoverageReport/badge_branchcoverage.svg) |
| Line Coverage   | ![Line Coverage](./CoverageReport/badge_linecoverage.svg)     |
| Method Coverage | ![Method Coverage](./CoverageReport/badge_methodcoverage.svg) |

## Useful links:

- https://github.com/dotnet/AspNetCore.Docs.Samples/blob/main/fundamentals/minimal-apis/samples/MinApiTestsSample/IntegrationTests/TodoEndpointsV1Tests.cs
