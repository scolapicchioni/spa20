<template>
<mdc-layout-app>
  <mdc-toolbar slot="toolbar" fixed>
    <mdc-toolbar-row>
      <mdc-toolbar-section align-start >
        <mdc-toolbar-menu-icon event="toggle-drawer"></mdc-toolbar-menu-icon>
        <mdc-toolbar-title>Market Place</mdc-toolbar-title>
      </mdc-toolbar-section>
      <mdc-toolbar-section align-end>
        <template v-if="!user.isAuthenticated">
          <mdc-toolbar-icon @click="login" icon="person"></mdc-toolbar-icon>
          <mdc-subheading>LOGIN</mdc-subheading>
        </template>
        <template v-else>
          <mdc-subheading>{{ user.name }}</mdc-subheading>
          <mdc-toolbar-icon @click="logout" icon="person"></mdc-toolbar-icon>
          <mdc-subheading>LOGOUT</mdc-subheading>
        </template>  
      </mdc-toolbar-section>
    </mdc-toolbar-row>
  </mdc-toolbar>
  <mdc-drawer slot="drawer" toggle-on="toggle-drawer">
    <mdc-drawer-list>
        <mdc-drawer-item start-icon="home" :to="{name: 'HomeView'}" :class="['mdc-list-item', $route.name === 'HomeView' ? 'mdc-list-item--activated' : '']">Home</mdc-drawer-item>
        <mdc-drawer-item v-if="user.isAuthenticated" start-icon="add" :to="{name: 'CreateView'}" :class="['mdc-list-item', $route.name === 'CreateView' ? 'mdc-list-item--activated' : '']">Add Product</mdc-drawer-item>
        <mdc-drawer-divider />
        <mdc-drawer-item v-if="!user.isAuthenticated" start-icon="person" @click="login">Login</mdc-drawer-item>
        <mdc-drawer-item v-else start-icon="person" @click="logout">Logout</mdc-drawer-item>
    </mdc-drawer-list>
  </mdc-drawer>
  <main>
    <router-view></router-view>
    <mdc-snackbar />
  </main>
</mdc-layout-app>
</template>

<script>
import applicationUserManager from './applicationusermanager'
import userAuth from './mixins/userAuth'
export default {
  name: 'app',
  mixins: [userAuth],
  data () {
    return {
    }
  },
  watch: {
    async '$route' (to, from) {
      await this.refreshUserInfo()
    }
  },
  methods: {
    async login () {
      try {
        await applicationUserManager.login()
      } catch (error) {
        console.log(error)
        this.$root.$emit('show-snackbar', { message: error })
      }
    },
    async logout () {
      try {
        await applicationUserManager.logout()
      } catch (error) {
        console.log(error)
        this.$root.$emit('show-snackbar', { message: error })
      }
    }
  }
}
</script>

<style lang="scss">

body {
  font-family: Roboto, sans-serif;
}
</style>
