<template>
  <br/>
  <router-link to="/" class="btn">Home</router-link>
  <router-link to="/datatable" class="btn">Data table</router-link>
  <button v-if="!accountStore.isAuthenticated" class="btn" @click="login">Log in</button>
  <button v-if="accountStore.isAuthenticated" class="btn" @click="logout">Log out</button>
  <button v-if="accountStore.isAuthenticated" class="btn" @click="onAccountClick">Account</button>
  <br/>
  <router-view>
  </router-view>
</template>

<script>
import {useAuth0} from "@auth0/auth0-vue";
import router from "@/router";
import {useAccountStore} from "@/stores/accountStore";

export default {
  name: 'App',
  setup() {
    const {loginWithRedirect, logout, user, isAuthenticated} = useAuth0();
    const accountStore = useAccountStore()

    return {
      isAuthenticated,
      user,
      loginWithRedirect,
      logout,
      accountStore
    }
  },
  methods: {
    login(){
      console.log(this.accountStore.name)
      if(this.accountStore.name === null){
        this.loginWithRedirect()
      }
      this.accountStore.isAuthenticated = true
    },
    logout(){
      this.accountStore.isAuthenticated = false
      router.push({name: "Home"})
    },
    onAccountClick(){
      if(this.user !== undefined){
        this.accountStore.name = this.user.name
        this.accountStore.nickname = this.user.nickname
        this.accountStore.email = this.user.email
        this.accountStore.isAuthenticated = this.isAuthenticated
      }
      router.push({name: "Account"})
    }
  }
}
</script>

<style>
@import "~bootstrap/dist/css/bootstrap.css";
</style>
