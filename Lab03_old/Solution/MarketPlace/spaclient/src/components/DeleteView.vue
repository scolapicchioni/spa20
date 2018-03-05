<template>
<div class="mdl-grid">
<div class="mdl-card mdl-shadow--4dp mdl-cell mdl-cell--12-col mdl-cell--8-col-tablet">
  <div class="mdl-card__title">
     <h2 class="mdl-card__title-text">Are you sure you want to delete {{ product.name }}?</h2>
  </div>
  <div class="mdl-card__supporting-text">
    <p>{{ product.description }}</p>
    <p>{{ product.price }}</p>  
    <p>({{ product.id }})</p>
  </div>
  <div class="mdl-card__actions mdl-card--border mdl-typography--text-right">
    <a href="#" @click.prevent="deleteProduct" id="btndelete" class="mdl-button mdl-js-button mdl-button--raised mdl-js-ripple-effect mdl-button--icon mdl-button--accent"><i class="material-icons">check</i></a><span for="btndelete" class="mdl-tooltip mdl-tooltip--top">Yes</span>
    <router-link id="btncancel" class="mdl-button mdl-js-button mdl-button--raised mdl-js-ripple-effect mdl-button--icon mdl-button--accent" :to="{name: 'HomeView'}"><i class="material-icons">close</i></router-link><span for="btncancel" class="mdl-tooltip mdl-tooltip--top">No</span>
  </div>
</div>
</div>
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
      this.$router.push('/')
    }
  }
}
</script>

<style>

</style>