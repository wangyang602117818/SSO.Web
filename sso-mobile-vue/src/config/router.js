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
import NavigationManage from '../components/navigation/navigation_manage'
import NavigationUpdate from '../components/navigation/navigation_update'
import NavigationAdd from '../components/navigation/navigation_add'
import LogManage from '../components/log/log_manage'
import LogDetail from '../components/log/log_detail'
import Companys from '../components/department/companys'
import DepartmentManage from '../components/department/department_manage'
import DepartmentAdd from '../components/department/department_add'
import DepartmentUpdate from '../components/department/department_update'
import Personal from '../components/me/personal'
import ChangePassword from '../components/me/change_password'
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
    {
        name: "navigation",
        path: "/navigation",
        component: NavigationManage,
        options: {
            transition: 'f7-parallax',
        }
    },
    {
        name: "navigationupdate",
        path: "/navigationupdate/:id",
        component: NavigationUpdate,
        options: {
            transition: 'f7-parallax',
        }
    },
    {
        name: "navigationadd",
        path: "/navigationadd",
        component: NavigationAdd,
        options: {
            transition: 'f7-parallax',
        }
    },
    {
        name: "logs",
        path: "/logs",
        component: LogManage,
        options: {
            transition: 'f7-parallax',
        }
    },
    {
        name: "logdetail",
        path: "/logdetail/:id",
        component: LogDetail,
        options: {
            transition: 'f7-parallax',
        }
    },
    {
        name:"coms",
        path: "/coms",
        component: Companys,
        options: {
            transition: 'f7-parallax',
        }
    },
    {
        name:"departmentmanage",
        path: "/departmentmanage/:id/:name",
        component: DepartmentManage,
        options: {
            transition: 'f7-parallax',
        }
    },
    {
        name:"departmentadd",
        path:"/departmentadd/:companyCode/:parentCode",
        component: DepartmentAdd,
        options: {
            transition: 'f7-parallax',
        }
    },
    {
        name:"departmentupdate",
        path:"/departmentupdate/:companyCode/:code",
        component: DepartmentUpdate,
        options: {
            transition: 'f7-parallax',
        }
    },
    {
        name:"personal",
        path:"/personal",
        component: Personal,
        options: {
            transition: 'f7-parallax',
        }
    },
    {
        name:"changepassword",
        path:"/changepassword",
        component: ChangePassword,
        options: {
            transition: 'f7-parallax',
        }
    }
]
export default routes