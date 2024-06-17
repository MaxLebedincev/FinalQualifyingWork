<template>
    <v-container>
        <v-row>
            <v-col>
                <v-text-field
                    label="Производитель"
                    v-model="localModelValue.manufacturer"
                ></v-text-field>
            </v-col>
            <v-col>
                <v-select
                    label="Видеопамять (Гб)"
                    clearable
                    v-model="localModelValue.vram"
                    :items="[1,2,4,6,8,16,32,64,128]"
                ></v-select>
            </v-col>
            <v-col>
                <v-select
                    label="Оперативная память (Гб)"
                    clearable
                    v-model="localModelValue.ram"
                    :items="[1,2,4,6,8,16,32,64,128]"
                ></v-select>
            </v-col>
        </v-row>
        <v-row>
            <v-col>
                <v-select
                    label="Тип диска"
                    clearable
                    v-model="localModelValue.typeDisk"
                    :items="['HDD', 'SSD', 'M.2 SSD']"
                ></v-select>
            </v-col>
            <v-col>
                <v-select
                    label="Размер диска (Гб)"
                    clearable
                    :disabled="!localModelValue.typeDisk"
                    v-model="localModelValue.sizeDisk"
                    :items="[64,128,256,512,1024]"
                ></v-select>
            </v-col>
            <v-col>
                <v-select
                    clearable
                    label="Количество ядер процессора"
                    v-model="localModelValue.countCors"
                    :items="[1,2,4,6,8,16,32,64,128]"
                ></v-select>
            </v-col>
        </v-row>
    </v-container>
</template>

<script>

export default {
    name: "UlProductFilter",
    props: {
        modelValue: {}
    },
    data: () => ({

    }),
    mounted() {

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
    }
}
</script>

<style lang="scss" scoped>
</style>