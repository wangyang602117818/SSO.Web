import Tabs from "../components/tabs"
import UserManage from '../components/user_manage'
var routes = [
    {
        path: "/",
        component: Tabs
    },
    {
        path: '/users',
        component: UserManage
    }
]
export default routes