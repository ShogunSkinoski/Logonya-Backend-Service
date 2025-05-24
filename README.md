# Logonya

## Description
Logonya is a comprehensive logging and monitoring solution designed to help developers and organizations track and analyze application logs effectively. It provides real-time log streaming, advanced search capabilities, and alerting features to ensure system health and quick troubleshooting.

## Features
- **Account Management:** Secure user registration and login. API key generation and management for programmatic access.
- **Real-time Logging:** Capture and store application logs in real-time.
- **Chat Functionality:** A chat module for real-time communication and collaboration.
- **Advanced Search:** Powerful search functionality to query logs by keywords, timeframe, and other criteria.
- **Alerting:** Configure alerts based on log patterns or thresholds to get notified of critical events.
- **Webhook Integration:** Extend functionality and integrate with third-party services using webhooks.
- **RAG Service:** Utilizes a Retrieval Augmented Generation service for intelligent log analysis and insights.

## Technologies Used
- **Backend:** .NET, ASP.NET Core
- **Data Storage:** PostgreSQL (or configurable to other relational databases)
- **Messaging:** Apache Kafka for asynchronous communication and log ingestion.
- **Containerization:** Docker for easy deployment and scaling.
- **API:** RESTful API for interaction with the system.

## Getting Started

### Prerequisites
- .NET SDK (version specified in global.json or project files)
- Docker Desktop
- PostgreSQL (or connection string to an existing instance)
- Apache Kafka (or connection string to an existing instance)

### Installation
1. **Clone the repository:**
   ```bash
   git clone https://your-repository-url.git
   cd logonya
   ```
2. **Configure environment variables:**
   - Create a `.env` file based on `.env.example` (if provided) or set up environment variables for database connection strings, Kafka brokers, JWT settings, etc.
   - Example for `appsettings.json` or environment variables:
     ```json
     {
       "ConnectionStrings": {
         "DefaultConnection": "Host=localhost;Port=5432;Database=logonya_db;Username=youruser;Password=yourpassword;"
       },
       "KafkaSettings": {
         "BootstrapServers": "localhost:9092"
       },
       "JwtSettings": {
         "Secret": "your-super-secret-jwt-key- حداقل-32-characters-long",
         "Issuer": "LogonyaAPI",
         "Audience": "LogonyaApp"
       }
     }
     ```
3. **Build the application:**
   ```bash
   dotnet build
   ```
4. **Run database migrations (if using Entity Framework Core):**
   ```bash
   dotnet ef database update --project Infrastructure --startup-project Logonya
   ```
5. **Run the application:**
   - **Using Docker Compose (recommended for development):**
     ```bash
     docker-compose up -d
     ```
   - **Directly using dotnet:**
     ```bash
     dotnet run --project Logonya
     ```

## Usage
Once the application is running:
- Access the API endpoints through `http://localhost:<port_specified_in_launchSettings_or_Dockerfile>`.
- Use a tool like Postman or curl to interact with the API.
- Refer to the API Endpoints section for details on available operations.

## API Endpoints
The application exposes RESTful APIs for various functionalities. Below is a summary of the main endpoint groups:

- **Account Management:**
  - `POST /api/account/register`: Create a new user account.
  - `POST /api/account/login`: Log in and receive a JWT token.
  - `POST /api/account/api-keys`: Generate an API key for the authenticated user.
  - `GET /api/account/api-keys`: List API keys for the authenticated user.
  - `DELETE /api/account/api-keys/{apiKey}`: Revoke an API key.
- **Chat:**
  - `POST /api/chat`: Create a new chat session.
  - `GET /api/chat/{chatId}/messages`: Retrieve messages for a specific chat.
  - `POST /api/chat/{chatId}/messages`: Send a new message to a chat.
- **Logging:**
  - `POST /api/logs`: Submit a new log entry (requires API key).
  - `GET /api/logs`: Query logs (supports filtering).
  - `POST /api/alerts`: Create a new alert definition.
  - `GET /api/alerts`: List existing alert definitions.
  - `POST /api/logs/replay`: Replay logs for a specific timeframe or query.
- **Webhooks:**
  - `POST /api/webhooks`: Register a new webhook.
  - `GET /api/webhooks`: List registered webhooks.
  - `DELETE /api/webhooks/{webhookId}`: Delete a webhook.

*Note: Some endpoints may require authentication (JWT token or API key). Refer to the Swagger documentation (`/swagger`) for detailed API specifications and to try out the endpoints.*

## Contributing
Contributions are welcome! Please follow these steps:
1. Fork the repository.
2. Create a new branch (`git checkout -b feature/your-feature-name`).
3. Make your changes.
4. Commit your changes (`git commit -m 'Add some feature'`).
5. Push to the branch (`git push origin feature/your-feature-name`).
6. Open a Pull Request.

Please ensure your code adheres to the project's coding standards and includes tests where applicable.

## License
This project is licensed under the MIT License. See the `LICENSE` file for details (if a `LICENSE` file is added).
