<template>
  <div>
    <a-row type="flex" align="middle">
      <a-col :span="12">
        <a-input-search
          :placeholder="$t('search')"
          style="width: 200px"
          @search="onSearch"
          v-model="searchValue"
        />
        <a-button
          type="primary"
          icon="plus"
          :title="$t('add')"
          @click="showDrawer()"
        ></a-button>
        <a-button
          type="default"
          icon="redo"
          @click="reload"
          :title="$t('refresh')"
        ></a-button>
        <a-button
          type="default"
          icon="edit"
          @click="editUser"
          :disabled="selectedRowKeys.length != 1"
          :title="$t('edit')"
        ></a-button>
        <a-button
          type="default"
          icon="key"
          :title="$t('permission')"
          @click="eidtPermission"
          :disabled="selectedRowKeys.length != 1"
        />
        <a-popconfirm
          :title="$t('sure_reset_password')"
          @confirm="resetPassword"
          :okText="$t('yes')"
          :cancelText="$t('no')"
        >
          <a-button
            type="default"
            icon="unlock"
            :title="$t('reset_password')"
          ></a-button>
        </a-popconfirm>
        <a-popconfirm
          :title="$t('confirm_delete')"
          @confirm="removeUser"
          :okText="$t('yes')"
          :cancelText="$t('no')"
          v-if="this.showDelete == false"
        >
          <a-button
            type="danger"
            icon="delete"
            :title="$t('delete')"
            :disabled="selectedRowKeys.length == 0"
          ></a-button>
        </a-popconfirm>
        <a-popconfirm
          :title="$t('sure_restore_user')"
          @confirm="restoreUser"
          :okText="$t('yes')"
          :cancelText="$t('no')"
          v-if="this.showDelete == true"
        >
          <a-button
            type="default"
            icon="rollback"
            :title="$t('restore')"
            :disabled="selectedRowKeys.length == 0"
          ></a-button>
        </a-popconfirm>
        <a-popconfirm
          :title="$t('sure_permanent_delete_user')"
          @confirm="deleteUser"
          :okText="$t('yes')"
          :cancelText="$t('no')"
          v-if="this.showDelete == true"
        >
          <a-button
            type="default"
            icon="close"
            :title="$t('permanent_delete')"
            :disabled="selectedRowKeys.length == 0"
          ></a-button>
        </a-popconfirm>
      </a-col>
      <a-col :span="12" align="right">
        <a-tooltip
          :title="
            this.showDelete ? $t('show_normal_user') : $t('show_delete_user')
          "
          placement="left"
        >
          <a-switch :defaultChecked="showDelete" @change="changeDeleteShow" />
        </a-tooltip>
      </a-col>
    </a-row>

    <a-table
      :columns="columns"
      :rowKey="(record) => record.UserId"
      :dataSource="data"
      :rowSelection="{
        selectedRowKeys: selectedRowKeys,
        onChange: onSelectChange,
      }"
      :loading="loading"
      :pagination="pagination"
      @change="handleTableChange"
    >
      <a-tag
        :key="CompanyName"
        slot="CompanyName"
        slot-scope="CompanyName"
        color="#108ee9"
        >{{ CompanyName }}</a-tag
      >
      <span
        slot="DepartmentName"
        slot-scope="DepartmentName"
        v-if="DepartmentName"
      >
        <a-tag
          v-for="tag in DepartmentName.split(',')"
          :key="tag"
          color="#87d068"
          >{{ tag }}</a-tag
        >
      </span>
      <span slot="RoleName" slot-scope="RoleName" v-if="RoleName">
        <a-tag v-for="tag in RoleName.split(',')" :key="tag">{{ tag }}</a-tag>
      </span>
      <span slot="IsModified" slot-scope="text, record">
        <a-tooltip
          placement="top"
          :title="$funtools.parseIsoDateTime(record.UpdateTime)"
          >{{ record.IsModified }}</a-tooltip
        >
      </span>
    </a-table>
    <a-drawer
      :title="isUpdate ? $t('update_user') : $t('add_user')"
      :width="400"
      @close="drawerVisible = false"
      :visible="drawerVisible"
    >
      <a-form :form="form" @submit.prevent="handleSubmit">
        <a-form-item
          :label="$t('userId')"
          :label-col="{ span: 6 }"
          :wrapper-col="{ span: 12 }"
        >
          <a-input
            :placeholder="$t('userId') + '/' + $t('loginId')"
            v-decorator="[
              'userId',
              { rules: [{ required: true, message: $t('user_id_required') }] },
            ]"
          />
        </a-form-item>
        <a-form-item
          :label="$t('user_name')"
          :label-col="{ span: 6 }"
          :wrapper-col="{ span: 15 }"
        >
          <a-input
            v-decorator="[
              'userName',
              {
                rules: [{ required: true, message: $t('user_name_required') }],
              },
            ]"
          />
        </a-form-item>
        <a-form-item
          :label="$t('sex')"
          :label-col="{ span: 6 }"
          :wrapper-col="{ span: 8 }"
        >
          <a-select
            v-decorator="[
              'sex',
              { rules: [{ required: true, message: $t('sex_required') }] },
            ]"
          >
            <a-select-option value="M">{{ $t("male") }}</a-select-option>
            <a-select-option value="F">{{ $t("female") }}</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item
          :label="$t('mobile')"
          :label-col="{ span: 6 }"
          :wrapper-col="{ span: 15 }"
        >
          <a-input
            v-decorator="[
              'mobile',
              { rules: [{ required: false, message: $t('mobile_required') }] },
            ]"
          />
        </a-form-item>
        <a-form-item
          :label="$t('email')"
          :label-col="{ span: 6 }"
          :wrapper-col="{ span: 15 }"
        >
          <a-input
            v-decorator="[
              'email',
              { rules: [{ required: false, message: $t('email_required') }] },
            ]"
          />
        </a-form-item>
        <a-form-item
          :label="$t('id_card')"
          :label-col="{ span: 6 }"
          :wrapper-col="{ span: 15 }"
        >
          <a-input
            v-decorator="[
              'idCard',
              { rules: [{ required: false, message: $t('id_card_required') }] },
            ]"
          />
        </a-form-item>
        <a-form-item
          :label="$t('comp')"
          :label-col="{ span: 6 }"
          :wrapper-col="{ span: 15 }"
        >
          <a-select
            allowClear
            v-decorator="[
              'companyCode',
              { rules: [{ required: true, message: $t('company_required') }] },
            ]"
            @change="changeCompany"
          >
            <a-select-option
              :value="item.Code"
              v-for="item in companyData"
              v-bind:key="item.Id"
              >{{ item.Name }}</a-select-option
            >
          </a-select>
        </a-form-item>
        <a-form-item
          :label="$t('dept')"
          :label-col="{ span: 6 }"
          :wrapper-col="{ span: 15 }"
        >
          <a-tree-select
            :treeData="departmentData"
            multiple
            v-decorator="['departments', { rules: [{ required: false }] }]"
            treeDefaultExpandAll
            allowClear
          ></a-tree-select>
        </a-form-item>
        <a-form-item
          :label="$t('rol')"
          :label-col="{ span: 6 }"
          :wrapper-col="{ span: 15 }"
        >
          <a-select
            allowClear
            mode="multiple"
            v-decorator="[
              'roles',
              { rules: [{ required: false, message: $t('rol_required') }] },
            ]"
          >
            <a-select-option
              :value="item.Name"
              v-for="item in roleData"
              v-bind:key="item.Id"
              >{{ item.Name }}</a-select-option
            >
          </a-select>
        </a-form-item>
        <a-divider />
        <a-button @click="form.resetFields()">{{ $t("reset") }}</a-button>
        <a-button type="primary" html-type="submit">{{
          $t("submit")
        }}</a-button>
      </a-form>
    </a-drawer>
    <a-modal
      :title="$t('role_permission')"
      :visible="permissionVisible"
      :confirm-loading="confirmLoading"
      :okText="$t('submit')"
      :cancelText="$t('cancel')"
      @ok="submitPermission"
      @cancel="cancelPermission"
    >
      <div class="card-container" v-if="permissions">
        <a-tabs :default-active-key="0" size="small" type="card">
          <a-tab-pane
            :key="index"
            :tab="name"
            v-for="(value, name, index) in permissions"
          >
            <a-checkbox
              :default-checked="userpermissions.indexOf(item) > -1"
              @change="permissionChange"
              v-for="(item, index) in value"
              :key="index"
              :id="item"
              >{{ $t("permissions." + item) }}</a-checkbox
            >
          </a-tab-pane>
        </a-tabs>
      </div>
    </a-modal>
  </div>
</template>
<script>
export default {
  name: "user_basic",
  data() {
    return {
      data: [],
      searchValue: "",
      selectedRowKeys: [],
      selectedRows: [],
      companyData: [],
      departmentData: [],
      roleData: [],
      form: this.$form.createForm(this),
      confirmDirty: false,
      columns: [
        {
          title: this.$t("id"),
          dataIndex: "Id",
          width: "5%",
        },
        {
          title: this.$t("userId"),
          dataIndex: "UserId",
          width: "7%",
          ellipsis: true,
        },
        {
          title: this.$t("user_name"),
          dataIndex: "UserName",
          width: "10%",
          ellipsis: true,
        },
        {
          title: this.$t("mobile"),
          dataIndex: "Mobile",
          width: "11%",
          ellipsis: true,
        },
        {
          title: this.$t("email"),
          dataIndex: "Email",
          width: "10%",
          ellipsis: true,
        },
        {
          title: this.$t("sex"),
          dataIndex: "Sex",
          width: "5%",
          ellipsis: true,
          customRender: (val) => {
            return val == "F" ? this.$t("female") : this.$t("male");
          },
        },
        {
          title: this.$t("comp"),
          dataIndex: "CompanyName",
          width: "10%",
          ellipsis: false,
          scopedSlots: { customRender: "CompanyName" },
        },
        {
          title: this.$t("dept"),
          dataIndex: "DepartmentName",
          width: "13%",
          ellipsis: false,
          scopedSlots: { customRender: "DepartmentName" },
        },
        {
          title: this.$t("rol"),
          dataIndex: "RoleName",
          width: "12%",
          ellipsis: false,
          scopedSlots: { customRender: "RoleName" },
        },
        {
          title: this.$t("modified"),
          dataIndex: "IsModified",
          width: "5%",
          ellipsis: true,
          scopedSlots: { customRender: "IsModified" },
        },
        {
          title: this.$t("permission_count"),
          dataIndex: "PermissionCount",
          ellipsis: true,
          width: "5%",
        },
        {
          title: this.$t("create_time"),
          dataIndex: "CreateTime",
          ellipsis: true,
          width: "7%",
          customRender: (val) => {
            return this.$funtools.parseIsoDateTime(val);
          },
        },
      ],
      drawerVisible: false,
      pagination: { current: 1, pageSize: 10, size: "small" },
      loading: false,
      isUpdate: false,
      showDelete: false,
      permissionVisible: false,
      confirmLoading: false,
      permissions: null,
      userpermissions: [],
    };
  },
  created() {
    this.getData();
  },
  methods: {
    parseBsonTime(val) {
      return this.$funtools.parseBsonTime(val);
    },
    onSearch() {
      this.pagination.current = 1;
      this.selectedRowKeys = [];
      this.selectedRows = [];
      this.getData();
    },
    reload() {
      this.selectedRowKeys = [];
      this.selectedRows = [];
      this.getData();
    },
    changeDeleteShow() {
      this.showDelete = !this.showDelete;
      this.getData();
    },
    resetPassword() {
      this.loading = true;
      this.$axios
        .post(this.$urls.user.resetpassword, { userIds: this.selectedRowKeys })
        .then((response) => {
          if (response.code == 0) {
            this.selectedRowKeys = [];
            this.selectedRows = [];
            this.getData();
            this.$message.warning(this.$t("reset_success"));
          }
          this.loading = false;
        });
    },
    changeCompany(value) {
      this.form.setFieldsValue({ departments: [] });
      this.getDepartmentData(value);
    },
    handleSubmit() {
      this.form.validateFields((err, values) => {
        if (!err) {
          if (this.isUpdate) this.updateUser(values);
          if (!this.isUpdate) this.addUser(values);
        }
      });
    },
    addUser(user) {
      this.$axios.post(this.$urls.user.add, user).then((response) => {
        if (response.code == 0) {
          this.getData();
        }
      });
    },
    updateUser(user) {
      user.id = this.selectedRows[0].Id;
      this.$axios.post(this.$urls.user.update, user).then((response) => {
        if (response.code == 0) {
          this.getData();
          this.$message.warning(this.$t("modify_success"));
        }
      });
    },
    editUser() {
      this.$axios
        .get(this.$urls.user.getbyuserid + "?userid=" + this.selectedRowKeys[0])
        .then((response) => {
          if (response.code == 0) {
            this.showDrawer(response.result.CompanyCode);
            this.$nextTick(function () {
              this.form.setFieldsValue({
                userId: response.result.UserId,
                userName: response.result.UserName,
                sex: response.result.Sex,
                mobile: response.result.Mobile,
                email: response.result.Email,
                idCard: response.result.IdCard,
                companyCode: response.result.CompanyCode,
                departments: response.result.DepartmentCode,
                roles: response.result.Role,
              });
            });
          }
        });
    },
    removeUser() {
      this.loading = true;
      this.$axios
        .post(this.$urls.user.remove, { userIds: this.selectedRowKeys })
        .then((response) => {
          if (response.code == 0) {
            this.selectedRowKeys = [];
            this.selectedRows = [];
            this.getData();
          }
          this.loading = false;
        });
    },
    deleteUser() {
      this.loading = true;
      this.$axios
        .post(this.$urls.user.delete, { userIds: this.selectedRowKeys })
        .then((response) => {
          if (response.code == 0) {
            this.selectedRowKeys = [];
            this.selectedRows = [];
            this.getData();
          }
          this.loading = false;
        });
    },
    restoreUser() {
      this.loading = true;
      this.$axios
        .post(this.$urls.user.restore, { userIds: this.selectedRowKeys })
        .then((response) => {
          if (response.code == 0) {
            this.selectedRowKeys = [];
            this.selectedRows = [];
            this.getData();
          }
          this.loading = false;
        });
    },
    addTableColumns() {
      var len = this.columns.length;
      if (this.showDelete) {
        this.columns.splice(len - 1, 1, {
          title: this.$t("delete_time"),
          dataIndex: "DeleteTime",
          width: "15%",
          customRender: (val) => {
            return this.$funtools.parseIsoDateTime(val);
          },
        });
      } else {
        this.columns.splice(len - 1, 1, {
          title: this.$t("create_time"),
          dataIndex: "CreateTime",
          width: "15%",
          customRender: (val) => {
            return this.$funtools.parseIsoDateTime(val);
          },
        });
      }
    },
    getData() {
      this.addTableColumns();
      this.loading = true;
      this.$axios
        .get(
          this.$urls.user.getbasic +
            "?pageIndex=" +
            this.pagination.current +
            "&pageSize=" +
            this.pagination.pageSize +
            "&filter=" +
            this.searchValue +
            "&delete=" +
            this.showDelete
        )
        .then((response) => {
          this.loading = false;
          const pagination = { ...this.pagination };
          pagination.total = response.count;
          pagination.showTotal = () => {
            return this.pagination.total;
          };
          this.pagination = pagination;
          if (response.code == 0) this.data = response.result;
        });
    },
    getCompanyData() {
      this.$axios.get(this.$urls.company.getall).then((response) => {
        if (response.code == 0) {
          this.companyData = response.result;
        }
      });
    },
    getDepartmentData(companyCode) {
      this.$axios
        .get(
          this.$urls.department.getdepartments + "?companyCode=" + companyCode
        )
        .then((response) => {
          if (response.code == 0) {
            if (response.result.length > 0) {
              this.departmentData = response.result;
            } else {
              this.departmentData = [];
            }
          }
        });
    },
    getRoleData() {
      this.$axios.get(this.$urls.role.getall).then((response) => {
        if (response.code == 0) {
          this.roleData = response.result;
        } else {
          this.roleData = [];
        }
      });
    },
    onSelectChange(selectedRowKeys, selectedRows) {
      this.selectedRowKeys = selectedRowKeys;
      this.selectedRows = selectedRows;
    },
    handleTableChange(pagination) {
      this.pagination.current = pagination.current;
      this.getData();
    },
    showDrawer(companyCode) {
      this.getCompanyData();
      this.getRoleData();
      if (companyCode) {
        this.getDepartmentData(companyCode);
        this.isUpdate = true;
      } else {
        this.isUpdate = false;
      }
      this.drawerVisible = true;
    },
    onClose() {
      this.drawerVisible = false;
    },
    eidtPermission() {
      var that = this;
      this.$axios
        .all([this.getPermissions(), this.getUserPermissions()])
        .then(function (results) {
          if (results[0].code == 0 && results[1].code == 0) {
            that.permissions = results[0].result;
            that.userpermissions = Array.from(new Set(results[1].result));
          }
          that.permissionVisible = true;
        });
    },
    getPermissions() {
      return this.$axios.get(this.$urls.permission.getlist);
    },
    getUserPermissions() {
      var userId = this.selectedRows[0].UserId;
      return this.$axios.get(
        this.$urls.permission.getUserPermission + "?userId=" + userId
      );
    },
    submitPermission() {
      this.confirmLoading = true;
      this.loading = true;
      this.$axios
        .post(this.$urls.permission.addUserPermission, {
          user: this.selectedRows[0].UserId,
          names: this.userpermissions,
        })
        .then((response) => {
          if (response.code == 0) {
            this.confirmLoading = false;
            this.loading = false;
            this.cancelPermission();
          }
        });
    },
    cancelPermission() {
      this.permissions = [];
      this.userpermissions = [];
      this.permissionVisible = false;
    },
    permissionChange(e) {
      var value = e.target.id;
      var index = this.userpermissions.indexOf(value);
      if (index == -1) {
        this.userpermissions.push(value);
      } else {
        this.userpermissions.splice(index, 1);
      }
    },
  },
};
</script>
<style scope>
</style>