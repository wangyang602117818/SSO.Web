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
      :rowKey="record => record._id"
      :dataSource="data"
      :rowSelection="{selectedRowKeys:selectedRowKeys,onChange:onSelectChange}"
      :loading="loading"
      :pagination="pagination"
      @change="handleTableChange"
    />
    <a-drawer
      :title="isUpdate?this.$lang.update_navigation:this.$lang.add_navigation"
      :width="360"
      handle="slot"
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
          dataIndex: "_id",
          width: "5%"
        },
        {
          title: this.$lang.name,
          dataIndex: "Title",
          width: "10%"
        },
        {
          title: this.$lang.link,
          dataIndex: "BaseUrl",
          width: "20%"
        },
        {
          title: this.$lang.logo,
          dataIndex: "LogoUrl",
          width: "25%"
        },
        {
          title: this.$lang.update_time,
          dataIndex: "UpdateTime.$date",
          width: "20%",
          customRender: val => {
            return this.$funtools.parseBsonTime(val);
          }
        },
        {
          title: this.$lang.create_time,
          dataIndex: "CreateTime.$date",
          width: "20%",
          customRender: val => {
            return this.$funtools.parseBsonTime(val);
          }
        }
      ],
      getlist:this.$urls.navigation.getlist
    };
  },
  methods: {
    eidtNavigation() {
      this.isUpdate = true;
      this.drawerVisible = true;
      this.$http
        .get(this.$urls.navigation.getbyid + "/" + this.selectedRowKeys[0])
        .then(response => {
          if (response.body.code == 0) {
            this.form.setFieldsValue({
              title: response.body.result.Title,
              baseUrl: response.body.result.BaseUrl,
              logoUrl: response.body.result.LogoUrl
            });
          }
        });
    },
    deleteNavigation() {
      this.loading = true;
      this.$http
        .post(this.$urls.navigation.delete, { ids: this.selectedRowKeys })
        .then(response => {
          if (response.body.code == 0) {
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
      this.$http
        .post(this.$urls.navigation.update, navigation)
        .then(response => {
          if (response.body.code == 400) {
            this.$message.warning(this.$lang.record_exists);
          }
          if (response.body.code == 0) {
            this.form.resetFields();
            this.getData();
          }
        });
    },
    addNavigation(navigation) {
      this.$http.post(this.$urls.navigation.add, navigation).then(response => {
        if (response.body.code == 400) {
          this.$message.warning(this.$lang.record_exists);
        }
        if (response.body.code == 0) {
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
