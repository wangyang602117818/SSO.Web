<template>
  <div>
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
      @click="
        drawerVisible = true;
        isUpdate = false;
      "
    ></a-button>
    <a-button
      type="default"
      icon="redo"
      :title="$t('refresh')"
      @click="reload"
    ></a-button>
    <a-button
      type="default"
      icon="edit"
      :title="$t('edit')"
      @click="eidtRole"
      :disabled="selectedRowKeys.length != 1"
    ></a-button>
    <a-button
      type="default"
      icon="key"
      :title="$t('permission')"
      @click="eidtPermission"
      :disabled="selectedRowKeys.length != 1"
    />
    <a-popconfirm
      :title="$t('sure_delete_role')"
      @confirm="deleteRole"
      :okText="$t('yes')"
      :cancelText="$t('no')"
    >
      <a-button
        type="danger"
        icon="delete"
        :title="$t('delete')"
        :disabled="selectedRowKeys.length == 0"
      ></a-button>
    </a-popconfirm>
    <a-table
      :columns="columns"
      :rowKey="(record) => record.Id"
      :dataSource="data"
      :rowSelection="{
        selectedRowKeys: selectedRowKeys,
        onChange: onSelectChange,
      }"
      :loading="loading"
      :pagination="pagination"
      @change="handleTableChange"
    />
    <a-drawer
      :title="isUpdate ? $t('update_role') : $t('add_role')"
      :width="360"
      @close="drawerVisible = false"
      :visible="drawerVisible"
    >
      <a-form :form="form" layout="vertical" @submit.prevent="handleSubmit">
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item :label="$t('name')">
              <a-input
                :placeholder="$t('role_name')"
                v-decorator="[
                  'name',
                  {
                    rules: [
                      { required: true, message: $t('name_required') },
                    ],
                  },
                ]"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item :label="$t('description')">
              <a-textarea
                :placeholder="$t('role_description')"
                :autoSize="{ minRows: 4, maxRows: 6 }"
                v-decorator="[
                  'description',
                  {
                    rules: [
                      {
                        required: true,
                        message: $t('description_required'),
                      },
                    ],
                  },
                ]"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col :span="24">
            <a-button @click="form.resetFields()">{{
              $t('reset')
            }}</a-button>
            <a-button type="primary" html-type="submit">{{
              $t('submit')
            }}</a-button>
          </a-col>
        </a-row>
      </a-form>
    </a-drawer>
    <a-modal
      :title="$t('role_permission')"
      :visible="permissionVisible"
      :confirm-loading="confirmLoading"
      :okText="$t('submit')"
      :cancelText="$t('cancel')"
      @ok="handleOk"
      @cancel="handleCancel"
    >
      <div class="card-container" v-if="permissions">
        <a-tabs :default-active-key="0" size="small" type="card">
          <a-tab-pane
            :key="index"
            :tab="name"
            v-for="(value, name, index) in permissions"
          >
            <a-checkbox-group @change="onChange">
              <a-checkbox
                v-for="(item, index) in value"
                :key="index"
                :value="item"
                >{{ item }}</a-checkbox
              >
            </a-checkbox-group>
          </a-tab-pane>
        </a-tabs>
      </div>
    </a-modal>
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
          title: this.$t('id'),
          dataIndex: "Id",
          width: "7%",
        },
        {
          title: this.$t('role_name'),
          dataIndex: "Name",
          width: "13%",
        },
        {
          title: this.$t('role_description'),
          dataIndex: "Description",
          width: "50%",
        },
        {
          title: this.$t('update_time'),
          dataIndex: "UpdateTime",
          width: "15%",
          customRender: (val) => {
            return this.$funtools.parseIsoDateTime(val);
          },
        },
        {
          title: this.$t('create_time'),
          dataIndex: "CreateTime",
          width: "15%",
          customRender: (val) => {
            return this.$funtools.parseIsoDateTime(val);
          },
        },
      ],
      permissionVisible: false,
      confirmLoading: false,
      getlist: this.$urls.role.getlist,
      permissions: null,
    };
  },
  methods: {
    getQuerystring() {
      return (
        "?pageIndex=" +
        this.pagination.current +
        "&pageSize=" +
        this.pagination.pageSize +
        "&filter=" +
        this.searchValue
      );
    },
    handleOk() {
      this.confirmLoading = true;
      this.permissionVisible = false;
    },
    handleCancel() {
      this.permissionVisible = false;
    },
    eidtPermission() {
      this.getPermissions();
      this.permissionVisible = true;
    },
    getPermissions() {
      this.$axios.get(this.$urls.permission.getlist).then((response) => {
        if (response.code == 0) {
          this.permissions = response.result;
        }
      });
    },
    onChange() {},
    eidtRole() {
      this.isUpdate = true;
      this.drawerVisible = true;
      this.$axios
        .get(this.$urls.role.getById + "/" + this.selectedRowKeys[0])
        .then((response) => {
          if (response.code == 0) {
            this.form.setFieldsValue({
              name: response.result.Name,
              description: response.result.Description,
            });
          }
        });
    },
    deleteRole() {
      this.loading = true;
      this.$axios
        .post(this.$urls.role.delete, { ids: this.selectedRowKeys })
        .then((response) => {
          if (response.code == 0) {
            this.selectedRowKeys = [];
            this.getData();
          }
        });
      this.loading = false;
    },
    addRole(role) {
      this.$axios.post(this.$urls.role.add, role).then((response) => {
        if (response.code == 400) {
          this.$message.warning(this.$t('record_exists'));
        }
        if (response.code == 0) {
          this.form.resetFields();
          this.getData();
        }
      });
    },
    updateRole(role) {
      role.id = this.selectedRowKeys[0];
      this.$axios.post(this.$urls.role.update, role).then((response) => {
        if (response.code == 400) {
          this.$message.warning(this.$t('record_exists'));
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
    },
  },
};
</script>
<style>
.ant-modal-body {
  padding: 0px;
}

.card-container {
  background: #f5f5f5;
  overflow: hidden;
}
.card-container > .ant-tabs-card > .ant-tabs-content {
  height: 100%;
  margin-top: -16px;
}

.card-container > .ant-tabs-card > .ant-tabs-content > .ant-tabs-tabpane {
  background: #fff;
  padding: 8px;
}

.card-container > .ant-tabs-card > .ant-tabs-bar {
  border-color: #fff;
}

.card-container > .ant-tabs-card > .ant-tabs-bar .ant-tabs-tab {
  border-color: transparent;
  background: transparent;
}

.card-container > .ant-tabs-card > .ant-tabs-bar .ant-tabs-tab-active {
  border-color: #fff;
  background: #fff;
}
.ant-checkbox-wrapper + .ant-checkbox-wrapper {
  margin-left: 0px;
}
.ant-checkbox-group .ant-checkbox-wrapper {
  height: 30px;
  line-height: 30px;
}
</style>