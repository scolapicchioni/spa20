# NOTE: THIS REPO IS OUTDATED. THE NEW VERSION USES VUE CLI 3.11 AND .NET CORE 3.0. REFER TO THE NEW REPO FOR THE NEW STEPS. https://github.com/scolapicchioni/spa30

# Security: Resource Based Authorization

We did not protect the update and delete operations, yet.

What we would like to have is an application where:
- Every product has an owner
- Products may be updated and deleted only by their respective owners

In order to achieve this, we have to update our MarketPlaceService and our JavaScriptClient.

## MarketPlaceService
- We'll add a UserName property to the Product class so that we can persist who the owner is
- We will update the Create action to check the UserName property of the product being added
- We will configure the Authorization by creating
    - A ProductOwner Policy
    - A ProductOwner Requirement
    - A ProductOwnerAuthorizationHandler. This handler will succeed only if the UserName property of the Product being updated/deleted matches the value of the name claim received in the access_token and the claim has been issued by our own IdentityServer Web Application.
- We will check the ProductOwner Policy on Update eventually denying the user the possibility to complete the action if he's not the product's owner
- We will check the ProductOwner Policy on Delete eventually denying the user the possibility to complete the action if he's not the product's owner

## JavaScriptClient
- We will update the User Interface of the Product Item by adding a userName property to product
- We will add a userName property to current product of the Vue instance setting it to the logged on user name
- We will pass the Credentials to our MarketPlaceService during update, just like we did during the Create
- We will pass the Credentials to our MarketPlaceService during delete, just like we did during the Create
- We will make sure that the Update and Delete buttons are shown only if allowed:
    - We will add a userIsOwner computed property to product-item component
    - We will show the update and delete buttons of each product-item only if userIsOwner is true


Let's start by updating our MarketPlace Service.

### The Model and the DataBase

We are currently missing a crucial information about our product: the name of the user that created the product. The easiest thing we can do is add a new property on our `Product` model and update the database schema accordingly. Thanks to Entity Framework Migrations, it is going to be easy.

Open the `Product` class (under the `Models` folder) and add a new `UserName` public property  of type `string`:

```cs
public string UserName { get; set; }
```

In order to update the database schema we need to add a migration in the code, then invoke the EF command to update the database.

Open the `Package manager Console` and type:

```
Add-Migration "ProductUserName"
```

Then type:

```
Update-DataBase
```

This will ensure that the model and the database match.


### Update the Create action to check the UserName property of the product being added

We may expect the client to send the Product to create already filled up with the correct UserName property, but just to be sure we are going to ensure that the UserName property matches the Name property of the User.Identity object, rejecting the request if it doesn't.

Open the `MarketPlaceService/Controllers/ProductsController.cs`, locate the `Create` action and replace this code:

```cs
if (product == null) {
    return BadRequest();
}
```

with the following:

```cs
if (product == null || product.UserName != User.Identity.Name) {
    return BadRequest();
}
```

What we also want to do is to update the database data with some user names (the newly added column contains a null right now).
We want to
- Update the `SeedData` method
- Fill the DB with the new data

Open the `Data/DbInitializer.cs` file, locate the `SeedData` method and replace this code

```cs
var products = new Product[] {
    new Product { Name = "Product 1", Description = "First Sample Product", Price = 1234 },
    new Product { Name = "Product 2", Description = "Lorem Ipsum", Price = 555 },
    new Product { Name = "Product 3", Description = "Third Sample Product", Price = 333 },
    new Product { Name = "Product 4", Description = "Fourth Sample Product", Price = 44 }
};
```

with the following

```cs
var products = new Product[] {
    new Product { Name = "Product 1", Description = "First Sample Product", Price = 1234 , UserName = "Alice Smith"},
    new Product { Name = "Product 2", Description = "Lorem Ipsum", Price = 555 , UserName = "Bob Smith"},
    new Product { Name = "Product 3", Description = "Third Sample Product", Price = 333 , UserName = "Alice Smith"},
    new Product { Name = "Product 4", Description = "Fourth Sample Product", Price = 44 , UserName = "Candice Smith"}
};
```

Now you have to update the database data. You can either 
- drop the database and let the application run in order to recreate the whole db from scratch, 
or 
- open the db, empty the Products table and let the application run in order to refill it with the new data
or 
- open the db an manually fill the UserName column for each product in your Products table

Whichever strategy you choose, start by opening the `SQL Server Object Explorer` in `Visual Studio` and select your `MarketPlaceService` database then proceed with one of the three options described above. 


[Resource Based Authorization](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/resourcebased?tabs=aspnetcore2x)


## Authorization

Now that the code for our Database is ready, let's proceed to enforce Authorization Policies.

### Custom Policy-Based Authorization

Role authorization and Claims authorization make use of 

- a requirement 
- a handler for the requirement 
- a pre-configured policy

These building blocks allow you to express authorization evaluations in code, allowing for a richer, reusable, and easily testable authorization structure.

An *authorization policy* is made up of one or more *requirements* and registered at application startup as part of the Authorization service configuration, in ```ConfigureServices``` in the ```Startup.cs``` file.

Open the ```Startup.cs``` and add the following code at the bottom of the ```ConfigureServices``` method  

```cs
services.AddAuthorization(options => {
    options.AddPolicy("ProductOwner", policy => policy.Requirements.Add(new ProductOwnerRequirement()));
});
```

Here you can see a "ProductOwner" policy is created with a single requirement, that of being the owner of a product, which is passed as a parameter to the requirement. ```ProductOwnerRequirement``` is a class that we will create in a following step, so don't worry if your code does not compile.

Policies can usually be applied using the ```Authorize``` attribute by specifying the policy name, but not in this case.
Our authorization depends upon the resource being accessed. A Product has a UserName property. Only the product owner is allowed to update it or delete it, so the resource must be loaded from the product repository before an authorization evaluation can be made. This cannot be done with an Authorize attribute, as attribute evaluation takes place before data binding and before your own code to load a resource runs inside an action. Instead of declarative authorization, the attribute method, we must use imperative authorization, where a developer calls an authorize function within their own code.

### Authorizing within your code

Authorization is implemented as a service, ```IAuthorizationService```, registered in the service collection and available via dependency injection for Controllers to access.

```cs
[Produces("application/json")]
[Route("api/Products")]
public class ProductsController : Controller
    //requires using MarketPlaceService.Data;
    private readonly IProductsRepository _ProductsRepository;
    private IAuthorizationService _authorizationService;

    public ProductsController(IProductsRepository ProductsRepository,
        IAuthorizationService authorizationService) {
        _ProductsRepository = ProductsRepository;
        _authorizationService = authorizationService;
    }
    //same code as before
```

The ```IAuthorizationService``` interface has two methods, one where you pass the resource and the policy name and the other where you pass the resource and a list of requirements to evaluate.
To call the service, load your product within your action then call the AuthorizeAsync, returning a ChallengeResult if the `Succeeded` property of the result is false. 

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

    AuthorizationResult authresult = await _authorizationService.AuthorizeAsync(User, original, "ProductOwner");
    if (!authresult.Succeeded) {
        if (User.Identity.IsAuthenticated) {
            return new ForbidResult();
        } else {
            return new ChallengeResult();
        }
    }

    original.Name = product.Name;
    original.Description = product.Description;
    original.Price = product.Price;

    await _ProductsRepository.UpdateAsync(original);
    return new NoContentResult();
}
```

And

```cs
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteAsync(int id) {
    var product = await _ProductsRepository.FindAsync(id);
    if (product == null) {
        return NotFound();
    }

    AuthorizationResult authresult = await _authorizationService.AuthorizeAsync(User, product, "ProductOwner");
    if (!authresult.Succeeded) {
        if (User.Identity.IsAuthenticated) {
            return new ForbidResult();
        } else {
            return new ChallengeResult();
        }
    }

    await _ProductsRepository.RemoveAsync(id);
    return new NoContentResult();
}
```

### Requirements

An authorization requirement is a collection of data parameters that a policy can use to evaluate the current user principal. In our ProductOwner policy the requirement we have is a single parameter, the owner. A requirement must implement ```IAuthorizationRequirement```. This is an empty, marker interface. 
Create a new Folder ```Authorization``` in your MarketPlaceService project.
Add a new ```ProductOwnerRequirement``` class and let the class implement the ```IAuthorizationRequirement``` interface by replacing the file content with the following code :

```cs
using Microsoft.AspNetCore.Authorization;
namespace MarketPlaceService.Authorization {
    public class ProductOwnerRequirement : IAuthorizationRequirement {
    }
}
```

A requirement doesn't need to have data or properties.

### Authorization Handlers

An *authorization handler* is responsible for the evaluation of any properties of a requirement. The authorization handler must evaluate them against a provided ```AuthorizationHandlerContext``` to decide if authorization is allowed. A requirement can have multiple handlers. Handlers must inherit ```AuthorizationHandler<T>``` where ```T``` is the requirement it handles.

We will first look to see if the current user principal has a name claim which has been issued by an Issuer we know and trust. If the claim is missing we can't authorize so we will return. If we have a claim, we'll have to figure out the value of the claim, and if it matches the UserName of the product then authorization will be successful. Once authorization is successful we will call context.Succeed() passing in the requirement that has been successful as a parameter.

In the ```MarketPlaceService/Authorization``` folder, add a ```ProductOwnerAuthorizationHandler``` class and replace its content with the following code:

```cs
using IdentityModel;
using MarketPlaceService.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace MarketPlaceService.Authorization {
    public class ProductOwnerAuthorizationHandler : AuthorizationHandler<ProductOwnerRequirement, Product> {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProductOwnerRequirement requirement, Product resource) {
            if (!context.User.HasClaim(c => c.Type == JwtClaimTypes.Name && c.Issuer == "http://localhost:5002"))
                return Task.CompletedTask;

            var userName = context.User.FindFirst(c => c.Type == JwtClaimTypes.Name && c.Issuer == "http://localhost:5002").Value;

            if (userName == resource.UserName)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
```

Handlers must be registered in the services collection during configuration. 

Each handler is added to the services collection by using ```services.AddSingleton<IAuthorizationHandler, YourHandlerClass>();``` passing in your handler class.

Open the Startup.cs and add this line of code at the bottom of the ```ConfigureServices``` method:

```cs
//requires using Microsoft.AspNetCore.Authorization;
//requires using MarketPlaceService.Authorization;
services.AddSingleton<IAuthorizationHandler, ProductOwnerAuthorizationHandler>();
```

We are now ready to move to the JavaScriptClient

## Update the HomeView and DetailsView components

Let's show the owner of each product. Open the `HomeView.vue` component under the `src\components` folder and replace this code

```html
<mdc-card-text> 
  <p>{{ product.description }}</p>
  <p>{{ product.price }}</p>
</mdc-card-text> 
```

with the following

```html
<mdc-card-text> 
  <p>{{ product.description }}</p>
  <p>{{ product.price }}</p>
  <p>{{ product.userName }}</p>
</mdc-card-text> 
```

Repeat for the `DetailsView.vue` component.

Now we want to show the Update and Delete buttons on each product only if the product itself is owned by the current user. In order to do that we have to use the `authenticationManager` to get who the current user is. We will use this information to to compare it with each product's `userName` property in the view template.
Basically we will repeat the process we followed to show / hide the `Create` button on the `App.vue` component, but instead of testing if the user is authenticated, we will test if the `product.userName` is equal to the `user.name`.
We will do this in both the `HomeView` and the `DetailsView` components.

Let's start with the `HomeView.vue` component.

## Import the applicationUserManager and use it to retrieve the current user

Open the `HomeView.vue` component under your `src/components` folder, locate the `<script>` tag and import the applicationUserManager constant by adding the followin line:

```js
import applicationUserManager from '../applicationusermanager'
```

Update the `data` method to also return a `user` object with a `name` and `isAuthenticated` properties.

```js
data () {
  return {
    products: [],
    user: {
      name: '',
      isAuthenticated: false
    }
  }
}
```

Add a new method `refreshUserInfo` that invokes the `getUser` method of the `applicationUserManager` constant and updates the `user` property accordingly:

```js
async refreshUserInfo () {
  const user = await applicationUserManager.getUser()
  if (user) {
    this.user.name = user.profile.email
    this.user.isAuthenticated = true
  } else {
    this.user.name = ''
    this.user.isAuthenticated = false
  }
}
```

Update the `created` method to invoke the `refreshUserInfo` method:

```js
async created () {
  await this.refreshUserInfo()
  this.products = await datalayer.getProducts()
}
```

We can finally update the template. Locate the following code

```html
<mdc-card-actions>
  <mdc-card-action-button :to="{name: 'DetailsView', params: {id: product.id}}">details</mdc-card-action-button>
  <mdc-card-action-button v-if="user.name===product.userName" :to="{name: 'UpdateView', params: {id: product.id}}">edit</mdc-card-action-button>
  <mdc-card-action-button v-if="user.name===product.userName" :to="{name: 'DeleteView', params: {id: product.id}}">delete</mdc-card-action-button>
</mdc-card-actions>
```

and update it like this:

```html
<mdc-card-actions>
  <mdc-card-action-button :to="{name: 'DetailsView', params: {id: product.id}}">details</mdc-card-action-button>
  <mdc-card-action-button v-if="user.name===product.userName" :to="{name: 'UpdateView', params: {id: product.id}}">edit</mdc-card-action-button>
  <mdc-card-action-button v-if="user.name===product.userName" :to="{name: 'DeleteView', params: {id: product.id}}">delete</mdc-card-action-button>
</mdc-card-actions>
```

If you run the application, you should see the update and delete button only on products created by the logged on user.

We will repeat the same process on the `DetailsView.vue` component.

Open the `src/components/DetailsView.vue` file.
Locate the `<script>` tag and import the `applicationUserManager` module

```js
import applicationUserManager from '../applicationusermanager'
```

Add the `user` property to the `data` function

```js
data () {
  return {
    product: {
      id: 0,
      name: '',
      description: '',
      price: 0
    },
    user: {
      name: '',
      isAuthenticated: false
    }
  }
}
```

Add a `methods` section with a `refreshUserInfo` method that updates the `user` property with the result of the `applicationUserManager.getUser` result

```js
methods: {
  async refreshUserInfo () {
    const user = await applicationUserManager.getUser()
    if (user) {
      this.user.name = user.profile.email
      this.user.isAuthenticated = true
    } else {
      this.user.name = ''
      this.user.isAuthenticated = false
    }
  }
}
```

Invoke the `refreshUserInfo` method from both the `created` and the `watch` methods:

```js
watch: {
  async '$route' (to, from) {
    await this.refreshUserInfo()
    this.product = await datalayer.getProductById(+this.$route.params.id)
  }
},
async created () {
  await this.refreshUserInfo()
  this.product = await datalayer.getProductById(+this.$route.params.id)
}
```

Now locate the `<template>` tag and replace the following code

```html
<mdc-card-actions>
  <mdc-card-action-button v-if="user.name===product.userName" :to="{name: 'UpdateView', params: {id: product.id}}">edit</mdc-card-action-button>
  <mdc-card-action-button v-if="user.name===product.userName" :to="{name: 'DeleteView', params: {id: product.id}}">delete</mdc-card-action-button>
</mdc-card-actions>
```

with whis code:

```html
<mdc-card-actions>
  <mdc-card-action-button v-if="user.name===product.userName" :to="{name: 'UpdateView', params: {id: product.id}}">edit</mdc-card-action-button>
  <mdc-card-action-button v-if="user.name===product.userName" :to="{name: 'DeleteView', params: {id: product.id}}">delete</mdc-card-action-button>
</mdc-card-actions>
```

If you run the application and go to the Details view, you should see the buttons only if the product was created by the currently logged on user.

Let's also show the `Create` button on our HomeView only if the user is authenticated.

Go back to the `src/components/HomeView.vue` file, locate the following code

```html
<mdc-fab fixed @click="addProduct" icon="add"></mdc-fab>
```

and replace it with

```html
<mdc-fab fixed v-if="user.isAuthenticated" @click="addProduct" icon="add"></mdc-fab>
```

You should now see the button only if the user is logged on.

Now let's proceed to update our `datalayer`: we need to pass the credentials during the update and delete, to make sure that our service can recognize the user by extracting the email from the token.

Modify the `updateProduct` method of your `datalayer.js` file as follows:

```js
async updateProduct (id, product) {
  const user = await applicationUserManager.getUser()
  return fetch(`${this.serviceUrl}/${id}`, {
    method: 'PUT',
    body: JSON.stringify(product),
    headers: new Headers({
    'Content-Type': 'application/json',
    'Authorization': 'Bearer ' + (user ? user.access_token : '')
    })
  })
},
```

The `deleteProduct` method becomes:

```js
async deleteProduct (id) {
  const user = await applicationUserManager.getUser()
  return fetch(`${this.serviceUrl}/${id}`, {
    method: 'DELETE',
    headers: new Headers({
    'Content-Type': 'application/json',
    'Authorization': 'Bearer ' + (user ? user.access_token : '')
    })
  })
}
```

If you run the application, you should be able to update and delete a product only if the product owner is the currently logged on user.


We have been repeating the same code on three different components. Let's refactor that by using [Mixins](https://vuejs.org/v2/guide/mixins.html).

> Mixins are a flexible way to distribute reusable functionalities for Vue components. A mixin object can contain any component options. When a component uses a mixin, all options in the mixin will be “mixed” into the component’s own options.

Create a new folder `src/mixins` and add a new file `UserAuth.js`.
In this file we will create a constant `userAuth` where we will transfer the elements that we use in the `App.vue`, `HomeView.vue` and `DetailsView.vue`.

```js
import applicationUserManager from '../applicationusermanager'
const userAuth = {
  data () {
    return {
      user: {
        name: '',
        isAuthenticated: false
      }
    }
  },
  methods: {
    async refreshUserInfo () {
      const user = await applicationUserManager.getUser()
      if (user) {
        this.user.name = user.profile.email
        this.user.isAuthenticated = true
      } else {
        this.user.name = ''
        this.user.isAuthenticated = false
      }
    }
  },
  async created () {
    await this.refreshUserInfo()
  }
}
export default userAuth
```

Now we can import this mixin in those three components and therefore remove the duplicated pieces of code.

The `<script>` section of the `App.vue` file becomes :

```js
<script>
import applicationUserManager from './applicationusermanager'
import userAuth from './mixins/userAuth'
export default {
  name: 'app',
  mixins: [userAuth],
  data () {
    return {
    }
  },
  watch: {
    async '$route' (to, from) {
      await this.refreshUserInfo()
    }
  },
  methods: {
    async login () {
      try {
        await applicationUserManager.login()
      } catch (error) {
        console.log(error)
        this.$root.$emit('show-snackbar', { message: error })
      }
    },
    async logout () {
      try {
        await applicationUserManager.logout()
      } catch (error) {
        console.log(error)
        this.$root.$emit('show-snackbar', { message: error })
      }
    }
  }
}
</script>
```

The `<script>` section of the `HomeView.vue` file becomes:

```js
<script>
import datalayer from '../datalayer'
import userAuth from '../mixins/userAuth'
export default {
  name: 'home-view',
  mixins: [userAuth],
  data () {
    return {
      products: []
    }
  },
  methods: {
    addProduct () {
      this.$router.push({name: 'CreateView'})
    }
  },
  async created () {
    this.products = await datalayer.getProducts()
  }
}
</script>
```

The `<script>` section of the `Details.vue` file becomes:

```js
<script>
import datalayer from '../datalayer'
import userAuth from '../mixins/userAuth'
export default {
  name: 'details-view',
  mixins: [userAuth],
  data () {
    return {
      product: {
        id: 0,
        name: '',
        description: '',
        price: 0,
        userName: ''
      }
    }
  },
  watch: {
    async '$route' (to, from) {
      await this.refreshUserInfo()
      this.product = await datalayer.getProductById(+this.$route.params.id)
    }
  },
  async created () {
    this.product = await datalayer.getProductById(+this.$route.params.id)
  }
}
</script>
```

This concludes this series of tutorials on how to create a Progressive Web pplication using Vuejs, .NET Core Web API and IdentityServer.

