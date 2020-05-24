import Tabs from "../components/tabs"
import UserManage from '../components/user_manage'
import UserUpdate from '../components/user_update'
var routes = [
    {
        name:"tab",
        path: "/",
        component: Tabs,
        options:{
            transition: 'f7-parallax',
        }
    },
    {
        name:"users",
        path: "/users",
        component: UserManage,
        options:{
            transition: 'f7-parallax',
        }
    },
    {
        name:"userupdate",
        path: "/userupdate/:userId",
        component: UserUpdate,
        options:{
            transition: 'f7-parallax',
        }
    },
]
export default routes