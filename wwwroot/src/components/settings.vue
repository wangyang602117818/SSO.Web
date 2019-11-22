<template>
  <a-layout>
    <a-layout-sider theme="light">
      <a-menu mode="inline" :defaultSelectedKeys="defaultSelectedCompany" @select="selectSetting">
        <a-menu-item key="1">基本设置</a-menu-item>
        <a-menu-item key="2">登录设置</a-menu-item>
        <a-menu-item key="3">安全设置</a-menu-item>
      </a-menu>
    </a-layout-sider>
    <a-layout-content>
      <a-form :form="userform" @submit="updateUser">
        <a-form-item label="UserName">
          <a-input
            v-decorator="['userName', { rules: [{ required: true, message: 'Please input your userName!' }] }]"
            style="width: 240px"
          />
        </a-form-item>
        <a-form-item label="Sex">
          <a-select
            placeholder="性别"
            v-decorator="['sex',{rules: [{ required: true, message: 'Sex is required!' }]}]"
            style="width: 150px"
          >
            <a-select-option value="M">男</a-select-option>
            <a-select-option value="F">女</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item label="Mobile">
          <a-input
            v-decorator="['mobile', { rules: [{ required: true, message: 'Please input your mobile!' }] }]"
            style="width: 240px"
          />
        </a-form-item>
        <a-form-item label="Email">
          <a-input
            v-decorator="['email', { rules: [{ required: true, message: 'Please input your email!' }] }]"
            style="width: 240px"
          />
        </a-form-item>
        <a-form-item label="IdCard">
          <a-input
            v-decorator="['idCard', { rules: [{ required: false, message: 'Please input your idCard!' }] }]"
            style="width: 260px"
          />
        </a-form-item>

        <a-form-item label="Company">
          <a-select
            allowClear
            v-decorator="[ 'companyCode', {rules: [{ required: true, message: 'Please select company!' }]}]"
            placeholder="所属公司"
            @change="changeCompany"
            style="width: 240px"
          >
            <a-select-option
              :value="item.Code"
              v-for="item in companyData"
              v-bind:key="item._id"
            >{{item.Name}}</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item label="Department">
          <a-tree-select
            style="width: 240px"
            :treeData="departmentData"
            multiple
            v-decorator="[ 'departments', {rules: [{ required: false }]}]"
            placeholder="所属部门"
            treeDefaultExpandAll
            allowClear
          ></a-tree-select>
        </a-form-item>
        <br />
        <a-form-item>
          <a-button type="primary" html-type="submit">确定</a-button>
        </a-form-item>
      </a-form>
    </a-layout-content>
  </a-layout>
</template>
<script>
export default {
  name: "settings",
  data() {
    return {
      userform: this.$form.createForm(this),
      companyData: [],
      departmentData: [],
      defaultSelectedCompany: ["1"]
    };
  },
  created() {
    
  },
  methods: {
    selectSetting(item) {
      this.defaultSelectedCompany = [item.key];
    },
    changeCompany(value) {
      this.form.setFieldsValue({ departments: "" });
      this.getDepartmentData(value);
    },
    getDepartmentData(companyCode) {
      this.$http
        .get(
          this.$urls.department.getdepartments + "?companyCode=" + companyCode
        )
        .then(response => {
          if (response.body.code == 0) {
            if (response.body.result.length > 0) {
              this.departmentData = response.body.result;
            } else {
              this.departmentData = [];
            }
          }
        });
    },
    updateUser() {}
  }
};
</script>
<style scoped>
.ant-layout {
  margin: 10px 0 !important;
}

.ant-form-horizontal .ant-row {
  margin-bottom: 6px;
}
.ant-btn-primary {
  margin-left: 0px !important;
}
</style>