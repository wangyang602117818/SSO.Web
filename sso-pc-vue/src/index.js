import Vue from 'vue'
import { axios } from './config/http';
import funtools from 'sso-util'

import Index from '@/components/index'

Vue.prototype.$funtools = funtools
Vue.prototype.$axios = axios
axios.defaults.baseURL = "";

Vue.prototype.$baseUrl = axios.defaults.baseURL
var urls = {
    geturlmeta: '/navigation/geturlmeta',
    login: '/sso/login',
    logout: '/sso/logout',
    decodetoken: '/sso/decodetoken',
    setLang: '/settings/setlanglocal'
};
Vue.prototype.$urls = urls

var vue = new Vue({
    render: h => h(Index),
});
window.vue = vue;
vue.$mount('#app')