# Homework Exercise - IBT Message Processing

## Overview
This project is a solution to the coding exercise for processing Internet Based Terms (IBT) messages. The application consumes IBT XML messages from a local directory, processes them, logs the `EventType` and timestamp to a database, and notifies two partners (Partner A via email, Partner B via an XML file). The solution is designed to be modular, extensible, and maintainable, with a focus on clean architecture and best practices.

### Features
- **File Reading**: Reads IBT XML files from `data/inbox`.
- **XML Parsing**: Extracts required fields (`EventType`, `ProductNameFull`, `IBTTypeCode`, `ISIN`) from IBT messages.
- **Database Logging**: Logs `EventType` and timestamp to an in-memory database (simulated, per requirements).
- **Partner Notifications**:
  - Partner A: Simulates sending an email with message details.
  - Partner B: Saves an XML file to `data/outbox` if `EventType` is "9097".
- **File Archiving**: Moves processed files to `data/archive`.
- **Scheduled Processing**: Processes messages every minute using a hosted service.
- **Structured Logging**: Uses Serilog for structured logging to the console.

## Project Structure
The solution follows a clean architecture approach with separation of concerns:

```
Homework_Exercise/
├── data/
│   ├── inbox/        # Input directory for IBT XML files
│   ├── outbox/       # Output directory for Partner B XML files
│   ├── archive/      # Archive directory for processed files
├── src/
│   ├── Application/  # Application layer (services, business logic)
│   ├── Domain/ # Domain layer (models, interfaces)
│   ├── Infrastructure/ # Infrastructure layer (data access, external services)
│   ├── Presentation/ # Entry point (console app, hosted service)
├── tests/
│   └── Tests.Unit/ # Unit tests
└── README.md
```

- **Application**: Contains business logic, services (`IbtMessageOrchestrator`, `IbtMessageParser`, etc.), and interfaces.
- **Infrastructure**: Contains implementations for data access (`IbtEventRepository`), file operations (`FileReader`, `PathResolver`), and notifications (`EmailNotifier`, `FileNotifier`).
- **Presentation**: The console app entry point (`Program.cs`) and hosted service (`IbtMessageHostedService`).
- **Tests**: Unit tests for all services using xUnit, NSubstitute, and FluentAssertions.

## Setup and Running

### Prerequisites
- .NET 8.0 SDK
- An IDE (e.g., Visual Studio, Rider) or CLI for running the app

### Steps to Run
1. **Clone the Repository**:
   ```bash
   git clone <repository-url>
   cd Homework_Exercise
   ```

2. **Build the Solution**:
   ```bash
   dotnet build
   ```

3. **Prepare the Data Directory**:
   - Ensure the `data/inbox`, `data/outbox`, and `data/archive` directories exist in the solution root.
   - Place IBT XML files in `data/inbox`. A sample `IBT.xml` might look like:
     ```xml
     <IBTUpload>
         <IBTTermSheet>
             <Instrument>
                 <ProductNameFull>Sample Product</ProductNameFull>
                 <IBTTypeCode>ABC123</IBTTypeCode>
             </Instrument>
             <Events>
                 <Event>
                     <EventType>9097</EventType>
                 </Event>
             </Events>
         </IBTTermSheet>
     </IBTUpload>
     ```

4. **Run the Application**:
   ```bash
   cd src/Presentation/Homework_Exercise.Presentation
   dotnet run
   ```
   - The app will process files in `data/inbox` every minute, log events to the database, notify partners, and move files to `data/archive`.

5. **Run Unit Tests**:
   ```bash
   cd src/Tests
   dotnet test
   ```

## Configuration
The application uses `appsettings.json` for configuration, located in `src/Presentation/Homework_Exercise.Presentation/`. Key settings include:

- **Serilog**: Configures structured logging (minimum level, output sinks).
- **FileSettings**: Defines paths for `inbox`, `outbox`, and `archive` directories.
- **EmailSettings**: Configures email notification settings for Partner A.
- **FileNotifierSettings**: Configures Partner B’s notification settings (`RequiredEventType`, `OutputFileNameFormat`).

Example `appsettings.json`:
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "outputTemplate": "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"
        }
      }
    ]
  },
  "FileSettings": {
    "InboxPath": "data/inbox",
    "ProcessedPath": "data/archive",
    "OutboxPath": "data/outbox",
    "FilePattern": "*.xml"
  },
  "EmailSettings": {
    "SenderName": "Sender",
    "SenderEmail": "sender@example.com",
    "RecipientName": "Partner A",
    "RecipientEmail": "partnerA@example.com",
    "Subject": "IBT Message Notification"
  },
  "PartnerBSettings": {
    "RequiredEventType": "9097",
    "OutputFileNameFormat": "InstrumentNotification_{0}.xml"
  }
}
```

## Design Decisions
1. **Clean Architecture**:
   - Separated the solution into layers (`Application`, `Infrastructure`, `Presentation`) to ensure separation of concerns and maintainability.
   - Used interfaces to define contracts, enabling dependency injection and testing.

2. **Dependency Injection**:
   - Used `Microsoft.Extensions.DependencyInjection` to manage service lifetimes and dependencies, ensuring loose coupling.

3. **Configuration**:
   - Moved all magic strings (file paths, email settings) to `appsettings.json` and used the Options Pattern (`IOptions<T>`) for strongly-typed configuration.
   - Added validation for configuration settings to fail fast if required values are missing.

4. **Path Resolution**:
   - Implemented `IPathResolver` to resolve paths relative to the solution root, ensuring file operations work consistently across different runtime environments.

5. **XML Parsing**:
   - Used `System.Xml.Linq` to parse IBT XML files, with helper methods (`EventTypes`, `ProductNameFull`, `IbtTypeCode`) to extract elements safely.
   - Designed `IbtMessageParser` to handle multiple messages per file, though the current requirement assumes one message per file.

6. **Notifications**:
   - Abstracted notifications with `IPartnerNotifier`, allowing easy addition of new partners (e.g., Partner C) by implementing the interface.
   - Used `FluentResults` for notification results, enabling robust error handling.

7. **Logging**:
   - Integrated Serilog with `Microsoft.Extensions.Logging` for structured logging, configured via `appsettings.json` for flexibility.

8. **Testing**:
   - Wrote unit tests against interfaces using xUnit, NSubstitute, and FluentAssertions, ensuring the contract is tested rather than implementation details.

9. **Future Considerations**:
   - While the current implementation uses an in-memory database (per requirements), it’s designed to easily switch to a real database by updating the `AppDbContext` configuration.
   - The `IbtMessageOrchestrator` could be refactored to use the Chain of Responsibility pattern for more extensibility, but this was deferred due to time constraints.

## Third-Party Libraries
- **FluentResults**: Used for result handling in notifications, providing a clean way to handle success/failure scenarios.
- **MimeKit**: Used for constructing email messages for Partner A, chosen for its reliability and widespread use in .NET for email tasks.
- **Serilog**: Used for structured logging, configured via `appsettings.json` for flexibility.
- **NSubstitute and FluentAssertions**: Used in unit tests for mocking and assertions, respectively.

## Known Limitations
- **ISIN Missing**: The `IbtMessageParser` notes that `ISIN` is missing in the provided `IBT.xml`. It’s currently set to an empty string, but this could be addressed by updating the XML structure or requirements.
- **Single Message per File**: The parser supports multiple messages per file, but the requirements assume one message per file. This can be adjusted based on future needs.
- **Simulated Email**: Email sending for Partner A is simulated, as no real SMTP server is required for this exercise.

## Future Improvements
- Implement the Chain of Responsibility pattern in `IbtMessageOrchestrator` to make the processing pipeline more extensible.
- Add a real database (e.g., SQL Server) for persistence, potentially using Docker for local development.
- Add retry logic for file operations and notifications to handle transient failures.
- Enhance logging with additional sinks (e.g., file, Seq) for production use.
