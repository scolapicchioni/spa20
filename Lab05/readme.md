# NOTE: THIS REPO IS OUTDATED. THE NEW VERSION USES VUE CLI 3.11 AND .NET CORE 3.0. REFER TO THE NEW REPO FOR THE NEW STEPS. https://github.com/scolapicchioni/spa30

# FrontEnd: Connecting with the BackEnd

In this lab we're going connect our two projects to each other.

Our client will issue http requests to our server and it will handle the results to update the model. Vue will take care of updating the UI.

We're going to replace our old datalayer with a new one that uses the [fetch API](https://developers.google.com/web/updates/2015/03/introduction-to-fetch), [Promises](https://developers.google.com/web/fundamentals/primers/promises) and [async functions](https://developers.google.com/web/fundamentals/primers/async-functions).

Before starting with the client side, though, let's ensure that our REST service is starts on port 5000 instead of the random one assigned by Visual Studio and that the client starts on port 5001 instead of 8080. This will make easier for us to understand which address is which part of the application, especially when we add a third project for Identity Server later on.

### Configure the REST API to start on Port 5000

- Open your MarketPlaceService project in Visual Studio 2017.
- On the `Solution Explorer`, right click on the project name and select the `Properties`.
- On the `Debug` tab, modify the `App URL` port to `5000`.

We recommend you do this on both the IIS Express and Kestrel settings.

### Configure the JavaScript Client to start on Port 5001

- Open your `spaclient` project in Visual Studio Code.
- Under the `config` folder, open `index.js`.
- Change the `port` property of the `dev` object from `8080` to `5001`.

### Modify the JavaScript datalayer to issue http requests

Under the `src` folder, open `datalayer.js`.

We don't need a `products` array anymore, so we will remove it.
What we do need is a property to store the address of the service.

In the `datalayer` constant, remove 

```js
products: [
  {id: 1, name: 'WIN-WIN survival strategies', description: 'Bring to the table win-win survival strategies to ensure proactive domination.', price: 12345},
  {id: 2, name: 'HIGH level overviews', description: 'Iterative approaches to corporate strategy foster collaborative thinking to further the overall value proposition.', price: 2345},
  {id: 3, name: 'ORGANICALLY grow world', description: 'Organically grow the holistic world view of disruptive innovation via workplace diversity and empowerment.', price: 45678},
  {id: 4, name: 'AGILE frameworks', description: 'Leverage agile frameworks to provide a robust synopsis for high level overviews', price: 9876}
]
```
  
and replace it with

```js
serviceUrl: 'http://localhost:5000/api/products'
```

Now let's change the `getProducts` method by fetching a request to our service, asynchronously waiting for the result and returning the response parsed as json. Don't forget to turn the method into an `async function`.

```js
async getProducts () {
  const response = await fetch(this.serviceUrl)
  return response.json()
}
```

The `getProductById` will be very similar. The only difference is the address to fetch, which will contain the `id` of the product to retrieve:

```js
async getProductById (id) {
  const response = await fetch(`${this.serviceUrl}/${id}`)
  return response.json()
}
```

The `insertProduct` will pass the `fetch` method not only the url to call, but also an object with three properties:
- `method` - set to `POST`
- `body` - set to a json string representing the `product` parameter
- `headers` - set to an instance of the `Header` class, with a `Content-type` property set to `application/json`

```js
async insertProduct (product) {
  const response = await fetch(this.serviceUrl, {
    method: 'POST',
    body: JSON.stringify(product),
    headers: new Headers({
      'Content-Type': 'application/json'
    })
  })
  return response.json()
}
```

The `updateProduct` will be very similar. The only differences are
- the address to fetch, which will contain the `id` of the product to update
- the `method` option, set to `PUT`

```js
async updateProduct (id, product) {
  return fetch(`${this.serviceUrl}/${id}`, {
    method: 'PUT',
    body: JSON.stringify(product),
    headers: new Headers({
      'Content-Type': 'application/json'
    })
  })
}
```

The `deleteProduct` will also fetch an url containing the `id` of the product to delete. The `method` will be `delete` and we won't need neither the `body` nor the `header`.

```js
async deleteProduct (id) {
  return fetch(`${this.serviceUrl}/${id}`, {
    method: 'DELETE'
  })
}
```

By starting both project, you will notice an error in the browser console: 

```
Failed to load http://localhost:5000/api/products: No 'Access-Control-Allow-Origin' header is present on the requested resource. Origin 'http://localhost:5001' is therefore not allowed access. If an opaque response serves your needs, set the request's mode to 'no-cors' to fetch the resource with CORS disabled.
```

This happens because our server does not allow [Cross Origin Requests (CORS)](https://docs.microsoft.com/en-us/aspnet/core/security/cors). Let's proceed to modify our server project.

- Open `Startup.cs`
- In the `ConfigureServices` method, add the following code:

```cs
services.AddCors(options =>
    options.AddPolicy("default", policy =>
        policy.WithOrigins("http://localhost:5001")
            .AllowAnyHeader()
            .AllowAnyMethod()
    )
);
```

- In the `Configure` method, **BEFORE**  `app.useMvc()`, add the following code:

```cs
app.UseCors("default");
```

Now we need to modify our Views to asyncronously wait for our datalayer.

Let's start with our `HomeView.vue`. The `created` method has to become and `async function` and it has to `await` the `getProducts`.

```js
async created () {
  this.products = await datalayer.getProducts()
}
```

Same goes for the `DetailsView.vue`, where both the `$route` method and the `created` method have to become `async functions` and have to `await` for the `getProductById` method of our `datalayer` object.

```js
watch: {
  async '$route' (to, from) {
    this.product = await datalayer.getProductById(+this.$route.params.id)
  }
},
async created () {
  this.product = await datalayer.getProductById(+this.$route.params.id)
}
```

The `insertProduct` of the `CreateView.vue` will also turn into an `async function`:

```js
async insertProduct () {
  await datalayer.insertProduct(this.product)
  this.$router.push('/')
}
```

The `$route`, `created` and `updateProduct` of the `UpdateView.vue` will get the same facelift.

```js
watch: {
  async '$route' (to, from) {
    this.product = await datalayer.getProductById(+this.$route.params.id)
  }
},
async created () {
  this.product = await datalayer.getProductById(+this.$route.params.id)
},
methods: {
  async updateProduct () {
    await datalayer.updateProduct(+this.$route.params.id, this.product)
    this.$router.push('/')
  }
}
```

Save and verify that the client can send and receive data to and from the server.

As a last step, we will configure our service to use a Sql Server database instead of the memory store that we've bee using so far. It is not strictly necessary for our scenario, but at least we will have a persisting data store.

- We will configure our DbContext to use Sql instead of InMemory
- We will create the schema on our DataBase by adding Migrations to our project
- We will seed the DB with a couple of example products 


### Use a Sql Server database

Let's start by configuring our `DbContext` to use SqlServer instead of InMemory

Open `Startup.cs`, locate the `ConfigureServices` method and replace the following code

```cs
services.AddDbContext<MarketPlaceContext>(opt => opt.UseInMemoryDatabase());
```
with this:

```cs
services.AddDbContext<MarketPlaceContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
```

Open the `appsettings.json` file and add a connection string as shown in the following example.

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MarketPlaceService;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

The connection string specifies a *SQL Server LocalDB* database. LocalDB is a lightweight version of the SQL Server Express Database Engine and is intended for application development, not production use. LocalDB starts on demand and runs in user mode, so there is no complex configuration. By default, LocalDB creates `.mdf` database files in the `C:/Users/<user>` directory

### Modify the MarketPlaceContext class

Open the `MarketPlaceService/Data/MarkePlaceContext.cs` file and add the following method to the MarketPlaceContext class:

```cs
protected override void OnModelCreating(ModelBuilder builder) {
    base.OnModelCreating(builder);
}
```

**Ensure that your project builds before going to the next section.**


### Add Migrations

From the [article](https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/migrations):

> When you develop a new application, your data model changes frequently, and each time the model changes, it gets out of sync with the database. The **EF Core Migrations** feature enables EF to create and update the database schema.

To work with migrations, you can use the Package Manager Console (PMC) or the command-line interface (CLI). These tutorials show how to use PMC commands.

### Create an initial migration

Open the `Package Manager Console`. Here's a quick way to do that:

In `Quick Launch` textbox, on the top-right side of Visual Studio, type `Package Manager Console` and open the `Package Manager Console` window.

Enter the following command:

```
Add-Migration InitialCreate
```

### Examine the Up and Down methods

When you executed the migrations add command, EF generated the code that will create the database from scratch. This code is in the `Migrations` folder, in the file named with the date and time followed by `_InitialCreate.cs`. The `Up` method of the `InitialCreate` class creates the database tables that correspond to the data model entity sets, and the `Down` method deletes them, as shown in the following example.

```cs
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Products",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                Description = table.Column<string>(nullable: true),
                Name = table.Column<string>(nullable: true),
                Price = table.Column<decimal>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Products", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Products");
    }
}
```

Migrations calls the `Up` method to implement the data model changes for a migration. When you enter a command to roll back the update, Migrations calls the `Down` method.

This code is for the initial migration that was created when you entered the migrations `Add-Migration InitialCreate` command. The migration name parameter ("InitialCreate" in the example) is used for the file name and can be whatever you want. It's best to choose a word or phrase that summarizes what is being done in the migration. For example, you might name a later migration "AddUserName".

If you created the initial migration when the database already exists, the database creation code is generated but it doesn't have to run because the database already matches the data model. When you deploy the app to another environment where the database doesn't exist yet, this code will run to create your database, so it's a good idea to test it first.

### Examine the data model snapshot

Migrations also creates a snapshot of the current database schema in `Migrations/MarketPlaceContextModelSnapshot.cs`. Here's what that code looks like:

```cs
[DbContext(typeof(MarketPlaceContext))]
partial class MarketPlaceContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
            .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

        modelBuilder.Entity("MarketPlaceService.Models.Product", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Description");

                b.Property<string>("Name");

                b.Property<decimal>("Price");

                b.HasKey("Id");

                b.ToTable("Products");
            });
#pragma warning restore 612, 618
    }
}
```

### Add code to create the database, apply the migrations and initialize the database with test data

In this section, you write a method that is called to create the database, apply every migration and populate it with test data. In the `Data` folder, create a new class file named `DbInitializer.cs` and replace the template code with the following code, which causes a database to be created when needed and loads test data into the new database.

```cs
using MarketPlaceService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MarketPlaceService.Data {
    public static class DbInitializer {
        public static void Initialize(MarketPlaceContext context) {
            //using Microsoft.EntityFrameworkCore;
            context.Database.Migrate();

            // Look for any products.
            if (context.Products.Any())
            {
                return;   // DB has been seeded
            }

            var products = new Product[] {
                new Product { Name = "Product 1", Description = "First Sample Product", Price = 1234 },
                new Product { Name = "Product 2", Description = "Lorem Ipsum", Price = 555 },
                new Product { Name = "Product 3", Description = "Third Sample Product", Price = 333 },
                new Product { Name = "Product 4", Description = "Fourth Sample Product", Price = 44 }
            };

            foreach (var product in products) {
                context.Products.Add(product);
            }

            context.SaveChanges();
        }
    }
}
```

The code checks if there are any products in the database, and if not, it assumes the database is new and needs to be seeded with test data. It loads test data into arrays rather than `List<T>` collections to optimize performance.

In `Startup.cs`, modify the `Configure` method to call this seed method on application startup. First, add the context to the method signature so that ASP.NET dependency injection can provide it to your `DbInitializer` class.

```cs
public void Configure(IApplicationBuilder app, IHostingEnvironment env,  MarketPlaceContext context) {
```

Then call your `DbInitializer.Initialize` method at the end of the `Configure` method.

```cs
app.UseMvc();

DbInitializer.Initialize(context);
```

### Remove the new Product from the Repository Constructor

We don't need to add a new product every time that our Repository gets created as we did before, so let's remove that. Open the `MarketPlaceService/Data/ProductsRepository.cs` file, locate the constructor and remove the following lines of code:

```cs
//init our db with some products if empty
if (_context.Products.Count() == 0) {
    _context.Products.AddRange(
        new Product { Name = "Product 1", Description = "First Sample Product", Price = 1234 },
        new Product { Name = "Product 2", Description = "Second Sample Product", Price = 2345 },
        new Product { Name = "Product 3", Description = "Third Sample Product", Price = 3456 },
        new Product { Name = "Product 4", Description = "Fourth Sample Product", Price = 4567 });
    _context.SaveChanges();
}
```

Run the application and verify that a database is actually created and filled up with the four initial products, by opening the `SQL Server Object Explorer` in `Visual Studio` and inspecting the `MarketPlaceService` database.

We did not implement any security yet. Our next lab will start with setup and configure a new project that will act as an *Authentication Server*. We will then protect the Create operation and we will use the Authentication Server to authenticate the user and have the client gain access to the protected operation.

Go to `Labs/Lab06`, open the `readme.md` and follow the instructions thereby contained.   
