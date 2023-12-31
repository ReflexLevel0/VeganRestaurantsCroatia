<script>
import {useAuth0} from '@auth0/auth0-vue';

export default {
  setup() {
    const {loginWithRedirect, logout, user, isAuthenticated} = useAuth0();

    return {
      login: () => {
        loginWithRedirect();
      },
      logout: () => {
        logout({logoutParams: {returnTo: window.location.origin}});
      },
      refreshCopies: () => {
        let link = document.createElement("a");
        link.download = name;
        link.href = "http://localhost:3000/json";
        link.click();
        link.href = "http://localhost:3000/csv";
        link.click();
      },
      user,
      isAuthenticated
    };
  },
  watch: {
    isAuthenticated(){
      if(this.isAuthenticated === true && this.user !== null){
        console.log(this.user.hasOwnProperty("name"))
        //localStorage.setItem('user', this.user.valueOf("name"))
        // localStorage.setItem('name', this.user.name)
        // localStorage.setItem('email', this.user.email)
        // localStorage.setItem('email_verified', this.user.email_verified.toString())
      }
    }
  }
};
</script>

<template>
  <h2>User Profile</h2>
  <button @click="login">Log in</button>
  <button @click="logout">Log out</button>
  <button @click="refreshCopies">Refresh copies</button>
  <router-link to="/account" custom v-slot="{navigate}">
    <button @click="navigate" role="link">Account</button>
  </router-link>
  <pre v-if="isAuthenticated">
    <code>{{ user }}</code>
  </pre>
</template>

<style scoped>

</style>