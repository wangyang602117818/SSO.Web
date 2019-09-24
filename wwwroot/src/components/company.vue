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
    <a-button type="default" icon="edit" @click="eidtCompany" :disabled="selectedRowKeys.length!=1"></a-button>
    <a-popconfirm
      title="Are you sure delete this company?"
      @confirm="deleteCompany"
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
      :title="isUpdate?'更新公司':'添加公司'"
      :width="360"
      handle="slot"
      @close="drawerVisible=false"
      :visible="drawerVisible"
    >
      <a-form :form="form" layout="vertical" @submit.prevent="handleSubmit">
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item label="Code">
              <a-input
                placeholder="公司编号"
                v-decorator="['code',{rules: [{ required: true, message: 'Code is required!' }]}]"
              >
                <a-icon slot="addonAfter" type="reload" @click="getRandomCode" />
              </a-input>
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item label="Name">
              <a-input
                placeholder="公司名称"
                v-decorator="['name',{rules: [{ required: true, message: 'Name is required!' }]}]"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item label="Description">
              <a-textarea
                placeholder="公司描述"
                :autosize="{ minRows: 4, maxRows: 6 }"
                v-decorator="['description',{rules: [{ required: false, message: 'Description is required!' }]}]"
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
  name: "company",
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
          title: "公司编号",
          dataIndex: "Code",
          width: "10%"
        },
        {
          title: "公司名称",
          dataIndex: "Name",
          width: "13%"
        },
        {
          title: "公司描述",
          dataIndex: "Description",
          width: "40%"
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
      pagination: { current: 1, pageSize: 10 },
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
    showDrawer() {
      this.drawerVisible = true;
      this.isUpdate = false;
      this.form.setFieldsValue({ code: this.$common.randomWord(12, 12) });
    },
    getRandomCode() {
      this.form.setFieldsValue({ code: this.$common.randomWord(12, 12) });
    },
    getData() {
      this.loading = true;
      this.$http
        .get(
          this.$urls.company.getlist +
            "?pageIndex=" +
            this.pagination.current +
            "&pageSize=" +
            this.pagination.pageSize +
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
    eidtCompany() {
      this.isUpdate = true;
      this.drawerVisible = true;
      this.$http
        .get(this.$urls.company.getById + "/" + this.selectedRowKeys[0])
        .then(response => {
          if (response.body.code == 0) {
            this.form.setFieldsValue({
              code: response.body.result.Code,
              name: response.body.result.Name,
              description: response.body.result.Description
            });
          }
        });
    },
    deleteCompany() {
      this.loading = true;
      this.$http
        .post(this.$urls.company.delete, { ids: this.selectedRowKeys })
        .then(response => {
          if (response.body.code == 0) {
            this.selectedRowKeys = [];
            this.getData();
          }
          this.loading = false;
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
    addCompany(company) {
      this.$http.post(this.$urls.company.add, company).then(response => {
        if (response.body.code == 400) {
          this.$message.warning("记录已存在!");
        }
        if (response.body.code == 0) {
          this.form.resetFields();
          this.getData();
        }
      });
    },
    updateCompany(company) {
      company.id = this.selectedRowKeys[0];
      this.$http.post(this.$urls.company.update, company).then(response => {
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
          if (that.isUpdate) that.updateCompany(values);
          if (!that.isUpdate) that.addCompany(values);
        }
      });
    }
  }
};
</script>
<style scoped>
.anticon-reload {
  cursor: pointer;
}
.anticon-reload:hover {
  color: #000;
}
</style>