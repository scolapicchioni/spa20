<template>
<mdc-layout-grid>
  <mdc-layout-cell desktop=12 tablet=8>
    <form>
      <div>
        <mdc-textfield v-model="product.name" label="Product Name" />
      </div>
      <div>
        <mdc-textfield v-model="product.description" label="Product Description" multiline rows="8" cols="40" />
      </div>
      <div>
        <mdc-textfield v-model.number="product.price" label="Product Price" />
      </div>
      <mdc-button @click="updateProduct" raised>update product</mdc-button>
    </form>
  </mdc-layout-cell>
</mdc-layout-grid>
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
      this.$router.push({name: 'CreateView'})
    }
  }
}
</script>

<style>

</style>