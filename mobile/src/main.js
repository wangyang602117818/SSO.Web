import Vue from 'vue'
import VueRouter from 'vue-router'
import VueResource from 'vue-resource'

import App from './App.vue'

Vue.config.productionTip = false

Vue.use(VueRouter)
Vue.use(VueResource)

Vue.http.options.root = 'http://www.sso.com:8030/'


new Vue({
  render: h => h(App),
  mounted () {
    
  }
}).$mount('#app')
