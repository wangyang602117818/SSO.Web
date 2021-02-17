import Vue from 'vue'
import VueI18n from "vue-i18n"
import VueRouter from 'vue-router'
import funtools from 'sso-util'
import { baseURL, cookieName, urls, axios } from './config/http';
import routes from "./config/routes";
import "./css/index.css"

import { Button, Icon, Layout, Menu, Table, Input, Select, TreeSelect, InputNumber, Drawer, Form, Row, Col, message, notification, Popconfirm, Tabs, Tree, Divider, Tag, Switch, Tooltip, Card, Dropdown, Spin, DatePicker, Modal, Checkbox,Radio } from 'ant-design-vue'
Vue.use(VueRouter)
Vue.use(VueI18n);

Vue.use(Button)
Vue.use(Icon)
Vue.use(Layout)
Vue.use(Menu)
Vue.use(Table)
Vue.use(Input)
Vue.use(InputNumber)
Vue.use(Select)
Vue.use(Drawer)
Vue.use(Form)
Vue.use(Row)
Vue.use(Col)
Vue.use(Tree)
Vue.use(TreeSelect)
Vue.use(Tabs)
Vue.use(Popconfirm)
Vue.use(Divider)
Vue.use(Tag)
Vue.use(Switch)
Vue.use(Tooltip)
Vue.use(Dropdown)
Vue.use(Card)
Vue.use(Spin)
Vue.use(DatePicker)
Vue.use(Modal)
Vue.use(Checkbox)
Vue.use(Radio)

Vue.prototype.$message = message
Vue.prototype.$notification = notification
Vue.prototype.$funtools = funtools
Vue.prototype.$baseUrl = baseURL
Vue.prototype.$cookieName = cookieName
Vue.prototype.$urls = urls
Vue.prototype.$axios = axios

funtools.authorize(baseURL, cookieName);
const i18n = new VueI18n({
  locale: window.token_jwt_data.Lang,
  messages: {
    'zh-cn': require('./locales/zh-cn.json'),
    'en-us': require('./locales/en-us.json')
  }
});

Vue.config.productionTip = false
var vue = new Vue({
  i18n,
  el: "#app",
  router: routes
})
window.vue = vue;
