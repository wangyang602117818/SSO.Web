<template>
  <a-layout>
    <a-layout-sider theme="light">
      <a-menu mode="inline" :defaultSelectedKeys="defaultSelectedCompany" @select="selectSetting">
        <a-menu-item key="1">基本设置</a-menu-item>
        <a-menu-item key="2">登录设置</a-menu-item>
      </a-menu>
    </a-layout-sider>
    <a-layout-content>
      <a-form v-if="defaultSelectedCompany[0]=='1'">
        <a-form-item label="UserId">{{user.UserId}}</a-form-item>
        <a-form-item label="UserName">{{user.UserName}}</a-form-item>
        <a-form-item label="Sex">{{user.Sex=='F'?'女':'男'}}</a-form-item>
        <a-form-item label="Mobile">{{user.Mobile}}</a-form-item>
        <a-form-item label="Email">{{user.Email}}</a-form-item>
        <a-form-item label="IdCard">{{user.IdCard}}</a-form-item>
        <a-form-item label="Company">
          <a-button size="small">{{user.CompanyName}}</a-button>
        </a-form-item>
        <a-form-item label="Department">
          <a-button size="small" v-for="dname in user.DepartmentName" :key="dname">{{dname}}</a-button>
        </a-form-item>
        <a-form-item label="Role">
          <a-button size="small" v-for="role in user.Role" :key="role">{{role}}</a-button>
        </a-form-item>
        <br />
        <a-form-item>
          <a-button type="primary" @click="showUserDrawer">修改</a-button>
        </a-form-item>
      </a-form>
      <a-form
        :form="passwordform"
        @submit.prevent="updateUserPassword"
        v-if="defaultSelectedCompany[0]=='2'"
      >
        <a-form-item label="OldPassWord">
          <a-input-password
            v-decorator="['oldPassword', { rules: [{ required: true, message: 'Please input your oldpassword!' }] }]"
            style="width: 240px"
          />
        </a-form-item>
        <a-form-item label="NewPassword">
          <a-input-password
            v-decorator="['newPassword', { rules: [
            {required: true, message: 'Please input your newPassword!' }
            ]}]"
            style="width: 240px"
          />
        </a-form-item>
        <a-form-item label="Confirm NewPassword">
          <a-input-password
            v-decorator="['newPassword2', { rules: [
            {required: true, message: 'Please confirm your newPassword!' },
            {validator: compareToFirstPassword}
            ]}]"
            style="width: 240px"
            @blur="handleConfirmBlur"
          />
        </a-form-item>
        <br />
        <a-form-item>
          <a-button type="primary" html-type="submit">确定</a-button>
        </a-form-item>
      </a-form>
      <a-drawer
        title="更新用户"
        :width="400"
        handle="slot"
        @close="userDrawerVisible=false"
        :visible="userDrawerVisible"
      >
        <a-form :form="userform" @submit.prevent="updateUser" v-if="defaultSelectedCompany[0]=='1'">
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
      </a-drawer>
    </a-layout-content>
  </a-layout>
</template>
<script>
export default {
  name: "settings",
  data() {
    return {
      userform: this.$form.createForm(this),
      passwordform: this.$form.createForm(this),
      user: {},
      companyData: [],
      departmentData: [],
      roleData: [],
      defaultSelectedCompany: ["1"],
      confirmDirty: false,
      userDrawerVisible: false
    };
  },
  created() {
    var selectMenu = this.defaultSelectedCompany[0];
    if (selectMenu == "1") {
      this.getUser();
    }
  },
  methods: {
    showUserDrawer() {
      this.getUser(true);
      this.userDrawerVisible = true;
    },
    handleConfirmBlur(e) {
      const value = e.target.value;
      this.confirmDirty = this.confirmDirty || !!value;
    },
    compareToFirstPassword(rule, value, callback) {
      const passwordform = this.passwordform;
      if (value && value !== passwordform.getFieldValue("newPassword")) {
        callback("Two passwords that you enter is inconsistent!");
      } else {
        callback();
      }
    },
    getCompanyData(companyCode) {
      this.$http.get(this.$urls.company.getall).then(response => {
        if (response.body.code == 0) {
          this.companyData = response.body.result;
          if (response.body.count > 0) {
            this.getDepartmentData(companyCode);
          }
        }
      });
    },
    selectSetting(item) {
      this.defaultSelectedCompany = [item.key];
      switch (item.key) {
        case "1":
          this.getUser();
      }
    },
    changeCompany(value) {
      this.userform.setFieldsValue({ departments: [] });
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
    getUser(drawer) {
      this.$http.get(this.$urls.user.getuser).then(response => {
        if (response.body.code == 0 && response.body.result) {
          this.user = response.body.result;
          if (drawer) {
            this.userform.setFieldsValue({
              userName: response.body.result.UserName,
              sex: response.body.result.Sex,
              mobile: response.body.result.Mobile,
              email: response.body.result.Email,
              idCard: response.body.result.IdCard,
              companyCode: response.body.result.CompanyCode,
              departments: response.body.result.DepartmentCode
            });
            this.getCompanyData(response.body.result.CompanyCode);
          }
        }
      });
    },
    updateUser() {
      this.userform.validateFields((error, values) => {
        if (!error) {
          values.id = this.user._id;
          values.userId = this.user.UserId;
          this.$http
            .post(this.$urls.user.updatebasicsetting, values)
            .then(response => {
              if (response.body.code == 0) {
                this.getUser();
                this.$message.warning("更新成功!");
                this.userDrawerVisible = false;
              }
            });
        }
      });
    },
    updateUserPassword() {
      this.passwordform.validateFields((error, values) => {
        if (!error) {
          this.$http
            .post(this.$urls.user.updatepassword, values)
            .then(response => {
              if (response.body.code == 114) {
                this.$message.warning("旧密码不正确!");
              }
              if (response.body.code == 0) {
                this.passwordform.setFieldsValue({
                  oldPassword: "",
                  newPassword: "",
                  newPassword2: ""
                });
                this.$message.warning("修改密码成功");
                window.location.href =
                  this.$http.options.root + this.$urls.logout;
              }
            });
        }
      });
    },
    updateSecure() {}
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
.ant-btn {
  margin-right: 5px;
}
</style>