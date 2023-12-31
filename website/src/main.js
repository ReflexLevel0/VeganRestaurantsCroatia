import { createApp } from 'vue'
import App from './App.vue'
import router from "@/router";
import { createAuth0 } from '@auth0/auth0-vue';

createApp(App).use(router).use(
    createAuth0({
        domain: "dev-bbrulmdzs2x816wl.eu.auth0.com",
        clientId: "tRChSVRtEN8tONiuCo0L233Y5mdCrCaY",
        authorizationParams: {
            redirect_uri: window.location.origin
        }
    })
).mount('#app')
