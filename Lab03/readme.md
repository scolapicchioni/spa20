# FrontEnd: Styling the views with the View Adapter for the Material Design Components

There are many different [CSS frameworks](https://onaircode.com/top-css-frameworks-web-designer/) around.
[Many](https://www.sitepoint.com/free-material-design-css-frameworks-compared/) implement the [Google Material Design Guidelines](https://material.io/guidelines/).
It's hard to make a choice, so we'll just jump right in and select the Google official [Material Design Components](https://material.io/components/web/), since it's
- Accurate & up to date
- Maintained by Google engineers and designers
- Tested for flexibility, accessibility, and internationalization

Instead of using it from scratch, wwe will take advantage of the [Material Components for Vue.js](https://stasson.github.io/vue-mdc-adapter)

> vue-mdc-adapter is an integration of Material Components and Vue.js which follows the best practices recommended by Google: Using Foundations and Adapters. 

Start by installing the library from npm:

```
npm install --save vue-mdc-adapter
```

Now let's import and use the Vue Components plugin. Open `main.js` and insert

```js
import VueMDCAdapter from 'vue-mdc-adapter'

Vue.use(VueMDCAdapter)
```

We are ready to proceed to `App.vue`.

Empty out the `<style>` tag.

Import the reset, material icons and fonts.

```css
  <style>

@import url('https://cdnjs.cloudflare.com/ajax/libs/normalize/7.0.0/normalize.min.css');
@import url('https://fonts.googleapis.com/icon?family=Material+Icons');
@import url('https://fonts.googleapis.com/css?family=Roboto:300,400,500');
@import url('https://fonts.googleapis.com/css?family=Roboto+Mono:300,400,500');

body {
  font-family: Roboto, sans-serif;
}

</style>
```

We're going to add a [Toolbar](https://material.io/components/web/catalog/toolbar/) which is available as a [component](https://stasson.github.io/vue-mdc-adapter/#/component/toolbar).


Wrap the items of our view in the following way: 

```html
<template>
  <div id="app">
    <mdc-toolbar fixed>
      <mdc-toolbar-row>
        <mdc-toolbar-section align-start >
          <mdc-toolbar-menu-icon event="toggle-drawer"></mdc-toolbar-menu-icon>
          <mdc-toolbar-title>Market Place</mdc-toolbar-title>
        </mdc-toolbar-section>
      </mdc-toolbar-row>
    </mdc-toolbar>
    <main>
      <router-view></router-view>
    </main>
  </div>
</template>
```

Next, we're going to change the [Theme](https://material.io/components/web/catalog/theme/) of our site. 

Install Material Components SASS as a dependency.

```
npm install material-components-web --save
```

Configure Webpack with sass-loader and make sure sass modules can be resolved. Open the `build/utils.js` file and locate the `generateLoaders` function. We need to add a configuration option to the returned object. Locate the `return` and replace the **scss** entry (not the **sass** one!) from

```js
scss: generateLoaders('sass'),
```

to

```js
scss: generateLoaders('sass', {includePaths: ['node_modules']}),
```

MDC Theme makes it easy to develop your brand colors. You override the default theme color through Sass variables. 

Add the sass-loader [pre-processor](https://github.com/vuejs-templates/pwa/blob/919c4fa92621012d7d7693e372bc475c1e47d117/docs/pre-processors.md) for sass:

```
npm install sass-loader node-sass --save-dev
```

Create a `src/theme.scss` file.

Modify the colors of your theme and import the mdc-theme afterwards:

```scss
$mdc-theme-primary: #FFC107; //lime
$mdc-theme-secondary: #CDDC39; //amber
$mdc-theme-background: #fff; //white

@import url('https://cdnjs.cloudflare.com/ajax/libs/normalize/7.0.0/normalize.css');
@import url('https://fonts.googleapis.com/icon?family=Material+Icons');
@import url('https://fonts.googleapis.com/css?family=Roboto:300,400,500');
@import url('https://fonts.googleapis.com/css?family=Roboto+Mono:300,400,500');

@import "vue-mdc-adapter/dist/styles"; 
@import "vue-mdc-adapter/components/styles.scss"; 
```

Open your `src/main.js` and add 

```js
import `./theme.scss`
```

as first line of code.

Optimize your build and leverage the source distribution. To resolve the vue-mdc-adapter sources, open your `build/webpack.base.conf.js`, locate the `module.exports.resolve.alias` property and add

```js
'vue-mdc-adapter': 'vue-mdc-adapter/components'
```

then locate the `module.exports.module.rules`, find the entry whose `loader` is `babel-loader` and change the `include` property from

```js
include: [resolve('src'), resolve('test')]
```

to

```js
include: [resolve('src'), resolve('test'),
        resolve('node_modules/@material'),
        resolve('node_modules/vue-mdc-adapter')]
```

Save and verify that you now have an amber header.

We're also going to add a temporary [Drawer](https://material.io/components/web/catalog/drawers/) wich is also available as [component](https://stasson.github.io/vue-mdc-adapter/#/component/drawer).

> The mdc-drawer component implements permanent, persistent, and temporary drawers. By default the drawer component is responsive and will switch from temporary to persistent design according to viewport width.

> A temporary drawer is usually closed, sliding out at a higher elevation than the content when opened. It is appropriate for any display size.

In our drawer we will add two links to wich we will [dynamically apply style classes](https://vuejs.org/v2/guide/class-and-style.html) depending on the current route name.

In order to position the toolbar, the drawer and the main content correctly, we're going to wrap our content in a [Layout App Component](https://stasson.github.io/vue-mdc-adapter/#/component/layout-app).


Our `template` will look like this:

```html
<template>
<mdc-layout-app>
  <mdc-toolbar slot="toolbar" fixed>
    <mdc-toolbar-row>
      <mdc-toolbar-section align-start >
        <mdc-toolbar-menu-icon event="toggle-drawer"></mdc-toolbar-menu-icon>
        <mdc-toolbar-title>Market Place</mdc-toolbar-title>
      </mdc-toolbar-section>
    </mdc-toolbar-row>
  </mdc-toolbar>
  <mdc-drawer slot="drawer" toggle-on="toggle-drawer">
    <mdc-drawer-list>
        <mdc-drawer-item start-icon="home" :to="{name: 'HomeView'}" :class="['mdc-list-item', $route.name === 'HomeView' ? 'mdc-list-item--activated' : '']">Home</mdc-drawer-item>
        <mdc-drawer-item start-icon="add" :to="{name: 'CreateView'}" :class="['mdc-list-item', $route.name === 'CreateView' ? 'mdc-list-item--activated' : '']">Add Product</mdc-drawer-item>
    </mdc-drawer-list>
  </mdc-drawer>
  <main>
    <router-view></router-view>
  </main>
</mdc-layout-app>
</template>
```

Save and verify that the drawer opens and closes if you click on the menu icon and that clicking on the links opens the correct view and styles the links in the drawer correctly.

Time to tackle the `HomeView`.
We will align our products into a [Grid Layout](https://material.io/components/web/catalog/layout-grid/) by using the [grid](https://stasson.github.io/vue-mdc-adapter/#/component/layout-grid) component.

```html
<template>
  <div>
    <mdc-layout-grid>
      <mdc-layout-cell v-for="product in products" :key="product.id">
        {{ product.id }} - {{ product.name }} 
          <p>{{ product.description }}</p>
          <p>{{ product.price }}</p>
          <div>
            <router-link :to="{name: 'DetailsView', params: {id: product.id}}">details</router-link>
            <router-link :to="{name: 'UpdateView', params: {id: product.id}}">edit</router-link>
            <router-link :to="{name: 'DeleteView', params: {id: product.id}}">delete forever</router-link>
          </div>
      </mdc-layout-cell>
    </mdc-layout-grid>
    <router-link :to="{name: 'CreateView'}">create</router-link>
  </div>
</template>
```

Each product will be displayed into a [Card](https://material.io/components/web/catalog/cards/) through a [Card](https://stasson.github.io/vue-mdc-adapter/#/component/card) component.

The inside of the cell will have this:

```html
<mdc-card>
  <mdc-card-header :title="product.name" :subtitle="product.id" />
  <mdc-card-text> 
    <p>{{ product.description }}</p>
    <p>{{ product.price }}</p>
  </mdc-card-text> 
  <mdc-card-actions>
    <mdc-card-action-button :to="{name: 'DetailsView', params: {id: product.id}}">details</mdc-card-action-button>
    <mdc-card-action-button :to="{name: 'UpdateView', params: {id: product.id}}">edit</mdc-card-action-button>
    <mdc-card-action-button :to="{name: 'DeleteView', params: {id: product.id}}">delete</mdc-card-action-button>
  </mdc-card-actions>
</mdc-card>
```

We will also transform the link to the create view into a [floating action button](https://material.io/components/web/catalog/buttons/floating-action-buttons/) thanks to the [FAB component](https://stasson.github.io/vue-mdc-adapter/#/component/fab)

```html
<mdc-fab fixed @click="addProduct" icon="add"></mdc-fab>
```

We will handle the click event by programmatically navigate to the create route.

```js
methods: {
  addProduct () {
    this.$router.push({name: 'CreateView'})
  }
}
```

The template section of the `DetailsView.vue` will look pretty much like the HomeView.

```html
<template>
  <mdc-layout-grid>
    <mdc-layout-cell desktop=12 tablet=8>
      <mdc-card>
        <mdc-card-header :title="product.name" :subtitle="product.id" />
        <mdc-card-text> 
          <p>{{ product.description }}</p>
          <p>{{ product.price }}</p>
        </mdc-card-text> 
        <mdc-card-actions>
          <mdc-card-action-button :to="{name: 'UpdateView', params: {id: product.id}}">edit</mdc-card-action-button>
          <mdc-card-action-button :to="{name: 'DeleteView', params: {id: product.id}}">delete</mdc-card-action-button>
        </mdc-card-actions>
      </mdc-card>
    </mdc-layout-cell>
  </mdc-layout-grid>
</template>
```

Let's tackle the `CreateView`. 
We will need [TextFields](https://material.io/components/web/catalog/input-controls/text-field/) 
therefore we are going to use the [adapter component for vue](https://stasson.github.io/vue-mdc-adapter/#/component/textfield).

We will also transform our link into a [Button](https://stasson.github.io/vue-mdc-adapter/#/component/button)

The `template` section will look like this:

```html
<template>
<mdc-layout-grid>
  <mdc-layout-cell desktop=12 tablet=8>
    <form>
      <div>
        <mdc-textfield v-model="product.name" label="Product Name" />
      </div>
      <div>
        <mdc-textfield v-model="product.description" label="Product Description" multiline rows="8" cols="40" />
      </div>
      <div>
        <mdc-textfield v-model.number="product.price" label="Product Price" />
      </div>
      <mdc-button @click="insertProduct" raised>insert product</mdc-button>
    </form>
  </mdc-layout-cell>
</mdc-layout-grid>
</template>
```

The `UpdateView` will look more or less the same, we just need to modify the button into update instead of add.

```html
<template>
<mdc-layout-grid>
  <mdc-layout-cell desktop=12 tablet=8>
    <form>
      <div>
        <mdc-textfield v-model="product.name" label="Product Name" />
      </div>
      <div>
        <mdc-textfield v-model="product.description" label="Product Description" multiline rows="8" cols="40" />
      </div>
      <div>
        <mdc-textfield v-model.number="product.price" label="Product Price" />
      </div>
      <mdc-button @click="updateProduct" raised>update product</mdc-button>
    </form>
  </mdc-layout-cell>
</mdc-layout-grid>
</template>
```

The last template we have to change is the one of the `DeleteView`.

```html
<template>
<mdc-layout-grid>
    <mdc-layout-cell desktop=12 tablet=8>
      <mdc-card>
        <mdc-card-header :title="'Are you sure you want to delete ' + product.name + '?'" :subtitle="product.id.toString()" />
        <mdc-card-text> 
          <p>{{ product.description }}</p>
          <p>{{ product.price }}</p>
        </mdc-card-text> 
        <mdc-card-actions>
          <mdc-card-action-button @click="deleteProduct">delete</mdc-card-action-button>
          <mdc-card-action-button :to="{name: 'HomeView'}">home</mdc-card-action-button>
        </mdc-card-actions>
      </mdc-card>
    </mdc-layout-cell>
  </mdc-layout-grid>
</template>
```
Our styling is complete.  Our next lab will focus on the back end: we're going to build a REST service using ASP.NET Core 2.0.

Go to `Labs/Lab04`, open the `readme.md` and follow the instructions thereby contained.   