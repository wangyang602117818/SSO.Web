<template>
  <div>
    <a-input-search placeholder="input search text" style="width: 200px" @search="onSearch" />
    <a-button type="primary" icon="plus" @click="drawerVisible=true"></a-button>
    <a-button type="default" icon="edit" @click="eidtRole" :disabled="selectedRowKeys.length!=1"></a-button>
    <a-button type="danger" icon="delete" :disabled="selectedRowKeys.length==0"></a-button>
    <a-table
      :columns="columns"
      :rowKey="record => record._id"
      :dataSource="data"
      :rowSelection="{selectedRowKeys:selectedRowKeys,onChange:onSelectChange}"
      :loading="loading"
      :pagination="pagination"
      @change="handleTableChange"
    />
    <a-drawer title="添加角色" :width="360" @close="drawerVisible=false" :visible="drawerVisible">
      <a-form :form="form" layout="vertical" hideRequiredMark @submit.prevent="handleSubmit">
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
            <a-button @click="drawerVisible=false">取 消</a-button>
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
      columns: [
        {
          title: "编号",
          dataIndex: "_id"
        },
        {
          title: "角色名称",
          dataIndex: "Name"
        },
        {
          title: "角色描述",
          dataIndex: "Description"
        },
        {
          title: "修改时间",
          dataIndex: "UpdateTime.$date"
        },
        {
          title: "创建时间",
          dataIndex: "CreateTime.$date"
        }
      ],
      selectedRowKeys: [],
      form: this.$form.createForm(this),
      drawerVisible: false,
      pagination: {},
      loading: false
    };
  },
  created() {
    this.getData();
  },
  methods: {
    onSearch() {},
    getData() {
      this.loading = true;
      this.$http.get("role/getlist").then(response => {
        this.loading = false;
        this.pagination.total = response.body.count;
        if (response.body.code == 0) this.data = response.body.result;
      });
    },
    eidtRole() {
      window.console.log("edit");
    },
    onSelectChange(selectedRowKeys) {
      this.selectedRowKeys = selectedRowKeys;
    },
    handleTableChange(pagination) {
      window.console.log(pagination);
    },
    handleSubmit() {
      this.form.validateFields((err, values) => {
        if (!err) {
          this.$http.post("role/add", values).then(response => {
            if (response.body.code == 0) this.getData();
          });
        }
      });
    }
  }
};
</script>
<style>
</style>