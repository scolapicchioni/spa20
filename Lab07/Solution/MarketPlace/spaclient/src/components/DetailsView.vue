<template>
  <mdc-layout-grid>
    <mdc-layout-cell desktop=12 tablet=8>
      <mdc-card>
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
import applicationUserManager from '../applicationusermanager'
export default {
  name: 'details-view',
  data () {
    return {
      product: {
        id: 0,
        name: '',
        description: '',
        price: 0,
        userName: ''
      },
      user: {
        name: '',
        isAuthenticated: false
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
    await this.refreshUserInfo()
    this.product = await datalayer.getProductById(+this.$route.params.id)
  },
  methods: {
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
  }
}
</script>

<style>

</style>
