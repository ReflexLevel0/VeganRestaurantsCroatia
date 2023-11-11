<script>
export default {
  data() {
    return {
      searchOption: "all",
      searchCategories: [
        {value: "all", text: "All fields"},
        {value: "name", text: "Name"},
        {value: "address", text: "Address"},
        {value: "zipcode", text: "Zipcode"},
        {value: "latitude", text: "Latitude"},
        {value: "longitude", text: "Longitude"},
        {value: "phone", text: "Phone"},
        {value: "opening_hours", text: "Opening hours"},
        {value: "delivery", text: "Delivery"},
        {value: "city", text: "City"},
        {value: "linkType", text: "Link type"},
        {value: "link", text: "Link"},
      ],
      searchText: "",
      restaurants: []
    }
  },
  methods: {
    refreshData(){
      alert(`search category ${this.$data.searchOption} for text "${this.$data.searchText}"`)
    }
  }
}
</script>

<template>

  <form>
    <input class="input-group" type="text" placeholder="Zagreb" v-model="$data.searchText"/>
    <select class="form-select" v-model="$data.searchOption">
      <option v-for="category in searchCategories" :value=category.value>{{category.text}}</option>
    </select>
    <button class="btn btn-primary" type="submit" @click="refreshData">Search</button>
  </form>

  <table>
    <tr id="searchSelector" v-for="restaurant in restaurants">
      <td>{{ restaurant.name }}</td>
    </tr>
  </table>

</template>

<style scoped>
form{
  width: 250px;
}
form>input{
  margin-bottom: 10px;
}
form>select{
  margin-bottom: 10px;
}
</style>