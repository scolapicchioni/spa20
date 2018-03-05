<template>
  <div>
    <mdc-layout-grid>
      <mdc-layout-cell v-for="product in products" :key="product.id">
        <mdc-card>
          <mdc-card-header :title="product.name" :subtitle="product.id" />
          <mdc-card-text> 
            <p>{{ product.description }}</p>
            <p>{{ product.price }}</p>
          </mdc-card-text> 
          <mdc-card-actions>
            <mdc-card-action-button :to="{name: 'DetailsView', params: {id: product.id}}">details</mdc-card-action-button>
            <mdc-card-action-button :to="{name: 'UpdateView', params: {id: product.id}}">edit</mdc-card-action-button>
            <mdc-card-action-button :to="{name: 'DeleteView', params: {id: product.id}}">delete</mdc-card-action-button>
          </mdc-card-actions>
        </mdc-card>
      </mdc-layout-cell>
    </mdc-layout-grid>
    <mdc-fab fixed @click="addProduct" icon="add"></mdc-fab>
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
  methods: {
    addProduct () {
      this.$router.push({name: 'CreateView'})
    }
  },
  async created () {
    this.products = await datalayer.getProducts()
  }
}
</script>

<style>
h1 {
  font-weight: normal;
}
</style>
