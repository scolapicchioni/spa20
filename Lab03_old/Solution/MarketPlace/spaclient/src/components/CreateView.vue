<template>
<div class="mdl-grid">
<form>
  <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
    <input class="mdl-textfield__input" v-model="product.name" type="text" id="name">
    <label class="mdl-textfield__label" for="name">Product Name</label>
  </div>
  <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
    <textarea class="mdl-textfield__input" type="text" rows="1" id="description" v-model="product.description"></textarea>
    <label class="mdl-textfield__label" for="description">Product Description</label>
  </div>
  <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
    <input class="mdl-textfield__input" id="price" v-model.number="product.price" type="text"/>
    <label class="mdl-textfield__label" for="price">Product Price</label>
  </div>
  <a href="#" @click.prevent="insertProduct">
    INSERT PRODUCT
  </a>
</form>
</div>
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
      this.$router.push('/')
    }
  }
}
</script>

<style>

</style>