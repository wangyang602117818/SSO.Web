import Vue from 'vue'
import { axios, baseURL } from './config/http';
import funtools from 'sso-util'

import Login from '@/login/login'

Vue.prototype.$funtools = funtools
Vue.prototype.$axios = axios
axios.defaults.baseURL = baseURL;

Vue.prototype.$baseUrl = baseURL;

var urls = {
    login: '/sso/login'
};
Vue.prototype.$urls = urls
Vue.config.productionTip = false


var vue = new Vue({
    render: h => h(Login),
}).$mount('#app')
window.vue = vue;
