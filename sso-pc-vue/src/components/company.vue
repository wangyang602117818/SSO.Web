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
      @click="eidtCompany"
      :disabled="selectedRowKeys.length!=1"
    ></a-button>
    <a-popconfirm
      :title="this.$lang.sure_delete_company"
      @confirm="deleteCompany"
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
      :title="isUpdate?this.$lang.update_company:this.$lang.add_company"
      :width="360"
      @close="drawerVisible=false"
      :visible="drawerVisible"
    >
      <a-form :form="form" layout="vertical" @submit.prevent="handleSubmit">
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item :label="this.$lang.company_code">
              <a-input
                v-decorator="['code',{rules: [{ required: true, message:this.$lang.company_code_required }]}]"
              >
                <a-icon slot="addonAfter" type="reload" @click="getRandomCode" />
              </a-input>
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item :label="this.$lang.company_name">
              <a-input
                v-decorator="['name',{rules: [{ required: true, message: this.$lang.company_name_required}]}]"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item :label="this.$lang.company_description">
              <a-textarea
                :autoSize="{ minRows: 4, maxRows: 6 }"
                v-decorator="['description',{rules: [{ required: false, message: this.$lang.company_description_required }]}]"
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
  name: "company",
  mixins: [base],
  data() {
    return {
      columns: [
        {
          title: this.$lang.id,
          dataIndex: "Id",
          width: "7%",
          ellipsis: true
        },
        {
          title: this.$lang.company_code,
          dataIndex: "Code",
          width: "15%",
          ellipsis: true
        },
        {
          title: this.$lang.company_name,
          dataIndex: "Name",
          width: "13%",
          ellipsis: true
        },
        {
          title: this.$lang.company_description,
          dataIndex: "Description",
          width: "35%",
          ellipsis: true
        },
        {
          title: this.$lang.update_time,
          dataIndex: "UpdateTime",
          width: "15%",
          ellipsis: true,
          customRender: val => {
            return this.$funtools.parseIsoDateTime(val);
          }
        },
        {
          title: this.$lang.create_time,
          dataIndex: "CreateTime",
          width: "15%",
          ellipsis: true,
          customRender: val => {
            return this.$funtools.parseIsoDateTime(val);
          }
        }
      ],
      getlist: this.$urls.company.getlist
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
    showDrawer() {
      this.drawerVisible = true;
      this.isUpdate = false;
      this.$nextTick(() => {
        this.getRandomCode();
      });
    },
    getRandomCode() {
      this.form.setFieldsValue({ code: this.$funtools.randomWord(12, 12) });
    },
    eidtCompany() {
      this.isUpdate = true;
      this.drawerVisible = true;
      this.$axios
        .get(this.$urls.company.getById + "/" + this.selectedRowKeys[0])
        .then(response => {
          if (response.code == 0) {
            this.form.setFieldsValue({
              code: response.result.Code,
              name: response.result.Name,
              description: response.result.Description
            });
          }
        });
    },
    deleteCompany() {
      this.loading = true;
      this.$axios
        .post(this.$urls.company.delete, { ids: this.selectedRowKeys })
        .then(response => {
          if (response.code == 0) {
            this.selectedRowKeys = [];
            this.getData();
          }
          this.loading = false;
        });
    },
    addCompany(company) {
      this.$axios.post(this.$urls.company.add, company).then(response => {
        if (response.code == 0) {
          this.form.resetFields();
          this.getData();
          this.getRandomCode();
        }
      });
    },
    updateCompany(company) {
      company.id = this.selectedRowKeys[0];
      this.$axios.post(this.$urls.company.update, company).then(response => {
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

