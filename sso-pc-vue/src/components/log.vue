<template>
  <div>
    <a-select :default-value="from" v-model="from" style="width: 150px" @change="fromChange">
      <a-select-option value>{{this.$lang.all}}</a-select-option>
      <a-select-option v-for="item in fromlist" :value="item.from" :key="item.from">{{item.from}}</a-select-option>
    </a-select>&nbsp;
    <a-input
      placeholder="controller"
      style="width: 120px"
      v-on:keyup.enter="onSearch"
      v-model="controllerName"
    />&nbsp;
    <a-input
      placeholder="action"
      style="width: 120px"
      v-on:keyup.enter="onSearch"
      v-model="actionName"
    />&nbsp;
    <a-input
      :placeholder="this.$lang.us"
      style="width: 100px"
      v-on:keyup.enter="onSearch"
      v-model="userName"
    />&nbsp;
    <a-date-picker
      valueFormat="YYYY-MM-DD"
      :value="startTime"
      @change="onStartChange"
      :placeholder="this.$lang.start_time"
      style="width: 120px"
    />&nbsp;
    <a-date-picker
      valueFormat="YYYY-MM-DD"
      :value="endTime"
      @change="onEndChange"
      :placeholder="this.$lang.end_time"
      style="width: 120px"
    />&nbsp;
    <a-button @click="handleReset">Clear</a-button>
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
          dataIndex: "UserName",
          width: "10%"
        },
        {
          title: this.$lang.ip,
          dataIndex: "UserHost",
          width: "9%",
          ellipsis: true
        },
        {
          title: this.$lang.agent,
          dataIndex: "UserAgent",
          width: "6%",
          ellipsis: true,
          customRender: val => {
            return this.$funtools.getDeviceType(val);
          }
        },
        {
          title: this.$lang.create_time,
          dataIndex: "CreateTime",
          width: "13%",
          ellipsis: true,
          customRender: val => {
            return this.$funtools.parseIsoDateTime(val);
          }
        }
      ],
      fromlist: [],
      from: "",
      controllerName: "",
      actionName: "",
      startTime: null,
      endTime: null,
      userName: "",
      getlist: this.$urls.log.getlist
    };
  },
  mounted() {
    this.getFromList();
  },
  methods: {
    handleReset() {
      this.from = "";
      this.controllerName = "";
      this.actionName = "";
      this.startTime = null;
      this.endTime = null;
      this.userName = "";
      this.getData();
    },
    fromChange(item) {
      this.from = item;
      this.getData();
    },
    onStartChange(date, dateString) {
      this.startTime = dateString;
      this.getData();
    },
    onEndChange(date, dateString) {
      this.endTime = dateString;
      this.getData();
    },
    getQuerystring() {
      var url =
        "?pageIndex=" +
        this.pagination.current +
        "&pageSize=" +
        this.pagination.pageSize;
      if (this.from) url += "&from=" + this.from;
      if (this.controllerName) url += "&controllerName=" + this.controllerName;
      if (this.actionName) url += "&actionName=" + this.actionName;
      if (this.userName) url += "&userName=" + this.userName;
      if (this.startTime) url += "&startTime=" + this.startTime;
      if (this.endTime) url += "&endTime=" + this.endTime;
      return url;
    },
    getFromList() {
      this.$axios.get(this.$urls.log.getfromlist).then(response => {
        if (response.code === 0) this.fromlist = response.result;
      });
    }
  }
};
</script>
<style scoped>
.ant-select-enabled,
.ant-input {
  margin: 10px 0px;
}
</style>