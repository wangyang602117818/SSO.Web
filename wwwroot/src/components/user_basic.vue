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
      :width="360"
      handle="slot"
      @close="drawerVisible=false"
      :visible="drawerVisible"
    >
      <a-form :form="form" layout="vertical" @submit.prevent="handleSubmit">
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item label="UserId">
              <a-input
                placeholder="用户编号"
                v-decorator="['userId',{rules: [{ required: true, message: 'UserId is required!' }]}]"
              >
              </a-input>
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item label="UserName">
              <a-input
                placeholder="用户名称"
                v-decorator="['userName',{rules: [{ required: true, message: 'UserName is required!' }]}]"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item label="Mobile">
              <a-input
                placeholder="手机号"
                v-decorator="['mobile',{rules: [{ required: false, message: 'Mobile is required!' }]}]"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item label="Email">
              <a-input
                placeholder="邮箱"
                v-decorator="['email',{rules: [{ required: false, message: 'Email is required!' }]}]"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item label="IdCard">
              <a-input
                placeholder="身份证号"
                v-decorator="['idCard',{rules: [{ required: false, message: 'IdCard is required!' }]}]"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-divider />
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item label="loginId">
              <a-input
                placeholder="登录名称"
                v-decorator="['LoginId',{rules: [{ required: false, message: 'LoginId is required!' }]}]"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item label="password">
              <a-input
                placeholder="登录密码"
                v-decorator="['Password',{rules: [{ required: false, message: 'Password is required!' }]}]"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item label="company">
              <a-input
                placeholder="所属公司"
                v-decorator="['Company',{rules: [{ required: false, message: 'Company is required!' }]}]"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item label="department">
              <a-input
                placeholder="所属部门"
                v-decorator="['Department',{rules: [{ required: false, message: 'Department is required!' }]}]"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item label="role">
              <a-input
                placeholder="角色"
                v-decorator="['Role',{rules: [{ required: false, message: 'role is required!' }]}]"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col :span="24">
            <a-button @click="form.resetFields();">取 消</a-button>
            <a-button type="primary" html-type="submit">确 定</a-button>
          </a-col>
        </a-row>
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
      form: this.$form.createForm(this),
      columns: [
        {
          title: "用户编号",
          dataIndex: "id"
        },
        {
          title: "用户名称",
          dataIndex: "name"
        },
        {
          title: "手机号",
          dataIndex: "mobile"
        },
        {
          title: "邮箱",
          dataIndex: "email"
        },
        {
          title: "照片",
          dataIndex: "pic"
        },
        {
          title: "创建时间",
          dataIndex: "createTime"
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
    handleSubmit(){

    },
    eidtUser() {},
    deleteUser() {},
    getData() {},
    onSelectChange(selectedRowKeys) {
      this.selectedRowKeys = selectedRowKeys;
    },
    handleTableChange(pagination) {
      this.pagination.current = pagination.current;
      this.getData();
    },
    showDrawer() {
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