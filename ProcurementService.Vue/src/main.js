import { createApp, h } from 'vue'
import App from './App.vue'
import router from "@/router/router";
import "./axios";

import './plugins/vuetify'
import { loadFonts } from './plugins/webfontloader'

import 'material-icons/iconfont/material-icons.css';

import { VueCookieNext } from 'vue-cookie-next'

import { createStore } from 'vuex'
import {createVuetify} from "vuetify";

export const store = createStore({
    state () {
        return {
            inverse: false,
            userInfo: {}
        }
    }
})

loadFonts()

const vuetify = createVuetify({
    theme: {
        defaultTheme: 'light'
    }
})

const app = createApp({
    render: ()=>h(App)
});

app.config.devtools = true

app
    .use(store)
    .use(router)
    .use(vuetify)
    .use(VueCookieNext)
    .mount('#app')