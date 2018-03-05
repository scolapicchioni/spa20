<template>
<form>
  <div class="mdc-text-field">
    <input type="text" id="name" class="mdc-text-field__input" data-mdc-auto-init="MDCTextField" v-model="product.name">
    <label class="mdc-text-field__label" for="name">Product Name</label>
    <div class="mdc-text-field__bottom-line"></div>
  </div>
  <div class="mdc-text-field mdc-text-field--textarea">
    <textarea id="description" class="mdc-text-field__input"  rows="8" cols="40" v-model="product.description"></textarea>
    <label for="description" class="mdc-text-field__label">Product Description</label>
  </div>
  <div class="mdc-text-field  mdc-text-field--upgraded">
    <input type="text" id="price" class="mdc-text-field__input" data-mdc-auto-init="MDCTextField" v-model.number="product.price">
    <label class="mdc-text-field__label mdc-text-field__label--float-above" for="price">Product Price</label>
    <div class="mdc-text-field__bottom-line"></div>
  </div>
  
  <a href="#" @click.prevent="insertProduct">
    INSERT PRODUCT
  </a>
</form>
</template>

<script>
// import * as mdc from 'material-components-web'
/* eslint-disable no-unused-vars */
// import {MDCTextField, MDCTextFieldFoundation} from '@material/textfield'
import datalayer from '../datalayer'

export default {
  name: 'create-view',
  // mounted () {
  // mdc.autoInit()
  // },
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