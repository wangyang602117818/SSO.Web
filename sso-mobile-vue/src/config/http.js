import ax from 'axios';
import funtools from 'sso-util';

export const baseURL = "http://www.sso.com:8030/";
export const cookieName = "sso.mobile.auth";
export const urls = {
    role: {
        add: "role/add",
        update: "role/update",
        delete: "role/delete",
        getlist: "role/getlist",
        getall: "role/getall",
        getById: "role/getbyid"
    },
    company: {
        add: "company/add",
        update: "company/update",
        delete: "company/delete",
        getlist: "company/getlist",
        getall: "company/getall",
        getById: "company/getbyid"
    },
    department: {
        add: "department/add",
        getdepartments: "department/getdepartments",
        get: "department/get",
        update: "department/update",
        delete: "department/delete"
    },
    user: {
        add: "user/add",
        getbasic: "user/getbasic",
        remove: "user/remove",
        delete: "user/delete",
        update: "user/update",
        restore: "user/restore",
        getbyuserid: "user/getbyuserid",
        getuser: 'user/getuser',
        updatebasicsetting: 'user/updatebasicsetting',
        updatepassword: 'user/updatepassword',
        resetpassword: 'user/resetpassword'
    },
    log: {
        getlist: 'log/getlist',
        getlistsimple:'log/getlistsimple',
        detail:'log/detail',
        getfromlist:'log/getfromlist',
        getControllersByFrom:'log/getcontrollersbyfrom',
        getActionsByController:'log/getactionsbycontroller',
        getOperations:'log/getoperations'
    },
    overview: {
        total: 'overview/total',
        opRecord: 'overview/oprecord',
        userRecord: 'overview/userrecord',
        userRatio: 'overview/userratio',
        userCompanyRatio: 'overview/userCompanyRatio',
        userDepartmentRatio: 'overview/userDepartmentRatio'
    },
    navigation: {
        getlist: 'navigation/getlist',
        add: 'navigation/add',
        delete: 'navigation/delete',
        getbyid: 'navigation/getbyid',
        update: 'navigation/update',
        geturlmeta: 'navigation/geturlmeta'
    },
    settings: {
        setLang: 'settings/setlang'
    },
    decodeToken: 'sso/decodetoken',
    login: 'sso/login',
    logout: 'sso/logout',
};

ax.defaults.baseURL = baseURL;
// 配置
ax.defaults.timeout = 7000;
ax.defaults.headers.post['Content-Type'] = 'application/json';
ax.defaults.headers.get['Content-Type'] = 'application/json';
ax.defaults.headers.delete['Content-Type'] = 'application/json';
ax.defaults.headers.put['Content-Type'] = 'application/json';

//请求拦截器
ax.interceptors.request.use((config) => {
    config.headers.Authorization = funtools.getCookie(cookieName);
    return config;
}, (error) => {
    return Promise.reject(error);
});
//响应拦截器
ax.interceptors.response.use(
    response => {
        if (response.data.code === 400) {
            window.vue.showInfo("记录已存在!");
            return false;
        }
        if (response.data.code === 401) {
            window.vue.showInfo("登录已过期!");
            return false
        }
        return response.data;
    },
    error => {
        if(error.message.includes('timeout')){
            window.vue.showInfo("请求超时!");
            return;
        }
        return Promise.reject(error.response) // 返回接口返回的错误信息
    }
);
export const axios = ax;
