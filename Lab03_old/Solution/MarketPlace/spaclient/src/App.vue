<template>
<div id="app" class="mdl-layout mdl-js-layout mdl-layout--fixed-drawer
            mdl-layout--fixed-header">
  <header class="mdl-layout__header">
    <div class="mdl-layout__header-row">
      <span class="mdl-layout-title">MarketPlace</span>
    </div>
  </header>
  <div class="mdl-layout__drawer">
    <span class="mdl-layout-title">MarketPlace</span>
    <nav class="mdl-navigation">
        <router-link :to="{name: 'HomeView'}" class="mdl-navigation__link">Home</router-link>
        <router-link :to="{name: 'CreateView'}" class="mdl-navigation__link">Add a Product</router-link>
    </nav>
  </div>
  <main class="mdl-layout__content">
    <div class="page-content"><router-view></router-view></div>
  </main>
</div>
</template>

<script>
require('material-design-lite')
export default {
  name: 'app'
}
</script>

<style>
@import url('https://fonts.googleapis.com/icon?family=Material+Icons');
@import url('https://code.getmdl.io/1.2.1/material.lime-orange.min.css');
</style>
