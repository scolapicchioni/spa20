# Lab 01 - The Service

Time to create our first project.
We are going to follow (more or less) [this tutorial](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api), changing the model to our needs.

## Building Your First Web API with ASP.NET Core MVC and Visual Studio

Here is the API that you'll create:

| API                       | Description                | Request body           | Response body     |
| ------------------------- | -------------------------- | ---------------------- | ----------------- |
| GET /api/products	        | Get all products   	     | None	                  | Array of products |
| GET /api/products/{id}    | Get a product by ID        | None                   | Product           |
| POST /api/products        | Add a new product          | Product                | Product           |
| PUT /api/products/{id}    | Update an existing product | Product                |                   |	
| DELETE /api/products/{id} | Delete a product           | None. No request body- | None              |

The client submits a request and receives a response from the application. Within the application we find the controller, the model, and the data access layer. The request comes into the application's controller, and read/write operations occur between the controller and the data access layer. The model is serialized and returned to the client in the response.

The client is whatever consumes the web API (browser, mobile app, and so forth). We aren't writing a client in this tutorial. We'll use Postman to test the app.

A model is an object that represents the data in your application. In this case, the only model is a Product item. Models are represented as simple C# classes (POCOs).

A controller is an object that handles HTTP requests and creates the HTTP response. This app will have a single controller.

To keep the tutorial simple, the app doesn't use a persistent database. Instead, it stores Product items in an in-memory database.

### Create the project

From Visual Studio, select File menu, > New > Project.

Select the ASP.NET Core Web Application (.NET Core) project template. Name the Solution Marketplace. Name the Project MarketPlaceService and select OK.

In the New ASP.NET Core Web Application (.NET Core) - MarketPlaceService dialog, select the Web API template. Make sure to select ASP.NET Core 2.0. Do not select Enable Docker Support. Select OK. 

### Add a model class

A model is an object that represents the data in your application. In this case, the only model is a Product item.

Add a folder named "Models". In Solution Explorer, right-click the project. Select Add > New Folder. Name the folder Models.

Note: You can put model classes anywhere in your project, but the Models folder is used by convention.

Add a Product class. Right-click the Models folder and select Add > Class. Name the class Product and select Add.

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

The database context is the main class that coordinates Entity Framework functionality for a given data model. You create this class by deriving from the Microsoft.EntityFrameworkCore.DbContext class

Add a folder named "Data". Add a MarketPlaceContext class. Right-click the Data folder and select Add > Class. Name the class MarketPlaceContext and select Add.

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

A repository is an object that encapsulates the data layer. The repository contains logic for retrieving and mapping data to an entity model. Create the repository code in the Data folder

Defining a repository interface named IProductsRepository. Use the interface template (Add New Item -> Interface).

```cs
using MarketPlaceService.Models;
using System.Collections.Generic;

namespace MarketPlaceService.Data {
    public interface IProductsRepository {
        void Add(Product product);
        IEnumerable<Product> GetAll();
        Product Find(int id);
        Product Remove(int id);
        void Update(Product product);
    }
}
```

This interface defines basic CRUD operations.

### Add a ProductsRepository class that implements IProductsRepository:

```cs
using MarketPlaceService.Models;
using System.Collections.Generic;
using System.Linq;

namespace MarketPlaceService.Data {
    public class ProductsRepository : IProductsRepository {
        private MarketPlaceContext _context;
        public ProductsRepository(MarketPlaceContext context) {
            _context = context;
            if (_context.Products.Count()==0)
                Add(new Product { Name = "Product 1", Description = "First Sample Product", Price = 1234 });
        }

        public void Add(Product product) {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public Product Find(int id) {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> GetAll() {
            return _context.Products.ToList();
        }

        public Product Remove(int id) {
            var entity = _context.Products.First(p => p.Id == id);
            _context.Products.Remove(entity);
            _context.SaveChanges();
            return entity;
        }

        public void Update(Product product) {
            _context.Products.Update(product);
            _context.SaveChanges();
        }
    }
}
```

Build the app to verify you don't have any compiler errors.

### Register the repository

By defining a repository interface, we can decouple the repository class from the MVC controller that uses it. Instead of instantiating a ProductsRepository inside the controller we will inject an IProductsRepository using the built-in support in ASP.NET Core for dependency injection.

This approach makes it easier to unit test your controllers. Unit tests should inject a mock or stub version of IProductsRepository. That way, the test narrowly targets the controller logic and not the data access layer.

In order to inject the repository into the controller, we need to register it with the DI container. Open the Startup.cs file.

In the ConfigureServices method, add the highlighted code:

```cs
public void ConfigureServices(IServiceCollection services)
{
    // requires using Microsoft.EntityFrameworkCore;
    // and using MarketPlaceService.Data;
    services.AddDbContext<MarketPlaceContext>(opt => opt.UseInMemoryDatabase("MarketPlaceContext"));

    // Add framework services.
    services.AddMvc();
    
    // requires using MarketPlaceService.Models;
    services.AddScoped<IProductsRepository, ProductsRepository>();
}
```

### Add a controller

In Solution Explorer, right-click the Controllers folder. Select Add > New Item. In the Add New Item dialog, select the API Controller - Empty template. Name the class ProductsController. Select Minimal Dependencies

Replace the generated code with the following:

```cs
using MarketPlaceService.Data;
using MarketPlaceService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MarketPlaceService.Controllers {
    [Produces("application/json")]
    [Route("api/Products")]
    public class ProductsController : Controller
    {
        private readonly IProductsRepository _ProductsRepository;

        public ProductsController(IProductsRepository ProductsRepository) {
            _ProductsRepository = ProductsRepository;
        }
    }
}
```

This defines an empty controller class that depends on an IProductsRepository. In the next sections, we'll add methods to implement the API.

### Getting Product items

To get Product items, add the following methods to the ProductsController class

```cs
[HttpGet]
public IEnumerable<Product> GetAll() {
    return _ProductsRepository.GetAll();
}

[HttpGet("{id}", Name = "GetProduct")]
public IActionResult GetById(int id) {
    var item = _ProductsRepository.Find(id);
    if (item == null)
    {
        return NotFound();
    }
    return new ObjectResult(item);
}
```

These methods implement the two GET methods:

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

Later in the tutorial I'll show how you can view the HTTP response using Postman.

### Routing and URL paths

The [HttpGet] attribute specifies an HTTP GET method. The URL path for each method is constructed as follows:

Take the template string in the controller's route attribute, [Route("api/Products")]. ASP.NET Core routing is not case sensitive.
If the [HttpGet] attribute has a template string, append that to the path. 
In the GetById method:

```cs
[HttpGet("{id}", Name = "GetProduct")]
public IActionResult GetById(string id)
```

"{id}" is a placeholder variable for the ID of the product item. When GetById is invoked, it assigns the value of "{id}" in the URL to the method's id parameter.

Name = "GetProduct" creates a named route and allows you to link to this route in an HTTP Response. I'll explain it with an example later. 

Return values

The GetAll method returns an IEnumerable. MVC automatically serializes the object to JSON and writes the JSON into the body of the response message. The response code for this method is 200, assuming there are no unhandled exceptions. (Unhandled exceptions are translated into 5xx errors.)

In contrast, the GetById method returns the more general IActionResult type, which represents a wide range of return types. GetById has two different return types:

If no item matches the requested ID, the method returns a 404 error. This is done by returning NotFound.

Otherwise, the method returns 200 with a JSON response body. This is done by returning an ObjectResult

Launch the app

In Visual Studio, press CTRL+F5 to launch the app. Visual Studio launches a browser and navigates to http://localhost:port/api/values, where port is a randomly chosen port number. If you're using Chrome, Edge or Firefox, the data will be displayed in a json format. If you're using IE, IE will prompt to you open or save the values.json file. Navigate to the product controller we just created http://localhost:port/api/products
You should see 
```[{"id":1,"name":"Product 1","description":"First Sample Product","price":1234.0}]```


### Implement the other CRUD operations

We'll add Create, Update, and Delete methods to the controller. These are variations on a theme, so I'll just show the code and highlight the main differences. Build the project after adding or changing code.

Create

```cs
[HttpPost]
public IActionResult Create([FromBody] Product product) {
    if (product == null) {
        return BadRequest();
    }

    _ProductsRepository.Add(product);

    return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
}
```

This is an HTTP POST method, indicated by the [HttpPost] attribute. The [FromBody] attribute tells MVC to get the value of the Product item from the body of the HTTP request.

The CreatedAtRoute method returns a 201 response, which is the standard response for an HTTP POST method that creates a new resource on the server. CreatedAtRoute also adds a Location header to the response. The Location header specifies the URI of the newly created Product item. See 10.2.2 201 Created.

Use Postman to send a Create request

- Set the HTTP method to POST
- Select the Body radio button
- Select the raw radio button
- Set the type to JSON
- In the key-value editor, enter a product item such as

```json
{
  "name":"Product 2",
  "Description":"New Product",
  "Price":555
}
```

Select Send

Select the Headers tab in the lower pane and copy the Location header:

You can use the Location header URI to access the resource you just created. Recall the GetById method created the "GetProduct" named route:

```cs 
[HttpGet("{id}", Name = "GetProduct")]
public IActionResult GetById(string id)
```

Update

```cs
[HttpPut("{id}")]
public IActionResult Update(int id, [FromBody] Product product) {
    if (product == null || product.Id != id) {
        return BadRequest();
    }

    var original = _ProductsRepository.Find(id);
    if (original == null) {
        return NotFound();
    }

    original.Name = product.Name;
    original.Description = product.Description;
    original.Price = product.Price;

    _ProductsRepository.Update(original);
    return new NoContentResult();
}
```

Update is similar to Create, but uses HTTP PUT. The response is 204 (No Content). According to the HTTP spec, a PUT request requires the client to send the entire updated entity, not just the deltas. To support partial updates, use HTTP PATCH.

Delete

```cs
[HttpDelete("{id}")]
public IActionResult Delete(int id) {
    var product = _ProductsRepository.Find(id);
    if (product == null) {
        return NotFound();
    }

    _ProductsRepository.Remove(id);
    return new NoContentResult();
}
```

The response is 204 (No Content).

# Next steps

Proceed to Lab02/Start to continue.