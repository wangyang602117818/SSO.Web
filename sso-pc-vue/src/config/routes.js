import VueRouter from 'vue-router'
import Home from '@/components/home'
import OverView from '@/components/overview'
import UserBasic from '@/components/user_basic'
import Role from '@/components/role'
import Company from '@/components/company'
import Department from '@/components/department'
import Log from '@/components/log'
import Settings from '@/components/settings'
import TaskScheduling from '@/components/task_scheduling'
import TaskTrigger from '@/components/task_trigger'

const routes = new VueRouter({
    // mode: 'history',
    routes: [
        {
            path: '/',
            component: Home,
            children: [
                {
                    path: '',
                    component: OverView
                },
                {
                    path: 'index.html',
                    component: OverView
                },
                {
                    path: 'overview',
                    name: "overview",
                    component: OverView
                },
                {
                    path: 'userbasic',
                    name: "userbasic",
                    component: UserBasic
                },
                {
                    path: 'role',
                    name: "role",
                    component: Role
                },
                {
                    path: 'company',
                    name: "company",
                    component: Company
                },
                {
                    path: 'department',
                    name: "department",
                    component: Department
                },
                {
                    path: 'taskscheduling',
                    name: "taskscheduling",
                    component: TaskScheduling
                },
                {
                    path: 'tasktrigger',
                    name: "tasktrigger",
                    component: TaskTrigger
                },
                {
                    path: 'log',
                    name: "log",
                    component: Log
                },
                {
                    path: 'settings',
                    name: "settings",
                    component: Settings
                }
            ]
        }
    ]
})
export default routes