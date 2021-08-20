import VueRouter from 'vue-router'
import Home from '@/components/home'
import overview from '@/components/overview'
var components = require.context('../components/', false, /^.*\.vue$/);
var childrens = [
    {
        path: '',
        component: overview
    },
    {
        path: 'index.html',
        component: overview
    }
];
components.keys().forEach(fileName => {
    let com = components(fileName);
    childrens.push({
        path: com.default.name,
        name: com.default.name,
        component: com.default
    })
})
const routes = new VueRouter({
    // mode: 'history',
    routes: [
        {
            path: '/',
            component: Home,
            children: childrens
        }
    ]
})
export default routes