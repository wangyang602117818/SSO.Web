<template>
  <div>
    <a-row type="flex" align="middle">
      <a-col :span="12">
        <a-input-search
          placeholder="input search text"
          style="width: 200px"
          @search="onSearch"
          v-model="searchValue"
        />
        <a-button type="primary" icon="plus" @click="showDrawer()"></a-button>
        <a-button type="default" icon="redo" @click="reload"></a-button>
        <a-button
          type="default"
          icon="edit"
          @click="editUser"
          :disabled="selectedRowKeys.length!=1"
        ></a-button>

        <a-popconfirm
          title="Are you sure remove this user?"
          @confirm="removeUser"
          okText="Yes"
          cancelText="No"
          v-if="this.showDelete==false"
        >
          <a-button type="danger" icon="delete" :disabled="selectedRowKeys.length==0"></a-button>
        </a-popconfirm>
        <a-popconfirm
          title="Are you sure restore this user?"
          @confirm="restoreUser"
          okText="Yes"
          cancelText="No"
          v-if="this.showDelete==true"
        >
          <a-button type="default" icon="rollback" :disabled="selectedRowKeys.length==0"></a-button>
        </a-popconfirm>
        <a-popconfirm
          title="Are you sure permanent delete this user?"
          @confirm="deleteUser"
          okText="Yes"
          cancelText="No"
          v-if="this.showDelete==true"
        >
          <a-button type="default" icon="close" :disabled="selectedRowKeys.length==0"></a-button>
        </a-popconfirm>
      </a-col>
      <a-col :span="12" align="right">
        <a-tooltip :title="this.showDelete?'显示正常用户':'显示删除用户'" placement="top">
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
      <a-tag :key="CompanyName" slot="CompanyName" slot-scope="CompanyName">{{CompanyName}}</a-tag>
      <span slot="DepartmentName" slot-scope="DepartmentName" v-if="DepartmentName">
        <a-tag v-for="tag in DepartmentName.split(',')" :key="tag">{{tag}}</a-tag>
      </span>
      <span slot="RoleName" slot-scope="RoleName" v-if="RoleName">
        <a-tag v-for="tag in RoleName.split(',')" :key="tag">{{tag}}</a-tag>
      </span>
      <span slot="IsModified" slot-scope="text, record">
        <a-tooltip
          placement="top"
          :title="parseBsonTime(record.UpdateTime.$date)"
        >{{record.IsModified}}</a-tooltip>
      </span>
    </a-table>
    <a-drawer
      :title="isUpdate?'更新用户':'添加用户'"
      :width="400"
      handle="slot"
      @close="drawerVisible=false"
      :visible="drawerVisible"
    >
      <a-form :form="form" @submit.prevent="handleSubmit">
        <a-form-item label="UserId" :label-col="{ span: 6 }" :wrapper-col="{ span: 12 }">
          <a-input
            placeholder="用户编号/登录名"
            v-decorator="['userId',{rules: [{ required: true, message: 'UserId is required!' }]}]"
          />
        </a-form-item>
        <a-form-item label="UserName" :label-col="{ span: 6 }" :wrapper-col="{ span: 15 }">
          <a-input
            placeholder="用户名称"
            v-decorator="['userName',{rules: [{ required: true, message: 'UserName is required!' }]}]"
          />
        </a-form-item>
        <a-form-item label="Sex" :label-col="{ span: 6 }" :wrapper-col="{ span: 15 }">
          <a-select
            placeholder="性别"
            v-decorator="['sex',{rules: [{ required: true, message: 'Sex is required!' }]}]"
          >
            <a-select-option value="M">男</a-select-option>
            <a-select-option value="F">女</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item label="Password" :label-col="{ span: 6 }" :wrapper-col="{ span: 10 }">
          <a-input
            placeholder="登录密码"
            v-decorator="['password',{rules: [
            {required: true, message: 'Password is required!' },
            {validator: validateToNextPassword}
            ]}]"
            type="password"
          />
        </a-form-item>
        <a-form-item label="Confirm" :label-col="{ span: 6 }" :wrapper-col="{ span: 10 }">
          <a-input
            placeholder="确认密码"
            v-decorator="['confirmPassword',{rules: [
            {required: true, message: 'Password is required!' },
            {validator: compareToFirstPassword}]}]"
            type="password"
            @blur="handleConfirmBlur"
          />
        </a-form-item>
        <a-form-item label="Mobile" :label-col="{ span: 6 }" :wrapper-col="{ span: 15 }">
          <a-input
            placeholder="手机号"
            v-decorator="['mobile',{rules: [{ required: false, message: 'Mobile is required!' }]}]"
          />
        </a-form-item>
        <a-form-item label="Email" :label-col="{ span: 6 }" :wrapper-col="{ span: 15 }">
          <a-input
            placeholder="邮箱"
            v-decorator="['email',{rules: [{ required: false, message: 'Email is required!' }]}]"
          />
        </a-form-item>
        <a-form-item label="IdCard" :label-col="{ span: 6 }" :wrapper-col="{ span: 15 }">
          <a-input
            placeholder="证件号"
            v-decorator="['idCard',{rules: [{ required: false, message: 'IdCard is required!' }]}]"
          />
        </a-form-item>
        <a-form-item label="Company" :label-col="{ span: 6 }" :wrapper-col="{ span: 15 }">
          <a-select
            allowClear
            v-decorator="[ 'companyCode', {rules: [{ required: true, message: 'Please select company!' }]}]"
            placeholder="所属公司"
            @change="changeCompany"
          >
            <a-select-option
              :value="item.Code"
              v-for="item in companyData"
              v-bind:key="item._id"
            >{{item.Name}}</a-select-option>
          </a-select>
        </a-form-item>
        <a-form-item label="Department" :label-col="{ span: 6 }" :wrapper-col="{ span: 15 }">
          <a-tree-select
            :treeData="departmentData"
            multiple
            v-decorator="[ 'departments', {rules: [{ required: false }]}]"
            placeholder="所属部门"
            treeDefaultExpandAll
            allowClear
          ></a-tree-select>
        </a-form-item>
        <a-form-item label="Role" :label-col="{ span: 6 }" :wrapper-col="{ span: 15 }">
          <a-select
            allowClear
            mode="multiple"
            v-decorator="[ 'roles', {rules: [{ required: false, message: 'Please select role!' }]}]"
            placeholder="角色"
          >
            <a-select-option
              :value="item.Name"
              v-for="item in roleData"
              v-bind:key="item._id"
            >{{item.Name}}</a-select-option>
          </a-select>
        </a-form-item>
        <a-divider />
        <a-button @click="form.resetFields();">重 置</a-button>
        <a-button type="primary" html-type="submit">确 定</a-button>
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
          title: "编号",
          dataIndex: "_id",
          width: "5%"
        },
        {
          title: "用户编号",
          dataIndex: "UserId",
          width: "7%"
        },
        {
          title: "用户名",
          dataIndex: "UserName",
          width: "12%"
        },
        {
          title: "手机号",
          dataIndex: "Mobile",
          width: "9%"
        },
        {
          title: "邮箱",
          dataIndex: "Email",
          width: "13%"
        },
        {
          title: "性别",
          dataIndex: "Sex",
          width: "5%",
          customRender: val => {
            return val == "F" ? "女" : "男";
          }
        },
        {
          title: "公司",
          dataIndex: "CompanyName",
          width: "5%",
          scopedSlots: { customRender: "CompanyName" }
        },
        {
          title: "部门",
          dataIndex: "DepartmentName",
          width: "10%",
          scopedSlots: { customRender: "DepartmentName" }
        },
        {
          title: "角色",
          dataIndex: "RoleName",
          width: "12%",
          scopedSlots: { customRender: "RoleName" }
        },
        {
          title: "已修改",
          dataIndex: "IsModified",
          width: "7%",
          scopedSlots: { customRender: "IsModified" }
        },
        {
          title: "创建时间",
          dataIndex: "CreateTime.$date",
          width: "15%",
          customRender: val => {
            return this.$common.parseBsonTime(val);
          }
        }
      ],
      drawerVisible: false,
      pagination: { current: 1, pageSize: 10,size:'small' },
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
      return this.$common.parseBsonTime(val);
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
    handleConfirmBlur(e) {
      const value = e.target.value;
      this.confirmDirty = this.confirmDirty || !!value;
    },
    compareToFirstPassword(rule, value, callback) {
      const form = this.form;
      if (value && value !== form.getFieldValue("password")) {
        callback("Two passwords that you enter is inconsistent!");
      } else {
        callback();
      }
    },
    validateToNextPassword(rule, value, callback) {
      const form = this.form;
      if (value && this.confirmDirty) {
        form.validateFields(["confirm"], { force: true });
      }
      callback();
    },
    changeCompany(value) {
      this.form.setFieldsValue({ departments: "" });
      this.getDepartmentData(value);
    },
    handleSubmit() {
      this.form.validateFields((err, values) => {
        window.console.log(values);
        if (!err) {
          if (this.isUpdate) this.updateUser(values);
          if (!this.isUpdate) this.addUser(values);
        }
      });
    },
    addUser(user) {
      this.$http.post(this.$urls.user.add, user).then(response => {
        if (response.body.code == 400) {
          this.$message.warning("记录已存在!");
        }
        if (response.body.code == 0) {
          this.getData();
        }
      });
    },
    updateUser(user) {
      user.id = this.selectedRows[0]._id;
      this.$http.post(this.$urls.user.update, user).then(response => {
        if (response.body.code == 0) {
          this.getData();
        }
      });
    },
    editUser() {
      this.$http
        .get(this.$urls.user.getbyuserid + "?userid=" + this.selectedRowKeys[0])
        .then(response => {
          if (response.body.code == 0) {
            this.showDrawer(response.body.result.CompanyCode);
            this.form.setFieldsValue({
              userId: response.body.result.UserId,
              userName: response.body.result.UserName,
              sex: response.body.result.Sex,
              mobile: response.body.result.Mobile,
              email: response.body.result.Email,
              idCard: response.body.result.IdCard,
              companyCode: response.body.result.CompanyCode,
              departments: response.body.result.DepartmentCode,
              roles: response.body.result.Role
            });
          }
        });
    },
    removeUser() {
      this.loading = true;
      this.$http
        .post(this.$urls.user.remove, { userIds: this.selectedRowKeys })
        .then(response => {
          if (response.body.code == 0) {
            this.selectedRowKeys = [];
            this.selectedRows = [];
            this.getData();
          }
          this.loading = false;
        });
    },
    deleteUser() {
      this.loading = true;
      this.$http
        .post(this.$urls.user.delete, { userIds: this.selectedRowKeys })
        .then(response => {
          if (response.body.code == 0) {
            this.selectedRowKeys = [];
            this.selectedRows = [];
            this.getData();
          }
          this.loading = false;
        });
    },
    restoreUser() {
      this.loading = true;
      this.$http
        .post(this.$urls.user.restore, { userIds: this.selectedRowKeys })
        .then(response => {
          if (response.body.code == 0) {
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
          title: "删除时间",
          dataIndex: "DeleteTime.$date",
          width: "15%",
          customRender: val => {
            return this.$common.parseBsonTime(val);
          }
        });
      } else {
        this.columns.splice(len - 1, 1, {
          title: "创建时间",
          dataIndex: "CreateTime.$date",
          width: "15%",
          customRender: val => {
            return this.$common.parseBsonTime(val);
          }
        });
      }
    },
    getData() {
      this.addTableColumns();
      this.loading = true;
      this.$http
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
          pagination.total = response.body.count;
          pagination.showTotal=()=>{return this.pagination.total;};
          this.pagination = pagination;
          if (response.body.code == 0) this.data = response.body.result;
        });
    },
    getCompanyData() {
      this.$http.get(this.$urls.company.getall).then(response => {
        if (response.body.code == 0) {
          this.companyData = response.body.result;
        }
      });
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
    getRoleData() {
      this.$http.get(this.$urls.role.getall).then(response => {
        if (response.body.code == 0) {
          this.roleData = response.body.result;
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
.ant-drawer-body .ant-row {
  margin-bottom: 20px !important;
}
</style>