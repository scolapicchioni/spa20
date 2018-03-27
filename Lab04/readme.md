# Backend: Web API with ASP.NET Core and Visual Studio for Windows

In this lab we're going to build a REST service using ASP.NET Core Web API.

We are going to follow (more or less) [this tutorial](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api), changing the model to our needs.

## Building Your First Web API with ASP.NET Core MVC and Visual Studio

Here is the API that you'll create:

| API                       | Description                | Request body           | Response body     |
| ------------------------- | -------------------------- | ---------------------- | ----------------- |
| GET /api/products	        | Get all products   	       | None	                  | Array of products |
| GET /api/products/{id}    | Get a product by ID        | None                   | Product           |
| POST /api/products        | Add a new product          | Product                | Product           |
| PUT /api/products/{id}    | Update an existing product | Product                |                   |	
| DELETE /api/products/{id} | Delete a product           | None. No request body- | None              |

The client submits a request and receives a response from the application. Within the application we find the controller, the model, and the data access layer. The request comes into the application's controller, and read/write operations occur between the controller and the data access layer. The model is serialized and returned to the client in the response.

The client is whatever consumes the web API (browser, mobile app, and so forth). We aren't writing a client in this tutorial. We'll use [Postman](https://www.getpostman.com/apps) to test the app.

A model is an object that represents the data in your application. In this case, the only model is a Product item. Models are represented as simple C# classes (POCOs).

A controller is an object that handles HTTP requests and creates the HTTP response. This app will have a single controller.

To keep the tutorial simple, the app doesn't use a persistent database. Instead, it stores Product items in an in-memory database.

### Prerequisites
Install the following:

- [.NET Core 2.0.0 SDK](https://www.microsoft.com/net/core) or later.
- [Visual Studio 2017](https://www.visualstudio.com/downloads/) version 15.3 or later with the ASP.NET and web development workload.

### Create the project

From Visual Studio, select File menu, > New > Project.

Select the `ASP.NET Core Web Application (.NET Core)` project template. Name the Solution `MarketPlace`. Name the Project `MarketPlaceService` and select `OK`.

In the `New ASP.NET Core Web Application (.NET Core) - MarketPlaceService` dialog, select the `Web Application (Model-View-Controller)` template. Select `OK`. Leave `No Authentication`. Do not select `Enable Docker Support`.

### Add a model class

A model is an object that represents the data in your application. In this case, the only model is a Product item, whose properties are `Id` *(int)*, `Name` *(string)*, `Description` *(string)* and `Price` *(decimal)*.

Add a folder named `Models`. In Solution Explorer, right-click the project. Select `Add` > `New Folder`. Name the folder `Models`.

Note: You can put model classes anywhere in your project, but the `Models` folder is used by convention.

Add a `Product` class. Right-click the `Models` folder and select `Add` > `Class`. Name the class `Product` and select `Add`.

Replace the generated code with:

```cs
namespace MarketPlaceService.Models {
    public class Product {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
```

### Create the database context

The database context is the main class that coordinates [Entity Framework](https://docs.microsoft.com/en-us/ef/core/) functionality for a given data model. This class is created by deriving from the `Microsoft.EntityFrameworkCore.DbContext` class.

Add a folder named `Data`. Add a `MarketPlaceContext` class. Right-click the `Models` folder and select `Add` > `Class`. Name the class `MarketPlaceContext` and select `Add`.

```cs
using MarketPlaceService.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketPlaceService.Data {
    public class MarketPlaceContext : DbContext {
        public DbSet<Product> Products { get; set; }

        public MarketPlaceContext(DbContextOptions<MarketPlaceContext> options) : base(options) { }
    }
}
```

### Add a repository class

A [repository](http://deviq.com/repository-pattern/) is an object that encapsulates the data layer. The repository contains logic for retrieving and mapping data to an entity model. 

Create the repository code in the `Data` folder.

Defining a repository interface named `IProductsRepository`. Use the interface template (`Add New Item` -> `Interface`).

```cs
using MarketPlaceService.Models;
using System.Collections.Generic;

namespace MarketPlaceService.Data {
    public interface IProductsRepository {
        Task AddAsync(Product product);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> FindAsync(int id);
        Task<Product> RemoveAsync(int id);
        Task UpdateAsync(Product product);
    }
}
```

This interface defines basic CRUD operations.

### Add a ProductsRepository class that implements IProductsRepository:

### Add a ProductsRepository class that implements IProductsRepository:

We will add a `ProductsRepository` class implementing our `IProductsRepository` interface. 
In each method we will use our `MarketPlaceContext` to [save our data](https://docs.microsoft.com/en-us/ef/core/saving/basic). We will make use of the [async operations](https://docs.microsoft.com/en-us/ef/core/saving/async) provided by Entity Framework Core.
In order to retrieve the MarketPlaceContext instance, we will make use of the [Dependency Injection System](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection) provided by the ASP.NET Core framework. We will also insert some sample data in our constructor. 


```cs
using MarketPlaceService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceService.Data {
    public class ProductsRepository : IProductsRepository {

        private MarketPlaceContext _context;
        public ProductsRepository(MarketPlaceContext context) {
            _context = context;

            //init our db with some products if empty
            if (_context.Products.Count() == 0) {
                _context.Products.AddRange(
                    new Product { Name = "Product 1", Description = "First Sample Product", Price = 1234 },
                    new Product { Name = "Product 2", Description = "Second Sample Product", Price = 2345 },
                    new Product { Name = "Product 3", Description = "Third Sample Product", Price = 3456 },
                    new Product { Name = "Product 4", Description = "Fourth Sample Product", Price = 4567 });
                _context.SaveChanges();
            }
        }

        public async Task AddAsync(Product product) {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> FindAsync(int id) {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync() {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> RemoveAsync(int id) {
            var entity = _context.Products.First(p => p.Id == id);
            _context.Products.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Product product) {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
```

Build the app to verify you don't have any compiler errors.

### Register the repository

By defining a repository interface, we can decouple the repository class from the MVC controller that uses it. Instead of instantiating a `ProductsRepository` inside the controller we will inject an `IProductsRepository` using the built-in support in ASP.NET Core for dependency injection.

This approach makes it easier to unit test your controllers. Unit tests should inject a mock or stub version of `IProductsRepository`. That way, the test narrowly targets the controller logic and not the data access layer.

In order to inject the repository into the controller, we need to register it with the DI container. Open the `Startup.cs` file.

In the `ConfigureServices` method, add the following code:

```cs
// requires using MarketPlaceService.Data;
services.AddScoped<IProductsRepository, ProductsRepository>();
```

### Register the database context

In this step, the database context is registered with the dependency injection container. Services (such as the DB context) that are registered with the dependency injection (DI) container are available to the controllers.

Register the DB context with the service container using the built-in support for dependency injection, specifying that an in-memory database is injected into the service container. 

In the `ConfigureServices` method, add the following code:

```cs
// requires using Microsoft.EntityFrameworkCore;
// and using MarketPlaceService.Data;
services.AddDbContext<MarketPlaceContext>(opt => opt.UseInMemoryDatabase("MarketPlaceContext"));
```

### Add a controller

We are now going to add a [Controller](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/actions) class. We will start with an empty controller and take care of all the methods to implement the API. The constructor will use Dependency Injection to inject the repository into the controller. The repository will be used in each of the CRUD methods in the controller.

We will map every action to the correct route by using the [routing](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing) system, in particular [attribute routing](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing#attribute-routing) and [attribute routing using http verbs attributes](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing#attribute-routing-with-httpverb-attributes).


In `Solution Explorer`, right-click the `Controllers` folder. Select `Add` > `New Item`. In the `Add New Item` dialog, select the `Web API Controller Class` template. Name the class `ProductsController`. 

Replace the generated code with the following:

```cs
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlaceService.Data;
using MarketPlaceService.Models;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceService.Controllers {
    [Produces("application/json")]
    [Route("api/Products")]
    public class ProductsController : Controller
    {
        //requires using MarketPlaceService.Data;
        private readonly IProductsRepository _ProductsRepository;

        public ProductsController(IProductsRepository ProductsRepository) {
            _ProductsRepository = ProductsRepository;
        }
    }
}
```

### Getting Product items

Next, we want to implement the two GET methods:

- GET /api/product
- GET /api/product/{id}

Here is an example HTTP response for the GetAll method:

```
HTTP/1.1 200 OK
   Content-Type: application/json; charset=utf-8
   Server: Microsoft-IIS/10.0
   Date: Thu, 18 Jun 2015 20:51:10 GMT
   Content-Length: 82

   [{"Id":1,"Name":"Product 1","Description":"First Sample Product", "Price" : 1234}]
```

The GetAllAsync method will return an `IEnumerable`. MVC automatically serializes the object to JSON and writes the JSON into the body of the response message. The response code for this method is 200, assuming there are no unhandled exceptions. (Unhandled exceptions are translated into 5xx errors.)

In contrast, the GetByIdAsync method will return the more general `IActionResult` type, which represents a wide range of return types. GetByIdAsync will have two different return types:

- If no item matches the requested ID, the method returns a 404 error. Returning `NotFound` returns an HTTP 404 response.
- Otherwise, the method returns 200 with a JSON response body. Returning `ObjectResult` returns an HTTP 200 response.

To get `Product` items, add the following methods to the `ProductsController` class

```cs
//requires using MarketPlaceService.Models;
[HttpGet]
public async Task<IEnumerable<Product>> GetAllAsync() {
    return await _ProductsRepository.GetAllAsync();
}

[HttpGet("{id}", Name = "GetProduct")]
public async Task<IActionResult> GetByIdAsync(int id) {
    var item = await _ProductsRepository.FindAsync(id);
    if (item == null) {
        return NotFound();
    }
    return new ObjectResult(item);
}
```

In Visual Studio, press `CTRL+F5` to launch the app. Visual Studio launches a browser and navigates to http://localhost:port/api/values, where port is a randomly chosen port number. Navigate to the Products controller at `http://localhost:port/api/products`.

You should see a JSON array with the four products added in the constructor of our repository class.

```
HTTP/1.1 200 OK
Transfer-Encoding: chunked
Content-Type: application/json; charset=utf-8

[{"id":1,"name":"Product 1","description":"First Sample Product","price":1234.0},{"id":2,"name":"Product 2","description":"Second Sample Product","price":2345.0},{"id":3,"name":"Product 3","description":"Third Sample Product","price":3456.0},{"id":4,"name":"Product 4","description":"Fourth Sample Product","price":4567.0},{"id":5,"name":"Product 1","description":"First Sample Product","price":1234.0},{"id":6,"name":"Product 2","description":"Second Sample Product","price":2345.0},{"id":7,"name":"Product 3","description":"Third Sample Product","price":3456.0},{"id":8,"name":"Product 4","description":"Fourth Sample Product","price":4567.0}]
```

Navigate to `http://localhost:port/api/products/1`

You should see this response:

```
HTTP/1.1 200 OK
Transfer-Encoding: chunked
Content-Type: application/json; charset=utf-8

{"id":1,"name":"Product 1","description":"First Sample Product","price":1234.0}
```

Navigate to `http://localhost:port/api/products/99`

You should see this response header:

```
HTTP/1.1 404 Not Found
Content-Length: 0
```

### Implement the other CRUD operations

In the following sections, `Create`, `Update`, and `Delete` methods are added to the controller.

### Create

The Create is an `HTTP POST` method, indicated by the `[HttpPost]` attribute. The `[FromBody]` attribute tells MVC to get the value of the `Product` item from the body of the HTTP request.

The `CreatedAtRoute` method:

- Returns a 201 response. HTTP 201 is the standard response for an HTTP POST method that creates a new resource on the server.
- Adds a Location header to the response. The Location header specifies the URI of the newly created Product item. See [10.2.2 201 Created](https://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html).
- Uses the `GetProduct` named route to create the URL. The `GetProduct` named route is defined in `GetProductByIdAsync`

Add the following `Create` method.

```cs
[HttpPost]
public async Task<IActionResult> CreateAsync([FromBody] Product product) {
    if (product == null) {
        return BadRequest();
    }

    await _ProductsRepository.AddAsync(product);

    return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
}
```

### Use [Postman](https://www.getpostman.com/apps) to send a Create request

- Set the HTTP method to POST
- Select the Body radio button
- Select the raw radio button
- Set the type to JSON
- In the key-value editor, enter a product item such as

```json
{
  "Name":"Product 5",
  "Description":"New Product 5",
  "Price":555
}
```

Select `Send`

Select the `Headers` tab in the lower pane and copy the `Location` header.

You can use the `Location` header URI to access the resource you just created.

### Update

Update is similar to Create, but uses `HTTP PUT`. The response is 204 (No Content). According to the HTTP spec, a PUT request requires the client to send the entire updated entity, not just the deltas. To support partial updates, use HTTP PATCH.

```cs
[HttpPut("{id}")]
public async Task<IActionResult> UpdateAsync(int id, [FromBody] Product product) {
    if (product == null || product.Id != id) {
        return BadRequest();
    }

    var original = await _ProductsRepository.FindAsync(id);
    if (original == null) {
        return NotFound();
    }

    original.Name = product.Name;
    original.Description = product.Description;
    original.Price = product.Price;

    await _ProductsRepository.UpdateAsync(original);
    return new NoContentResult();
}
```

You can use [POSTMAN](https://www.getpostman.com/apps) to test this action.

- Set the HTTP method to PUT
- Select the Body radio button
- Select the raw radio button
- Set the type to JSON
- In the key-value editor, enter a product item such as

```json
{
  "Id":1,
  "Name":"Product 111",
  "Description":"Modified description",
  "Price":1111
}
```

Select `Send`

### Delete

The Delete uses `HTTP DELETE` verb and expects an `id` in the address. It returns 
- A 204 (No Content) if successful
- A 404 (Not Found) if the id is not found in the database

```cs
[HttpDelete("{id}")]
public async Task<IActionResult> Delete(int id) {
    var product = await _ProductsRepository.FindAsync(id);
    if (product == null) {
        return NotFound();
    }

    await _ProductsRepository.RemoveAsync(id);
    return new NoContentResult();
}
``` 

Our service is ready. In the next lab we will setup the client side. 

Go to `Labs/Lab05`, open the `readme.md` and follow the instructions thereby contained.   