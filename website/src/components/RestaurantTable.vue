<script>
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import 'primevue/resources/themes/lara-light-teal/theme.css'
import {useAccountStore} from "@/stores/accountStore";
export default {
  setup(){
    const accountStore = useAccountStore()
    return {
      accountStore
    }
  },
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
        {value: "telephone", text: "Phone"},
        {value: "openingHours", text: "Opening hours"},
        {value: "delivery", text: "Delivery"},
        {value: "city", text: "City"}
      ],
      searchText: "",
      restaurants: [],
      headers: [
        {text: "Name", value: "name"}
      ],
      filtered: false
    }
  },
  async mounted() {
    await this.refreshData()
  },
  methods: {
    async refreshData(e) {
      e?.preventDefault()
      fetch(`http://localhost:3000/Restaurant?${this.$data.searchOption}=${this.$data.searchText}`)
          .then(data => data.json())
          .then(json => {
            this.$data.restaurants = JSON.parse(json.response)["@graph"]
            for(const restaurant of this.$data.restaurants){

              //Combining all links into a single string
              let linkString = ''
              if(restaurant.websiteLinks !== null) {
                for(const link of restaurant.websiteLinks){
                  if(linkString !== '') linkString += ', '
                  linkString += `${link.link}`
                }
              }
              restaurant.websiteLinks = linkString

              //Parsing restaurant city
              restaurant.city = restaurant.address.addressLocality.split(',')[0];
            }

            this.$data.filtered = this.$data.searchOption!=='all' || this.$data.searchText!==''
          })
    }
  },
  components: {
    DataTable,
    Column
  }
}
</script>

<template>

  <div v-if="filtered && accountStore.isAuthenticated">
    <a href="http://localhost:3000/file/filteredJson" download>Filtered JSON file</a>
    <br/>
    <a href="http://localhost:3000/file/filteredCsv" download>Filtered CSV file</a>
  </div>

  <form>
    <input class="input-group" type="text" placeholder="Zagreb" v-model="$data.searchText"/>
    <select class="form-select" v-model="$data.searchOption">
      <option v-for="category in searchCategories" :value=category.value>{{ category.text }}</option>
    </select>
    <button class="btn btn-primary" type="submit" @click="refreshData">Search</button>
  </form>

  <DataTable :value="this.$data.restaurants">
    <Column field="name" header="Name" :sortable="true"></Column>
    <Column field="address.streetAddress" header="Address" :sortable="true"></Column>
    <Column field="city" header="City" :sortable="true"></Column>
    <Column field="address.postalCode" header="Zipcode" :sortable="true"></Column>
    <Column field="latitude" header="Latitude" :sortable="true"></Column>
    <Column field="longitude" header="Longitude" :sortable="true"></Column>
    <Column field="telephone" header="Phone" :sortable="true"></Column>
    <Column field="openingHours" header="Opening hours" :sortable="true"></Column>
    <Column field="delivery" header="Delivery" :sortable="true"></Column>
    <Column field="websiteLinks" header="Website links" :sortable="true"></Column>
  </DataTable>

</template>

<style scoped>
form {
  width: 250px;
}

form > input {
  margin-bottom: 10px;
}

form > select {
  margin-bottom: 10px;
}
</style>