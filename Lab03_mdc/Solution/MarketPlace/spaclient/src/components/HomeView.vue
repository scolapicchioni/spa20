<template>
<div>
  <div class="mdc-layout-grid">
    <div class="mdc-layout-grid__inner">
      <div class="mdc-layout-grid__cell" v-for="product in products" :key="product.id">
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
            <router-link class="mdc-button mdc-button--compact mdc-card__action mdc-theme--secondary" :to="{name: 'DetailsView', params: {id: product.id}}">details</router-link>
            <router-link class="mdc-button mdc-button--compact mdc-card__action mdc-theme--secondary" :to="{name: 'UpdateView', params: {id: product.id}}">edit</router-link>
            <router-link class="mdc-button mdc-button--compact mdc-card__action mdc-theme--secondary" :to="{name: 'DeleteView', params: {id: product.id}}">delete</router-link>
          </section>
        </div>
      </div>
    </div>
  </div>
  <button @click="addProduct" class="mdc-fab material-icons app-fab--absolute" aria-label="Create">
    <span class="mdc-fab__icon">add</span>
  </button>
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
  created () {
    this.products = datalayer.getProducts()
  },
  methods: {
    addProduct () {
      this.$router.push({name: 'CreateView'})
    }
  }
}
</script>

<style>
.app-fab--absolute {
  position: fixed;
  bottom: 1rem;
  right: 1rem;
}

@media(min-width: 1024px) {
   .app-fab--absolute {
    bottom: 1.5rem;
    right: 1.5rem;
  }
}
</style>
