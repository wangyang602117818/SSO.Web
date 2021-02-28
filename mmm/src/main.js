import Vue from 'vue'
import App from './App.vue'
import Framework7 from 'framework7';
import Framework7Vue from 'framework7-vue';

Framework7.use(Framework7Vue)

Vue.config.productionTip = false

new Vue({
  render: h => h(App),
}).$mount('#app')
