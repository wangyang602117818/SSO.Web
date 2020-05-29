<template>
  <div>
    <a-input-search
      :placeholder="this.$lang.search"
      style="width: 200px"
      @search="onSearch"
      v-model="searchValue"
    />
    <a-button
      type="primary"
      icon="plus"
      :title="this.$lang.add"
      @click="drawerVisible=true;isUpdate=false"
    ></a-button>
    <a-button type="default" icon="redo" :title="this.$lang.refresh" @click="reload"></a-button>
    <a-button
      type="default"
      icon="edit"
      :title="this.$lang.edit"
      @click="eidtRole"
      :disabled="selectedRowKeys.length!=1"
    ></a-button>
    <a-popconfirm
      :title="this.$lang.sure_delete_role"
      @confirm="deleteRole"
      :okText="this.$lang.yes"
      :cancelText="this.$lang.no"
    >
      <a-button
        type="danger"
        icon="delete"
        :title="this.$lang.delete"
        :disabled="selectedRowKeys.length==0"
      ></a-button>
    </a-popconfirm>
    <a-table
      :columns="columns"
      :rowKey="record => record.Id"
      :dataSource="data"
      :rowSelection="{selectedRowKeys:selectedRowKeys,onChange:onSelectChange}"
      :loading="loading"
      :pagination="pagination"
      @change="handleTableChange"
    />
    <a-drawer
      :title="isUpdate?this.$lang.update_role:this.$lang.add_role"
      :width="360"
      handle="slot"
      @close="drawerVisible=false"
      :visible="drawerVisible"
    >
      <a-form :form="form" layout="vertical" @submit.prevent="handleSubmit">
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item :label="this.$lang.name">
              <a-input
                :placeholder="this.$lang.role_name"
                v-decorator="['name',{rules: [{ required: true, message: this.$lang.name_required }]}]"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item :label="this.$lang.description">
              <a-textarea
                :placeholder="this.$lang.role_description"
                :autoSize="{ minRows: 4, maxRows: 6 }"
                v-decorator="['description',{rules: [{ required: true, message: this.$lang.description_required }]}]"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col :span="24">
            <a-button @click="form.resetFields();">{{this.$lang.reset}}</a-button>
            <a-button type="primary" html-type="submit">{{this.$lang.submit}}</a-button>
          </a-col>
        </a-row>
      </a-form>
    </a-drawer>
  </div>
</template>
<script>
import base from "./Base";
export default {
  name: "role",
  mixins: [base],
  data() {
    return {
      columns: [
        {
          title: this.$lang.id,
          dataIndex: "Id",
          width: "7%"
        },
        {
          title: this.$lang.role_name,
          dataIndex: "Name",
          width: "13%"
        },
        {
          title: this.$lang.role_description,
          dataIndex: "Description",
          width: "50%"
        },
        {
          title: this.$lang.update_time,
          dataIndex: "UpdateTime",
          width: "15%",
          customRender: val => {
            return this.$funtools.parseIsoDateTime(val);
          }
        },
        {
          title: this.$lang.create_time,
          dataIndex: "CreateTime",
          width: "15%",
          customRender: val => {
            return this.$funtools.parseIsoDateTime(val);
          }
        }
      ],
      getlist: this.$urls.role.getlist
    };
  },
  methods: {
    eidtRole() {
      this.isUpdate = true;
      this.drawerVisible = true;
      this.$axios
        .get(this.$urls.role.getById + "/" + this.selectedRowKeys[0])
        .then(response => {
          if (response.code == 0) {
            this.form.setFieldsValue({
              name: response.result.Name,
              description: response.result.Description
            });
          }
        });
    },
    deleteRole() {
      this.loading = true;
      this.$axios
        .post(this.$urls.role.delete, { ids: this.selectedRowKeys })
        .then(response => {
          if (response.code == 0) {
            this.selectedRowKeys = [];
            this.getData();
          }
        });
      this.loading = false;
    },
    addRole(role) {
      this.$axios.post(this.$urls.role.add, role).then(response => {
        if (response.code == 400) {
          this.$message.warning(this.$lang.record_exists);
        }
        if (response.code == 0) {
          this.form.resetFields();
          this.getData();
        }
      });
    },
    updateRole(role) {
      role.id = this.selectedRowKeys[0];
      this.$axios.post(this.$urls.role.update, role).then(response => {
        if (response.code == 400) {
          this.$message.warning(this.$lang.record_exists);
        }
        if (response.code == 0) {
          this.form.resetFields();
          this.getData();
        }
      });
    },
    handleSubmit() {
      var that = this;
      that.form.validateFields((err, values) => {
        if (!err) {
          if (that.isUpdate) that.updateRole(values);
          if (!that.isUpdate) that.addRole(values);
        }
      });
    }
  }
};
</script>
<style>
</style>