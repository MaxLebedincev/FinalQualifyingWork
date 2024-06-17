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
                <v-text-field
                    label="Email"
                    v-model="email"
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
                    text="Ошибка при регестрации!"
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
                    @click="isAlert === false ? register() : isAlert = !isAlert"
                >{{isAlert ? "Хорошо" : "Зарегистрироваться"}}</v-btn>
            </v-col>
        </v-row>
    </v-container>
</template>

<script>
import {UseAuthorization} from "@/hooks/access/useAuthorization";
import {UseRegistration} from "@/hooks/access/useRegistration";
import setAuthHeader from "@/utils/setAuthHeader";

export default {
    name: "UlVerification",
    props: {
    },
    data: ()=> ({
        dialog: false,
        login: '',
        password: '',
        email: '',
        isAlert: false,
        alert: '',
        isLoaded: false
    }),
    emits: ['updateRole'],
    mounted() {
    },
    methods: {
        async auth() {
            this.isLoaded = true;
            let {userinfo, answer} = await UseAuthorization(this.login, this.password);
            this.isLoaded = false;
            if (answer.value) {
                setAuthHeader(this.$cookie.getCookie('jwt'));
                this.$emit('updateRole', this.isAlert);
            } else {
                this.isAlert = true;
                this.alert = userinfo.value;
                this.$emit('updateRole', !this.isAlert);
            }
        },
        async register() {
            this.isLoaded = true;
            let {userinfo, answer} = await UseRegistration(this.login, this.password, this.email);
            this.isLoaded = false;
            if (answer.value) {
                await this.auth();
            } else {
                this.alert = userinfo.value;
                this.isAlert = true;
                this.$emit('updateRole', !this.isAlert);
            }
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