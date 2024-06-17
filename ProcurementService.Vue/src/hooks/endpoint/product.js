import axios from "axios";
import {ref} from 'vue';

const endpoint = '/Product';

export async function ProductGet(search = '', pagination = 1, priceMin = undefined, priceMax = undefined, filter = {}) {
    const data = ref([])
    const answer = ref(false)
    const fetching = async () => {
        try {
            const response = await axios.post(`${endpoint}/Get`, {
                Term: search,
                Offset: pagination,
                PriceMin: priceMin,
                PriceMax: priceMax,
                Filter: filter
            }, {Authorization: `Bearer ${document.cookie.split('=')[1]}`});
            data.value = response.data;
            answer.value = true;
        } catch (e) {
            answer.value = false;
        }
    }
    await fetching();
    return {
        data, answer
    }
}

export async function ProductCreate(name, description, price, type, filter) {
    const product = ref({})
    const answer = ref(false)
    const fetching = async () => {
        try {
            const response = await axios.post(`${endpoint}/Create`, {
                name: name,
                description: description,
                price: price,
                type: type,
                filter: filter
            }, {Authorization: `Bearer ${document.cookie.split('=')[1]}`});
            product.value = response.data;
            answer.value = true;
        } catch (e) {
            answer.value = false;
        }
    }
    await fetching();
    return {
        product, answer
    }
}

export async function ProductDelete(id) {
    const message = ref([])
    const answer = ref(false)
    const fetching = async () => {
        try {
            const response = await axios.delete(`${endpoint}/Delete/${id}`,
                {Authorization: `Bearer ${document.cookie.split('=')[1]}`});
            message.value = response.data.error ?? response.data.success;
            answer.value = true;
        } catch (e) {
            answer.value = false;
        }
    }
    await fetching();
    return {
        message, answer
    }
}

export async function ProductEdit(id, name, description, price, count, filter) {
    const message = ref([])
    const answer = ref(false)
    const fetching = async () => {
        try {
            const response = await axios.put(`${endpoint}/Update/${id}`,
                {
                    name: name,
                    description: description,
                    price: price,
                    count: count,
                    filter: filter
                },
                {Authorization: `Bearer ${document.cookie.split('=')[1]}`});
            message.value = response.data.error ?? response.data.success;
            answer.value = true;
        } catch (e) {
            answer.value = false;
        }
    }
    await fetching();
    return {
        message, answer
    }
}