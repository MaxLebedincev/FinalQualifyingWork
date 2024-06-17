<template>
    <v-container class="container h-100">
        <v-row align="start" v-if="!isAlert">
            <v-col>
                <v-text-field
                    label="Логин"
                    v-model="login"
                    hide-details="auto"
                    :loading=isLoaded
                ></v-text-field>
                <v-text-field
                    label="Пароль"
                    v-model="password"
                    type="password"
                    hide-details="auto"
                    :loading=isLoaded
                ></v-text-field>
            </v-col>
        </v-row>
        <v-row v-if="isAlert">
            <v-col>
                <v-alert
                    type="warning"
                    title="Ошибка"
                    text="Ошибка при авторизации!"
                ></v-alert>
            </v-col>
        </v-row>
        <v-row align="end" justify="center">
            <v-col align="center">
                <v-btn
                    width="200"
                    color="#297fee"
                    :disabled="isLoaded"
                    :loading=isLoaded
                    @click="isAlert === false ? auth() : isAlert = !isAlert"
                >{{isAlert ? "Хорошо" : "Авторизироваться"}}</v-btn>
            </v-col>
        </v-row>
    </v-container>
</template>

<script>
import {UseAuthorization} from "@/hooks/access/useAuthorization";
import setAuthHeader from "@/utils/setAuthHeader";

export default {
    name: "UlAuthorization",
    props: {
    },
    data: ()=> ({
        login: '',
        password: '',
        placeholderLogin: null,
        isAlert: false,
        isLoaded: false
    }),
    emits: ['updateRole'],
    mounted() {
        const name = localStorage.getItem("login");
        this.placeholderLogin = name === 'undefined' ? null : name
    },
    methods: {
        async auth() {
            this.isLoaded = true;
            let {userinfo, answer} = await UseAuthorization(this.login, this.password);
            this.isLoaded = false;
            if (answer.value) {
                setAuthHeader(this.$cookie.getCookie('jwt'))
                this.clearPersonInfo();
                this.placeholderLogin = userinfo.value.login;
                this.$emit('updateRole', this.isAlert);
            } else {
                this.placeholderLogin = null;
                this.isAlert = true;
                this.$emit('updateRole', !this.isAlert);
            }
        },
        clearPersonInfo() {
            this.login = '';
            this.password = '';
        }
    }
}
</script>

<style scoped>
.container {
    display: flex; 
    flex-direction: column; 
}
</style>