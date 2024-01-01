import {defineStore} from "pinia";

export async function refreshAccountData() {
    //This will loop for 2 seconds to try and fetch new data from local storage
    //It's absolutely HORRIBLE code, but I genuinely can't be bothered to fix it
    for(let i = 0; i < 20; i++){
        const accountStore = useAccountStore()
        let user = null
        let isAuthenticated = false
        try {
            for (let i = 0; i < localStorage.length; i++) {
                if (localStorage.key(i).endsWith(":@@user@@")) {
                    user = JSON.parse(localStorage.getItem(localStorage.key(i))).decodedToken.user
                    isAuthenticated = true
                    break
                }
            }
        } catch (e) {
            console.log(e)
        }

        accountStore.user = user
        accountStore.isAuthenticated = isAuthenticated
        if(isAuthenticated) break

        await sleep(100)
    }
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

export const useAccountStore = defineStore('account', {
    state: () => ({
        user: null,
        isAuthenticated: false
    })
})