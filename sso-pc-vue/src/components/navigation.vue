<template>
  <div>
    <a-input-search
      :placeholder="this.$lang.search"
      style="width: 200px"
      @search="onSearch"
      v-model="searchValue"
    />
    <a-button type="primary" icon="plus" :title="this.$lang.add" @click="showDrawer"></a-button>
    <a-button type="default" icon="redo" :title="this.$lang.refresh" @click="reload"></a-button>
    <a-button
      type="default"
      icon="edit"
      :title="this.$lang.edit"
      @click="eidtNavigation"
      :disabled="selectedRowKeys.length!=1"
    ></a-button>
    <a-popconfirm
      :title="this.$lang.sure_delete_navigation"
      @confirm="deleteNavigation"
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
      :title="isUpdate?this.$lang.update_navigation:this.$lang.add_navigation"
      :width="360"
      @close="drawerVisible=false"
      :visible="drawerVisible"
    >
      <a-form :form="form" layout="vertical" @submit.prevent="handleSubmit">
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item :label="this.$lang.navigation_name">
              <a-input
                v-decorator="['title',{rules: [{ required: true, message: this.$lang.navigation_name_required}]}]"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item :label="this.$lang.link">
              <a-input
                v-decorator="['baseUrl',{rules: [{ required: false, message: this.$lang.link_required }]}]"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item :label="this.$lang.logo">
              <a-input
                v-decorator="['logoUrl',{rules: [{ required: false, message: this.$lang.link_required }]}]"
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
  name: "navigation",
  mixins: [base],
  data() {
    return {
      columns: [
        {
          title: this.$lang.id,
          dataIndex: "Id",
          width: "5%",
          ellipsis: true
        },
        {
          title: this.$lang.name,
          dataIndex: "Title",
          width: "10%",
          ellipsis: true
        },
        {
          title: this.$lang.link,
          dataIndex: "BaseUrl",
          width: "20%",
          ellipsis: true
        },
        {
          title: this.$lang.logo,
          dataIndex: "LogoUrl",
          width: "25%",
          ellipsis: true
        },
        {
          title: this.$lang.update_time,
          dataIndex: "UpdateTime",
          width: "20%",
          ellipsis: true,
          customRender: val => {
            return this.$funtools.parseIsoDateTime(val);
          }
        },
        {
          title: this.$lang.create_time,
          dataIndex: "CreateTime",
          width: "20%",
          ellipsis: true,
          customRender: val => {
            return this.$funtools.parseIsoDateTime(val);
          }
        }
      ],
      getlist:this.$urls.navigation.getlist
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
    eidtNavigation() {
      this.isUpdate = true;
      this.drawerVisible = true;
      this.$axios
        .get(this.$urls.navigation.getbyid + "/" + this.selectedRowKeys[0])
        .then(response => {
          if (response.code == 0) {
            this.form.setFieldsValue({
              title: response.result.Title,
              baseUrl: response.result.BaseUrl,
              logoUrl: response.result.LogoUrl
            });
          }
        });
    },
    deleteNavigation() {
      this.loading = true;
      this.$axios
        .post(this.$urls.navigation.delete, { ids: this.selectedRowKeys })
        .then(response => {
          if (response.code == 0) {
            this.selectedRowKeys = [];
            this.getData();
          }
          this.loading = false;
        });
    },
    handleSubmit() {
      var that = this;
      that.form.validateFields((err, values) => {
        if (!err) {
          if (that.isUpdate) that.updateNavigation(values);
          if (!that.isUpdate) that.addNavigation(values);
        }
      });
    },
    updateNavigation(navigation) {
      navigation.id = this.selectedRowKeys[0];
      this.$axios
        .post(this.$urls.navigation.update, navigation)
        .then(response => {
          if (response.code == 400) {
            this.$message.warning(this.$lang.record_exists);
          }
          if (response.code == 0) {
            this.form.resetFields();
            this.getData();
          }
        });
    },
    addNavigation(navigation) {
      this.$axios.post(this.$urls.navigation.add, navigation).then(response => {
        if (response.code == 400) {
          this.$message.warning(this.$lang.record_exists);
        }
        if (response.code == 0) {
          this.form.resetFields();
          this.getData();
        }
      });
    }
  }
};
</script>

<style scoped>
</style>
