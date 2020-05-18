import Vue from 'vue'
import App from './App.vue'
import Framework7 from 'framework7/framework7-lite.esm.bundle'
import Framework7Vue from 'framework7-vue'
import axios from 'axios';
import funtools from 'fun-tools';

import 'framework7/css/framework7.bundle.css'
import 'framework7-icons';
import { f7App, f7Tabs, f7Tab, f7Toolbar, f7View, f7Page, f7Link, f7Navbar, f7Icon, f7Row, f7Col } from "framework7-vue"
import { baseURL, cookieName, urls } from './config/http';

Vue.component('f7-app', f7App);
Vue.component('f7-view', f7View);
Vue.component('f7-page', f7Page);
Vue.component('f7-link', f7Link);
Vue.component('f7-navbar', f7Navbar);
Vue.component('f7-tabs', f7Tabs);
Vue.component('f7-toolbar', f7Toolbar);
Vue.component('f7-tab', f7Tab);
Vue.component('f7-icon', f7Icon);
Vue.component('f7-row', f7Row);
Vue.component('f7-col', f7Col);

Framework7.use(Framework7Vue);

Vue.prototype.$funtools = funtools;

funtools.authorize(baseURL, cookieName)

Vue.prototype.$urls = urls
Vue.prototype.$axios = axios

Vue.config.productionTip = false
new Vue({
  render: h => h(App),
}).$mount('#app')
