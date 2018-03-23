# Security: Authentication and Authorization

We have not implemented any security yet. In this lab we are going to setup and configure a new project that will act as an *Authentication Server*. We will then protect the **Create** operation and we will use the Authentication Server to authenticate the user and issue a *token*, then have the client gain access to the protected operation by using such token.

## The Authentication Server

We are going to use [Identity Server 4](https://identityserver4.readthedocs.io/en/release/index.html)

We are going to clone the [Samples](https://github.com/IdentityServer/IdentityServer4.Samples)

We are going to use the Combined Project that you find on the following path: IdentityServer4.Samples-release\Quickstarts\Combined_AspNetIdentity_and_EntityFrameworkStorage\src\IdentityServerWithAspIdAndEF

- Create a Folder `IdentityServer` in your `Labs\Lab06\Start\MarketPlace` folder.
- Copy the `IdentityServer4.Samples-release\Quickstarts\Combined_AspNetIdentity_and_EntityFrameworkStorage\src\IdentityServerWithAspIdAndEF` folder into your `Lab06\Start\MarketPlace\IdentityServer` folder
- Open an istance of Visual Studio
- Create a new Blank Solution by going to `File -> New Project -> Other Project Types -> Visual Studio Solutions -> Blank Solution` 
    - Name the Solution `IdentityServer`
    - Create the solution in the `Lab06\Solution\MarketPlace\` folder
- In Visual Studio, in the Solution Explorer, select `Add -> Existing Project`
- Select `Lab06\Start\MarketPlace\IdentityServer\IdentityServerWithAspIdAndEF\IdentityServerWithAspIdAndEF.csproj`   

Now that we have a project, we need to cofigure it for our own purposes.

The first thing we need to configure is our [Resource](https://identityserver4.readthedocs.io/en/release/topics/resources.html)

The client will only need the email, so we will configure that.

In your project, open the `Config.cs` file located in the root of your project. Find the `GetIdentityResources` method and replace its code with:

```cs
public static IEnumerable<IdentityResource> GetIdentityResources() {
    return new List<IdentityResource> {
        new IdentityResources.OpenId(),
        new IdentityResources.Email(),
    };
}
```

The second component to configure is the ApiResource. 
- We will name it `marketplaceapi`
- We will describe it as `MarketPlace API`
- We will include the `Name` of the user in the access token. We will use the name in a future lab to allow products update and deletion only to the product owner.

```cs
public static IEnumerable<ApiResource> GetApiResources(){
    return new List<ApiResource> {
        new ApiResource("marketplaceapi", "MarketPlace API") {
            // include the following using claims in access token (in addition to subject id)
            //requires using using IdentityModel;
            UserClaims = { JwtClaimTypes.Name }
        }
    };
}
```

The last thing we need to configure is the [Javascript Client](https://identityserver4.readthedocs.io/en/release/topics/clients.html)

- The `ClientId` will be `marketplacejs`
- The `ClientName` will be `MarketPlace JavaScript Client`
- We are going to use `GrantTypes.Implicit` as `AllowedGrantTypes` 
- We will set `AllowAccessTokensViaBrowser` to `true`
- `RedirectUris` will be an object with one string set to `http://localhost:5001/#/callback#`
- `PostLogoutRedirectUris` will be an object with one string set to `http://localhost:5001/index.html`
- `AllowedCorsOrigins` will be an object with one string set to `http://localhost:5001`
- The `AllowedScopes` property will be an object with the following 3 values:
    - `IdentityServerConstants.StandardScopes.OpenId`
    - `IdentityServerConstants.StandardScopes.Email`
    - `"marketplaceapi"`

Locate the `GetClients` method and replace its code with the following:

```cs
public static IEnumerable<Client> GetClients() {
    // client credentials client
    return new List<Client> {
        new Client {
            ClientId = "marketplacejs",
            ClientName = "MarketPlace JavaScript Client",
            AllowedGrantTypes = GrantTypes.Implicit,
            AllowAccessTokensViaBrowser = true,

            RedirectUris =           { "http://localhost:5001/#/callback/#" },
            PostLogoutRedirectUris = { "http://localhost:5001/index.html" },
            AllowedCorsOrigins =     { "http://localhost:5001" },

            AllowedScopes = {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Email,
                "marketplaceapi"
            }
        }
    };
}
```

We also need to make sure that our IdentityServer project starts on port `5002`. We will follow the instructions provided under the [Modify Hosting Chapter](https://identityserver4.readthedocs.io/en/release/quickstarts/0_overview.html) of the overview (setting the port to 5002 instead of 5000, since the latter is already occupied by our API).

You may also want to change the `ConnectionString` in the `appsettings.json` to `"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MarketPlaceService-IdentityServer;Trusted_Connection=True;MultipleActiveResultSets=true"`. This is not strictly necessary, but your db will be easier to find in your SQL Explorer, should you want to take a look at it.

This project is also configured to use Google Authentication and an online Demo Identity Server. We can remove those because we're not going to use them.

Open the `Startup.cs`, locate the `ConfigureServices` method and remove the `AddGoogle` and `AddOpenIdConnect` calls. This code

```cs
services.AddAuthentication()
.AddGoogle("Google", options =>
{
    options.ClientId = "434483408261-55tc8n0cs4ff1fe21ea8df2o443v2iuc.apps.googleusercontent.com";
    options.ClientSecret = "3gcoTrEDPPJ0ukn_aYYT6PWo";
})
.AddOpenIdConnect("oidc", "OpenID Connect", options =>
{
    options.Authority = "https://demo.identityserver.io/";
    options.ClientId = "implicit";
    options.SaveTokens = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = "name",
        RoleClaimType = "role"
    };
});
```

simply becomes

```cs
services.AddAuthentication();
```

Before we start our IdentityServer, we need to create and seed the DataBase used for the configuration. If you open `Program.cs` you can see that it checks for a `/seed` argument. If found, it invokes `SeedData.EnsureSeedData(host.Services)`.
The `SeedData` ensures that all the migrations are applied (hence creating the database if not found), reads the `Config.cs` file we modified and uses it to seed the database if the tables are empty. This means that every time you need to change a configuration, not only you need to modify the `Config.cs` file, but you also need to empty the tables and seed the db once again. The easiest would be to drop the db and let the SeedData start again.

Ensure that the applicaction builds, then open a console window under your project folder, by right clicking on the Solution Explorer and selecting `Open Command Line`.
Run the command

```
dotnet run /seed
```

When prompted, Press `CTRL+C` to shut down, go back to Visual Studio and open the `SQL Server Object Explorer`. You should see a `MarketPlace-IdentityServer` Database.
Feel free to explore its structure and content. You will notice the configuration tables used by IdentityServer, but no table for the IdentityDbContext. This happens because our SeedData doesn't use it.  In order to add thos tables, open the Package Manager Console in your Visual Studio environment and type

```
Update-Database -Context "IdentityServerWithAspNetIdentity.Data.ApplicationDbContext"
```

If you refresh the SQL Server Object Explorer you should now see more tables. 

In Visual Studio, run the application and test a user registration, by navigating to `http://localhost:5002/Account/Register` and using `alice@alice.com` as UserName and `Pa$$w0rd` as password. You should see the user correctly registered and logged on.

## Configuring the REST Service

We can now switch to our Web Api project. We need to:
- Configure it to use Identity Server
- Protect the access to the `Create` action to allow only authenticated users

As explained it the [Adding an API](https://identityserver4.readthedocs.io/en/release/quickstarts/1_client_credentials.html#adding-an-api) tutorial, we need to configure the API.

We need to add the authentication services to DI and the authentication middleware to the pipeline. These will:

- validate the incoming token to make sure it is coming from a trusted issuer
- validate that the token is valid to be used with this api (aka scope)


Add the `IdentityServer4.AccessTokenValidation` NuGet package to your project.

We now need to add the authentication services to DI and configure "Bearer" as the default scheme. We can do that thanks to the `AddAuthentication` extension method. We then also have to add the IdentityServer access token validation handler into DI for use by the authentication services, throught the invocation of the `AddIdentityServerAuthentication` extension method, to which we have to configure the `Authority` (which is the http address of our Identity Server) and the `ApiName` (which we set in the previous project as `marketplaceapi`). The Metadata Address or Authority must use HTTPS unless disabled for development by setting `RequireHttpsMetadata=false`.

Open your `Startup` class, locate the `ConfigureServices` method and add the following code:

```cs
services.AddAuthentication("Bearer")
    .AddIdentityServerAuthentication(options => {
        options.Authority = "http://localhost:5002";
        options.RequireHttpsMetadata = false;
        options.ApiName = "marketplaceapi";
    });
```

We also need to add the authentication middleware to the pipeline so authentication will be performed automatically on every call into the host, by invoking the `UseAuthentication` extension method **BEFORE** the `UseMvc` in the `Configure` method of our `Startup` class.

Locate the `Configure` method and add the following code right before the `app.UseMvc()` line:

```cs
app.UseAuthentication();
```

The last step is to protect the `Create` action of our `ProductsController` by using the [Authorize](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/simple) attribute.

Open your `ProductsController` class, locate the `CreateAsync` method and add the `[Authorize]` attribute right before the definition of the method:

```cs
[HttpPost]
[Authorize]
public async Task<IActionResult> CreateAsync([FromBody] Product product) {
    if (product == null) {
        return BadRequest();
    }

    await _ProductsRepository.AddAsync(product);

    return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
}
```

If you use the POSTMAN to invoke the Create action (http://localhost:5001/products/create), you should get a 401 status code in return. This means your API requires a credential.

The API is now protected by IdentityServer.

## Configuring the Javascript Client

The third part requires the configuration of our client project.

Let's begin by testing if the create still works. Run all the three projects, then try to post a new product using our client application. You should see that the product is not added, while you  should still be able to get the list of all products, and also to get the details, modify and delete a specific product.

The user does not get any warning, though. Let's fix this. We need to:
- Add a component to show an error message
- Modify the data layer to return the error message
- Modify the Create View to show the message instead of going to the home route

### Add a component to show an error message

We're going to add a [Snackbar](https://material.io/components/web/catalog/snackbars/) to our `App` View, so that we can also use it from other views. The Snackbar is available as [MDC Adapter Component](https://stasson.github.io/vue-mdc-adapter/#/component/snackbar).

Open your `App.vue` component and locate the `<main>` section.
Add a `<mdc-snackbar />` tag right before the `<\main>` closing tag.

### Modify the data layer to return the error message

Right now our datalayer is not dealing with any problem. We need to change its behavior so that it checks wether the response status is a 201 or not. We expect a 201 (Created) if everything goes fine. Any other number would mean something went wrong. In that case we're going to return the statusMessage. It will be the view's responsibility to show it to the user.

Open the `datalayer.js` file, locate the `insertProduct` method and replace its code with

```js
async insertProduct (product) {
  const response = await fetch(this.serviceUrl, {
    method: 'POST',
    body: JSON.stringify(product),
    headers: new Headers({
    'Content-Type': 'application/json'
    })
  })
  let result
  if (response.status !== 201) {
    result = response.statusText
  } else {
    result = await response.json()
  }
  return result
}
```

### Modify the Create View to show the message instead of going to the home route

We can now inform the user of one of the many problems that could result from an incorrect state. We will set the stage to deal with different results, but we will focus on the `Unauthorized` message and, of course, on the *happy path*. The default behavior will be to push the state of the router as we did before, but, in case the result of the insertProduct method of the datalayer is `Unauthorized`, we will emit the `show-snackbar` event passing the message we got as a result.

Open the `CreateView.vue` file, locate the `insertProduct` method and replace its code with the following:
  
```js
async insertProduct () {
  const result = await datalayer.insertProduct(this.product)
  switch (result) {
    case 'Unauthorized':
        this.$root.$emit('show-snackbar', { message: result })
        break
    default:
        this.$router.push({name: 'HomeView'})
        break
  }
}
```   

If you try to add a new product, you should now see an `Unauthorized` error message popping up from the bottom of the page.

What we need to do is to give the user the chance to log in, get the tokens from Identity Server and add the access token to the post request in order to be authorized.
The process of configuring a javascript client is described in the 
[Identity Server Documentation](https://identityserver4.readthedocs.io/en/release/quickstarts/7_javascript_client.html)

We are going to:
- Add the `oidc-client` library
- Create a `ApplicationUserManager` class extending the [UserManager](https://github.com/IdentityModel/oidc-client-js/wiki) that
    - autoconfigures itself in the constructor
    - provides login and logout functionalities
- Expose a global constant instance of the ApplicationUserManager
- Add a `Login` functionality
    - Add a button to the `App` ViewComponent
    - Handle its click by invoking the `login` method of our `applicationUserManager`
- Implement the LoginCallBack
    - Create a `LoginCallBackView` ViewComponent
    - Invoke the `signinRedirectCallback` method of our `applicationUserManager`
    - Go back to the HomeView
    - Configure the route to the `LoginCallbackView`
- Use the `applicationUserManager` in the `datalayer` to
    - get the access token
    - pass the token in the header of the post request



In order to add the `oidc-client` library, follow the instructions described on the [oidc-client git page](https://github.com/IdentityModel/oidc-client-js): 
open a console window, ensure to navigate to the client project folder, then type

```
npm install oidc-client --save
```

The following steps are:

- Create an `ApplicationUserManager` class extending the [UserManager](https://github.com/IdentityModel/oidc-client-js/wiki) that
    - autoconfigures itself in the constructor
    - provides login and logout functionalities
- Expose a global constant instance of the ApplicationUserManager

Let's start by creating a new file `applicationusermanager.js` in your `src` folder.

- Start by importing the `UserManager` dependency from the `oidc-client` package.
- Create a new class `ApplicationUserManager` that extends `UserManager`
- implement a constructor and invoke the constructor of the base class passing an object with the following properties:
    - authority: 'http://localhost:5002',
    - client_id: 'marketplacejs',
    - redirect_uri: 'http://localhost:5001/#/callback/#',
    - response_type: 'id_token token',
    - scope: 'openid email marketplaceapi',
    - post_logout_redirect_uri: 'http://localhost:5001/index.html'
- implent an `async login` method that
    - asynchronously waits for the `signinRedirect` method
    - invokes the `getUser` method and returns the result
- implement an `async logout` method that invokes the `signoutRedirect` method and returns the result
- Create an instance of the ApplicationUserManager class and export it as a default constant

Your `applicationusermanager.js` file should look like this:

```js
import { UserManager } from 'oidc-client'

class ApplicationUserManager extends Oidc.UserManager {
  constructor () {
    super({
      authority: 'http://localhost:5002',
      client_id: 'marketplacejs',
      redirect_uri: 'http://localhost:5001/#/callback/#',
      response_type: 'id_token token',
      scope: 'openid email marketplaceapi',
      post_logout_redirect_uri: 'http://localhost:5001/index.html'
    })
  }

  async login () {
    await this.signinRedirect()
    return this.getUser()
  }

  async logout () {
    return this.signoutRedirect()
  }
}

const applicationUserManager = new ApplicationUserManager()
export { applicationUserManager as default }

```

Next, we need to:
- Add a `Login` functionality
    - Add a button to the toolbar of the `App` ViewComponent
    - Handle its click by invoking the `login` method of our `applicationUserManager`
    
### Add a button to the toolbar of the `App` ViewComponent

We are going to add to the App toolbar a new right-aligned section containing an icon whose click event will be handled by a `login` method that we will add in a following step.
Let's first think about the UI.

Open the `App.vue` file and locate the `<mdc-toolbar-row>` tag. Right before the end of the toolbar, between the `</mdc-toolbar-section>` and the `</mdc-toolbar-row>`, add the following code:

```html
<mdc-toolbar-section align-end>
  <mdc-toolbar-icon @click="login" icon="person"></mdc-toolbar-icon>
</mdc-toolbar-section>
```

Now it's time to write the logic. We need to make use of our applicationUserManager constant, so the first thing we need to do is to import it from the module.
Locate the `<script>` tag and add this code as first statement:

```js
import applicationUserManager from './applicationusermanager'
```

Now we need to add a new `async login` method that invokes the `login` of our `applicationUserManager` object and asynchronously wait for the result:

```js
export default {
  name: 'app',
  methods: {
    async login () {
      try {
        await applicationUserManager.login()
      } catch (error) {
        console.log(error)
        this.$root.$emit('show-snackbar', { message: error })
      }
    }
  }
}
```

Whenever we login, we are sent to the IdentityServer site where we can proceed to enter our credentials. If we get authenticated by the system, we are then redirected to a callback url. This is what we still don't have and that we're going to create next.

As described in the [Identity Server Documentation](https://identityserver4.readthedocs.io/en/release/quickstarts/7_javascript_client.html)

> This HTML file is the designated `redirect_uri` page once the user has logged into IdentityServer. It will complete the OpenID Connect protocol sign-in handshake with IdentityServer. The code for this is all provided by the UserManager class we used earlier. Once the sign-in is complete, we can then redirect the user back to the main index.html page. 

- To implement the LoginCallBack we need to:
    - Create a `LoginCallBackView` ViewComponent
    - Invoke the `signinRedirectCallback` method of our `applicationUserManager`
    - Go back to the HomeView
    - Configure the route to the `LoginCallbackView`  

Should our site be slow, we will also show a message to inform the user that no further actions are necessary, since the page should soon automatically refresh.

In your `components` folder, create a `LoginCallbackView.vue` file.
In the template, show a message informing that the page should refresh.
In the script, start by importing the `applicationUserManager` object from the `applicationusermanager` module.
Then export the default object giving it a `name` of `logincallback-view`. Ensure that the `created` event is handled by asynchronously waiting for the `signinRedirectCallback` method of the `applicationUserManager` object. Remember to also push the HomeView route so that the page is refreshed.

Your `LoginCallbackView.vue` should look like this:

```html
<template>
<mdc-layout-grid>
  <mdc-layout-cell desktop=12 tablet=8>
    <mdc-text-section>
      <mdc-headline>Login successful</mdc-headline>
      <mdc-title>Your browser should be redirected soon</mdc-title>
    </mdc-text-section>
  </mdc-layout-cell>
</mdc-layout-grid>
</template>

<script>
import applicationUserManager from '../applicationusermanager'

export default {
  name: 'logincallback-view',
  data () {
    return {
    }
  },
  async created () {
    try {
      await applicationUserManager.signinRedirectCallback()
      this.$router.push({name: 'HomeView'})
    } catch (e) {
      console.log(e)
      this.$root.$emit('show-snackbar', { message: e })
    }
  }
}
</script>

<style>

</style>
```  

Now we need to use the `applicationUserManager` in the datalayer to:
- get the access token
- pass the token in the header of the post request

- Open the `datalayer.js` file in your `src` folder.
- Import the `applicationUserManager` object from the `./applicationusermanager` module.
- Modify the `async insertProduct` method to
    - asynchronously wait for the `applicationUserManager.getUser()` method and put the result into a `user` constant
    - add a new `Authorization` property to the `Headers` object, set to `'Bearer '` followed by the `user.access_token` property (if the `user` constant has a value)

Your `datalayer` should begin with

```js
import applicationUserManager from './applicationusermanager'
```

and the `insertProduct` should now look like this:

```js
async insertProduct (product) {
  const user = await applicationUserManager.getUser()
  const response = await fetch(this.serviceUrl, {
    method: 'POST',
    body: JSON.stringify(product),
    headers: new Headers({
    'Content-Type': 'application/json',
    'Authorization': 'Bearer ' + (user ? user.access_token : '')
    })
  })
  let result
  if (response.status !== 201) {
    result = response.statusText
  } else {
    result = await response.json()
  }
  return result
}
```

If you now use our client application to log on, you should see how you get redirected to the IdentityServer site. You should be able to log on using the username and password you registered earlier (`alice@gmail.com` and `Pa$$w0rd`) and you should briefly see the callback url and then the home page. At that point you should be able to insert a new product.

To complete our lab, we're going to give some feedback to the user by 
- Showing the user name if the user is logged on
- Showing a "LOGIN" button if he's not
- Showing the buttons to insert a product only if he's logged on
- Adding a logout option
 
In the `App.vue` file, we're going to [conditionally render a template](https://vuejs.org/v2/guide/conditional.html#Conditional-Groups-with-v-if-on-lt-template-gt) on the toolbar: if the user is not yet authenticated, we will show a "LOGIN" message next to the icon and we will bind the click event of the icon to the `login` method; else, we will show the user name, a "LOGOUT message and we will bind the click event of the icon to a `logout` method. 

We will also update the drawer.
The `Add Product` drawer item will be shown only if the user is authenticated.
After that we will add a divider and then either a login or a logout item. 

In order to do that, we will have to update the `data` function to return a `user` object  with a `name` and a `isAuthenticated` property. We will update this object during creation and at every change in route by invoking the `getUser` method of our `applicationUserManager` object, testing for a result and reading the `profile.email` property of the return value.

The `<script>` section of our `App.vue` will become:

```js
<script>
import applicationUserManager from './applicationusermanager'

export default {
  name: 'app',
  data () {
    return {
      user: {
        name: '',
        isAuthenticated: false
      }
    }
  },
  watch: {
    async '$route' (to, from) {
      await this.refreshUserInfo()
    }
  },
  async created () {
    await this.refreshUserInfo()
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
    },
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
}
</script>
```

The `<template>` section will become:

```html
<template>
<mdc-layout-app>
  <mdc-toolbar slot="toolbar" fixed>
    <mdc-toolbar-row>
      <mdc-toolbar-section align-start >
        <mdc-toolbar-menu-icon event="toggle-drawer"></mdc-toolbar-menu-icon>
        <mdc-toolbar-title>Market Place</mdc-toolbar-title>
      </mdc-toolbar-section>
      <mdc-toolbar-section align-end>
        <template v-if="!user.isAuthenticated">
          <mdc-toolbar-icon @click="login" icon="person"></mdc-toolbar-icon>
          <mdc-subheading>LOGIN</mdc-subheading>
        </template>
        <template v-else>
          <mdc-subheading>{{ user.name }}</mdc-subheading>
          <mdc-toolbar-icon @click="logout" icon="person"></mdc-toolbar-icon>
          <mdc-subheading>LOGOUT</mdc-subheading>
        </template>  
      </mdc-toolbar-section>
    </mdc-toolbar-row>
  </mdc-toolbar>
  <mdc-drawer slot="drawer" toggle-on="toggle-drawer">
    <mdc-drawer-list>
        <mdc-drawer-item start-icon="home" :to="{name: 'HomeView'}" :class="['mdc-list-item', $route.name === 'HomeView' ? 'mdc-list-item--activated' : '']">Home</mdc-drawer-item>
        <mdc-drawer-item v-if="user.isAuthenticated" start-icon="add" :to="{name: 'CreateView'}" :class="['mdc-list-item', $route.name === 'CreateView' ? 'mdc-list-item--activated' : '']">Add Product</mdc-drawer-item>
        <mdc-drawer-divider />
        <mdc-drawer-item v-if="!user.isAuthenticated" start-icon="person" @click="login">Login</mdc-drawer-item>
        <mdc-drawer-item v-else start-icon="person" @click="logout">Logout</mdc-drawer-item>
    </mdc-drawer-list>
  </mdc-drawer>
  <main>
    <router-view></router-view>
    <mdc-snackbar />
  </main>
</mdc-layout-app>
</template>
```

Save, rebuild and test. You should be able to login and logout and see the UI change accordingly.

We have successfully managed to protect the `Create` operation.

What we need to do next is to allow updates and deletes only to the product owners.
This is what we're going to do in the next lab.


Go to `Labs/Lab07`, open the `readme.md` and follow the instructions thereby contained.   