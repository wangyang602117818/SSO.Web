import Vue from 'vue'
import Vuex from 'vuex'
Vue.use(Vuex)
const store = new Vuex.Store({
    state: {
        currentUser: {}
    },
    mutations: {
        getUser(state) {
            var vue = this._vm;
            vue.$axios.get(vue.$urls.user.getuser).then(response => {
                if(response.code===0){
                    state.currentUser = response.result;
                }
            });
        }
    }
})

export default store