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

export default {
  name: 'app',
  data () {
    return {
      user: {
        name: '',
        isAuthenticated: false
      }
    }
  },
  watch: {
    async '$route' (to, from) {
      await this.refreshUserInfo()
    }
  },
  async created () {
    await this.refreshUserInfo()
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
  }
}
</script>

<style lang="scss">

@import url('https://cdnjs.cloudflare.com/ajax/libs/normalize/7.0.0/normalize.css');
@import url('https://fonts.googleapis.com/icon?family=Material+Icons');
@import url('https://fonts.googleapis.com/css?family=Roboto:300,400,500');
@import url('https://fonts.googleapis.com/css?family=Roboto+Mono:300,400,500');

$mdc-theme-primary: #FFC107; //lime
$mdc-theme-secondary: #CDDC39; //amber
$mdc-theme-background: #fff; //white

@import "./node_modules/@material/theme/mdc-theme";

body {
  font-family: Roboto, sans-serif;
}
</style>
