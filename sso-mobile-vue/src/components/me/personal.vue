<template>
  <f7-page name="personal">
    <f7-navbar title="个人信息" back-link="返回">
      <f7-link slot="right" @click="save">保存</f7-link>
    </f7-navbar>
    <f7-list inline-labels v-if="user.UserId && companyData &&departmentData">
      <f7-list-item title="用户编号" :after="user.UserId"></f7-list-item>
      <f7-list-item title="性别" smart-select :smart-select-params="{openIn: 'popover'}">
        <select name="sex" @change="($event)=>{user.Sex = $event.target.value}">
          <option value="M" :selected="user.Sex==='M'">男</option>
          <option value="F" :selected="user.Sex==='F'">女</option>
        </select>
      </f7-list-item>
      <f7-list-input
        label="手机号码"
        type="text"
        validate
        pattern="[0-9]*"
        placeholder="手机号码"
        :value="user.Mobile"
        @input="($event)=>{user.Mobile=$event.target.value}"
      ></f7-list-input>
      <f7-list-input
        label="邮箱"
        type="email"
        validate
        placeholder="邮箱"
        error-message="邮箱格式不正确!"
        :value="user.Email"
        @input="($event)=>{user.Email=$event.target.value}"
      ></f7-list-input>
      <f7-list-input
        label="证件号"
        type="text"
        :value="user.IdCard"
        @input="($event)=>{user.IdCard=$event.target.value}"
      ></f7-list-input>
      <f7-list-item title="公司" smart-select :smart-select-params="{openIn: 'popover'}">
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
      <f7-list-item title="角色" :after="user.Role.join(',')"></f7-list-item>
      <f7-list-item title="上次修改时间" :after="user.UpdateTime"></f7-list-item>
    </f7-list>
    <f7-block class="text-align-center" v-else>
      <f7-preloader></f7-preloader>
    </f7-block>
  </f7-page>
</template>

<script>
export default {
  name: "personal",
  data() {
    return {
      user: {
        id: "",
        UserId: "",
        UserName: "",
        Sex: "",
        Mobile: "",
        Email: "",
        IdCard: "",
        CompanyCode: "",
        Departments: [],
        UpdateTime:"",
        Role:[]
      },
      companyData: null,
      departmentData: null
    };
  },
  created() {
    this.getUser();
    this.getCompanyData();
  },
  methods: {
    getUser() {
      var userId = this.$f7route.params.userId;
      this.$axios
        .get(this.$urls.user.getuser)
        .then(response => {
          if (response.code === 0) {
            this.user = {
              id: response.result.Id,
              UserId: response.result.UserId,
              UserName: response.result.UserName,
              Sex: response.result.Sex,
              Mobile: response.result.Mobile,
              Email: response.result.Email,
              IdCard: response.result.IdCard,
              CompanyCode: response.result.CompanyCode,
              Departments: response.result.DepartmentCode,
              UpdateTime:response.result.UpdateTime,
              Role:response.result.Role,
            };
          }
        });
    },
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
    },
    save() {
      if (this.user.UserId.trim() == "") return;
      if (this.user.UserName.trim() == "") return;
      this.$axios.post(this.$urls.user.updatebasicsetting, this.user).then(response => {
        if (response.code === 0) {
          this.$f7router.back();
          this.showSuccess();
        }
      });
    }
  }
};
</script>

<style scoped>
.me_content {
  width: 90%;
  margin: 0 auto;
  overflow-y: scroll;
  padding-bottom: 30px;
}
</style>
