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
</template>

<script>
import datalayer from '../datalayer'
export default {
  name: 'details-view',
  data () {
    return {
      product: {
        id: 0,
        name: '',
        description: '',
        price: 0
      }
    }
  },
  watch: {
    '$route' (to, from) {
      this.product = datalayer.getProductById(+this.$route.params.id)
    }
  },
  created () {
    this.product = datalayer.getProductById(+this.$route.params.id)
  }
}
</script>

<style>

</style>
