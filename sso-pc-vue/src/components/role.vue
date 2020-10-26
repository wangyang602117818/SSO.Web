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
                    rules: [{ required: true, message: $t('name_required') }],
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
            <a-button @click="form.resetFields()">{{ $t("reset") }}</a-button>
            <a-button type="primary" html-type="submit">{{
              $t("submit")
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
              :default-checked="rolepermissions.indexOf(item) > -1"
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
import base from "./Base";
export default {
  name: "role",
  mixins: [base],
  data() {
    return {
      columns: [
        {
          title: this.$t("id"),
          dataIndex: "Id",
          width: "7%",
        },
        {
          title: this.$t("role_name"),
          dataIndex: "Name",
          width: "13%",
        },
        {
          title: this.$t("role_description"),
          dataIndex: "Description",
          width: "40%",
        },
        {
          title: this.$t("permission_count"),
          dataIndex: "PermissionCount",
          width: "10%",
        },
        {
          title: this.$t("update_time"),
          dataIndex: "UpdateTime",
          width: "15%",
          customRender: (val) => {
            return this.$funtools.parseIsoDateTime(val);
          },
        },
        {
          title: this.$t("create_time"),
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
      rolepermissions: [],
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
    submitPermission() {
      this.confirmLoading = true;
      this.loading = true;
      this.$axios
        .post(this.$urls.permission.addRolePermission, {
          role: this.selectedRows[0].Name,
          names: this.rolepermissions,
        })
        .then((response) => {
          this.confirmLoading = false;
          this.loading = false;
          if (response.code == 0) {
            this.cancelPermission();
            this.getData();
          }
        });
    },
    cancelPermission() {
      this.permissions = [];
      this.rolepermissions = [];
      this.permissionVisible = false;
    },
    eidtPermission() {
      var that = this;
      this.$axios
        .all([this.getPermissions(), this.getRolePermissions()])
        .then(function (results) {
          if (results[0].code == 0 && results[1].code == 0) {
            that.permissions = results[0].result;
            that.rolepermissions = Array.from(new Set(results[1].result));
          }
          that.permissionVisible = true;
        });
    },
    getPermissions() {
      return this.$axios.get(this.$urls.permission.getlist);
    },
    getRolePermissions() {
      var role = this.selectedRows[0].Name;
      return this.$axios.get(
        this.$urls.permission.getRolePermission + "?role=" + role
      );
    },
    permissionChange(e) {
      var value = e.target.id;
      var index = this.rolepermissions.indexOf(value);
      if (index == -1) {
        this.rolepermissions.push(value);
      } else {
        this.rolepermissions.splice(index, 1);
      }
    },
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
          this.loading = false;
        });
    },
    addRole(role) {
      this.$axios.post(this.$urls.role.add, role).then((response) => {
        if (response.code == 0) {
          this.form.resetFields();
          this.getData();
        }
      });
    },
    updateRole(role) {
      role.id = this.selectedRowKeys[0];
      this.$axios.post(this.$urls.role.update, role).then((response) => {
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
  padding: 4px;
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
.ant-tabs-tabpane .ant-checkbox-wrapper {
  height: 35px;
  line-height: 35px;
  padding: 0 5px;
}
</style>