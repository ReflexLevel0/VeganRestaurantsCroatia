<template>
  <a href="http://localhost:3000/json" download="veganCroatia.json">Full JSON file</a>
  <br/>
  <a href="http://localhost:3000/csv" download="veganCroatia.csv">Full CSV file</a>
  <RestaurantTable/>
</template>

<script>
import RestaurantTable from "@/components/RestaurantTable.vue";

export default {
  name: 'App',
  components: {
    RestaurantTable
  }
}
</script>

<style>
@import "~bootstrap/dist/css/bootstrap.css";
</style>
