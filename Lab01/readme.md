# NOTE: THIS REPO IS OUTDATED. THE NEW VERSION USES VUE CLI 3.11 AND .NET CORE 3.0. REFER TO THE NEW REPO FOR THE NEW STEPS. https://github.com/scolapicchioni/spa30

# The Vue FrontEnd: A Progressive Web Application

We will start by building the client side application using Vue.js.

From [the official Vue documentation:](https://vuejs.org/v2/guide/)

## What is Vue.js?

> Vue (pronounced /vjuː/, like view) is a progressive framework for building user interfaces. Unlike other monolithic frameworks, Vue is designed from the ground up to be incrementally adoptable. The core library is focused on the view layer only, and is easy to pick up and integrate with other libraries or existing projects. On the other hand, Vue is also perfectly capable of powering sophisticated Single-Page Applications when used in combination with modern tooling and supporting libraries.

Watch the video on the documentation site in order to understand what Vue is and how it works.

Although not recommended for beginners, we will make use of already made templates that will take care of configuration and build steps for us.
In order to install and use these, we first need **npm**.

If do not know what npm is, how to install it and how to use it, watch lessons 1 to 10 of the [documentation](
https://docs.npmjs.com/getting-started/what-is-npm).

When you're done, install the latest version of **node** 

## vue-cli Installation

We are going to install and use [vue-cli](https://vuejs.org/v2/guide/installation.html)

> Vue.js provides an [official CLI](https://github.com/vuejs/vue-cli) for quickly scaffolding ambitious Single Page Applications. It provides batteries-included build setups for a modern frontend workflow. It takes only a few minutes to get up and running with hot-reload, lint-on-save, and production-ready builds

And we are going to create a new project using the [PWA template](https://github.com/vuejs-templates/pwa)

Do not worry if you don't know what a Progressive Web Application (PWA) is, we are going to focus on this aspect in a later lab. For now we're just going to use this template in order to get a project that will invoke **WebPack** to build our assets.
If you want to know more about WebPack, read the [documentation](https://webpack.js.org/concepts/) and follow the [Core Concepts of the WebPack Academy](https://webpack.academy/p/the-core-concepts).

- Open a command prompt
- Navigate to the ```Labs\Lab01\Start\MarketPlace``` folder
- Type the following commands

```
npm install -g vue-cli
vue init pwa spaclient
```

You will be asked a few questions.
This is a configuration you could use:

```
? Project name spaclient
? Project short name: fewer than 12 characters to not be truncated on homescreens (default: same as name)
? Project description A Vue.js project
? Author (default)
? Vue build standalone
? Install vue-router? Yes
? Use ESLint to lint your code? Yes
? Pick an ESLint preset Standard
? Setup unit tests with Karma + Mocha? Yes
? Setup e2e tests with Nightwatch? No

   vue-cli · Generated "spaclient".
```

Now type

```
cd spaclient
npm install
npm run dev
```

After a while you should be able to see your browser opening a page with the Vue logo and some links to the Vue docs, resources, ecosystem and so on.

[We also recommend installing the dev tools](https://github.com/vuejs/vue-devtools#vue-devtools)

Open the spaclient folder in Visual Studio Code.

Our entry point is **index.html**.
If you inspect this file you will find in the ```<body>``` tag a 

```html
<div id="app"></div>
``` 

This is where Vue will render its content.

If you open the ```build\webpack.base.conf.js``` file, you will see that the ```entry``` is set to ```app: './src/main.js'```

That file is where our Vue application begins.

Open the ```src/main.js``` file and examine the code that has been written for us:

```js
// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import router from './router'

Vue.config.productionTip = false

/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  template: '<App/>',
  components: { App }
})
```

The code begins by importing the **vue** npm package.
We will skip the other imports and the config for now in order to focus on the [Vue Instance](https://vuejs.org/v2/guide/instance.html)

> Every Vue application starts by creating a new Vue instance with the ```Vue``` function
> 
> (...)
>
> When you create a Vue instance, you pass in an options object.

The first value we are passing as an option is [el](https://012.vuejs.org/api/options.html#el):

>Provide the Vue instance with an existing DOM element. It can be a CSS selector string, an actual HTMLElement, or a function that returns an HTMLElement. Note that the provided element merely serves as a mounting point; it will be replaced if a template is also provided

We are indeed also providing a [template](https://012.vuejs.org/api/options.html#template)

> A string template to be used as the markup for the Vue instance. By default, the template will replace the mounted element.

We are also passing a [component](https://vuejs.org/v2/guide/components.html)

> Components are one of the most powerful features of Vue. They help you extend basic HTML elements to encapsulate reusable code. At a high level, components are custom elements that Vue’s compiler attaches behavior to.

>  ...a component can be used in an instance’s template as a custom element

Example:

Let's say this is my Vue instance:

```js
new Vue({
  el: '#example'
})
```

and this is the template of my instance:

```html
<div id="example">
  <my-component></my-component>
</div>
``` 

If this is the template of **my-component**

```html
<div>A custom component!</div>
```
then Vue will render

```html
<div id="example">
  <div>A custom component!</div>
</div>
```

The **PWA** template we used to create our application uses [Single File Components](https://vuejs.org/v2/guide/single-file-components.html)

The only component used by our Vue Instance is App, which we ```import``` (on line 2) from ```App.vue```.
In order to proceed, let's switch to ```App.vue``` that we can also find in the ```src``` folder.

If you open ```src/App.vue``` you will find the three section of any Single File Component:

```html
<template>
...  
</template>

<script>
...
</script>

<style>
...
</style>
```

The ```<template>``` section will replace the template defined in our Vue Instance. Try to change the 

```html
<header>
    <span>Vue.js PWA</span>
</header>
``` 

with

```html
<header>
    <span>MarketPlace</span>
</header>
```

Save and check your terminal window. You should see the build output and the browser should automatically refresh with the new content (upper left).

**Note: if you have stopped the npm job, restart it by running ```npm run dev``` in the ```spaclient``` folder**

**Note: ensure that the terminal output is ```DONE Compiled successfully```. If it isn't, correct all the error from top to bottom and save the file again.** 

Remove the img with the logo, leaving the ```main``` html element as follows:

```html
<main>
    <router-view></router-view>
</main>
```

Save the file and notice that the page in your browser now has no logo anymore but it still has the links to the documentation and resources.
This content is rendered in place of the ```router-view```.


In order to understand the ```<router-view>``` element of ```App.vue``` and the ```router``` option in ```main.js``` we have to understand the [official vue router](https://github.com/vuejs/vue-router)

With the Vue Router plugin we can map routes to components, then let Vue Router: 
- render the correct component depending on the route
- generate links to specific routes

If we open our entry point (main.js) we can see that we configure the ```route``` option of our Vue Instance with an object that we import from ```router```, which points to the file ```router/index.js```.
That's where we can find the routing configuration.

Open ```router/index.js```.

```js
import Vue from 'vue'
import Router from 'vue-router'
import Hello from '@/components/Hello'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name: 'Hello',
      component: Hello
    }
  ]
})
```

We are importing Vue, the Vue-Router plugin and a custom Home component, then we are configuring Vue to use the Routing plugin, then we make an instance of the Router class passing a configuration object in the constructor.
In the configuration object we map a "/" route to the ```Hello``` component, that we import from the ```src/components/Hello.vue``` file.

Because in the App.vue we use the [router-view element](https://router.vuejs.org/en/api/router-view.html), navigating to the root will render the Hello component.

So to continue we can go to the Hello component, to be found under ```src/components/Hello.vue```.

This is also a Single File Component, in fact we can see once more the three elements ```<template>```, ```<script>``` and ```<style>```.

If you modify the content of the template node as follows and save the component, you should see the page refresh in your browser.

```html
<template>
  <div class="hello">
    <h1>{{ msg }}</h1>
  </div>
</template>

<script>
export default {
  name: 'hello',
  data () {
    return {
      msg: 'Welcome to the MarketPlace'
    }
  }
}
</script>

<style>
h1 {
  font-weight: normal;
}
</style>
```

Here you can see a first example of [Declarative Rendering](https://vuejs.org/v2/guide/index.html#Declarative-Rendering)

Vue has a [Reactive System](https://vuejs.org/v2/guide/instance.html#Data-and-Methods) that binds the data to the view.

> When a Vue instance is created, it adds all the properties found in its data object to Vue’s reactivity system. When the values of those properties change, the view will “react”, updating to match the new values.

Since we are creating a component, our ```data``` property is a [function](https://vuejs.org/v2/guide/components.html#data-Must-Be-a-Function).

What we want to do next is to replace our Hello component with a HomeView component that will render a list of products.

Let's start by renaming our ```Hello.vue``` in ```HomeView.vue```.
You will see the build fail. This is because the router still looks for a Hello component, so let's change that too.

Open ```router/index.js``` search ```Hello``` and replace it with ```HomeView```. Your ```index.js``` should look like this:

```js
import Vue from 'vue'
import Router from 'vue-router'
import HomeView from '@/components/HomeView'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name: 'HomeView',
      component: HomeView
    }
  ]
})
```

If you save your file you should see the build succeed.

Now let's change our ```HomeView```.

First of all, our ```data``` should now be more than just a simple message. We don't have a server to give us the real products yet, therefore we are going to use some fake array at first, just so that we can render something on the screen.
The ```data``` function should return an object with a ```products``` property. The products property should be an ```array``` with some ```product``` object. Each product object should have four properties: ```id``` *(number)*, ```name``` *(string)*, ```description``` *(string)* and ```price``` *(number)*.

You ```data``` function could look something like this:

```js
data () {
    return {
      products: [
        {id: 1, name: 'Win-win survival strategies', description: 'Bring to the table win-win survival strategies to ensure proactive domination.', price: 1234},
        {id: 2, name: 'High level overviews', description: 'Iterative approaches to corporate strategy foster collaborative thinking to further the overall value proposition.', price: 23},
        {id: 3, name: 'Organically grow world', description: 'Organically grow the holistic world view of disruptive innovation via workplace diversity and empowerment.', price: 456},
        {id: 4, name: 'Agile frameworks', description: 'Leverage agile frameworks to provide a robust synopsis for high level overviews', price: 98765}
      ]
    }
  }
```

Save your file and ensure that it gets compiled. If not, resolve your issues before going further.

Now let's update the UI. We will use a loop in order to render multiple divs, one for each product.

The directive to loop through array items is [v-for](https://vuejs.org/v2/guide/list.html)

> We can use the v-for directive to render a list of items based on an array. The v-for directive requires a special syntax in the form of *item in items*, where *items* is the source data array and *item* is an alias for the array element being iterated on

We are also going to use our ```id``` property as [key](https://vuejs.org/v2/guide/list.html#key)

> To give Vue a hint so that it can track each node’s identity, and thus reuse and reorder existing elements, you need to provide a unique key attribute for each item. An ideal value for key would be the unique id of each item.

Replace the ```<h1>{{ message }}</h1>``` with the following code:

```html
<div v-for="product in products" :key="product.id">
  {{ product.id }} - {{ product.name }} 
  <p>{{ product.description }}</p>
  {{ product.price }}
</div>
```

Save your file. The page now contains four divs with the details of our products.
Do not worry about the style. We will fix it on a later lab.

In the next lab we will build four additional views:

- Details
- Insert
- Update
- Delete 

To continue, open ```Labs/Lab02/readme.md``` and follow the provided instructions.

- To know more about vue-router, check this [tutorial](https://scotch.io/tutorials/getting-started-with-vue-router)
- For more information on PWA with Vue, check this [tutorial](https://blog.sicara.com/a-progressive-web-application-with-vue-js-webpack-material-design-part-1-c243e2e6e402)
