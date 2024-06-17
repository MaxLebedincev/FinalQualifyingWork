<template>
    <v-app>
        <ul-app-bar></ul-app-bar>
        <v-main>
            <router-view></router-view>
        </v-main>
    </v-app>
</template>

<script>
import UlAppBar from "@/components/UlAppBar";
export default {
    name: 'App',

    components: {
        UlAppBar
    },

    data: () => ({
    }),
    mounted() {
    },
    methods: {
    }
}
</script>

<style lang="scss">
html {
    overflow-y: auto !important; 
}
.page{
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    height: 100%;
}
</style>
