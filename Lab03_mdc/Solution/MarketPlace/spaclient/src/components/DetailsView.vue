<template>
<div class="mdc-layout-grid">
    <div class="mdc-layout-grid__inner">
      <div class="mdc-layout-grid__cell">
        <div class="mdc-card">
          <section class="mdc-card__primary">
            <h1 class="mdc-card__title mdc-card__title--large">{{ product.name }}</h1>
            <h2 class="mdc-card__subtitle">{{ product.id }}</h2>
          </section>
          <section class="mdc-card__supporting-text">
            <p>{{ product.description }}</p>
            <p>{{ product.price }}</p>
          </section>
          <section class="mdc-card__actions">
            <router-link class="mdc-button mdc-button--compact mdc-card__action mdc-theme--secondary" :to="{name: 'UpdateView', params: {id: product.id}}">edit</router-link>
            <router-link class="mdc-button mdc-button--compact mdc-card__action mdc-theme--secondary" :to="{name: 'DeleteView', params: {id: product.id}}">delete</router-link>
          </section>
        </div>
      </div>
    </div>
</div>
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
    '$route' (to, from) {
      this.product = datalayer.getProductById(+this.$route.params.id)
    }
  },
  created () {
    this.product = datalayer.getProductById(+this.$route.params.id)
  }
}
</script>

<style>

</style>
