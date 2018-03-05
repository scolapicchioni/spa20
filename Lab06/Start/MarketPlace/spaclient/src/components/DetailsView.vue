<template>
  <mdc-layout-grid>
    <mdc-layout-cell desktop=12 tablet=8>
      <mdc-card>
        <mdc-card-header :title="product.name" :subtitle="product.id" />
        <mdc-card-text> 
          <p>{{ product.description }}</p>
          <p>{{ product.price }}</p>
        </mdc-card-text> 
        <mdc-card-actions>
          <mdc-card-action-button :to="{name: 'UpdateView', params: {id: product.id}}">edit</mdc-card-action-button>
          <mdc-card-action-button :to="{name: 'DeleteView', params: {id: product.id}}">delete</mdc-card-action-button>
        </mdc-card-actions>
      </mdc-card>
    </mdc-layout-cell>
  </mdc-layout-grid>
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
    async '$route' (to, from) {
      this.product = await datalayer.getProductById(+this.$route.params.id)
    }
  },
  async created () {
    this.product = await datalayer.getProductById(+this.$route.params.id)
  }
}
</script>

<style>

</style>
