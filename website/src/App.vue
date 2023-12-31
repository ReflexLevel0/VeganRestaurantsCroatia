<template>
  <br/>
  <router-link to="/" class="btn">Home</router-link>
  <router-link to="/datatable" class="btn">Data table</router-link>
  <button v-if="!isAuthenticated" class="btn" @click="login">Log in</button>
  <button v-if="isAuthenticated" class="btn" @click="logout">Log out</button>
  <button v-if="isAuthenticated" class="btn" @click="onAccountClick">Account</button>
  <br/>
  <router-view>
  </router-view>
</template>

<script>
import {useAuth0} from "@auth0/auth0-vue";
import router from "@/router";
import { useAccountStore } from "./stores/accountStore"

export default {
  name: 'App',
  setup() {
    const {loginWithRedirect, logout, user, isAuthenticated} = useAuth0();
    const accStore = useAccountStore()

    return {
      login: () => {
        loginWithRedirect();
      },
      logout: () => {
        logout({logoutParams: {returnTo: window.location.origin}});
        localStorage.setItem("name", "")
        localStorage.setItem("nickname", "")
        localStorage.setItem("email", "")
        localStorage.setItem("isAuthenticated", false.toString())
      },
      isAuthenticated,
      user,
      store: accStore
    }
  },
  methods: {
    onAccountClick(){
      localStorage.setItem("name", this.user.name)
      localStorage.setItem("nickname", this.user.nickname)
      localStorage.setItem("email", this.user.email)
      localStorage.setItem("isAuthenticated", this.isAuthenticated.toString())
      router.push({name: "Account"})
    }
  }
}
</script>

<style>
@import "~bootstrap/dist/css/bootstrap.css";
</style>
