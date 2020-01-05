import Vue from 'vue'
import VueResource from 'vue-resource'
import babelPolyfill from 'babel-polyfill'

import common from './js/common.js'

import Login from '@/components/login'

import { Button, Icon, Input, Form, Row, Col, message, Card } from 'ant-design-vue'
import 'ant-design-vue/dist/antd.css'

Vue.use(Button)
Vue.use(Icon)
Vue.use(Input)
Vue.use(Form)
Vue.use(Row)
Vue.use(Col)
Vue.use(Card)

Vue.prototype.$message = message
Vue.prototype.$common = common

Vue.use(VueResource)
Vue.use(babelPolyfill)

Vue.http.options.root = ''
var urls = {
    login: 'sso/login'
};
Vue.prototype.$urls = urls
Vue.config.productionTip = false

new Vue({
    render: h => h(Login),
  }).$mount('#app')