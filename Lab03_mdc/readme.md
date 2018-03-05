# FrontEnd: Styling the views with Material Design Components

There are many different [CSS frameworks](https://onaircode.com/top-css-frameworks-web-designer/) around.
[Many](https://www.sitepoint.com/free-material-design-css-frameworks-compared/) implement the [Google Material Design Guidelines](https://material.io/guidelines/).
It's hard to make a choice, so we'll just jump right in and select the Google official [Material Design Components](https://material.io/components/web/), since it's
- Accurate & up to date
- Maintained by Google engineers and designers
- Tested for flexibility, accessibility, and internationalization

Start by installing the library from npm:

```
npm install --save material-components-web
```

The only thing we need to do in the `index.html` is to add a `mdc-typography` class to the `body` tag:

```html
<body class="mdc-typography">
```

We will do the rest in our `App.vue`.

Empty out the `<style>` tag.
Include the MDC Web stylesheet

```css
<style>
  @import url('https://unpkg.com/material-components-web@latest/dist/material-components-web.min.css');
</style>
```

Load Roboto from Google Fonts

```css
  @import url('https://fonts.googleapis.com/css?family=Roboto:300,400,500');
```



Add the MDC Web scripts and call MDC Auto Init.

```html
<script src="node_modules/material-components-web/dist/material-components-web.js"></script>
<script>mdc.autoInit()</script>
</body>
```


We're going to add a [Toolbar](https://material.io/components/web/catalog/toolbar/).

By default, toolbars scroll with the page content. To keep the toolbar fixed to the top of the screen, add an `mdc-toolbar--fixed` class to the toolbar element.

When using `mdc-toolbar--fixed`, you need to set the margin of the content to prevent toolbar overlaying your content. You can add the `mdc-toolbar-fixed-adjust` helper class to the toolbar’s adjacent sibling element, which will add default margin-top.

Wrap the items of our view in the following way: 

```html
<template>
  <div id="app">
    <header class="mdc-toolbar mdc-toolbar--fixed">
      <div class="mdc-toolbar__row">
        <section class="mdc-toolbar__section mdc-toolbar__section--align-start">
          <a href="#" class="material-icons mdc-toolbar__menu-icon">menu</a>
          <span class="mdc-toolbar__title">MarketPlace</span>
        </section>
      </div>
    </header>
    <main class="mdc-toolbar-fixed-adjust">
      <router-view></router-view>
    </main>
  </div>
</template>
```

Next, we're going to change the [Theme](https://material.io/components/web/catalog/theme/) of our site. 

Let's install the theme component:

```
npm install --save @material/theme
```

MDC Theme makes it easy to develop your brand colors. You override the default theme color through Sass variables. 

Add the sass-loader [pre-processor](https://github.com/vuejs-templates/pwa/blob/919c4fa92621012d7d7693e372bc475c1e47d117/docs/pre-processors.md) for sass:

```
npm install sass-loader node-sass --save-dev
```

Specify that your stle is using the sass syntax:

```scss
<style lang="scss" >
</style>
```

Modify the colors of your theme and import the mdc-theme afterwards:

```scss
$mdc-theme-primary: #CDDC39; //lime
$mdc-theme-secondary: #FFC107; //amber
$mdc-theme-background: #fff; //white

@import "./node_modules/@material/theme/mdc-theme";
```

Save and verify that you now have a green header.

We're also going to add a temporary [Drawer](https://material.io/components/web/catalog/drawers/).

> A temporary drawer is usually closed, sliding out at a higher elevation than the content when opened. It is appropriate for any display size.

In order to handle the click event of the menu button, we will add a method to our App component and link it to the button:

```html
<a href="#" class="material-icons mdc-toolbar__menu-icon" @click="openDrawer">menu</a>
```

```js
<script>
import * as mdc from 'material-components-web'
mdc.autoInit()

export default {
  name: 'app',
  methods: {
    openDrawer () {
      let drawer = new mdc.drawer.MDCTemporaryDrawer(document.querySelector('.mdc-drawer--temporary'))
      drawer.open = true
    }
  }
}
</script>
```
In our drawer we will add two links to wich we will [dynamically apply style classes](https://vuejs.org/v2/guide/class-and-style.html) depending on the current route name.

The `main` section of our `template` will look like this:

```html
<main class="mdc-toolbar-fixed-adjust">
  <aside class="mdc-drawer mdc-drawer--temporary mdc-typography">
    <nav class="mdc-drawer__drawer">
      <header class="mdc-drawer__header">
        <div class="mdc-drawer__header-content">
          Market Place
        </div>
      </header>
      <nav id="icon-with-text-demo" class="mdc-drawer__content mdc-list">
        <router-link :to="{name: 'HomeView'}" :class="['mdc-list-item', $route.name === 'HomeView' ? 'mdc-list-item--activated' : '']">
          <i class="material-icons mdc-list-item__graphic" aria-hidden="true">home</i>Home
        </router-link>
        <router-link :to="{name: 'CreateView'}" :class="['mdc-list-item', $route.name === 'CreateView' ? 'mdc-list-item--activated' : '']">
          <i class="material-icons mdc-list-item__graphic" aria-hidden="true">add</i>Add Product
        </router-link>
      </nav>
    </nav>
  </aside>
  <router-view></router-view>
</main>
```

Save and verify that the drawer opens if you click on the menu icon and that clicking on the links opens the correct view and styles the links in the drawer correctly.

Time to tackle the `HomeView`.
We will align our products into a [Grid Layout](https://material.io/components/web/catalog/layout-grid/)

```html
<template>
<div>
  <div class="mdc-layout-grid">
    <div class="mdc-layout-grid__inner">
      <div class="mdc-layout-grid__cell" v-for="product in products" :key="product.id">
        {{ product.id }} - {{ product.name }} 
        <p>{{ product.description }}</p>
        <p>{{ product.price }}</p>
        <div>
          <router-link :to="{name: 'DetailsView', params: {id: product.id}}">details</router-link>
          <router-link :to="{name: 'UpdateView', params: {id: product.id}}">edit</router-link>
          <router-link :to="{name: 'DeleteView', params: {id: product.id}}">delete forever</router-link>
        </div>
      </div>
    </div>
  </div>
  <router-link :to="{name: 'CreateView'}">create</router-link>
</div>
</template>
```

Each product will be displayed into a [Card](https://material.io/components/web/catalog/cards/)

The inside of the cell will have this:

```html
<div class="mdc-card">
  <section class="mdc-card__primary">
    <h1 class="mdc-card__title mdc-card__title--large">{{ product.name }}</h1>
    <h2 class="mdc-card__subtitle">{{ product.id }}</h2>
  </section>
  <section class="mdc-card__supporting-text">
    <p>{{ product.description }}</p>
    <p>{{ product.price }}</p>
  </section>
  <section class="mdc-card__actions">
    <router-link class="mdc-button mdc-button--compact mdc-card__action" :to="{name: 'DetailsView', params: {id: product.id}}">details</router-link>
    <router-link class="mdc-button mdc-button--compact mdc-card__action" :to="{name: 'UpdateView', params: {id: product.id}}">edit</router-link>
    <router-link class="mdc-button mdc-button--compact mdc-card__action" :to="{name: 'DeleteView', params: {id: product.id}}">delete forever</router-link>
  </section>
</div>
```

We will also transform the link to the create view into a [floating action button](https://material.io/components/web/catalog/buttons/floating-action-buttons/)

```html
<button @click="addProduct" class="mdc-fab material-icons app-fab--absolute" aria-label="Create">
  <span class="mdc-fab__icon">add</span>
</button>
```

We will add our own style to position it 

```css
<style>
.app-fab--absolute {
  position: fixed;
  bottom: 1rem;
  right: 1rem;
}

@media(min-width: 1024px) {
   .app-fab--absolute {
    bottom: 1.5rem;
    right: 1.5rem;
  }
}
</style>
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
<div class="mdc-layout-grid">
    <div class="mdc-layout-grid__inner">
      <div class="mdc-layout-grid__cell">
        <div class="mdc-card">
          <section class="mdc-card__primary">
            <h1 class="mdc-card__title mdc-card__title--large">{{ product.name }}</h1>
            <h2 class="mdc-card__subtitle">{{ product.id }}</h2>
          </section>
          <section class="mdc-card__supporting-text">
            <p>{{ product.description }}</p>
            <p>{{ product.price }}</p>
          </section>
          <section class="mdc-card__actions">
            <router-link class="mdc-button mdc-button--compact mdc-card__action mdc-theme--secondary" :to="{name: 'UpdateView', params: {id: product.id}}">edit</router-link>
            <router-link class="mdc-button mdc-button--compact mdc-card__action mdc-theme--secondary" :to="{name: 'DeleteView', params: {id: product.id}}">delete</router-link>
          </section>
        </div>
      </div>
    </div>
</div>
</template>
```


Go to `Labs/Lab04`, open the `readme.md` and follow the instructions thereby contained.   