import ax from 'axios';
import funtools from 'sso-util';
// export const baseURL = "http://www.ssoapi.com/";
export const baseURL = "http://api.ssoutil.cn/";
export const cookieName = "sso.pc.auth";
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
    taskscheduling: {
        gettriggerlist: 'taskscheduling/gettriggerlist',
        getSchedulingList: 'taskscheduling/getSchedulingList',
        gettriggertypes: 'taskscheduling/gettriggertypes',
        getExamples: 'taskscheduling/getExamples',
        addTrigger: "taskscheduling/addTrigger",
        addScheduling: "taskscheduling/addScheduling",
        updateTrigger: "taskscheduling/updateTrigger",
        deleteTrigger: "taskscheduling/deleteTrigger",
        deleteScheduling: "taskscheduling/deleteScheduling",
        getTriggerById: "taskscheduling/getTriggerById",
        getSchedulingById: "taskscheduling/getSchedulingById",
        updateScheduling: "taskscheduling/updateScheduling",
        startScheduling: "taskscheduling/startScheduling",
        stopScheduling: "taskscheduling/stopScheduling",
        enableScheduling: "taskscheduling/enableScheduling",
        getSchedulingNames: "taskscheduling/getSchedulingNames",
        getSchedulingHistory: "taskscheduling/getSchedulingHistory"
    },
    log: {
        getlist: 'log/getlist',
        getfromlist: 'log/getfromlist',
        gettolist: '/log/gettolist',
        getControllersByTo: 'log/getcontrollersbyto',
        getActionsByController: 'log/getactionsbycontroller'
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
    permission: {
        getlist: 'permission/getpermission',
        addRolePermission: 'permission/addRolePermission',
        getRolePermission: "permission/getRolePermission",
        addUserPermission: "permission/addUserPermission",
        getUserPermission: "permission/getUserPermission"
    },
    decodeToken: 'sso/decodetoken',
    login: 'sso/login',
    logout: 'sso/logout',
};
ax.defaults.baseURL = baseURL;
// 配置
ax.defaults.timeout = 20000;
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
        if (response.data.code == -1000) {
            if (window.vue.$toast)
                window.vue.$toast("error", window.vue.$t("response." + response.data.message));
            return;
        }
        if (response.data.code > 10) {
            if (window.vue.$toast) {
                window.vue.$toast("info", window.vue.$t("response." + response.data.message));
            }
        }
        return response.data;
    },
    error => {
        if (error.message.includes('timeout')) {
            if (window.vue.$toast) {
                window.vue.$toast("info", window.vue.$t("response.request_timeout"));
                return;
            }
        }
        return Promise.reject(error.response) // 返回接口返回的错误信息
    }
);
export const axios = ax;