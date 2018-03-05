# Some theory before we start coding

## APIs, Web Services and REST

The architecture we are going to use has a Web API REST Service and a JavaScript Client.
Before we start practicing you may want to know 
- what an API is 
- what a Web Service is 
- what REST is and how it works 
- You can find a great explanation of API, REST and Web Services on [http://idratherbewriting.com/docapis_what-is-a-rest-api/](http://idratherbewriting.com/docapis_what-is-a-rest-api/)
- You can find info on the HTTP Methods and Status Codes on [http://www.restapitutorial.com/lessons/httpmethods.html](http://www.restapitutorial.com/lessons/httpmethods.html)

---

## What is ASP.NET Core and .NET Core

We are going to build three web sites with ASP.NET Core. You can find a detailed introduction to .NET Core and ASP.NET Core on [https://weblog.west-wind.com/posts/2016/jun/13/aspnet-core-and-net-core-overview](https://weblog.west-wind.com/posts/2016/jun/13/aspnet-core-and-net-core-overview).
It may be a bit outdated, but the world of .NET Core goes so fast that almost everything you read is already old. Don't worry: the main concepts are still valid.

---

## The MVC Pattern
Server side we're going to have a Model and a Controller. The Client application will take care of the View. 
In his article [Understanding Model-View-Controller](https://blog.codinghorror.com/understanding-model-view-controller/), Jeff Atwood says:


> 1. Model
> - The classes which are used to store and manipulate state, typically in a database of some kind.
> 2. View
> - The user interface bits necessary to render the model to the user.
> 3. Controller
> - The brains of the application. The controller decides what the user's input was, how the model needs to change as a result of that input, and which resulting view should be used.

In order to better understand this pattern, you may want to read the full article before you start building the server application.

## ASP.NET Core
- [Application Startup](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup)
- [Dependency Injection (Services)](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)
- [Middleware](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware?tabs=aspnetcore2x)
- [Handling requests with controllers in ASP.NET Core MVC](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/actions)
- [Routing](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing)
- [Dependency injection into controllers](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/dependency-injection)

## Entity Framework

[Entity Framework Core Docs](https://docs.microsoft.com/en-us/ef/core/)