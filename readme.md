# MarketPlace Project

We're going to build a simple web application where people can manage products they want to sell.
- Everyone can browse existing products.
- Only authenticated users can add new products.
- Only a product owner can edit or delete a product.

We are going to build 3 projects. The FrontEnd project will be a Progressive Web Application built using Vue, while server side we are going to build two .NET Core 2.0 Web Applications using Visual Studio 2017: one will expose a REST API while the second will take care of Authentication.

1. FrontEnd Client
   - Javascript (ECMAScript 2017)
   - HTML 5
   - CSS 3
   - Vue.js 2
   - Material Design Lite
   - Open Id Connect Client
   - Service Worker
   - Cache API
   - Fetch API

This project will interact with the user through a browser by dinamically constructing an HTML user interface and will talk to the server by using javascript and json.

2. REST Service 
   - .NET Core 2 Web API Controller
   - Entity Framework Core
   - Sql Server Database
   - Identity Server Client Authentication

This project will be responsible to store the data on the server and respond to the client requests through http and json.

3. Authentication Server
   - Identity Server 4
   - Entity Framework Core

This project will take care of the authentication part. It will issue tokens that will be used by the client application to gain access to the server.

## What you already need to know:
- C#
- Javascript (ECMAScript 2017)
- HTML 5
- CSS 3

## What you're going to learn:
- What is REST
- What is .NET Core
- ASP.NET Core 
- What is a Web API Controller
- Kestrel
- Middleware
- Environment variables
- ASP.NET Core Configuration
- Dependency Injection
- Entity Framework Core
- PostMan
- CORS
- Vue.js
- Fetch API
- WebPack
- Service Workers
- Cache API
- Material Design Lite
- Authentication and Authorization
- OAuth 2 and Open Id Connect
- Identity Server 4
- Resource Owner Authorization

## Before you begin, you need
- [Visual Studio 2017 (Community Edition is enough)](https://www.visualstudio.com/thank-you-downloading-visual-studio/?sku=community) 

**Make sure you installed the workload ".NET Core cross-platform development". You can check and install workloads by launching the Visual Studio Installer.**

- [Visual Studio Code](https://code.visualstudio.com/download)
- [Node](https://nodejs.org/en/)

## For more information on the .NET Core installation

Please see [https://www.microsoft.com/net/download/windows](https://www.microsoft.com/net/download/windows)


---

# Our workflow

We are going to split our projects into simple steps. Each step will focus on one task and will build on top of the previous step. We will start with simple projects that will become more and more complex along the way. For example, we will not focus on authentication and authorization at first. We will add it at a later step.

This folder contains different subfolder. Each subfolder represents a phase in our project. "Start" folders are the starting points of each step. "Solution" folders are the final versions of each step, given to you just in case you want to check what your project is supposed to become at the end of each lab.
What you have to do is to open a start folder corresponding to the lab you want to try (for example Lab01/Start in order to begin) and follow the instructions you find on the readme.md file. When you are done, feel free to compare your work with the solution provided in the Solution folder.     

# To START

1. Open the Labs folder
2. Navigate to the Lab01 subfolder
3. Navigate to the Start subfolder
4. Follow the instructions contained in the readme.md file to continue