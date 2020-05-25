<template>
  <f7-list inline-labels v-if="user && companyData && roleData &&departmentData">
    <f7-list-input
      required
      validate
      label="编号*"
      type="text"
      placeholder="编号/登录名称"
      error-message="编号是必填项!"
      clear-button
      :value="user.UserId"
      @input="($event)=>{user.UserId=$event.target.value}"
    ></f7-list-input>
    <f7-list-input
      label="用户名*"
      type="text"
      placeholder="用户名称"
      error-message="用户名称是必填项!"
      required
      validate
      clear-button
      :value="user.UserName"
      @input="($event)=>{user.UserName=$event.target.value}"
    ></f7-list-input>
    <f7-list-item title="性别" smart-select :smart-select-params="{openIn: 'popover'}">
      <select name="sex" @change="($event)=>{user.Sex = $event.target.value}">
        <option value="M" :selected="user.Sex==='M'">男</option>
        <option value="F" :selected="user.Sex==='F'">女</option>
      </select>
    </f7-list-item>
    <f7-list-input
      label="手机号码"
      validate
      pattern="[0-9]*"
      type="text"
      placeholder="手机号码"
      clear-button
      :value="user.Mobile"
      @input="($event)=>{user.Mobile=$event.target.value}"
    ></f7-list-input>
    <f7-list-input
      label="邮箱"
      type="email"
      validate
      placeholder="邮箱"
      error-message="邮箱格式不正确!"
      clear-button
      :value="user.Email"
      @input="($event)=>{user.Email=$event.target.value}"
    ></f7-list-input>
    <f7-list-input
      label="身份证号"
      type="text"
      placeholder="身份证号"
      clear-button
      :value="user.IdCard"
      @input="($event)=>{user.IdCard=$event.target.value}"
    ></f7-list-input>
    <f7-list-item title="公司" smart-select :smart-select-params="{openIn: 'popover'}">
      <select name="company" @change="changeCompany($event)">
        <option
          v-for="item in companyData"
          :value="item.Code"
          :key="item._id"
          :selected="item.Code===user.CompanyCode"
        >{{item.Name}}</option>
      </select>
    </f7-list-item>
    <f7-list-item
      link="#"
      title="部门"
      smart-select
      :smart-select-params="{
          formatValueText:formatValueText,
          openIn: 'popover',
        }"
    >
      <select multiple name="department" @change="changeDepartment($event)">
        <option
          :value="item.key"
          :key="item.key"
          v-for="item in departmentData"
          :selected="user.Departments.indexOf(item.key)>-1"
        >{{getDepartmentShow(item)}}</option>
      </select>
    </f7-list-item>
    <f7-list-item title="角色" smart-select :smart-select-params="{openIn: 'popover'}">
      <select name="role" multiple @change="changeRole($event)">
        <option
          v-for="item in roleData"
          :value="item.Name"
          :key="item._id"
          :selected="user.Roles.indexOf(item.Name)>-1"
        >{{item.Name}}</option>
      </select>
    </f7-list-item>
  </f7-list>
  <f7-block class="text-align-center" v-else>
    <f7-preloader></f7-preloader>
  </f7-block>
</template>

<script>
export default {
  name: "user_base",
  props: ["user"],
  data() {
    return {
      companyData: null,
      roleData: null,
      departmentData: null
    };
  },
  created() {
    this.getCompanyData();
    this.getRoleData();
  },
  methods: {
    formatValueText(values) {
      var arr = [];
      for (var i = 0; i < values.length; i++)
        arr.push(values[i].replace(/&nbsp;/gi, ""));
      return arr;
    },
    getDepartmentShow(item) {
      var str = "";
      for (var i = 0; i < item.layer; i++) str += "&nbsp;";
      return str + item.title;
    },
    changeCompany(event) {
      this.user.CompanyCode = event.target.value;
      this.getDepartmentData(event.target.value);
    },
    changeRole(event) {
      var roles = [];
      for (var i = 0; i < event.target.length; i++) {
        if (event.target[i].selected) roles.push(event.target[i].value);
      }
      this.user.Roles = roles;
    },
    changeDepartment(event) {
      var depts = [];
      for (var i = 0; i < event.target.length; i++) {
        if (event.target[i].selected) depts.push(event.target[i].value);
      }
      this.user.Departments = depts;
    },
    getCompanyData() {
      this.$axios.get(this.$urls.company.getall).then(response => {
        if (response.code === 0) {
          this.companyData = response.result;
          if (this.user.CompanyCode.trim() === "")
            this.user.CompanyCode = this.companyData[0].Code;
          this.getDepartmentData(this.user.CompanyCode);
        }
      });
    },
    getRoleData() {
      //   if (this.$f7.roleData) {
      //     this.roleData = this.$f7.roleData;
      //     return;
      //   }
      this.$axios.get(this.$urls.role.getall).then(response => {
        if (response.code === 0) {
          //   this.$f7.roleData = response.result;
          this.roleData = response.result;
        }
      });
    },
    getDepartmentData(companyCode) {
      this.$axios
        .get(
          this.$urls.department.getdepartments + "?companyCode=" + companyCode
        )
        .then(response => {
          if (response.code === 0) {
            var dataList = [];
            this.generateDepartmentList(response.result, dataList);
            this.departmentData = dataList;
          }
        });
    },
    generateDepartmentList(data, dataList) {
      for (let i = 0; i < data.length; i++) {
        dataList.push({
          key: data[i].key,
          title: data[i].title,
          layer: data[i].Layer
        });
        if (data[i].children) {
          this.generateDepartmentList(data[i].children, dataList);
        }
      }
    }
  }
};
</script>

<style scoped>
</style>
