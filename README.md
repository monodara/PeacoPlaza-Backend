# PeacoPlaza Online Shop

![TypeScript](https://img.shields.io/badge/TypeScript-v.4-green)
![SASS](https://img.shields.io/badge/SASS-v.4-hotpink)
![React](https://img.shields.io/badge/React-v.18-blue)
![Redux toolkit](https://img.shields.io/badge/Redux-v.1.9-brown)
![.NET Core](https://img.shields.io/badge/.NET%20Core-v.8-purple)
![EF Core](https://img.shields.io/badge/EF%20Core-v.8-cyan)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-v.16-drakblue)

PeacoPlaza is an e-commerce website developed as a full-stack project. The backend is build using `C#/ASP.NET 8`, `React & Typescript` primarily facilitating the frontend, and `PostgreSQL` supporting data storage.

The system allows users to browse products, filter and sort by their preference, create account if they intend to make orders, and also provide a platform for administrative roles to manage users, products and orders.

The project is deployed on [https://peacoplaza.azurewebsites.net/]. Here are its frontend deployed project[https://monodara.github.io/PeacoPlaza-Frontend/] and its repository[https://github.com/monodara/PeacoPlaza-Frontend]
This web application is developed by
project involves in creating a Fullstack system with React and Redux in the frontend, ASP.NET Core 8 in the backend and PostgreSql for data storage.

## Table of Contents

1. [Introduction](#introduction)
2. [Technologies](#fTechnologies)
3. [Getting Started](#getting-started)
4. [Architecture & code structure](#ArchitectureandFolderStructure)

## Introduction

This repository is the codebase for backend server, which built as a .Net/Core solution.

## Technologies

- **Backend**:
  - ASP.NET Core
  - Entity Framework Core
  - PostgreSQL
  - Token service for authentication
  - role-based and resource-based authorisation
- **Testing**:
  - XUnit and Moq for unit tests
  - SwaggerUI for API testing
- **Deployment**:
  - Azure

## Getting Started

1. Open your terminal and clone the front-end repository with the following command:

```
git clone
```

3. Navigate the infrastructure layer

```
  cd Server.Infrastructure
```

4. Set up PostgreSQL database connection in `appsettings.json` file by replacing values of `Host`, `Database` , `Username` and `password`.
5. In `appsettings.json` file, set up your `JwtKey` for token generating, and the name of `Issuer`.

6. Try to build the application by run `dotnet build`
7. User `dotnet ef` commands to build the database

```
dotnet ef migrations add CreateDb
dotnet ef database update // push changes to database
```
8. Start the server by `dotnet watch` (Remember locate yourself in Infrastructure layer.)

## Architecture and folder structure
The development followed `CLEAN` architecture to minimarize dependency.  
- **Core Layer** : Also known as Domain layer , houses all the Entities, Aggregate, ValueObjects & interfaces for the Repository.

- **Service Layer** : Commonly known as Business layer is responsible for all validations, Data Transformations & Mapping. It houses DTO,Service Interfaces & their implementation including the Authentication Service.

- **Controller Layer** : This layer houses the endpoints that communicates with both front and database.

- **Infrastructure Layer** : Also known as Web API layer serves the entry point of the application. Contains the program.cs files, Migrations, DbContext, Middleware,Services only the external ones, like Token Serviec, Sms service etc and Repository Implementation.

- **Tests** : It is not any layer technically and is not part of the architecture. But to know the application better you can run the tests and check the results.

The folder structure indicates this design. 
```.
├── Server
│   ├── sln
│   ├── Controller
│   │   ├── Server.Controller.csproj
│   │   └── src
│   │       └── Controller
│   │           ├── AuthController.cs
│   │           ├── CategoryController.cs
│   │           ├── OrderController.cs
│   │           ├── ProductController.cs
│   │           ├── UserController.cs
│   │           └── ...
│   ├── Server.Core
│   │   ├── Server.Core.csproj
│   │   └── src
│   │       ├── Common
│   │       │   ├── QueryOptions.cs
│   │       │   ├── UserCredential.cs
│   │       ├── Entity
│   │       │   ├── BaseEntity.cs
│   │       │   ├── Category.cs
│   │       │   ├── Product.cs
│   │       │   ├── ProductImage.cs
│   │       │   └── User.cs
│   │       │   └── ...
│   │       ├── RepoAbstract
│   │       │   ├── ICategoryRepo.cs
│   │       │   ├── IOrderRepo.cs
│   │       │   ├── IProductImageRepo.cs
│   │       │   ├── IProductRepo.cs
│   │       │   ├── IReviewRepo.cs
│   │       │   └── IUserRepo.cs
│   │       │   ├── ...
│   │       └── ValueObject
│   │           ├── OrderStatus.cs
│   │           └── UserRole.cs
                └── PaymentMethod.cs
│   ├── Server.Service
│   │   ├── Server.Service.csproj
│   │   └── src
│   │       ├── DTO
│   │       │   ├── CategoryDto.cs
│   │       │   ├── OrderDto.cs
│   │       │   ├── OrderProductDto.cs
│   │       │   ├── ProductImageDto.cs
│   │       │   ├── ProductDto.cs
│   │       │   └── UserDto.cs
│   │       │   └── ...
│   │       ├── Service
│   │       │   ├── AuthService.cs
│   │       │   ├── CategoryService.cs
│   │       │   ├── OrderService.cs
│   │       │   ├── ProductService.cs
│   │       │   └── UserService.cs
│   │       │   └── ....
│   │       ├── ServiceAbstract
│   │       │   ├── IAuthService.cs
│   │       │   ├── ICategoryService.cs
│   │       │   ├── IOrderService.cs
│   │       │   ├── IPasswordService.cs
│   │       │   ├── IProductImageService.cs
│   │       │   ├── IProductService.cs
│   │       │   ├── IReviewService.cs
│   │       │   ├── ITokenService.cs
│   │       │   └── IUserService.cs
│   │       │   └── ...
│   │       └── Shared
│   ├── Server.Test
│   │   ├── Server.Test.csproj
│   ├── Server.Infrastructure
│   │   ├── Server.Infrastructure.csproj
│   │   ├── Properties
│   │   │   └── launchSettings.json
│   │   ├── appsettings.Development.json
│   │   ├── appsettings.json
│   │   └── src
│   │       ├── Database
│   │       |   ├── AppDbContext.cs
│   │       │   ├── SeedingData.cs
│   │       │   └── TimeStampInterceptor.cs
│   │       ├── Middleware
│   │       │   └── ExceptionHandlerMiddleware.cs
│   │       ├── Program.cs
│   │       └── Repo
│   │           ├── CategoryRepo.cs
│   │           ├── OrderRepo.cs
│   │           ├── ProductRepo.cs
│   │           ├── AddressRepo.cs
│   │           └── UserRepo.cs
|   |           |── ...
└── README.md

