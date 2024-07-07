<div align="center" text-align="center" width="100%">
    <img src="/docs/cover.png" alt="Icon" align="center">
</div>
<big>
  <h1 align="center">.NET FWDays 2024</h1>
</big>

This project provides a GraphQL API for managing data related to the simplified FWDays conference.
It utilizes ASP.NET 8 and a microservice architecture to achieve scalability and maintainability.

### Disclaimer

This project is a demonstration of the capabilities of HotChocolate and GraphQL. It is a simplified example and may not reflect production-ready code.

### Purpose

Demo aims to demonstrate the implementation of a GraphQL API using ASP.NET 8 and the HotChocolate library. 
It showcases microservice architecture for building APIs with clear separation of concerns. 
The chosen domain, conference, involves managing participants, tracks, and speakers.

### Entities
- Participant: Represents an attendee of the FWDays conference.
- Track: Represents a topic or subject covered during the conference.
- Speaker: Represents an individual presenting at a specific track.

### Architecture
The project adopts a microservice architecture with four independent services:

1. Tracks Service: Manages track data, including creation, retrieval, and updates.
2. Participants Service: Manages participant data, including registration, information retrieval, and updates.
3. Speakers Service: Manages speaker data, including speaker information, and assignment to tracks.
4. Gateway Service: Acts as the entry point for the API, combining schemas from other services using schema stitching from HotChocolate.

### Technologies
- ASP.NET 8.
- HotChocolate v13: GraphQL server library for defining and implementing the GraphQL API.
- Postgres: Relational database for persistent data storage.
- Redis: In-memory data store

### How to Start

#### With Docker

1. Update or use default configuration from `docker-compose` file.
2. Run:
```Bash
docker compose up --build
```
#### Standalone setup
1. Prerequisites (for standalone app):

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [PostgresSQL server](https://www.postgresql.org/)
- [Redis](https://redis.io/)

2. Restore dependencies:
```Bash
cd fwdays-graphql-the-good-parts
dotnet restore
```
3. Provide correct configuration for each microservice (`appsettings.json`)

5. Start the services: Run each microservice project individually using `dotnet run`


### Additional Notes
- Security considerations (authentication, authorization) and implementation are not covered in this project.