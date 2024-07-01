# User Management System - Backend

## Overview

This is the backend of the User Management System, developed with .NET, Entity Framework Core, and SQL Server. It supports user registration and login functionalities, secured with JWT authentication.

## Features

- **User Registration**: Allows new users to register by providing personal and authentication details.
- **User Login**: Enables registered users to log in using their credentials.
- **JWT Authentication**: Secures the API endpoints with JWT tokens.

## Technologies Used

- **Backend**: .NET, Entity Framework Core
- **Database**: SQL Server
- **Authentication**: JSON Web Tokens (JWT)

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

## Getting Started

### Set Up the Project

1. **Clone the Repository**:
    ```bash
    git clone https://github.com/JuanJSalzar/WebFormBackend.git
    cd WebFormBackend
   code .
    ```

2. **Set Up Database**:
   
   - Ensure SQL Server is running.
   - Update the `appsettings.json` with your SQL Server connection string.


3. **Run Migrations**:
    ```bash
    dotnet ef database update
    ```

4. **Run the Backend**:
    ```bash
    dotnet run
    ```

## API Endpoints

### User Registration

- **URL**: `http://localhost:5282/api/register`
- **Method**: `POST`
- **Request Body**:
    ```json
    {
      "username": "string",
      "password": "string",
      "firstName": "string",
      "lastName": "string",
      "identificationNumber": "string",
      "email": "string",
      "identificationType": "string"
    }
    ```

### User Login

- **URL**: `http://localhost:5282/api/login`
- **Method**: `POST`
- **Request Body**:
    ```json
    {
      "username": "string",
      "password": "string"
    }
    ```