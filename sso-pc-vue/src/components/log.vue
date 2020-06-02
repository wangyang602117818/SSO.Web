<template>
  <div>
    <a-input-search
      :placeholder="this.$lang.search"
      style="width: 200px"
      @search="onSearch"
      v-model="searchValue"
    />
    <a-button type="default" icon="redo" @click="reload"></a-button>
    <a-table
      :columns="columns"
      :rowKey="record => record._id"
      :dataSource="data"
      :loading="loading"
      :pagination="pagination"
      @change="handleTableChange"
    >
      <span slot="RoleName" slot-scope="RoleName" v-if="RoleName">
        <a-tag v-for="tag in RoleName.split(',')" :key="tag">{{tag}}</a-tag>
      </span>
      <span slot="path" slot-scope="text, record">{{record.Controller+"/"+record.Action}}</span>
    </a-table>
  </div>
</template>
<script>
import base from "./Base";
export default {
  name: "log",
  mixins: [base],
  data() {
    return {
      columns: [
        {
          title: this.$lang.source,
          dataIndex: "From",
          width: "12%",
          ellipsis: true
        },
        {
          title: "controller",
          dataIndex: "Controller",
          width: "10%",
          ellipsis: true
        },
        {
          title: "action",
          dataIndex: "Action",
          width: "10%",
          ellipsis: true
        },
        {
          title: this.$lang.query,
          dataIndex: "QueryString",
          width: "15%",
          ellipsis: true
        },
        {
          title: this.$lang.content,
          dataIndex: "Content",
          width: "15%",
          ellipsis: true
        },
        {
          title: this.$lang.us,
          dataIndex: "UserId",
          width: "8"
        },
        {
          title: this.$lang.ip,
          dataIndex: "UserHost",
          width: "10%",
          ellipsis: true
        },
        {
          title: this.$lang.agent,
          dataIndex: "UserAgent",
          width: "5%",
          ellipsis: true,
          customRender: val => {
            return this.$funtools.getDeviceType(val);
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
      getlist: this.$urls.log.getlist
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
    }
  }
};
</script>
<style scoped>
</style>