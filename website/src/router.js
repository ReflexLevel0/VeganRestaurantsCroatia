import {createRouter, createWebHistory} from "vue-router";
import DataDownload from "@/components/DataDownload.vue";
import RestaurantTable from "@/components/RestaurantTable.vue";

const routes = [
    {
        path: '/',
        name: 'Home',
        component: DataDownload
    },
    {
        path: '/datatable',
        name: 'Data table',
        component: RestaurantTable
    }
]

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes
})

export default router