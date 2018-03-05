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
  <a href="#" @click.prevent="insertProduct">
    INSERT PRODUCT
  </a>
</form>
</template>

<script>
import datalayer from '../datalayer'
export default {
  name: 'create-view',
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
  methods: {
    insertProduct () {
      datalayer.insertProduct(this.product)
      this.$router.push({name: 'HomeView'})
    }
  }
}
</script>

<style>

</style>