<template>
  <div>
    <a-input-search
      placeholder="input search text"
      style="width: 200px"
      @search="onSearch"
      v-model="searchValue"
    />
    <a-button type="primary" icon="plus" @click="showDrawer"></a-button>
    <a-button type="default" icon="redo" @click="reload"></a-button>
    <a-button type="default" icon="edit" @click="eidtUser" :disabled="selectedRowKeys.length!=1"></a-button>
    <a-popconfirm
      title="Are you sure delete this user?"
      @confirm="deleteUser"
      okText="Yes"
      cancelText="No"
    >
      <a-button type="danger" icon="delete" :disabled="selectedRowKeys.length==0"></a-button>
    </a-popconfirm>
    <a-table
      :columns="columns"
      :rowKey="record => record._id"
      :dataSource="data"
      :rowSelection="{selectedRowKeys:selectedRowKeys,onChange:onSelectChange}"
      :loading="loading"
      :pagination="pagination"
      @change="handleTableChange"
    />
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
            v-decorator="['password',{rules: [{ required: true, message: 'Password is required!' }]}]"
          />
        </a-form-item>
        <a-form-item label="Mobile" :label-col="{ span: 6 }" :wrapper-col="{ span: 11 }">
          <a-input
            placeholder="手机号"
            v-decorator="['mobile',{rules: [{ required: false, message: 'Mobile is required!' }]}]"
          />
        </a-form-item>
        <a-form-item label="Email" :label-col="{ span: 6 }" :wrapper-col="{ span: 13 }">
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

        <a-button @click="form.resetFields();">取 消</a-button>
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
      companyData: [],
      departmentData: [],
      roleData: [],
      form: this.$form.createForm(this),
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
          width: "10%"
        },
        {
          title: "手机号",
          dataIndex: "Mobile",
          width: "10%"
        },
        {
          title: "邮箱",
          dataIndex: "Email",
          width: "13%"
        },
        {
          title: "证件号",
          dataIndex: "IdCard",
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
          dataIndex: "CompanyCode",
          width: "5%"
        },
        {
          title: "部门",
          dataIndex: "",
          width: "10%"
        },
        {
          title: "角色",
          dataIndex: "",
          width: "10%"
        },
        {
          title: "创建时间",
          dataIndex: "CreateTime.$date",
          width: "12%",
          customRender: val => {
            return this.$common.parseBsonTime(val);
          }
        }
      ],
      rowSelection: {
        onChange: (selectedRowKeys, selectedRows) => {
          window.console.log(
            `selectedRowKeys: ${selectedRowKeys}`,
            "selectedRows: ",
            selectedRows
          );
        },
        onSelect: (record, selected, selectedRows) => {
          window.console.log(record, selected, selectedRows);
        },
        onSelectAll: (selected, selectedRows, changeRows) => {
          window.console.log(selected, selectedRows, changeRows);
        }
      },
      drawerVisible: false,
      pagination: { current: 1 },
      loading: false,
      isUpdate: false
    };
  },
  created() {
    this.getData();
  },
  methods: {
    onSearch() {
      this.pagination.current = 1;
      this.selectedRowKeys = [];
      this.getData();
    },
    reload() {
      this.selectedRowKeys = [];
      this.getData();
    },
    changeCompany(value) {
      this.form.setFieldsValue({ departments: "" });
      this.getDepartmentData(value);
    },
    handleSubmit() {
      this.form.validateFields((err, values) => {
        if (!err) {
          this.$http.post(this.$urls.user.add, values).then(response => {
            if (response.body.code == 400) {
              this.$message.warning("记录已存在!");
            }
            if (response.body.code == 0) {
              this.getData();
            }
          });
        }
      });
    },
    eidtUser() {},
    deleteUser() {},
    getData() {
      this.loading = true;
      this.$http
        .get(
          this.$urls.user.getbasic +
            "?pageIndex=" +
            this.pagination.current +
            "&filter=" +
            this.searchValue
        )
        .then(response => {
          this.loading = false;
          const pagination = { ...this.pagination };
          pagination.total = response.body.count;
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
    onSelectChange(selectedRowKeys) {
      this.selectedRowKeys = selectedRowKeys;
    },
    handleTableChange(pagination) {
      this.pagination.current = pagination.current;
      this.getData();
    },
    showDrawer() {
      this.getCompanyData();
      this.getRoleData();
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