<template>
  <mdc-layout-grid>
    <mdc-layout-cell desktop=12 tablet=8>
      <mdc-card>
        <mdc-card-media :src="'data:' + product.imageMimeType + ';base64,' + product.imageFile" />
        <mdc-card-header :title="product.name" :subtitle="product.id.toString()" />
        <mdc-card-text> 
          <p>{{ product.description }}</p>
          <p>{{ product.price }}</p>
          <p>{{ product.userName }}</p>
        </mdc-card-text> 
        <mdc-card-actions>
          <mdc-card-action-button v-if="user.name===product.userName" :to="{name: 'UpdateView', params: {id: product.id}}">edit</mdc-card-action-button>
          <mdc-card-action-button v-if="user.name===product.userName" :to="{name: 'DeleteView', params: {id: product.id}}">delete</mdc-card-action-button>
        </mdc-card-actions>
      </mdc-card>
    </mdc-layout-cell>
  </mdc-layout-grid>
</template>

<script>
import datalayer from '../datalayer'
import userAuth from '../mixins/userAuth'
export default {
  name: 'details-view',
  mixins: [userAuth],
  data () {
    return {
      product: {
        id: 0,
        name: '',
        description: '',
        price: 0,
        userName: ''
      }
    }
  },
  watch: {
    async '$route' (to, from) {
      await this.refreshUserInfo()
      this.product = await datalayer.getProductById(+this.$route.params.id)
    }
  },
  async created () {
    this.product = await datalayer.getProductById(+this.$route.params.id)
  }
}
</script>

<style>
.mdc-card-media  {
  height: 50vh;
  background-size:  100% 100%;
}
</style>
