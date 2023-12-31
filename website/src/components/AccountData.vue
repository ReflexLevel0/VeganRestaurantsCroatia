<script>
export default{
  mounted(){
    this.name = localStorage.getItem("name")
    this.nickname = localStorage.getItem("nickname")
    this.email = localStorage.getItem("email")
    this.isAuthenticated = localStorage.getItem("isAuthenticated") !== "false"
  },
  data() {
    return {
      name: null,
      email: null,
      nickname: null,
      isAuthenticated: false
    }
  }
}
</script>

<template>
<div v-if="isAuthenticated === true">
  <div>Name: {{this.$data.name}}</div>
  <div>Nickname: {{this.$data.nickname}}</div>
  <div>Email: {{this.$data.email}}</div>
</div>
</template>

<style scoped>

</style>