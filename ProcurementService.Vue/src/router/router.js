import MainView from "@/views/MainView.vue"
import PurchasesView from "@/views/PurchasesView.vue"
import PurchaseView from "@/views/PurchaseView.vue"
import UsersView from "@/views/UsersView.vue"
import {createRouter, createWebHistory} from "vue-router"

const routes = [
    {
        path: '/',
        component: MainView
    },
    {
        path: '/purchases',
        component: PurchasesView
    },
    {
        path: '/users',
        component: UsersView
    },
    {
        path: '/purchase',
        component: PurchaseView
    },
    {
        path: '/purchase/:id',
        component: PurchaseView
    }
]

const router = createRouter({
    routes,
    history: createWebHistory(process.env.BASE_URL)
})

export default router;
