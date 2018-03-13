<template>
  <div>
    <mdc-layout-grid>
      <mdc-layout-cell v-for="product in products" :key="product.id">
        <mdc-card>
          <mdc-card-media :src="'data:' + product.imageMimeType + ';base64,' + product.imageFile" />
          <mdc-card-header :title="product.name" :subtitle="product.id.toString()" />
          <mdc-card-text> 
            <p>{{ product.description }}</p>
            <p>{{ product.price }}</p>
            <p>{{ product.userName }}</p>
          </mdc-card-text> 
          <mdc-card-actions>
            <mdc-card-action-button :to="{name: 'DetailsView', params: {id: product.id}}">details</mdc-card-action-button>
            <mdc-card-action-button v-if="user.name===product.userName" :to="{name: 'UpdateView', params: {id: product.id}}">edit</mdc-card-action-button>
            <mdc-card-action-button v-if="user.name===product.userName" :to="{name: 'DeleteView', params: {id: product.id}}">delete</mdc-card-action-button>
          </mdc-card-actions>
        </mdc-card>
      </mdc-layout-cell>
    </mdc-layout-grid>
    <mdc-fab fixed v-if="user.isAuthenticated" @click="addProduct" icon="add"></mdc-fab>
  </div>
</template>

<script>
import datalayer from '../datalayer'
import userAuth from '../mixins/userAuth'
export default {
  name: 'home-view',
  mixins: [userAuth],
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
.mdc-card-media  {
  height: 50vh;
  background-size: 100% 100%;
}
</style>
