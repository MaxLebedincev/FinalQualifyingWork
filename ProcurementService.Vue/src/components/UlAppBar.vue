<template>
    <v-app-bar :elevation="2" class="px-3 custom-header">
        <template #title>
            <span >
                Закупка компьютерной техники и других материальных средств 
            </span>
        </template>
        <template #default>
            <v-tabs
                centered
                v-model="activeRoute"
            >
                <div v-for="route in activeRoutes" :key="route.id">
                    <v-tab
                        @click="$router.push(route.link)"
                    >
                        {{route.name}}
                    </v-tab>
                </div>
            </v-tabs>
        </template>
        <template #append>
            <v-switch
                v-model="isNotification"
                true-icon="mdi-bell-ring"
                false-icon="mdi-bell-off"
                width="50px"
                hide-details
                inset
            ></v-switch>
            <v-btn
                :icon="this.$store.state.inverse ? 'mdi-weather-sunny-off' : 'mdi-weather-sunny'"
                hide-details
                inset
                @click="toggleTheme"
            ></v-btn>
            <ul-btn-sign-up v-model="dialog" @logout="() => { logout(); }" />
        </template>
        <template #image>
        </template>
    </v-app-bar>
</template>

<script>
import {useTheme} from "vuetify";
import UlBtnSignUp from "@/components/UlBtnSignUp";

export default {
    name: "UlAppBar",
    components: 
    {
        UlBtnSignUp
    },
    setup () {
        const theme = useTheme()
        theme.global.name.value = localStorage.getItem("inverse") ?? 'light'
        return {
            theme,
            toggleTheme: () => {
                theme.global.name.value = theme.global.current.value.dark ? 'light' : 'dark'
                localStorage.setItem("inverse", theme.global.name.value)
            }
        }
    },
    data: () => ({
        dialog: false,
        isNotification: false,
        rolesUser: localStorage.getItem('role')?.split(',') ?? [],
        routes: 
        [
            {
                id: 0,
                link: '/',
                name: 'Главная',
                roles: []
            },
            {
                id: 1,
                link: '/purchases',
                name: 'Заявки',
                roles: ['signatory', 'applicant']
            },
            {
                id: 2,
                link: '/users',
                name: 'Пользователи',
                roles: ['admin']
            },
            {
                id: 3,
                link: '/purchase',
                name: 'Создание заявки',
                roles: ['signatory', 'applicant']
            }
        ],
        activeRoutes: [],
        activeRoute: Number(localStorage.getItem('activeRoute')) ?? 0,
    }),
    mounted() {
        this.activeRoutes = this.recalculationRoutes();
    },
    watch: {
        activeRoute(active) {
            localStorage.setItem('activeRoute', active)
            this.activeRoute = active;
        },
        dialog(value) {
            this.activeRoutes = this.recalculationRoutes();
            this.dialog = value;
        }
    },
    methods: {
        recalculationRoutes() {
            this.rolesUser = localStorage.getItem('role')?.split(',') ?? [];
            return this.routes.filter(function(elem) {
                if (elem.roles.length === 0 || this.rolesUser.some(ru => elem.roles.includes(ru)))
                    return elem;
            }.bind(this))
        },
        logout() {
            this.activeRoutes = this.recalculationRoutes(); 
            this.activeRoute = 0;
            this.$router.push(`/`)
        }
    }
}
</script>


<style lang="scss" scoped>
.v-toolbar__append {
    div {
        margin: 0px 10px;
    }
}
.custom-header {
    color: #297fee !important;
}
</style>