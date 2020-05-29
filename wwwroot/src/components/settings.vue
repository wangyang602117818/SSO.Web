<template>
  <a-layout>
    <a-layout-sider theme="light">
      <a-menu mode="inline" :defaultSelectedKeys="defaultSelectedCompany" @select="selectSetting">
        <a-menu-item key="1">{{this.$lang.basic_setting}}</a-menu-item>
        <a-menu-item key="2">{{this.$lang.login_setting}}</a-menu-item>
      </a-menu>
    </a-layout-sider>
    <a-layout-content>
      <a-spin size="small" v-if="loading" style="width:100%" />
      <div v-else>
        <a-form layout="inline" v-if="defaultSelectedCompany[0]=='1'">
          <a-form-item :label="this.$lang.userId">{{user.UserId}}</a-form-item>
          <a-form-item :label="this.$lang.user_name">{{user.UserName}}</a-form-item>
          <a-form-item :label="this.$lang.sex">{{user.Sex=='F'?this.$lang.female:this.$lang.male}}</a-form-item>
          <a-form-item :label="this.$lang.mobile">{{user.Mobile}}</a-form-item>
          <a-form-item :label="this.$lang.email">{{user.Email}}</a-form-item>
          <a-form-item :label="this.$lang.id_card">{{user.IdCard}}</a-form-item>
          <a-form-item :label="this.$lang.comp">
            <a-button size="small">{{user.CompanyName}}</a-button>
          </a-form-item>
          <a-form-item :label="this.$lang.dept">
            <a-button size="small" v-for="dname in user.DepartmentName" :key="dname">{{dname}}</a-button>
          </a-form-item>
          <a-form-item :label="this.$lang.rol">
            <a-button size="small" v-for="role in user.Role" :key="role">{{role}}</a-button>
          </a-form-item>
          <br />
          <a-form-item>
            <a-button type="primary" @click="showUserDrawer">{{this.$lang.modify}}</a-button>
          </a-form-item>
        </a-form>
      </div>
      <a-form
        :form="passwordform"
        @submit.prevent="updateUserPassword"
        v-if="defaultSelectedCompany[0]=='2'"
      >
        <a-form-item :label="this.$lang.old_password">
          <a-input-password
            v-decorator="['oldPassword', { rules: [{ required: true, message: this.$lang.old_password_required }] }]"
            style="width: 240px"
          />
        </a-form-item>
        <a-form-item :label="this.$lang.new_password">
          <a-input-password
            v-decorator="['newPassword', { rules: [
            {required: true,pattern:'(?=.*[0-9])(?=.*[a-zA-Z]).{6,30}', message: this.$lang.password_pattern_invalid }
            ]}]"
            style="width: 240px"
          />
        </a-form-item>
        <a-form-item :label="this.$lang.confirm_new_password">
          <a-input-password
            v-decorator="['newPassword2', { rules: [
            {required: true,pattern:'(?=.*[0-9])(?=.*[a-zA-Z]).{6,30}', message: this.$lang.password_pattern_invalid },
            {validator: compareToFirstPassword}
            ]}]"
            style="width: 240px"
            @blur="handleConfirmBlur"
          />
        </a-form-item>
        <br />
        <a-form-item>
          <a-button type="primary" html-type="submit">{{this.$lang.submit}}</a-button>
        </a-form-item>
      </a-form>
      <a-drawer
        :title="this.$lang.update_user"
        :width="400"
        handle="slot"
        @close="userDrawerVisible=false"
        :visible="userDrawerVisible"
      >
        <a-form :form="userform" @submit.prevent="updateUser" v-if="defaultSelectedCompany[0]=='1'">
          <a-form-item :label="this.$lang.user_name">
            <a-input
              v-decorator="['userName', { rules: [{ required: true, message:this.$lang.user_name_required }] }]"
              style="width: 240px"
            />
          </a-form-item>
          <a-form-item :label="this.$lang.sex">
            <a-select
              v-decorator="['sex',{rules: [{ required: true, message: this.$lang.sex_required }]}]"
              style="width: 150px"
            >
              <a-select-option value="M">{{this.$lang.male}}</a-select-option>
              <a-select-option value="F">{{this.$lang.female}}</a-select-option>
            </a-select>
          </a-form-item>
          <a-form-item :label="this.$lang.mobile">
            <a-input
              v-decorator="['mobile', { rules: [{ required: true, message: this.$lang.mobile_required }] }]"
              style="width: 240px"
            />
          </a-form-item>
          <a-form-item :label="this.$lang.email">
            <a-input
              v-decorator="['email', { rules: [{ required: true, message: this.$lang.email_required }] }]"
              style="width: 240px"
            />
          </a-form-item>
          <a-form-item :label="this.$lang.id_card">
            <a-input
              v-decorator="['idCard', { rules: [{ required: false, message: this.$lang.id_card_required }] }]"
              style="width: 260px"
            />
          </a-form-item>
          <a-form-item :label="this.$lang.comp">
            <a-select
              allowClear
              v-decorator="[ 'companyCode', {rules: [{ required: true, message: this.$lang.company_required }]}]"
              @change="changeCompany"
              style="width: 240px"
            >
              <a-select-option
                :value="item.Code"
                v-for="item in companyData"
                v-bind:key="item.Id"
              >{{item.Name}}</a-select-option>
            </a-select>
          </a-form-item>
          <a-form-item :label="this.$lang.dept">
            <a-tree-select
              style="width: 240px"
              :treeData="departmentData"
              multiple
              v-decorator="[ 'departments', {rules: [{ required: false }]}]"
              treeDefaultExpandAll
              allowClear
            ></a-tree-select>
          </a-form-item>
          <br />
          <a-form-item>
            <a-button type="primary" html-type="submit">{{this.$lang.submit}}</a-button>
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
      userDrawerVisible: false,
      loading: false
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
        callback(this.$lang.two_passwords_inconsistent);
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
      this.loading = true;
      this.$http.get(this.$urls.user.getuser).then(response => {
        this.loading = false;
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
          values.id = this.user.Id;
          values.userId = this.user.UserId;
          this.$http
            .post(this.$urls.user.updatebasicsetting, values)
            .then(response => {
              if (response.body.code == 0) {
                this.getUser();
                this.$message.warning(this.$lang.update_success);
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
                this.$message.warning(this.$lang.old_password_error);
              }
              if (response.body.code == 0) {
                this.passwordform.setFieldsValue({
                  oldPassword: "",
                  newPassword: "",
                  newPassword2: ""
                });
                this.$message.warning(this.$lang.update_success);
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