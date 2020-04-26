import Vue from 'vue'
import VueResource from 'vue-resource'
import funtools from 'fun-tools'

import Login from '@/components/login'

import { Button, Icon, Input, Form, Row, Col, message, Card } from 'ant-design-vue'

Vue.use(Button)
Vue.use(Icon)
Vue.use(Input)
Vue.use(Form)
Vue.use(Row)
Vue.use(Col)
Vue.use(Card)

Vue.prototype.$message = message
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