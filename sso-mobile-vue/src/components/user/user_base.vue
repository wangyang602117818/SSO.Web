<template>
  <f7-list inline-labels v-if="user && companyData && roleData &&departmentData">
    <f7-list-input
      required
      validate
      :label="$t('manage.user_id')+'*'"
      type="text"
      :placeholder="$t('common.id')+'/'+$t('manage.login_name')"
      :error-message="$t('valid.id_required')"
      clear-button
      :value="user.UserId"
      @input="($event)=>{user.UserId=$event.target.value}"
    ></f7-list-input>
    <f7-list-input
      :label="$t('manage.user_name')+'*'"
      type="text"
      :placeholder="$t('manage.user_name')"
      :error-message="$t('valid.user_name_required')"
      required
      validate
      clear-button
      :value="user.UserName"
      @input="($event)=>{user.UserName=$event.target.value}"
    ></f7-list-input>
    <f7-list-item :title="$t('common.sex')" smart-select :smart-select-params="{openIn: 'popover'}">
      <select name="sex" @change="($event)=>{user.Sex = $event.target.value}">
        <option value="M" :selected="user.Sex==='M'">{{$t('common.male')}}</option>
        <option value="F" :selected="user.Sex==='F'">{{$t('common.female')}}</option>
      </select>
    </f7-list-item>
    <f7-list-input
      :label="$t('common.mobile')"
      validate
      pattern="[0-9]*"
      type="text"
      :placeholder="$t('common.mobile')"
      clear-button
      :value="user.Mobile"
      @input="($event)=>{user.Mobile=$event.target.value}"
    ></f7-list-input>
    <f7-list-input
      :label="$t('common.email')"
      type="email"
      validate
      :placeholder="$t('common.email')"
      :error-message="$t('valid.email_format_invalid')"
      clear-button
      :value="user.Email"
      @input="($event)=>{user.Email=$event.target.value}"
    ></f7-list-input>
    <f7-list-input
      :label="$t('common.idCard')"
      type="text"
      :placeholder="$t('common.idCard')"
      clear-button
      :value="user.IdCard"
      @input="($event)=>{user.IdCard=$event.target.value}"
    ></f7-list-input>
    <f7-list-item :title="$t('common.company')" smart-select :smart-select-params="{openIn: 'popover'}">
      <select name="company" @change="changeCompany($event)">
        <option
          v-for="item in companyData"
          :value="item.Code"
          :key="item.Id"
          :selected="item.Code===user.CompanyCode"
        >{{item.Name}}</option>
      </select>
    </f7-list-item>
    <f7-list-item
      link="#"
      :title="$t('common.department')"
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
    <f7-list-item :title="$t('common.role')" smart-select :smart-select-params="{openIn: 'popover'}">
      <select name="role" multiple @change="changeRole($event)">
        <option
          v-for="item in roleData"
          :value="item.Name"
          :key="item.Id"
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
      this.$axios.get(this.$urls.role.getall).then(response => {
        if (response.code === 0) {
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
