import axios from "axios";
import {ref} from 'vue';

export async function UseAuthorization(login, password) {
    const userinfo = ref({})
    const answer = ref(false)
    const fetching = async () => {
        try {
            const response = await axios.post(
            '/Account/Token',
            {
                    login: login,
                    password: password
            });
            userinfo.value = response.data;
            answer.value = userinfo.value.errorText  ? false: true;
        } catch (e) {
            answer.value = false;
        }
    }
    await fetching();
    localStorage.setItem("role", userinfo.value.role)
    localStorage.setItem("login", userinfo.value.login)
    localStorage.setItem("email", userinfo.value.email)
    return {
        userinfo, answer
    }
}