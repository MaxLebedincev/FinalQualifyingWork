<template>
    <v-container class="page">
        <v-card class="card">
            <v-card-title>
                <h3>Формирование заявки</h3>
                <v-slider
                    readonly
                    :max="4"
                    v-model="proggressBar"
                    :ticks="tickLabels"
                    show-ticks="always"
                    step="1"
                    tick-size="5"
                ></v-slider>
                <div style="direction: rtl;">
                    <v-btn
                        v-if="proggressBar === 2"
                        :disabled="proggressBar !== 2 || bucketProducts.length === 0"
                        @click="bucket = true"
                        append-icon="mdi-bucket"
                        :color="(summarySubTriger || summaryMainTriger) ? 'red' : 'green'"
                    >
                        Корзина
                    </v-btn>
                </div>
            </v-card-title>
            <v-card-text>
                <v-container v-if="proggressBar === 1">
                    <v-row>
                        <v-col>
                            <v-select
                                label="Выбор загрузки суммы"
                                v-model="typeSummary"
                                :items="optionsSummary"
                            ></v-select>
                        </v-col>    
                        <v-col>
                            <v-text-field
                                v-if="typeSummary === 'Ручной'"
                                label="Сумма для основных товаров"
                                v-model="manualSummaryMain"
                                type="number"
                                hide-details="auto"
                            ></v-text-field>
                            <v-text-field
                                v-if="typeSummary === 'Ручной'"
                                label="Сумма для канцтоваров"
                                v-model="manualSummarySub"
                                type="number"
                                hide-details="auto"
                            ></v-text-field>
                            <v-file-input 
                                v-if="typeSummary === 'Загрузка из файла'"
                                label="Сумма"
                                v-model="fileSummary"
                            ></v-file-input>
                        </v-col>
                    </v-row>
                </v-container>
                <v-container v-if="proggressBar === 2">
                    <v-row>
                        <v-col cols="2">
                            <v-container>
                                <v-row>
                                    <v-col>
                                        <v-select
                                            label="Выбор товара"
                                            v-model="typeProduct"
                                            :items="optionsTypeProduct"
                                        ></v-select>
                                    </v-col>
                                </v-row>
                            </v-container>
                        </v-col>
                        <v-col>
                            <v-container v-if="typeProduct === 'Новый'">
                                <v-row>
                                    <v-col>
                                        <h2 style="text-align: center;">Добавление нового товара</h2>
                                    </v-col>
                                </v-row>
                                <v-row>
                                    <v-col>
                                        <v-text-field
                                            label="Наименование"
                                            v-model="newProduct.name"
                                            hide-details="auto"
                                        ></v-text-field>
                                        <v-textarea 
                                            label="Описание"
                                            v-model="newProduct.description"
                                            hide-details="auto"
                                        ></v-textarea>
                                        <div class="d-flex">
                                            <v-text-field
                                                label="Цена за штуку"
                                                v-model="newProduct.price"
                                                type="number"
                                                hide-details="auto"
                                            ></v-text-field> 
                                            <v-select
                                                label="Тип товара"
                                                v-model="newProduct.type"
                                                width="300px"
                                                hide-details="auto"
                                                :items="['Цифровой товар', 'Канцелярия']"
                                            ></v-select>
                                        </div>
                                        <v-expansion-panels flat focusable :disabled="newProduct.type === 'Канцелярия'">
                                            <v-expansion-panel>
                                                <v-expansion-panel-title>Дополнительные параметры</v-expansion-panel-title>
                                                <v-expansion-panel-text>
                                                    <ul-product-filter
                                                        v-model="newProduct.filter"
                                                    ></ul-product-filter>
                                                    <v-slider
                                                        label="Диагональ (дюйм)"
                                                        v-model="newProduct.filter.diagonal"
                                                        thumb-label="always"
                                                        min="10"
                                                        max="50"
                                                    ></v-slider>
                                                </v-expansion-panel-text>
                                            </v-expansion-panel>
                                        </v-expansion-panels> 
                                    </v-col>
                                </v-row>
                                <v-row>
                                    <v-col>
                                    </v-col>
                                    <v-col cols="3">
                                        <v-btn
                                            width="100%"
                                            color="green"
                                            @click="createProduct"
                                        >
                                            Добавить
                                        </v-btn>
                                    </v-col>
                                </v-row>
                            </v-container>
                            <v-container v-if="typeProduct === 'Из истории закупок'">
                                <v-row>
                                    <v-col class="d-flex">
                                        <v-range-slider
                                            style="margin: 10px 40px;"
                                            label="Цена"
                                            v-model="price"
                                            min="0"
                                            step="1"
                                            max="500000"
                                            thumb-label
                                        ></v-range-slider>
                                        <v-text-field
                                            label="Поиск"
                                            v-model="search"
                                        ></v-text-field>
                                        <v-btn 
                                            style="margin: 10px 40px;"
                                            prepend-icon="mdi-filter"
                                            variant="outlined"
                                            @click="filterSearchDialog = true"    
                                        >
                                            Дополнительные фильтры
                                        </v-btn>
                                    </v-col>
                                </v-row>
                                <v-row>
                                    <v-col style="height: 320px;">
                                        <v-table>
                                            <thead>
                                                <tr>
                                                    <th width="20%" class="text-center">Наименование</th>
                                                    <th width="50%" class="text-center">Описание</th>
                                                    <th width="10%" class="text-center">Тип товара</th>
                                                    <th width="10%" class="text-center">Цена</th>
                                                    <th></th>
                                                </tr>
                                            </thead>
                                            <tbody class="text-center">
                                                <tr
                                                    v-for="item in products"
                                                    :key="item.id"
                                                >
                                                    <td width="20%">{{  item.name.length > 30 ? item.name.substring(0, 30) + '...' : item.name }}</td>
                                                    <td width="60%">{{ item?.description.length > 100 ? item.description.substring(0, 100) + '...' : item.description }}</td>
                                                    <td width="10%">{{ item.type === 2 ? 'Канцелярия' : 'Цифровой товар' }}</td>
                                                    <td width="10%">{{ item.price }}</td>
                                                    <td class="d-flex align-center text-left button flex-row-reverse">
                                                        <v-btn
                                                            icon="mdi-plus"
                                                            size="small"
                                                            flat
                                                            @click="addProductInBucket(item)"
                                                        ></v-btn>
                                                        <v-btn
                                                            v-if="item.filter"
                                                            icon="mdi-devices"
                                                            size="small"
                                                            flat
                                                            @click="() => {}"
                                                        ></v-btn>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </v-table>
                                    </v-col>
                                </v-row>
                                <v-row>
                                    <v-col  justify="center">
                                        <v-pagination v-model="pageProduct" :length="paginationLength"></v-pagination>
                                    </v-col>
                                </v-row>
                            </v-container>
                        </v-col>
                    </v-row>
                </v-container>
                <v-container v-if="proggressBar >= 3">
                    <v-row>
                        <v-col>
                            <v-text-field
                                v-if="proggressBar === 3"
                                label="Наименование заявки"
                                v-model="requestName"
                            ></v-text-field>
                            <div v-else>
                                <h3>{{ requestName }}</h3>
                            </div>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <h2>Список товаров</h2>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <h3>Начальная сумма основных товаров: {{ moneyFrendly(summaryMain) }} руб.</h3>
                        </v-col>
                        <v-col>
                            <h3>Потраченная сумма основных товаров: {{ moneyFrendly(summaryMainSpend) }} руб.</h3>
                        </v-col>
                        <v-col>
                            <h3>Остаток для суммы основных товаров: {{ moneyFrendly(summaryMain - summaryMainSpend) }} руб.</h3>
                        </v-col>
                    </v-row>
                    <v-row>
                        <v-col>
                            <h3>Начальная сумма канцелярских товаров: {{ moneyFrendly(summaryMain) }} руб.</h3>
                        </v-col>
                        <v-col>
                            <h3>Потраченная сумма канцелярских товаров: {{ moneyFrendly(summaryMainSpend) }} руб.</h3>
                        </v-col>
                        <v-col>
                            <h3>Остаток для суммы канцелярских товаров: {{ moneyFrendly(summaryMain - summaryMainSpend) }} руб.</h3>
                        </v-col>
                    </v-row>
                    <v-card style="margin: 25px 0;" v-for="myItem in bucketProducts" :key="myItem.id">
                        <template #title>
                                <h2>{{ myItem.name }} | {{ myItem.type === 2 ? 'Канцелярия' : 'Основной товар' }}</h2>
                        </template>
                        <template #subtitle>
                            <div>Цена за штуку: {{ moneyFrendly(myItem.price) }} руб.</div>
                            <div>Количество: {{ myItem.count }} шт.</div>
                            <div>Общая сумма: {{ moneyFrendly(myItem.price * myItem.count) }} руб.</div>
                        </template>
                        <template #text>
                            <div style="margin: 20px;">{{ myItem.description }}</div>
                            <v-expansion-panels flat focusable v-if="myItem.filter">
                                <v-expansion-panel>
                                    <v-expansion-panel-title>Дополнительные параметры</v-expansion-panel-title>
                                    <v-expansion-panel-text>
                                        <v-container>
                                            <v-row v-if="myItem.filter.manufacturer">
                                                Производитель: {{ myItem.filter.manufacturer }}
                                            </v-row>
                                            <v-row v-if="myItem.filter.ram">
                                                Оперативная память: {{ myItem.filter.ram }} Гб
                                            </v-row>
                                            <v-row v-if="myItem.filter.vram">
                                                Видеопамять: {{ myItem.filter.vram }} Гб
                                            </v-row>
                                            <v-row v-if="myItem.filter.typeDisk">
                                                Тип диска: {{ myItem.filter.typeDisk }} {{ myItem.filter.sizeDisk ? `| Размер:` + myItem.filter.sizeDisk + `Гб` : '' }}
                                            </v-row>
                                            <v-row v-if="myItem.filter.countCors">
                                                Количество ядер: {{ myItem.filter.countCors }}
                                            </v-row>
                                            <v-row v-if="myItem.filter.diagonal">
                                                Диагональ: {{ myItem.filter.diagonal / 10 }}"
                                            </v-row>
                                        </v-container>
                                    </v-expansion-panel-text>
                                </v-expansion-panel>
                            </v-expansion-panels> 
                        </template>
                    </v-card>
                </v-container>
            </v-card-text>
            <v-card-actions>
                <v-row>
                    <v-col>
                        <v-btn
                            width="100%"
                            :disabled="(summarySubTriger || summaryMainTriger || isConfirmed)"
                            @click="() => {proggressBar -= 1;}"
                        >
                            Назад
                        </v-btn>
                    </v-col>
                    <v-col>
                        <v-btn
                            width="100%"
                            v-if="proggressBar < 3"
                            :disabled="!(manualSummaryMain || manualSummarySub || fileSummary || requestId) || (summarySubTriger || summaryMainTriger)"
                            @click="nextStep"
                        >
                            Далее
                        </v-btn>
                        <v-btn
                            width="100%"
                            v-if="proggressBar >= 3 && !isConfirmed"
                            :disabled="!requestName"
                            @click="nextStep"
                        >
                            {{ requestId ? 'Обновить заявку' : 'Создать заявку'}}
                        </v-btn>
                    </v-col>
                    <v-col v-if="proggressBar === 3">
                        <v-btn
                            width="100%"
                            :disabled="(roleUser !== 'signatory' && !isConfirmed && !requestId) || !requestName"
                            :color="roleUser === 'signatory' ? 'green' : 'red'"
                            @click="requestSign"
                        >
                            Утвердить
                        </v-btn>
                    </v-col>
                    <v-col v-if="proggressBar === 4">
                        <v-btn
                            width="100%"
                            @click="requestGetFile"
                        >
                            Скачать архив
                        </v-btn>
                    </v-col>
                </v-row>
            </v-card-actions>
        </v-card>
        <v-dialog v-model="bucket"
            width="auto"
        >
            <v-card class="card-dialog">
                <template #title>
                    <v-row>
                        <v-col>
                            <h3>Список товаров</h3>
                        </v-col>
                        <v-col :class="summaryMainTriger ? 'danger' : ''">
                            Остаток для суммы товаров: {{ summaryMain - summaryMainSpend }}
                        </v-col>
                        <v-col :class="summarySubTriger ? 'danger' : ''">
                            Остаток для суммы канцелярских товаров: {{ summarySub - summarySubSpend }}
                        </v-col>
                    </v-row>
                </template>
                <template #text>
                    <v-card style="margin: 25px 0;" v-for="myItem in bucketProducts" :key="myItem.id">
                        <template #title>
                                <h2>{{ myItem.name }} | {{ myItem.type === 2 ? 'Канцелярия' : 'Основной товар' }}</h2>
                        </template>
                        <template #subtitle>
                            <div>Цена: {{ moneyFrendly(myItem.price) }} руб.</div>
                        </template>
                        <template #text>
                            <div style="margin: 20px;">{{ myItem.description }}</div>
                            <v-expansion-panels flat focusable v-if="myItem.filter">
                                <v-expansion-panel>
                                    <v-expansion-panel-title>Дополнительные параметры</v-expansion-panel-title>
                                    <v-expansion-panel-text>
                                        <v-container>
                                            <v-row v-if="myItem.filter.manufacturer">
                                                Производитель: {{ myItem.filter.manufacturer }}
                                            </v-row>
                                            <v-row v-if="myItem.filter.ram">
                                                Оперативная память: {{ myItem.filter.ram }} Гб
                                            </v-row>
                                            <v-row v-if="myItem.filter.vram">
                                                Видеопамять: {{ myItem.filter.vram }} Гб
                                            </v-row>
                                            <v-row v-if="myItem.filter.typeDisk">
                                                Тип диска: {{ myItem.filter.typeDisk }} {{ myItem.filter.sizeDisk ? `| Размер:` + myItem.filter.sizeDisk + `Гб` : '' }}
                                            </v-row>
                                            <v-row v-if="myItem.filter.countCors">
                                                Количество ядер: {{ myItem.filter.countCors }}
                                            </v-row>
                                            <v-row v-if="myItem.filter.diagonal">
                                                Диагональ: {{ myItem.filter.diagonal / 10 }}"
                                            </v-row>
                                        </v-container>
                                    </v-expansion-panel-text>
                                </v-expansion-panel>
                            </v-expansion-panels> 
                        </template>
                        <template #actions>
                            <v-row>
                                <v-col></v-col>
                                <v-col cols="3" class="d-flex" style="margin-right: 10px;">
                                    <v-text-field
                                        label="Количество"
                                        type="number"
                                        hide-details="auto"
                                        variant="outlined"
                                        v-model="myItem.count"
                                    ></v-text-field>
                                    <v-btn
                                        style="height: 100%; margin-left: 50px;"
                                        append-icon="mdi-delete"
                                        color="red"
                                        variant="outlined"
                                        @click="removeProductFromBucket(myItem)"
                                    >Удалить</v-btn>
                                </v-col>
                            </v-row>
                        </template>
                    </v-card>
                </template>
            </v-card>
        </v-dialog>
        <v-dialog v-model="filterSearchDialog"
            width="auto"
        >
            <v-card class="card-dialog">
                <template #title>
                    Дополнительные фильтры
                </template>
                <template #text>
                    <ul-product-filter v-model="filter" />
                    <v-container>
                        <v-row>
                            <v-col>
                                <v-range-slider
                                    label="Диагональ"
                                    v-model="filter.diagonal"
                                    min="10"
                                    max="50"
                                    thumb-label="always"
                                ></v-range-slider>
                            </v-col>
                        </v-row>
                        <v-row>
                            <v-col align="center">
                                <v-btn
                                    width="200"
                                    @click="clear"
                                >Очистить</v-btn>
                            </v-col>
                            <v-col align="center">
                                <v-btn
                                    width="200"
                                    @click="filterSearchDialog = false"
                                >Принять</v-btn>                
                            </v-col>
                        </v-row>
                    </v-container>
                </template>
            </v-card>
        </v-dialog>
    </v-container>
</template>

<script>

import UlProductFilter from "@/components/UlProductFilter";

import {ProductGet, ProductCreate} from "@/hooks/endpoint/product";
import {RequestCreate, RequestProductGet, RequestProductUpdate, RequestSign, RequestGetFile} from "@/hooks/endpoint/request";

export default {
    name: "PurchasesView",
    data: () => ({
        proggressBar: 1,
        tickLabels: {
            1: 'Указание суммы',
            2: 'Выбор товаров',
            3: 'Подача на утверждение',
            4: 'Просмотр заявки'
        },

        summaryMain: 0,
        summarySub: 0,

        summaryMainSpend: 0,
        summarySubSpend: 0,
        
        summaryMainTriger: false,
        summarySubTriger: false,

        bucket: false,

        //#region Step 1
        optionsSummary: ['Ручной', 'Загрузка из файла'],
        typeSummary: 'Ручной',

        manualSummaryMain: '',
        manualSummarySub:'',
        fileSummary: undefined,
        //#endregion

        //#region Step 2
        optionsTypeProduct: ['Новый', 'Из истории закупок'],
        typeProduct: 'Новый',
        bucketProducts: [],

        //#region Step 2 - Search
        products: [],
        paginationLength: 1,
        pageProduct: 1,
        filterSearchDialog: false,

        search: '',
        price: [0, 500000],
        filter: {
            manufacturer: '',
            vram: undefined,
            ram: undefined,
            sizeDisk: undefined,
            typeDisk: '',
            countCors: undefined,
            diagonal: [10, 50]
        },
        //#endregion

        //#region Step 2 - New Product
        newProductDialog: false,

        newProduct: {
            name: '',
            description: '',
            price: 0,
            type: 'Канцелярия',
            filter: {
                manufacturer: '',
                vram: undefined,
                ram: undefined,
                sizedisk: undefined,
                typeDisk: undefined,
                countCors: undefined,
                diagonal: undefined
            }
        },
        //#endregion

        //#endregion

        roleUser: '',
        requestName: '',
        requestId: 0,
        isConfirmed: false
    }),
    components: 
    {
        UlProductFilter
    },
    watch: {
        typeSummary(str) {
            if(str === 'Ручной') {
                this.fileSummary = undefined;
            }
            if(str === 'Загрузка из файла') {
                this.manualSummaryMain = '';
                this.manualSummarySub = '';
            }
            this.typeSummary = str;
        },
        typeProduct(str) {
            if(str === 'Из истории закупок') {
                this.getProducts();
            }
            this.typeProduct = str;
        },
        search(value){
            this.search = value;
            this.getProducts();
        },
        price(value){
            this.price = value;
            this.getProducts();
        },
        filter: {
            deep: true,
            handler() {
                this.getProducts();
            }
        },
        pageProduct(value) {
            this.pageProduct = value;
            this.getProducts();
        },
        roleUser(value) {
            this.roleUser = value;
        },
        bucketProducts: {
            deep: true,
            handler() {
                this.updateStatusBucket();
            }
        },
    },
    mounted() {
        let arr = window.location.href.split('/');
        let id = arr[arr.length - 1];

        this.stepTwo();

        if(id.match(/\d+/))
        {
            this.requestId = id;
            this.requestProductGet(this.requestId);
            this.proggressBar = 3;
        }
    },
    methods: {
        moneyFrendly(number) {
            return new Intl.NumberFormat().format(number);
        },
        updateStatusBucket() {

            this.summaryMainSpend = 0;
            this.summarySubSpend = 0;

            this.bucketProducts.forEach((el) => {
                if (el.type === 1) {
                    this.summaryMainSpend += el.count * el.price;
                }
                if (el.type === 2) {
                    this.summarySubSpend += el.count * el.price;
                }
            })

            this.summaryMainTriger = (this.summaryMain < this.summaryMainSpend) ? true : false;
            this.summarySubTriger = (this.summarySub < this.summarySubSpend) ? true : false;
        },
        nextStep() {
            if (this.proggressBar === 1) {
                this.stepOne();
            }
            else if (this.proggressBar === 2) {
                this.stepTwo();
                if (this.requestId !== 0) {
                    this.requestProductUpdate();
                }
            } 
            else if (this.proggressBar >= 3 && this.requestId === 0) {
                this.requestCreate();
            }
            else if (this.proggressBar === 3 && this.requestId !== 0) {
                this.requestCreate();
            }

            if (this.proggressBar !== 3)
            {
                this.proggressBar += 1;
            }
        },
        stepOne() {
            if(this.typeSummary === 'Загрузка из файла') {
                this.fileSummary = undefined;
            }
            if(this.typeSummary === 'Ручной') {
                this.summaryMain = Number(this.manualSummaryMain);
                this.summarySub = Number(this.manualSummarySub);
            }
        },
        stepTwo() {
            let role = localStorage.getItem('role')?.split(',') ?? []
            
            this.roleUser = role.filter(el => {
                return el === 'signatory'
            }).join('')
        },
        addProductInBucket(value) {
            let isExist = false;
            this.bucketProducts.forEach((obj) => {
                if(obj.id === value.id) {
                    obj.count++;
                    isExist = true;
                } 
            })

            if(!isExist)
            {
                value.count = 1;
                this.bucketProducts.push(value);
            }
        },
        removeProductFromBucket(value) {
            this.bucketProducts = this.bucketProducts.filter((obj) => {
                if(obj.id === value.id) {
                    return false;
                } 
                return true;
            })

            if(!this.bucketProducts.length)
            {
                this.bucket = false;
            }
        },
        async createProduct() {

            let typeP = this.newProduct.type;

            if (this.newProduct.type === 'Канцелярия') {
                this.newProduct.type = 2;
            } else {
                this.newProduct.type = 1;
            }

            let key = Object.keys(this.newProduct.filter).find(elem => {
                if (this.newProduct.filter[elem]) {
                    return true;
                }
            });
            
            if (!key) {
                this.newProduct.filter = undefined;
            } 
            else {
                if (this.newProduct.filter.diagonal) {
                    this.newProduct.filter.diagonal = Math.trunc(this.newProduct.filter.diagonal * 10);
                }
            }

            let result = await ProductCreate(this.newProduct.name, this.newProduct.description, this.newProduct.price, this.newProduct.type, this.newProduct.filter);
            if (result) {
                this.newProduct = {
                    name: '',
                    description: '',
                    price: 0,
                    type: typeP,
                    filter: {
                        manufacturer: '',
                        vram: undefined,
                        ram: undefined,
                        sizedisk: undefined,
                        typeDisk: undefined,
                        countCors: undefined,
                        diagonal: undefined
                    }
                }

                result.product.value.count = 1;

                this.bucketProducts.push(result.product.value);
            }
        },
        async getProducts() {
            let {data, answer} = await ProductGet(this.search, this.pageProduct, this.price[0], this.price[1], this.filter);
            if (answer.value) {
                this.products = data.value.list;
                this.paginationLength = data.value.count;
            }
            else {
                this.isAlert = true;
            }
        },
        clear() {
            this.filter = {
                manufacturer: '',
                vram: undefined,
                ram: undefined,
                sizeDisk: undefined,
                typeDisk: '',
                countCors: undefined,
                diagonal: [10, 50]
            }
        },
        async requestCreate() {
            let requestProduct = []
            
            this.bucketProducts.forEach(el => {
                requestProduct.push({
                    id: el.id,
                    count: el.count
                })
            })
            
            let result = await RequestCreate(this.requestName, this.summaryMain, this.summarySub, requestProduct);
            
            if (result) {
                this.requestId = result.request.value;
                this.$router.push(`/purchase/` + this.requestId);
            }
        },
        async requestProductGet(id) {
            let result = await RequestProductGet(id);
            
            if (result) {
                this.requestId = id;
                this.bucketProducts = result.data.value.list;
                this.isConfirmed = result.data.value.request.isConfirmed;
                this.requestName = result.data.value.request.name;
                this.summaryMain = result.data.value.request.summaryMain;
                this.summarySub = result.data.value.request.summarySub;

                if (this.isConfirmed) {
                    this.proggressBar = 4;
                }
            }
        },
        async requestProductUpdate() {
            let requestProduct = []
            
            this.bucketProducts.forEach(el => {
                requestProduct.push({
                    id: el.id,
                    count: el.count
                })
            })
            
            let result = await RequestProductUpdate(this.requestId, this.requestName, this.summaryMain, this.summarySub, requestProduct);
            
            if (result) {
                this.requestId = result.request.value;
                this.$router.push(`/purchase/` + this.requestId);
            }
        },
        async requestSign() {
            let result = await RequestSign(this.requestId);
            debugger
            if (result) {
                this.proggressBar = 4;
                this.isConfirmed = true;
            }
        },
        async requestGetFile() {
            await RequestGetFile(this.requestId);
        },
    }
}
</script>

<style lang="scss" scoped>
.danger {
    color: red;
}

.card{
    display: flex;
    flex-direction: column;

    &-dialog {
        width: 1300px;
    }
}

.v-card {
    &.v-theme--light.v-card--density-default.v-card--variant-elevated.card-dialog {
        background-color: #faf8ff;
    }
    &.v-theme--dark.v-card--density-default.v-card--variant-elevated.card-dialog {
        background-color: #3b3b3b;
    }
}
.v-expansion-panels {
    &.v-theme--light {
        button.v-expansion-panel-title.v-expansion-panel-title--active {
            background-color: #f6f6f6;
        }
    }
    &.v-theme--dark {
        button.v-expansion-panel-title.v-expansion-panel-title--active {
            background-color: #2a2a2a;
        }
    }
}
</style>
<style>
.v-slider-track__fill {
    background-color: #1dcd1d;
}
.v-theme--light {
    .v-slider-thumb__surface.elevation-2 {
        background-color: green;
    }
    .v-slider-track__tick {
        background-color: green;
    }
}
.v-theme--dark {
    .v-slider-thumb__surface.elevation-2 {
        background-color: greenyellow;
    }
    .v-slider-track__tick {
        background-color: greenyellow;
    }
}
</style>
