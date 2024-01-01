<script>
import {useAccountStore} from "@/stores/accountStore";
export default{
  setup(){
    const accountStore = useAccountStore()
    return {
      accountStore
    }
  }
}
</script>

<template>
<div v-if="accountStore.isAuthenticated === true">
  <div>Name: {{accountStore.user.name}}</div>
  <div>Nickname: {{accountStore.user.nickname}}</div>
  <div>Email: {{accountStore.user.email}}</div>
</div>
</template>

<style scoped>

</style>