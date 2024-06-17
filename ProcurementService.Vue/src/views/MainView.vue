<template>
    <v-container class="page">
        <v-row v-if="person">
            <v-col>
                <v-card>
                    <template #text>
                        <h3>Имя пользователя: {{person.login}}</h3>
                        <h3>Почта пользователя: {{person.email}}</h3>
                        <h3>Дата регистрации: {{ getDateFriendly(person.created) }}</h3>
                    </template>
                </v-card>
            </v-col>
            <v-col>
                <v-card>
                    <template #title>Роли пользователя</template>
                    <template #text>
                        <v-container v-if="person.roles">
                            <v-row>
                                <v-col v-for="item in person.roles" :key="item">{{ item }}</v-col>
                            </v-row>
                        </v-container>
                    </template>
                </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>

<script>

import {UserGetPerson} from "@/hooks/endpoint/user";

export default {
    name: "MainView",
    data: ()=> ({ 
        person: undefined,
        images:[
          {source: "https://ulstu.ru/upload/iblock/dac/820lq5b6g8lu5hkz0sctg1gann7w73ap/IMG_0286-_1_.jpg", alt:"asd"},
          {source: "https://ic.pics.livejournal.com/zdorovs/16627846/1075242/1075242_original.jpg", alt:"sds"},
          {source: "https://ic.pics.livejournal.com/zdorovs/16627846/1075242/1075242_original.jpg", alt:"sds"},
          {source: "https://ic.pics.livejournal.com/zdorovs/16627846/1075242/1075242_original.jpg", alt:"sds"},
          {source: "https://ic.pics.livejournal.com/zdorovs/16627846/1075242/1075242_original.jpg", alt:"sds"},
          {source: "https://historydepositarium.ru/upload/iblock/c6b/zhweue3bw9qjsqles5ehveh3t3dbknix.png", alt:"asdf"}
      ]
    }),
    mounted() {

        let login = localStorage.getItem("login")

        if (login)
        {
            this.userGetPerson(login);
        }
    },
    methods: {
        async userGetPerson(login) {
            let result = await UserGetPerson(login);
            
            if (result) {
                this.person = result.data.value;
                this.person.roles = this.updateRoleSystemToFriendly(this.person.roles)

            }
        },
        updateRoleSystemToFriendly(array) {
            let arr = [];
            if (array.includes('admin')) {
                arr.push('Администратор');
            }
            if (array.includes('signatory')) {
                arr.push('Утверждающий');
            }
            if (array.includes('applicant')) {
                arr.push('Подающий');
            }
            return arr;
        },
        getDateFriendly(date) {
            return date.split('T')[0] + ' ' + date.split('T')[1].split('.')[0];
        }
    }
}
</script>

<style lang="scss" scoped>
</style>
