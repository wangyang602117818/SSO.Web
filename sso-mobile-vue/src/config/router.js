import Tabs from "../components/tabs"
import UserManage from '../components/user/user_manage'
import UserUpdate from '../components/user/user_update'
import UserAdd from '../components/user/user_add'
import RoleManage from '../components/role/role_manage'
import RoleUpdate from '../components/role/role_update'
import RoleAdd from '../components/role/role_add'
import CompanyManage from '../components/company/company_manage'
import CompanyUpdate from '../components/company/company_update'
import CompanyAdd from '../components/company/company_add'
var routes = [
    {
        name: "tab",
        path: "/",
        component: Tabs,
        options: {
            transition: 'f7-parallax',
        }
    },
    {
        name: "users",
        path: "/users",
        component: UserManage,
        options: {
            transition: 'f7-parallax',
        }
    },
    {
        name: "userupdate",
        path: "/userupdate/:userId",
        component: UserUpdate,
        options: {
            transition: 'f7-parallax',
        }
    },
    {
        name: "useradd",
        path: "/useradd",
        component: UserAdd,
        options: {
            transition: 'f7-parallax',
        }
    },
    {
        name: "roles",
        path: "/roles",
        component: RoleManage,
        options: {
            transition: 'f7-parallax'
        },
        beforeEnter: function (routeTo, routeFrom, resolve, reject) {
            resolve();
        },
        on: {
            pageBeforeIn: function (event, page) {

            }
        }
    },
    {
        name: "roleadd",
        path: "/roleadd",
        component: RoleAdd,
        options: {
            transition: 'f7-parallax',
        }
    },
    {
        name: "roleupdate",
        path: "/roleupdate/:id",
        component: RoleUpdate,
        options: {
            transition: 'f7-parallax',
        }
    },
    {
        name: "company",
        path: "/company",
        component: CompanyManage,
        options: {
            transition: 'f7-parallax',
        }
    },
    {
        name: "companyupdate",
        path: "/companyupdate/:id",
        component: CompanyUpdate,
        options: {
            transition: 'f7-parallax',
        }
    },
    {
        name: "companyadd",
        path: "/companyadd",
        component: CompanyAdd,
        options: {
            transition: 'f7-parallax',
        }
    },
]
export default routes