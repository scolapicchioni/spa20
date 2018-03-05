<template>
<form>
  <div>
    <label for="name">Product Name</label>
    <input id="name" v-model="product.name" type="text" placeholder="name"/>
  </div>
  <div>
    <label for="description">Product Description</label>
    <textarea id="description" v-model="product.description" placeholder="description"></textarea>
  </div>
  <div>
    <label for="price">Product Price</label>
    <input id="price" v-model.number="product.price" type="text" placeholder="price"/>
  </div>
  <div>
    <a href="#" @click.prevent="updateProduct">
      UPDATE PRODUCT
    </a>
  </div>
</form>
</template>

<script>
import datalayer from '../datalayer'
export default {
  name: 'update-view',
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
    updateProduct () {
      datalayer.updateProduct(+this.$route.params.id, this.product)
      this.$router.push({name: 'HomeView'})
    }
  }
}
</script>

<style>

</style>