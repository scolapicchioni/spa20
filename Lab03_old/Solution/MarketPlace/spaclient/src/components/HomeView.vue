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
