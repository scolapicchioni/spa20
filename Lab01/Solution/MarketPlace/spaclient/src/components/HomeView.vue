<template>
  <div class="homeview">
    <div v-for="product in products" :key="product.id">
      {{ product.id }} - {{ product.name }} 
      <p>{{ product.description }}</p>
      {{ product.price }}
    </div>
  </div>
</template>

<script>
export default {
  name: 'home-view',
  data () {
    return {
      products: [
        {id: 1, name: 'Win-win survival strategies', description: 'Bring to the table win-win survival strategies to ensure proactive domination.', price: 1234},
        {id: 2, name: 'High level overviews', description: 'Iterative approaches to corporate strategy foster collaborative thinking to further the overall value proposition.', price: 23},
        {id: 3, name: 'Organically grow world', description: 'Organically grow the holistic world view of disruptive innovation via workplace diversity and empowerment.', price: 456},
        {id: 4, name: 'Agile frameworks', description: 'Leverage agile frameworks to provide a robust synopsis for high level overviews', price: 98765}
      ]
    }
  }
}
</script>

<style>
h1 {
  font-weight: normal;
}
</style>
