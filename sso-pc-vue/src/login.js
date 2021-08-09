import Vue from 'vue'
import { axios } from './config/http';
import funtools from 'sso-util'

import Login from '@/components/login'

Vue.prototype.$funtools = funtools
Vue.prototype.$axios = axios
axios.defaults.baseURL = "";

Vue.prototype.$baseUrl = axios.defaults.baseURL
var urls = {
    login: '/sso/login'
};
Vue.prototype.$urls = urls
Vue.config.productionTip = false


var vue = new Vue({
    render: h => h(Login),
}).$mount('#app')
window.vue = vue;
