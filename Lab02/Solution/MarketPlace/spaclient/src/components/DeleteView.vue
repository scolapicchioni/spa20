<template>
<div>
    <h1>Are you sure you want to delete this product?</h1>
    <div>{{ product.id }}</div>
    <div>{{ product.name }}</div>
    <div>{{ product.description }}</div>
    <div>{{ product.price }}</div>
    <a href="#" @click.prevent="deleteProduct">
      DELETE PRODUCT
    </a>
</div>
</template>

<script>
import datalayer from '../datalayer'
export default {
  name: 'delete-view',
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
  },
  methods: {
    deleteProduct () {
      datalayer.deleteProduct(+this.$route.params.id)
      this.$router.push({name: 'HomeView'})
    }
  }
}
</script>

<style>

</style>