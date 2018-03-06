<template>
  <div>
    <mdc-layout-grid>
      <mdc-layout-cell v-for="product in products" :key="product.id">
        <mdc-card>
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
import applicationUserManager from '../applicationusermanager'
export default {
  name: 'home-view',
  data () {
    return {
      products: [],
      user: {
        name: '',
        isAuthenticated: false
      }
    }
  },
  methods: {
    addProduct () {
      this.$router.push({name: 'CreateView'})
    },
    async refreshUserInfo () {
      const user = await applicationUserManager.getUser()
      if (user) {
        this.user.name = user.profile.email
        this.user.isAuthenticated = true
      } else {
        this.user.name = ''
        this.user.isAuthenticated = false
      }
    }
  },
  async created () {
    await this.refreshUserInfo()
    this.products = await datalayer.getProducts()
  }
}
</script>

<style>
h1 {
  font-weight: normal;
}
</style>
