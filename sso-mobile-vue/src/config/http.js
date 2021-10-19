import ax from 'axios';
import funtools from 'sso-util';
// export const fileBaseURL = "http://www.fileserverpc.com/";
// export const baseURL = "http://www.ssoapi.com:8030/";
export const fileBaseURL = "http://file.ssoutil.cn/";
export const baseURL = "http://api.ssoutil.cn/";
export const cookieName = "sso.mobile.auth";
export const urls = {
    decodeToken: 'sso/decodetoken',
    login: 'sso/login',
    logout: 'sso/logout',
    preview: fileBaseURL + "file",
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
        detail: 'log/detail',
        getfromlist: 'log/getfromlist',
        gettolist: 'log/gettolist',
        getControllersByTo: 'log/getcontrollersbyto',
        getActionsByController: 'log/getactionsbycontroller',
        getOperations: 'log/getoperations'
    },
    overview: {
        total: 'overview/total',
        opRecord: 'overview/oprecord',
        userRecord: 'overview/userrecord',
        userRatio: 'overview/userratio',
        userCompanyRatio: 'overview/userCompanyRatio',
        userDepartmentRatio: 'overview/userDepartmentRatio'
    },
    settings: {
        setLang: 'settings/setlang'
    },
    file: {
        getlist: "file/getfilelist",
        upload: "file/upload",
        uploads: "file/uploads",
        downloadPic: "file/downloadpic",
        downloadFile: "file/downloadFile",
        fileState: "file/fileState",
        getFromList: "file/getFromList",
        getFileTypeMapping: "file/getFileTypeMapping",
        remove: "file/remove"
    }

};

ax.defaults.baseURL = baseURL;
// 配置
ax.defaults.timeout = 30000;
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
        // if (response.data.code === 400) {
        //     window.vue.showInfo("记录已存在!");
        //     return false;
        // }
        // if (response.data.code === 401) {
        //     window.vue.showInfo("登录已过期!");
        //     return false
        // }
        if (response.data.code > 10) {
            window.vue.showInfo(window.vue.$t("response." + response.data.message));
        }
        return response.data;
    },
    error => {
        if (error.message.includes('timeout')) {
            window.vue.showInfo(window.vue.$t("response.request_timeout"));
            return;
        }
        return Promise.reject(error.response) // 返回接口返回的错误信息
    }
);
export const axios = ax;
