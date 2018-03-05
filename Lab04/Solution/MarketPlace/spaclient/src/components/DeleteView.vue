<template>
<mdc-layout-grid>
    <mdc-layout-cell desktop=12 tablet=8>
      <mdc-card>
        <mdc-card-header :title="'Are you sure you want to delete ' + product.name + '?'" :subtitle="product.id.toString()" />
        <mdc-card-text> 
          <p>{{ product.description }}</p>
          <p>{{ product.price }}</p>
        </mdc-card-text> 
        <mdc-card-actions>
          <mdc-card-action-button @click="deleteProduct">delete</mdc-card-action-button>
          <mdc-card-action-button :to="{name: 'HomeView'}">home</mdc-card-action-button>
        </mdc-card-actions>
      </mdc-card>
    </mdc-layout-cell>
  </mdc-layout-grid>
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
      this.$router.push({name: 'CreateView'})
    }
  }
}
</script>

<style>

</style>