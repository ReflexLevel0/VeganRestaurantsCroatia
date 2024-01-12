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
        let jsonLink = document.createElement("a");
        jsonLink.href = "http://localhost:3000/file/json";
        jsonLink.download = "data.json";
        document.body.appendChild(jsonLink)
        jsonLink.click();

        let csvLink = document.createElement("a");
        csvLink.href = "http://localhost:3000/file/csv";
        csvLink.download = "data.csv";
        document.body.appendChild(csvLink)
        csvLink.click();
      },
      user,
      isAuthenticated
    };
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