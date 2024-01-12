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
  <h1 class="title">Vegan restaurants</h1>
  <div>This website contains all data about vegan restaurants in croatia.</div>
  <p>Github page: <a href="https://github.com/ReflexLevel0/VeganRestaurantsCroatia">https://github.com/ReflexLevel0/VeganRestaurantsCroatia</a></p>
  <div v-if="accountStore.isAuthenticated">
    <div class="bold">Download links:</div>
    <a href="http://localhost:3000/file/json" download="file.json">Full JSON file</a>
    <br/>
    <a href="http://localhost:3000/file/csv" download="file.csv">Full CSV file</a>
  </div>
  <div v-else>
    <p class="bold">You have to log in to download restaurant data!</p>
  </div>
</template>

<style scoped>
.title{
  width: 100%;
  text-align: center;
}

.bold{
  font-weight: bold;
}
</style>