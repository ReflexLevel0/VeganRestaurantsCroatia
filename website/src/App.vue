<template>
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
