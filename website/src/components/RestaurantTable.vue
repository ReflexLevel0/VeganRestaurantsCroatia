<script>
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import 'primevue/resources/themes/lara-light-teal/theme.css'
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
      fetch(`http://localhost:3000/?${this.$data.searchOption}=${this.$data.searchText}`)
          .then(data => data.json())
          .then(json => {
            this.$data.restaurants = json
            for(const restaurant of this.$data.restaurants){
              let linkString = ''
              if(restaurant.websitelinks !== null) {
                for(const link of restaurant.websitelinks){
                  if(linkString !== '') linkString += ', '
                  linkString += `${link.link}`
                }
              }
              restaurant.websitelinks = linkString
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

  <div v-if="filtered">
    <a href="http://localhost:3000/filtered/json">Filtered JSON file</a>
    <br/>
    <a href="http://localhost:3000/filtered/csv">Filtered CSV file</a>
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
    <Column field="address" header="Address" :sortable="true"></Column>
    <Column field="city" header="City" :sortable="true"></Column>
    <Column field="zipcode" header="Zipcode" :sortable="true"></Column>
    <Column field="latitude" header="Latitude" :sortable="true"></Column>
    <Column field="longitude" header="Longitude" :sortable="true"></Column>
    <Column field="phone" header="Phone" :sortable="true"></Column>
    <Column field="opening_hours" header="Opening hours" :sortable="true"></Column>
    <Column field="delivery" header="Delivery" :sortable="true"></Column>
    <Column field="websitelinks" header="Website links" :sortable="true"></Column>
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