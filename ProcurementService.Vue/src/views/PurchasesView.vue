<template>
    <v-container class="page">
        <v-card height="100%" class="card">
            <v-card-title>
                <h3>Cписок заявок</h3>
            </v-card-title>
            <v-card-item>
                <v-text-field
                    label="Поиск"
                    v-model="search"
                ></v-text-field>
            </v-card-item>
            <v-card-text>
                <v-table>
                    <thead>
                        <tr>
                            <th class="text-center">Имя</th>
                            <th class="text-center">Дата создания</th>
                            <th class="text-center">Дата обновления</th>
                            <th class="text-center">Сумма</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody class="text-center">
                        <tr
                            v-for="item in purchases"
                            :key="item.id"
                        >
                            <td>{{ item.name }}</td>
                            <td>{{ getDateFriendly(item.createAt) }}</td>
                            <td>{{ getDateFriendly(item.updateAt) }}</td>
                            <td>{{ moneyFrendly(item.summaryMain) }} руб. | {{ moneyFrendly(item.summarySub) }} руб.</td>
                            <td class="d-flex align-center button">
                                <v-btn
                                    icon="mdi-file-document-edit"
                                    size="small"
                                    flat
                                    @click="this.$router.push(`/purchase/` + item.id);"
                                ></v-btn>
                                <v-btn
                                    v-if="item.isConfirmed"
                                    icon="mdi-download"
                                    size="small"
                                    flat
                                    @click="requestGetFile(item.id)"
                                ></v-btn>
                                <v-icon 
                                    v-if="item.isConfirmed"
                                    icon="mdi-check-bold"
                                    color="green"
                                ></v-icon>    
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
    </v-container>
</template>

<script>

import {RequestGetList, RequestGetFile} from "@/hooks/endpoint/request";

export default {
    name: "PurchasesView",
    data: ()=> ({
        search: '',
        paginationLength: 1,

        purchases: [
        ]
    }),
    watch: {
        search(str) {
            this.search = str;
            this.getPurchases();
        },
        paginationLength(count) {
            this.paginationLength = count;
            this.getPurchases();
        }
    },
    mounted() {
        this.getPurchases()
    },
    methods: {
        async getPurchases() {
            let {data, answer} = await RequestGetList(this.search, this.paginationLength);
            if (answer.value) {
                this.purchases = data.value.list;
                this.paginationLength = data.value.count;
            }
            else {
                this.isAlert = true;
            }
        },
        async requestGetFile(id) {
            await RequestGetFile(id);
        },
        getDateFriendly(date) {
            return date.split('T')[0] + ' ' + date.split('T')[1].split('.')[0];
        },
        moneyFrendly(number) {
            return new Intl.NumberFormat().format(number);
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
}
</style>
