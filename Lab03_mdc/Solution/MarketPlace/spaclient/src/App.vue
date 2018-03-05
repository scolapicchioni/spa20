<template>
  <div id="app">
    <header class="mdc-toolbar mdc-toolbar--fixed">
      <div class="mdc-toolbar__row">
        <section class="mdc-toolbar__section mdc-toolbar__section--align-start">
          <a href="#" class="material-icons mdc-toolbar__menu-icon" @click="openDrawer">menu</a>
          <span class="mdc-toolbar__title">MarketPlace</span>
        </section>
      </div>
    </header>
    <main class="mdc-toolbar-fixed-adjust">
      <aside class="mdc-drawer mdc-drawer--temporary mdc-typography">
        <nav class="mdc-drawer__drawer">
          <header class="mdc-drawer__header">
            <div class="mdc-drawer__header-content">
              Market Place
            </div>
          </header>
          <nav id="icon-with-text-demo" class="mdc-drawer__content mdc-list">
            <router-link :to="{name: 'HomeView'}" :class="['mdc-list-item', $route.name === 'HomeView' ? 'mdc-list-item--activated' : '']">
              <i class="material-icons mdc-list-item__graphic" aria-hidden="true">home</i>Home
            </router-link>
            <router-link :to="{name: 'CreateView'}" :class="['mdc-list-item', $route.name === 'CreateView' ? 'mdc-list-item--activated' : '']">
              <i class="material-icons mdc-list-item__graphic" aria-hidden="true">add</i>Add Product
            </router-link>
          </nav>
        </nav>
      </aside>
      <router-view></router-view>
    </main>
  </div>
</template>

<script>
import * as mdc from 'material-components-web'

export default {
  name: 'app',
  mounted () {
    mdc.autoInit()
  },
  methods: {
    openDrawer () {
      let drawer = new mdc.drawer.MDCTemporaryDrawer(document.querySelector('.mdc-drawer--temporary'))
      drawer.open = true
    }
  }
}
</script>

<style lang="scss" >
@import url('https://fonts.googleapis.com/icon?family=Material+Icons');
@import url('https://unpkg.com/material-components-web@latest/dist/material-components-web.min.css');
@import url('https://fonts.googleapis.com/css?family=Roboto:300,400,500');

$mdc-theme-primary: #CDDC39; //lime
$mdc-theme-secondary: #FFC107; //amber
$mdc-theme-background: #fff; //white

@import "./node_modules/@material/theme/mdc-theme";


</style>
