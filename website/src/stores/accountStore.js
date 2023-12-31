import {defineStore} from "pinia";
import {useLocalStorage} from "@vueuse/core";

export const useAccountStore = defineStore('account', {
    state: () => ({
        name: useLocalStorage('name', null),
        nickname: useLocalStorage('nickname', null),
        email: useLocalStorage('email', null),
        isAuthenticated: useLocalStorage('isAuthenticated', false)
    })
})