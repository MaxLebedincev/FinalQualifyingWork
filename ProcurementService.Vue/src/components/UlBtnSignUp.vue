<template>
    <v-btn class="custom-button" color="#297fee" @click="placeholderLogin === null ? update(true) : logout()">
        <span v-if="placeholderLogin === null">Войти</span>
        <span v-else>{{placeholderLogin}} | выход</span>
    </v-btn>
    <v-dialog
        v-model="localModelValue"
        width="auto"
    >
        <v-card class="card-dialog">
            <template #title>
                <v-tabs v-model="dialogTabs" fixed-tabs color="#297fee">
                    <v-tab value="auth">Авторизация</v-tab>
                    <v-tab value="reg">Регистрация</v-tab>
                </v-tabs>
            </template>
            <template #text>
                <v-window style="height: 100%" v-model="dialogTabs">
                    <v-window-item style="height: 100%" value="auth">
                        <ul-authorization @update-role="checkRole" />
                    </v-window-item>
                    <v-window-item style="height: 100%" value="reg">
                        <ul-verification @update-role="checkRole" />
                    </v-window-item>
                </v-window>
            </template>
        </v-card>
    </v-dialog>
</template>

<script>
import {UseLogout} from "@/hooks/access/useLogout";
import UlAuthorization from "@/components/UlAuthorization";
import UlVerification from "@/components/UlVerification";

export default {
    name: "UlBtnSignUp",
    props: {
        modelValue: Boolean
    },
    components: 
    {
        UlAuthorization,
        UlVerification
    },
    emits: ['update:modelValue', 'logout'],
    data: () => ({
        dialogTabs: 'auth',
        placeholderLogin: null,
    }),
    mounted() {
        const name = localStorage.getItem("login");
        this.placeholderLogin = name === 'undefined' ? null : name
    },
    computed: {
        localModelValue: {
            get() {
                return this.modelValue
            },
            set(newValue) {
                this.$emit('update:modelValue', newValue)
            },
        },
    },
    methods: {
        async logout() {
            await UseLogout();
            this.placeholderLogin = null;
            this.$emit('logout');
        },
        checkRole(success) {
            const name = localStorage.getItem("login");
            this.placeholderLogin = name === 'undefined' ? null : name
            this.$emit('update:modelValue', success);
        },
        update(dialog) {
            this.$emit('update:modelValue', dialog);
        }
    }
}
</script>

<style lang="scss" scoped>
.card-dialog {
    height: 500px;
    width: 500px;
}
.v-btn .custom-button {
    color: #297fee !important;
}
</style>