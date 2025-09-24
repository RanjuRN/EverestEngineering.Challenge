# Everest Coding Challenge - Courier Service

A .NET 8 console application for estimating delivery costs and times for packages, featuring offer code discounts and vehicle-based delivery planning.

## Features

- **Delivery Cost Estimation:** Calculates delivery cost per package, including applicable offer code discounts.
- **Delivery Time Estimation:** Determines delivery time for each package based on vehicle availability, speed, and weight constraints.
- **Configurable Offer Codes:** Supports offer code rules via external JSON configuration.
- **Extensible Architecture:** Organized into Business, Services, Models, Interfaces, and Challenges for maintainability.

## Folder Structure

Everest.CodingChallenge.CourierService/ 
├── Business/         # Core business logic (calculators, etc.) 
├── Challenges/       # Challenge orchestrators (cost/time estimation) 
├── Controllers/      # Challenge flow controller 
├── Helpers/          # Dependency injection and utility classes 
├── Interfaces/       # Service and challenge interfaces 
├── Model/            # Domain models (Package, Vehicle, etc.) 
├── Services/         # Service implementations (OfferCode, DeliveryPlanner, etc.) 
├── Configs/          # External configuration files (OfferCodes.json) 
├── appsettings.json  # Application configuration 
├── Program.cs        # Application entry point


## Getting Started

1. **Requirements**
   - [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
   - Visual Studio 2022 or later

2. **Configuration**
   - Edit `appsettings.json` for base delivery cost and vehicle settings.
   - Update `Configs/OfferCodes.json` for offer code rules.

3. **Build & Run**

dotnet build 
dotnet run --project Everest.CodingChallenge.CourierService


4. **Usage**
- Follow console prompts to enter package and vehicle details.
- View calculated delivery costs, discounts, and estimated delivery times.

## Testing

- Unit tests are located in the `Everest.CodingChallenge.CourierService.Tests` project.
- Run tests with:

dotnet test

## Extending

- Add new business rules in the `Business/` folder.
- Implement new challenges in the `Challenges/` folder.
- Register new services via `Helpers/ServiceDependeciesCollectionManager.cs`.

## License

This project is for educational and demonstration purposes.
