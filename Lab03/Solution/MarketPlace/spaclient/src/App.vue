<template>
<mdc-layout-app>
  <mdc-toolbar slot="toolbar" fixed>
    <mdc-toolbar-row>
      <mdc-toolbar-section align-start >
        <mdc-toolbar-menu-icon event="toggle-drawer"></mdc-toolbar-menu-icon>
        <mdc-toolbar-title>Market Place</mdc-toolbar-title>
      </mdc-toolbar-section>
    </mdc-toolbar-row>
  </mdc-toolbar>
  <mdc-drawer slot="drawer" toggle-on="toggle-drawer">
    <mdc-drawer-list>
        <mdc-drawer-item start-icon="home" :to="{name: 'HomeView'}" :class="['mdc-list-item', $route.name === 'HomeView' ? 'mdc-list-item--activated' : '']">Home</mdc-drawer-item>
        <mdc-drawer-item start-icon="add" :to="{name: 'CreateView'}" :class="['mdc-list-item', $route.name === 'CreateView' ? 'mdc-list-item--activated' : '']">Add Product</mdc-drawer-item>
    </mdc-drawer-list>
  </mdc-drawer>
  <main>
    <router-view></router-view>
  </main>
</mdc-layout-app>
</template>

<script>
export default {
  name: 'app'
}
</script>

<style lang="scss">

body {
  font-family: Roboto, sans-serif;
}
</style>
