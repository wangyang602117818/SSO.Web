import Vue from 'vue'
import VueResource from 'vue-resource'
import funtools from 'sso-util'

import Login from '@/components/login'

Vue.prototype.$funtools = funtools

Vue.use(VueResource)

// Vue.http.options.root = 'http://www.sso.com:8030/'
Vue.http.options.root = ''
var urls = {
    login: 'sso/login'
};
Vue.prototype.$urls = urls
Vue.config.productionTip = false

new Vue({
    render: h => h(Login),
  }).$mount('#app')