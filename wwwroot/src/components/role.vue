<template>
  <div>
    <a-input-search
      placeholder="input search text"
      style="width: 200px"
      @search="onSearch"
      v-model="searchValue"
    />
    <a-button type="primary" icon="plus" @click="drawerVisible=true;isUpdate=false"></a-button>
    <a-button type="default" icon="redo" @click="reload"></a-button>
    <a-button type="default" icon="edit" @click="eidtRole" :disabled="selectedRowKeys.length!=1"></a-button>
    <a-button type="danger" icon="delete" @click="deleteRole" :disabled="selectedRowKeys.length==0"></a-button>

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
      :title="isUpdate?'更新角色':'添加角色'"
      :width="360"
      @close="drawerVisible=false"
      :visible="drawerVisible"
    >
      <a-form :form="form" layout="vertical" @submit.prevent="handleSubmit">
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item label="Name">
              <a-input
                placeholder="角色名称"
                v-decorator="['name',{rules: [{ required: true, message: 'Name is required!' }]}]"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item label="Description">
              <a-textarea
                placeholder="角色描述"
                :autosize="{ minRows: 4, maxRows: 6 }"
                v-decorator="['description',{rules: [{ required: true, message: 'Description is required!' }]}]"
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
  name: "role",
  data() {
    return {
      data: [],
      searchValue: "",
      columns: [
        {
          title: "编号",
          dataIndex: "_id",
          width: "7%"
        },
        {
          title: "角色名称",
          dataIndex: "Name",
          width: "13%"
        },
        {
          title: "角色描述",
          dataIndex: "Description",
          width: "50%"
        },
        {
          title: "修改时间",
          dataIndex: "UpdateTime.$date",
          width: "15%",
          customRender: val => {
            return this.$common.parseBsonTime(val);
          }
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
      selectedRowKeys: [],
      form: this.$form.createForm(this),
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
    getData() {
      this.loading = true;
      this.$http
        .get(
          this.$urls.role.getlist +
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
    eidtRole() {
      this.isUpdate = true;
      this.drawerVisible = true;
      this.$http
        .get(this.$urls.role.getById + "/" + this.selectedRowKeys[0])
        .then(response => {
          if (response.body.code == 0) {
            this.form.setFieldsValue({
              name: response.body.result.Name,
              description: response.body.result.Description
            });
          }
        });
    },
    deleteRole() {
      var that = this;
      that.$confirm({
        title: "确定删除角色?",
        content: this.selectedRowKeys.join(","),
        okText: "Yes",
        okType: "danger",
        cancelText: "No",
        onOk() {
          that.loading = true;
          that.$http
            .post(that.$urls.role.delete, { ids: that.selectedRowKeys })
            .then(response => {
              if (response.body.code == 0) {
                that.loading = false;
                that.selectedRowKeys = [];
                that.getData();
              }
            });
        }
      });
    },
    reload() {
      this.selectedRowKeys = [];
      this.getData();
    },
    onSelectChange(selectedRowKeys) {
      this.selectedRowKeys = selectedRowKeys;
    },
    handleTableChange(pagination) {
      this.pagination.current = pagination.current;
      this.getData();
    },
    addRole(role) {
      this.$http.post(this.$urls.role.add, role).then(response => {
        if (response.body.code == 400) {
          this.$message.warning("记录已存在!");
        }
        if (response.body.code == 0) {
          this.form.resetFields();
          this.getData();
        }
      });
    },
    updateRole(role) {
      role.id = this.selectedRowKeys[0];
      this.$http.post(this.$urls.role.update, role).then(response => {
        if (response.body.code == 400) {
          this.$message.warning("记录已存在!");
        }
        if (response.body.code == 0) {
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