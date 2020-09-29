<template>
  <a-layout>
    <a-layout-sider theme="light">
      <a-menu mode="inline" :defaultSelectedKeys="defaultSelectedCompany" @select="selectSetting">
        <a-menu-item key="1">{{$t('basic_setting')}}</a-menu-item>
        <a-menu-item key="2">{{$t('login_setting')}}</a-menu-item>
      </a-menu>
    </a-layout-sider>
    <a-layout-content>
      <a-spin size="small" v-if="loading" style="width:100%" />
      <div v-else>
        <a-form layout="inline" v-if="defaultSelectedCompany[0]=='1'">
          <a-form-item :label="$t('userId')">{{user.UserId}}</a-form-item>
          <a-form-item :label="$t('user_name')">{{user.UserName}}</a-form-item>
          <a-form-item :label="$t('sex')">{{user.Sex=='F'?$t('female'):$t('male')}}</a-form-item>
          <a-form-item :label="$t('mobile')">{{user.Mobile}}</a-form-item>
          <a-form-item :label="$t('email')">{{user.Email}}</a-form-item>
          <a-form-item :label="$t('id_card')">{{user.IdCard}}</a-form-item>
          <a-form-item :label="$t('comp')">
            <a-button size="small">{{user.CompanyName}}</a-button>
          </a-form-item>
          <a-form-item :label="$t('dept')">
            <a-button size="small" v-for="dname in user.DepartmentName" :key="dname">{{dname}}</a-button>
          </a-form-item>
          <a-form-item :label="$t('rol')">
            <a-button size="small" v-for="role in user.Role" :key="role">{{role}}</a-button>
          </a-form-item>
          <br />
          <a-form-item>
            <a-button type="primary" @click="showUserDrawer">{{$t('modify')}}</a-button>
          </a-form-item>
        </a-form>
      </div>
      <a-form
        :form="passwordform"
        @submit.prevent="updateUserPassword"
        v-if="defaultSelectedCompany[0]=='2'"
      >
        <a-form-item :label="$t('old_password')">
          <a-input-password
            v-decorator="['oldPassword', { rules: [{ required: true, message: $t('old_password_required') }] }]"
            style="width: 240px"
          />
        </a-form-item>
        <a-form-item :label="$t('new_password')">
          <a-input-password
            v-decorator="['newPassword', { rules: [
            {required: true,pattern:'(?=.*[0-9])(?=.*[a-zA-Z]).{6,30}', message: $t('password_pattern_invalid') }
            ]}]"
            style="width: 240px"
          />
        </a-form-item>
        <a-form-item :label="$t('confirm_new_password')">
          <a-input-password
            v-decorator="['newPassword2', { rules: [
            {required: true,pattern:'(?=.*[0-9])(?=.*[a-zA-Z]).{6,30}', message: $t('password_pattern_invalid') },
            {validator: compareToFirstPassword}
            ]}]"
            style="width: 240px"
            @blur="handleConfirmBlur"
          />
        </a-form-item>
        <br />
        <a-form-item>
          <a-button type="primary" html-type="submit">{{$t('submit')}}</a-button>
        </a-form-item>
      </a-form>
      <a-drawer
        :title="$t('update_user')"
        :width="400"
        @close="userDrawerVisible=false"
        :visible="userDrawerVisible"
      >
        <a-form :form="userform" @submit.prevent="updateUser" v-if="defaultSelectedCompany[0]=='1'">
          <a-form-item :label="$t('user_name')">
            <a-input
              v-decorator="['userName', { rules: [{ required: true, message:$t('user_name_required') }] }]"
              style="width: 240px"
            />
          </a-form-item>
          <a-form-item :label="$t('sex')">
            <a-select
              v-decorator="['sex',{rules: [{ required: true, message: $t('sex_required') }]}]"
              style="width: 150px"
            >
              <a-select-option value="M">{{$t('male')}}</a-select-option>
              <a-select-option value="F">{{$t('female')}}</a-select-option>
            </a-select>
          </a-form-item>
          <a-form-item :label="$t('mobile')">
            <a-input
              v-decorator="['mobile', { rules: [{ required: true, message: $t('mobile_required') }] }]"
              style="width: 240px"
            />
          </a-form-item>
          <a-form-item :label="$t('email')">
            <a-input
              v-decorator="['email', { rules: [{ required: true, message: $t('email_required') }] }]"
              style="width: 240px"
            />
          </a-form-item>
          <a-form-item :label="$t('id_card')">
            <a-input
              v-decorator="['idCard', { rules: [{ required: false, message: $t('id_card_required') }] }]"
              style="width: 260px"
            />
          </a-form-item>
          <a-form-item :label="$t('comp')">
            <a-select
              allowClear
              v-decorator="[ 'companyCode', {rules: [{ required: true, message: $t('company_required') }]}]"
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
          <a-form-item :label="$t('dept')">
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
            <a-button type="primary" html-type="submit">{{$t('submit')}}</a-button>
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
        callback(this.$t('two_passwords_inconsistent'));
      } else {
        callback();
      }
    },
    getCompanyData(companyCode) {
      this.$axios.get(this.$urls.company.getall).then(response => {
        if (response.code == 0) {
          this.companyData = response.result;
          if (response.count > 0) {
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
      this.$axios
        .get(
          this.$urls.department.getdepartments + "?companyCode=" + companyCode
        )
        .then(response => {
          if (response.code == 0) {
            if (response.result.length > 0) {
              this.departmentData = response.result;
            } else {
              this.departmentData = [];
            }
          }
        });
    },
    getUser(drawer) {
      this.loading = true;
      this.$axios.get(this.$urls.user.getuser).then(response => {
        this.loading = false;
        if (response.code == 0 && response.result) {
          this.user = response.result;
          if (drawer) {
            this.userform.setFieldsValue({
              userName: response.result.UserName,
              sex: response.result.Sex,
              mobile: response.result.Mobile,
              email: response.result.Email,
              idCard: response.result.IdCard,
              companyCode: response.result.CompanyCode,
              departments: response.result.DepartmentCode
            });
            this.getCompanyData(response.result.CompanyCode);
          }
        }
      });
    },
    updateUser() {
      this.userform.validateFields((error, values) => {
        if (!error) {
          values.id = this.user.Id;
          values.userId = this.user.UserId;
          this.$axios
            .post(this.$urls.user.updatebasicsetting, values)
            .then(response => {
              if (response.code == 0) {
                this.getUser();
                this.$message.warning(this.$t('update_success'));
                this.userDrawerVisible = false;
              }
            });
        }
      });
    },
    updateUserPassword() {
      this.passwordform.validateFields((error, values) => {
        if (!error) {
          this.$axios
            .post(this.$urls.user.updatepassword, values)
            .then(response => {
              if (response.code == 114) {
                this.$message.warning(this.$t('old_password_error'));
              }
              if (response.code == 0) {
                this.passwordform.setFieldsValue({
                  oldPassword: "",
                  newPassword: "",
                  newPassword2: ""
                });
                this.$message.warning(this.$t('update_success'));
                window.location.href =
                  this.$baseUrl +
                  this.$urls.logout +
                  "?returnUrl=" +
                  window.location.href;
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