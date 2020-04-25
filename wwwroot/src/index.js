import Vue from 'vue'
import VueResource from 'vue-resource'
import funtools from 'fun-tools'

import Index from '@/components/index'

Vue.prototype.$funtools = funtools

Vue.use(VueResource)

Vue.http.options.root = 'http://www.sso.com:8030/'
// Vue.http.options.root = ''

var urls = {
    geturlmeta: 'navigation/geturlmeta',
    login: 'sso/login',
    logout: 'sso/logout',
    decodetoken: 'sso/decodetoken'
};
Vue.prototype.$urls = urls

new Vue({
    render: h => h(Index),
}).$mount('#app')