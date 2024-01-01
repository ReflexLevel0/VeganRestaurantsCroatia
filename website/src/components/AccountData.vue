<script>
import {useAccountStore} from "@/stores/accountStore";

export default {
  setup() {
    const accountStore = useAccountStore()
    return {
      accountStore
    }
  }
}
</script>

<template>
  <div v-if="accountStore.isAuthenticated">
    <div>Name: {{ accountStore.user.name }}</div>
    <div>Nickname: {{ accountStore.user.nickname }}</div>
    <div>Email: {{ accountStore.user.email }}</div>
  </div>
  <div v-else class="error">
    ERROR: Not logged in
  </div>
</template>

<style scoped>
.error{
  font-size: 24px;
  color: red;
}
</style>