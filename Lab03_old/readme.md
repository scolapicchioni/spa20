# FrontEnd: Material Design Lite

You can find an exhaustive documentation here: [Get MDL.io](https://getmdl.io/)

Update our dependencies:

```
npm install material-design-lite --save
```

Update `src/App.vue` component to import MDL style and load MDL module:

```html
<script>
require('material-design-lite')
export default {
  name: 'app'
}
</script>

<style>
@import url('https://fonts.googleapis.com/icon?family=Material+Icons');
@import url('https://code.getmdl.io/1.2.1/material.lime-orange.min.css');
</style>
```

We will use a [layout](https://getmdl.io/components/index.html#layout-section) with a fixed header and drawer. In the drawer we're going to add two links, one for the HomeView and one for the CreateView.

```html
<template>
<div id="app" class="mdl-layout mdl-js-layout mdl-layout--fixed-drawer
            mdl-layout--fixed-header">
  <header class="mdl-layout__header">
    <div class="mdl-layout__header-row">
      <span class="mdl-layout-title">MarketPlace</span>
    </div>
  </header>
  <div class="mdl-layout__drawer">
    <span class="mdl-layout-title">MarketPlace</span>
    <nav class="mdl-navigation">
        <router-link :to="{name: 'HomeView'}" class="mdl-navigation__link">Home</router-link>
        <router-link :to="{name: 'CreateView'}" class="mdl-navigation__link">Add a Product</router-link>
    </nav>
  </div>
  <main class="mdl-layout__content">
    <div class="page-content"><router-view></router-view></div>
  </main>
</div>
</template>
```

Save `App.vue` and check that the layout changes.

Now let's proceed to modify the `HomeView.vue` layout.
We're going to use [cards](https://getmdl.io/components/index.html#cards-section) for our products.

We will also use the [grid system](https://getmdl.io/components/index.html#layout-section/grid) to ensure that each card expands nicely depending on the viewport size: there will be two cards next to each other on a desktop and one card per row on a tablet or phone.

Our links will have the style of [buttons](https://getmdl.io/components/index.html#buttons-section) and they will use [icons](https://material.io/icons/) 

We will add [tootltips](https://getmdl.io/components/#tooltips-section) to our buttons, which will require us to [dynamically generate each button id](https://vuejs.org/v2/guide/syntax.html#Attributes) so that we can link the corresponding tooltip.

We will also add a custom style to ensure that the create button remains at the bottom left of the screen at all times.

This is how our HomeView becomes:

```html
<template>
<div class="mdl-grid">
<div v-for="product in products" :key="product.id" class="mdl-card mdl-shadow--4dp mdl-cell mdl-cell--6-col mdl-cell--8-col-tablet">
  <div class="mdl-card__title">
     <h2 class="mdl-card__title-text">{{ product.name }}</h2>
  </div>
  <div class="mdl-card__supporting-text">
    <p>{{ product.description }}</p>
    <p>{{ product.price }}</p>  
    <p>({{ product.id }})</p>
  </div>
  <div class="mdl-card__actions mdl-card--border mdl-typography--text-right">
    <router-link :id="'btndetails' + product.id" class="mdl-button mdl-js-button mdl-button--raised mdl-js-ripple-effect mdl-button--icon mdl-button--accent" :to="{name: 'DetailsView', params: {id: product.id}}"><i class="material-icons">details</i></router-link><span :for="'btndetails' + product.id" class="mdl-tooltip mdl-tooltip--top	">Details</span>
    <router-link :id="'btnedit' + product.id" class="mdl-button mdl-js-button mdl-button--raised mdl-js-ripple-effect mdl-button--icon mdl-button--accent" :to="{name: 'UpdateView', params: {id: product.id}}"><i class="material-icons">edit</i></router-link><span :for="'btnedit' + product.id" class="mdl-tooltip mdl-tooltip--top	">Edit</span>
    <router-link :id="'btndelete' + product.id" class="mdl-button mdl-js-button mdl-button--raised mdl-js-ripple-effect mdl-button--icon mdl-button--accent" :to="{name: 'DeleteView', params: {id: product.id}}"><i class="material-icons">delete forever</i></router-link><span :for="'btndelete' + product.id" class="mdl-tooltip mdl-tooltip--top	">Delete</span>
  </div>
</div>
<router-link class="mdl-button mdl-js-button mdl-button--fab md-fab-bottom-right mdl-button--mini-fab mdl-button--accent" :to="{name: 'CreateView'}"><i class="material-icons">add</i></router-link>
</div>
</template>

<script>
import datalayer from '../datalayer'
export default {
  name: 'home-view',
  data () {
    return {
      products: []
    }
  },
  created () {
    this.products = datalayer.getProducts()
  }
}
</script>

<style>
.md-fab-bottom-right {
  position: fixed;
  bottom: 3%;
  right: 3%;
  z-index:4;
}
</style>
```

Now let's adjust the Details View.

We will use a card, very similar to the HomeView. We just need to remember to remove the details button and the add button. 

```html
<template>
<div class="mdl-grid">
<div class="mdl-card mdl-shadow--4dp mdl-cell mdl-cell--12-col mdl-cell--8-col-tablet">
  <div class="mdl-card__title">
     <h2 class="mdl-card__title-text">{{ product.name }}</h2>
  </div>
  <div class="mdl-card__supporting-text">
    <p>{{ product.description }}</p>
    <p>{{ product.price }}</p>  
    <p>({{ product.id }})</p>
  </div>
  <div class="mdl-card__actions mdl-card--border mdl-typography--text-right">
    <router-link :id="'btnedit' + product.id" class="mdl-button mdl-js-button mdl-button--raised mdl-js-ripple-effect mdl-button--icon mdl-button--accent" :to="{name: 'UpdateView', params: {id: product.id}}"><i class="material-icons">edit</i></router-link><span :for="'btnedit' + product.id" class="mdl-tooltip mdl-tooltip--top	">Edit</span>
    <router-link :id="'btndelete' + product.id" class="mdl-button mdl-js-button mdl-button--raised mdl-js-ripple-effect mdl-button--icon mdl-button--accent" :to="{name: 'DeleteView', params: {id: product.id}}"><i class="material-icons">delete forever</i></router-link><span :for="'btndelete' + product.id" class="mdl-tooltip mdl-tooltip--top	">Delete</span>
  </div>
</div>
</div>
``` 

The delete view will get the same treatment: a card.

```html
<template>
<div class="mdl-grid">
<div class="mdl-card mdl-shadow--4dp mdl-cell mdl-cell--12-col mdl-cell--8-col-tablet">
  <div class="mdl-card__title">
     <h2 class="mdl-card__title-text">Are you sure you want to delete {{ product.name }}?</h2>
  </div>
  <div class="mdl-card__supporting-text">
    <p>{{ product.description }}</p>
    <p>{{ product.price }}</p>  
    <p>({{ product.id }})</p>
  </div>
  <div class="mdl-card__actions mdl-card--border mdl-typography--text-right">
    <a href="#" @click.prevent="deleteProduct" id="btndelete" class="mdl-button mdl-js-button mdl-button--raised mdl-js-ripple-effect mdl-button--icon mdl-button--accent"><i class="material-icons">check</i></a><span for="btndelete" class="mdl-tooltip mdl-tooltip--top">Yes</span>
    <router-link id="btncancel" class="mdl-button mdl-js-button mdl-button--raised mdl-js-ripple-effect mdl-button--icon mdl-button--accent" :to="{name: 'HomeView'}"><i class="material-icons">close</i></router-link><span for="btncancel" class="mdl-tooltip mdl-tooltip--top">No</span>
  </div>
</div>
</div>
</template>
```


Go to `Labs/Lab04`, open the `readme.md` and follow the instructions thereby contained.   