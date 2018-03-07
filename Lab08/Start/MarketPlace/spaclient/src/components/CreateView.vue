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
      <mdc-button @click="insertProduct" raised>insert product</mdc-button>
    </form>
  </mdc-layout-cell>
</mdc-layout-grid>
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
    async insertProduct () {
      const result = await datalayer.insertProduct(this.product)
      switch (result) {
        case 'Unauthorized':
          this.$root.$emit('show-snackbar', { message: result })
          break
        default:
          this.$router.push({name: 'HomeView'})
          break
      }
    }
  }
}
</script>

<style>

</style>