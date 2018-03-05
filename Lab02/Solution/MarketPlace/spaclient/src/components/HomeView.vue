<template>
  <div class="homeview">
    <div v-for="product in products" :key="product.id">
      {{ product.id }} - {{ product.name }} 
      <p>{{ product.description }}</p>
      <p>{{ product.price }}</p>
      <div>
        <router-link :to="{name: 'DetailsView', params: {id: product.id}}">details</router-link>
        <router-link :to="{name: 'UpdateView', params: {id: product.id}}">edit</router-link>
        <router-link :to="{name: 'DeleteView', params: {id: product.id}}">delete forever</router-link>
      </div>
    </div>
    <router-link :to="{name: 'CreateView'}">create</router-link>
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
h1 {
  font-weight: normal;
}
</style>
