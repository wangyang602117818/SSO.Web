<template>
  <div>
    <a-row type="flex" align="middle">
      <a-col :span="12">
        <a-input-search
          :placeholder="this.$lang.search"
          style="width: 200px"
          @search="onSearch"
          v-model="searchValue"
        />
        <a-button type="primary" icon="plus" :title="this.$lang.add" @click="showDrawer()"></a-button>
        <a-button type="default" icon="redo" @click="reload" :title="this.$lang.refresh"></a-button>
        <a-button
          type="default"
          icon="edit"
          @click="editUser"
          :disabled="selectedRowKeys.length!=1"
          :title="this.$lang.edit"
        ></a-button>
        <a-popconfirm
          :title="this.$lang.sure_reset_password"
          @confirm="resetPassword"
          :okText="this.$lang.yes"
          :cancelText="this.$lang.no"
        >
          <a-button type="default" icon="unlock" :title="this.$lang.reset_password"></a-button>
        </a-popconfirm>
        <a-popconfirm
          :title="this.$lang.confirm_delete"
          @confirm="removeUser"
          :okText="this.$lang.yes"
          :cancelText="this.$lang.no"
          v-if="this.showDelete==false"
        >
          <a-button
            type="danger"
            icon="delete"
            :title="this.$lang.delete"
            :disabled="selectedRowKeys.length==0"
          ></a-button>
        </a-popconfirm>
        <a-popconfirm
          :title="this.$lang.sure_restore_user"
          @confirm="restoreUser"
          :okText="this.$lang.yes"
          :cancelText="this.$lang.no"
          v-if="this.showDelete==true"
        >
          <a-button
            type="default"
            icon="rollback"
            :title="this.$lang.restore"
            :disabled="selectedRowKeys.length==0"
          ></a-button>
        </a-popconfirm>
        <a-popconfirm
          :title="this.$lang.sure_permanent_delete_user"
          @confirm="deleteUser"
          :okText="this.$lang.yes"
          :cancelText="this.$lang.no"
          v-if="this.showDelete==true"
        >
          <a-button
            type="default"
            icon="close"
            :title="this.$lang.permanent_delete"
            :disabled="selectedRowKeys.length==0"
          ></a-button>
        </a-popconfirm>
      </a-col>
      <a-col :span="12" align="right">
        <a-tooltip
          :title="this.showDelete?this.$lang.show_normal_user:this.$lang.show_delete_user"
          placement="left"
        >
          <a-switch :defaultChecked="showDelete" @change="changeDeleteShow" />
        </a-tooltip>
      </a-col>
    </a-row>

    <a-table
      :columns="columns"
      :rowKey="record => record.UserId"
      :dataSource="data"
      :rowSelection="{selectedRowKeys:selectedRowKeys,onChange:onSelectChange}"
      :loading="loading"
      :pagination="pagination"
      @change="handleTableChange"
    >
      <a-tag :key="CompanyName" slot="CompanyName" slot-scope="CompanyName" color="#108ee9">{{CompanyName}}</a-tag>
      <span slot="DepartmentName" slot-scope="DepartmentName" v-if="DepartmentName">
        <a-tag v-for="tag in DepartmentName.split(',')" :key="tag" color="#87d068">{{tag}}</a-tag>
      </span>
      <span slot="RoleName" slot-scope="RoleName" v-if="RoleName">
        <a-tag v-for="tag in RoleName.split(',')" :key="tag">{{tag}}</a-tag>
      </span>
      <span slot="IsModified" slot-scope="text, record">
        <a-tooltip
          placement="top"
          :title="$funtools.parseIsoDateTime(record.UpdateTime)"
        >{{record.IsModified}}</a-tooltip>
      </span>
    </a-table>
    <a-drawer
      :title="isUpdate?this.$lang.update_user:this.$lang.add_user"
      :width="400"
      @close="drawerVisible=false"
      :visible="drawerVisible"
    >
      <a-form :form="form" @submit.prevent="handleSubmit">
        <a-form-item
          :label="this.$lang.userId"
          :label-col="{ span: 6 }"
          :wrapper-col="{ span: 12 }"
        >
          <a-input
            :placeholder="this.$lang.userId+'/'+this.$lang.loginId"
            v-decorator="['userId',{rules: [{ required: true, message: this.$lang.user_id_required }]}]"
          />
        </a-form-item>
        <a-form-item
          :label="this.$lang.user_name"
          :label-col="{ span: 6 }"
          :wrapper-col="{ span: 15 }"
        >
          <a-input
            v-decorator="['userName',{rules: [{ required: true, message: this.$lang.user_name_required }]}]"
          />
        </a-form-item>
        <a-form-item :label="this.$lang.sex" :label-col="{ span: 6 }" :wrapper-col="{ span: 8 }">
          <a-select
            v-decorator="['sex',{rules: [{ required: true, message:this.$lang.sex_required }]}]"
          >
            <a-select-option value="M">{{this.$lang.male}}</a-select-option>
            <a-select-option value="F">{{this.$lang.female}}</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item
          :label="this.$lang.mobile"
          :label-col="{ span: 6 }"
          :wrapper-col="{ span: 15 }"
        >
          <a-input
            v-decorator="['mobile',{rules: [{ required: false, message: this.$lang.mobile_required }]}]"
          />
        </a-form-item>
        <a-form-item :label="this.$lang.email" :label-col="{ span: 6 }" :wrapper-col="{ span: 15 }">
          <a-input
            v-decorator="['email',{rules: [{ required: false, message:this.$lang.email_required }]}]"
          />
        </a-form-item>
        <a-form-item
          :label="this.$lang.id_card"
          :label-col="{ span: 6 }"
          :wrapper-col="{ span: 15 }"
        >
          <a-input
            v-decorator="['idCard',{rules: [{ required: false, message: this.$lang.id_card_required }]}]"
          />
        </a-form-item>
        <a-form-item :label="this.$lang.comp" :label-col="{ span: 6 }" :wrapper-col="{ span: 15 }">
          <a-select
            allowClear
            v-decorator="[ 'companyCode', {rules: [{ required: true, message: this.$lang.company_required }]}]"
            @change="changeCompany"
          >
            <a-select-option
              :value="item.Code"
              v-for="item in companyData"
              v-bind:key="item.Id"
            >{{item.Name}}</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item :label="this.$lang.dept" :label-col="{ span: 6 }" :wrapper-col="{ span: 15 }">
          <a-tree-select
            :treeData="departmentData"
            multiple
            v-decorator="[ 'departments', {rules: [{ required: false }]}]"
            treeDefaultExpandAll
            allowClear
          ></a-tree-select>
        </a-form-item>
        <a-form-item :label="this.$lang.rol" :label-col="{ span: 6 }" :wrapper-col="{ span: 15 }">
          <a-select
            allowClear
            mode="multiple"
            v-decorator="[ 'roles', {rules: [{ required: false, message: this.$lang.rol_required }]}]"
          >
            <a-select-option
              :value="item.Name"
              v-for="item in roleData"
              v-bind:key="item.Id"
            >{{item.Name}}</a-select-option>
          </a-select>
        </a-form-item>
        <a-divider />
        <a-button @click="form.resetFields();">{{this.$lang.reset}}</a-button>
        <a-button type="primary" html-type="submit">{{this.$lang.submit}}</a-button>
      </a-form>
    </a-drawer>
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
          title: this.$lang.id,
          dataIndex: "Id",
          width: "5%"
        },
        {
          title: this.$lang.userId,
          dataIndex: "UserId",
          width: "7%",ellipsis: true
        },
        {
          title: this.$lang.user_name,
          dataIndex: "UserName",
          width: "10%",ellipsis: true
        },
        {
          title: this.$lang.mobile,
          dataIndex: "Mobile",
          width: "11%",
          ellipsis: true
        },
        {
          title: this.$lang.email,
          dataIndex: "Email",
          width: "10%",
          ellipsis: true
        },
        {
          title: this.$lang.sex,
          dataIndex: "Sex",
          width: "5%",
          ellipsis: true,
          customRender: val => {
            return val == "F" ? this.$lang.female : this.$lang.male;
          }
        },
        {
          title: this.$lang.comp,
          dataIndex: "CompanyName",
          width: "10%",
          ellipsis: false,
          scopedSlots: { customRender: "CompanyName" }
        },
        {
          title: this.$lang.dept,
          dataIndex: "DepartmentName",
          width: "13%",
          ellipsis: false,
          scopedSlots: { customRender: "DepartmentName" }
        },
        {
          title: this.$lang.rol,
          dataIndex: "RoleName",
          width: "12%",
          ellipsis: false,
          scopedSlots: { customRender: "RoleName" }
        },
        {
          title: this.$lang.modified,
          dataIndex: "IsModified",
          width: "7%",
          ellipsis: true,
          scopedSlots: { customRender: "IsModified" }
        },
        {
          title: this.$lang.create_time,
          dataIndex: "CreateTime",
          ellipsis: true,
          width: "10%",
          customRender: val => {
            return this.$funtools.parseIsoDateTime(val);
          }
        }
      ],
      drawerVisible: false,
      pagination: { current: 1, pageSize: 10, size: "small" },
      loading: false,
      isUpdate: false,
      showDelete: false
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
        .then(response => {
          if (response.code == 0) {
            this.selectedRowKeys = [];
            this.selectedRows = [];
            this.getData();
            this.$message.warning(this.$lang.reset_success);
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
      this.$axios.post(this.$urls.user.add, user).then(response => {
        if (response.code == 400) {
          this.$message.warning(this.$lang.record_exists);
        }
        if (response.code == 0) {
          this.getData();
        }
      });
    },
    updateUser(user) {
      user.id = this.selectedRows[0].Id;
      this.$axios.post(this.$urls.user.update, user).then(response => {
        if (response.code == 0) {
          this.getData();
          this.$message.warning(this.$lang.modify_success);
        }
      });
    },
    editUser() {
      this.$axios
        .get(this.$urls.user.getbyuserid + "?userid=" + this.selectedRowKeys[0])
        .then(response => {
          if (response.code == 0) {
            this.showDrawer(response.result.CompanyCode);
            this.form.setFieldsValue({
              userId: response.result.UserId,
              userName: response.result.UserName,
              sex: response.result.Sex,
              mobile: response.result.Mobile,
              email: response.result.Email,
              idCard: response.result.IdCard,
              companyCode: response.result.CompanyCode,
              departments: response.result.DepartmentCode,
              roles: response.result.Role
            });
          }
        });
    },
    removeUser() {
      this.loading = true;
      this.$axios
        .post(this.$urls.user.remove, { userIds: this.selectedRowKeys })
        .then(response => {
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
        .then(response => {
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
        .then(response => {
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
          title: this.$lang.delete_time,
          dataIndex: "DeleteTime",
          width: "15%",
          customRender: val => {
            return this.$funtools.parseIsoDateTime(val);
          }
        });
      } else {
        this.columns.splice(len - 1, 1, {
          title: this.$lang.create_time,
          dataIndex: "CreateTime",
          width: "15%",
          customRender: val => {
            return this.$funtools.parseIsoDateTime(val);
          }
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
        .then(response => {
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
      this.$axios.get(this.$urls.company.getall).then(response => {
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
    getRoleData() {
      this.$axios.get(this.$urls.role.getall).then(response => {
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
    }
  }
};
</script>
<style scope>
</style>