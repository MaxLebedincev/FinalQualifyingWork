<template>
    <v-container class="page">
        <v-card height="100%" class="card">
            <v-card-title>
                <h3>Список пользователей</h3>
            </v-card-title>
            <v-card-item>
                <v-row>
                    <v-col class="d-flex">
                        <v-text-field
                            label="Поиск"
                            v-model="search"
                        ></v-text-field>
                        <v-btn 
                            class="button-add" 
                            variant="outlined"
                            prepend-icon="mdi-account-plus"
                            @click="() => {dialog = true; currentId = undefined; getEditorUser(); }"    
                        >
                            Добавить пользователя
                        </v-btn>
                    </v-col>
                </v-row>
            </v-card-item>
            <v-card-text>
                <v-table>
                    <thead>
                        <tr>
                            <th class="text-center">Имя</th>
                            <th class="text-center">Почта</th>
                            <th class="text-center">Роли</th>
                            <th class="text-center">Дата создания</th>
                            <th class="text-center">Дата обновления</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody class="text-center">
                        <tr
                            v-for="item in users"
                            :key="item.id"
                        >
                            <td>{{ item.login }}</td>
                            <td>{{ item.email }}</td>
                            <td>{{ getRolesFriendly(item.roles) }}</td>
                            <td>{{ getDateFriendly(item.created) }}</td>
                            <td>{{ getDateFriendly(item.updated) }}</td>
                            <td class="d-flex align-center button">
                                <v-btn
                                    v-if="!item.isConfirmed"
                                    icon="mdi-account-edit"
                                    size="small"
                                    flat
                                    @click="() => {dialog = true; currentId = item.id;  getEditorUser(item.id)}"
                                ></v-btn>
                                <v-btn
                                    v-if="!item.isConfirmed"
                                    icon="mdi-close-circle"
                                    size="small"
                                    flat
                                    @click="() => {deleteUser(item.id)}"
                                ></v-btn>
                            </td>
                        </tr>
                    </tbody>
                </v-table>
            </v-card-text>
            <v-card-actions class="d-flex justify-space-evenly">
                <v-row>
                    <v-col  justify="center">
                        <v-pagination :length="paginationLength"></v-pagination>
                    </v-col>
                </v-row>
            </v-card-actions>
        </v-card>
        <v-dialog
            v-model="dialog"
            width="auto"
        >
            <v-card class="card-dialog">
                <template #title>
                    {{ currentId ? 'Изменение' : 'Создание' }}
                </template>
                <template #text>
                    <v-container class="container h-100">
                        <v-row>
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
                                <v-select
                                    label="Роли"
                                    v-model="role"
                                    :items="roleItems"
                                    multiple
                                    clearable
                                ></v-select>
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
                                    :style="currentId ? `margin-right: 50px;` : ``"
                                    width="200"
                                    color="green"
                                    :disabled=isLoaded
                                    :loading=isLoaded
                                    @click="currentId ? editUser() : createUser()"
                                >{{isAlert ? "Хорошо" : currentId ? 'Изменить' : 'Создать'}}</v-btn>
                                <v-btn
                                    v-if="currentId"
                                    style="margin-left: 50px;"
                                    width="200"
                                    color="#dd0000"
                                    :disabled=isLoaded
                                    :loading=isLoaded
                                    @click="deleteUser(currentId)"
                                >{{isAlert ? "Хорошо" : 'Удалить'}}</v-btn>
                            </v-col>
                        </v-row>
                    </v-container>
                </template>
            </v-card>
        </v-dialog>
    </v-container>
</template>

<script>

import {UserGet, UserCreate, UserEdit, UserDelete} from "@/hooks/endpoint/user";

export default {
    name: "UsersView",
    data: ()=> ({
        search: '',
        users: [],
        paginationLength: 1,

        dialog: false,
        isLoaded: false,
        currentId: undefined,
        isAlert: false,

        login: '',
        password: '',
        email: '',
        roleItems: 
        [
            'Администратор',
            'Утверждающий',
            'Подающий'
        ],
        role: [],
    }),
    mounted() {
        this.getUsers();
    },
    watch: {
        search(str) {
            this.search = str;
            this.getUsers();
        },
        paginationLength(count) {
            this.paginationLength = count;
            this.getUsers();
        },
        dialog(flag) {
            this.dialog = flag;
            this.isAlert = false;
        }
    },
    methods: {
        getRolesFriendly(array) {
            let str = '';
            if (array.includes('admin')) {
                str += 'Администратор '
            }
            if (array.includes('signatory')) {
                str += 'Утверждающий '
            }
            if (array.includes('applicant')) {
                str += 'Подающий '
            }
            return str; 
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
        updateRoleFriendlyToSystem(array) {
            let arr = [];
            if (array.includes('Администратор')) {
                arr.push('admin');
            }
            if (array.includes('Утверждающий')) {
                arr.push('signatory');
            }
            if (array.includes('Подающий')) {
                arr.push('applicant');
            }
            return arr;
        },
        getDateFriendly(date) {
            return date.split('T')[0] + ' ' + date.split('T')[1].split('.')[0];
        },
        getEditorUser(id) {
            if (id) {
                let currentUser = this.users.find(u => u.id === id);
                this.login = currentUser.login;
                this.email = currentUser.email;
                this.password = currentUser.password;
                this.role = this.updateRoleSystemToFriendly(currentUser.roles);
            }
            else {
                this.login = '';
                this.email = '';
                this.password = '';
                this.role = [];
            }
        },
        async getUsers() {
            let {data, answer} = await UserGet(this.search, this.paginationLength);
            if (answer.value) {
                this.users = data.value.list;
                this.paginationLength = data.value.count;
            }
            else {
                this.isAlert = true;
            }
        },
        async createUser() {
            let result = await UserCreate(this.login, this.password, this.email, this.updateRoleFriendlyToSystem(this.role));
            if (result) {
                this.getUsers();
                this.dialog = false;
                this.login = '';
                this.email = '';
                this.password = '';
                this.role = [];
            }
        },
        async editUser() {
            let result = await UserEdit(this.currentId, this.login, this.email, this.password, this.updateRoleFriendlyToSystem(this.role));
            if (result) {
                this.getUsers();
                this.dialog = false;
                this.login = '';
                this.email = '';
                this.password = '';
                this.role = [];
            }
        },
        async deleteUser(id) {
            let result = await UserDelete(id);
            if (result) {
                await this.getUsers();
                this.dialog = false;
            }
        }
    }
}
</script>

<style lang="scss" scoped>
.card{
    display: flex;
    flex-direction: column;
}
.button {
    direction: rtl;
    &-add {
        margin: 10px;
    }
}
.card-dialog {
    height: 700px;
    width: 1000px;
}
.container {
    display: flex; 
    flex-direction: column; 
}
</style>
<style>
.mdi-close-circle::before{
    color: red;
}
.mdi-account-edit::before {
    color: lightgreen;
}
</style>
