<template>
<mdc-layout-grid>
  <mdc-layout-cell desktop=12 tablet=8>
    <mdc-text-section>
      <mdc-headline>Login successful</mdc-headline>
      <mdc-title>Your browser should be redirected soon</mdc-title>
    </mdc-text-section>
  </mdc-layout-cell>
</mdc-layout-grid>
</template>

<script>
import applicationUserManager from '../applicationusermanager'

export default {
  name: 'logincallback-view',
  data () {
    return {
    }
  },
  async created () {
    try {
      await applicationUserManager.signinRedirectCallback()
      this.$router.push({name: 'HomeView'})
    } catch (e) {
      console.log(e)
      this.$root.$emit('show-snackbar', { message: e })
    }
  }
}
</script>

<style>

</style>