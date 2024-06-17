import axios from "axios";
import {ref} from 'vue';

const endpoint = '/Request';

export async function RequestGet(search = '', pagination = 1, priceMin = undefined, priceMax = undefined, filter = {}) {
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
            });
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

export async function RequestCreate(name, summaryMain, summarySub, product) {
    const request = ref({})
    const answer = ref(false)
    const fetching = async () => {
        try {
            const response = await axios.post(`${endpoint}/Create`, {
                name: name,
                summaryMain: summaryMain,
                summarySub: summarySub,
                product: product
            }, {Authorization: `Bearer ${document.cookie.split('=')[1]}`});
            request.value = response.data;
            answer.value = true;
        } catch (e) {
            answer.value = false;
        }
    }
    await fetching();
    return {
        request, answer
    }
}

export async function RequestProductGet(id) {
    const data = ref({})
    const answer = ref(false)
    const fetching = async () => {
        try {
            const response = await axios.post(`${endpoint}/Get/${id}`, {Authorization: `Bearer ${document.cookie.split('=')[1]}`});
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

export async function RequestGetFile(id) {
    const answer = ref(false)
    const fetching = async () => {
        try {
            const response = await axios(
                {
                    url: `${endpoint}/GetFile/${id}`, 
                    method: 'GET',
                    Authorization: `Bearer ${document.cookie.split('=')[1]}`, 
                    responseType: 'blob'
                });

            answer.value = true;
            const href = URL.createObjectURL(response.data);
            const link = document.createElement('a');
            link.href = href;
            link.setAttribute('download', 'Архив.zip');
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
            URL.revokeObjectURL(href);
        } catch (e) {
            answer.value = false;
        }
    }
    await fetching();
    return {
        answer
    }
}

export async function RequestProductUpdate(id, name, summaryMain, summarySub, product) {
    const request = ref({})
    const answer = ref(false)
    const fetching = async () => {
        try {
            const response = await axios.post(`${endpoint}/Update/${id}`, {
                name: name,
                summaryMain: summaryMain,
                summarySub: summarySub,
                product: product
            }, {Authorization: `Bearer ${document.cookie.split('=')[1]}`});
            request.value = response.data;
            answer.value = true;
        } catch (e) {
            answer.value = false;
        }
    }
    await fetching();
    return {
        request, answer
    }
}

export async function RequestSign(id) {
    const message = ref([])
    const answer = ref(false)
    const fetching = async () => {
        try {
            const response = await axios.post(`${endpoint}/Sign/${id}`,
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

export async function RequestDelete(id) {
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

export async function RequestEdit(id, name, description, price, count, filter) {
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

export async function RequestGetList(search = '', pagination = 1) {
    const data = ref([])
    const answer = ref(false)
    const fetching = async () => {
        try {
            const response = await axios.post(`${endpoint}/Get`, {
                Term: search,
                Offset: pagination
            });
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