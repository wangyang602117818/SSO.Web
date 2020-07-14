<template>
  <div>
    <a-select :default-value="from" v-model="from" style="width: 130px" @change="fromChange">
      <a-select-option value>{{this.$lang.source}}</a-select-option>
      <a-select-option v-for="item in fromlist" :value="item.from" :key="item.from">{{item.from}}</a-select-option>
    </a-select>&nbsp;
    <a-select
      :default-value="from"
      v-model="controllerName"
      style="width: 120px"
      @change="controllerChange"
    >
      <a-select-option value>controller</a-select-option>
      <a-select-option
        v-for="item in controllers"
        :value="item.controller"
        :key="item.controller"
      >{{item.controller}}</a-select-option>
    </a-select>&nbsp;
    <a-select
      :default-value="from"
      v-model="actionName"
      style="width: 120px"
      @change="actionChange"
    >
      <a-select-option value>action</a-select-option>
      <a-select-option
        v-for="item in actions"
        :value="item.action"
        :key="item.action"
      >{{item.action}}</a-select-option>
    </a-select>&nbsp;
    <a-select v-model="orderBy" style="width: 100px">
      <a-select-option
        value="Time"
        @click="orderChange"
      >{{this.$lang.response_time+this.getOrderSymbol('Time')}}</a-select-option>
      <a-select-option
        value="CreateTime"
        @click="orderChange"
      >{{this.$lang.create_time+this.getOrderSymbol('CreateTime')}}</a-select-option>
    </a-select>&nbsp;
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
    <a-button @click="handleReset">Clear</a-button>&nbsp;
    <a-button v-bind:type="exception?'danger':''" @click="showError">Error</a-button>&nbsp;
    <a-button icon="redo" @click="reload"></a-button>
    <a-table
      :columns="columns"
      :rowKey="record => record._id"
      :dataSource="data"
      :loading="loading"
      :pagination="pagination"
      :rowClassName="getRowClassName"
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
          width: "10%",
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
          title: this.$lang.response_time,
          dataIndex: "Time",
          width: "8%",
          ellipsis: true
        },
        {
          title: this.$lang.query,
          dataIndex: "QueryString",
          width: "12%",
          ellipsis: true
        },
        {
          title: this.$lang.content,
          dataIndex: "Content",
          width: "12%",
          ellipsis: true
        },
        {
          title: this.$lang.us,
          dataIndex: "UserName",
          width: "11%"
        },
        {
          title: this.$lang.count+"/"+this.$lang.minute,
          dataIndex: "CountPerMinute",
          width: "8%",
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
      controllers: [],
      controllerName: "",
      actions: [],
      actionName: "",
      orderBy: "CreateTime",
      orderType: "desc",
      startTime: null,
      endTime: null,
      userName: "",
      exception: null,
      getlist: this.$urls.log.getlist
    };
  },
  mounted() {
    this.getFromList();
  },
  methods: {
    getOrderSymbol: function(orderBy) {
      if (orderBy == this.orderBy) {
        if (this.orderType == "asc") return "↑";
        if (this.orderType == "desc") return "↓";
      }
      return "";
    },
    handleReset() {
      this.pagination.current = 1;
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
      this.controllerName = "";
      this.actionName = "";
      this.getData();
      this.getControllers();
    },
    controllerChange(item) {
      this.controllerName = item;
      this.actionName = "";
      this.getData();
      this.getActions();
    },
    actionChange(item) {
      this.actionName = item;
      this.getData();
    },
    orderChange(item) {
      var key = item.key;
      if (this.orderBy == key) {
        this.orderBy = key;
        this.orderType = this.orderType == "desc" ? "asc" : "desc";
      } else {
        this.orderBy = key;
        this.orderType = "desc";
      }
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
    showError() {
      if (this.exception == null) {
        this.exception = true;
      } else {
        this.exception = null;
      }
      this.pagination.current = 1;
      this.getData();
    },
    getControllers() {
      this.$axios
        .get(this.$urls.log.getControllersByFrom + "?from=" + this.from)
        .then(response => {
          if (response.code == 0) {
            this.controllers = response.result;
          }
        });
    },
    getActions() {
      this.$axios
        .get(
          this.$urls.log.getActionsByController +
            "?from=" +
            this.from +
            "&controllerName=" +
            this.controllerName
        )
        .then(response => {
          if (response.code == 0) {
            this.actions = response.result;
          }
        });
    },
    getQuerystring() {
      var url =
        "?pageIndex=" +
        this.pagination.current +
        "&pageSize=" +
        this.pagination.pageSize +
        "&sorts[0].key=" +
        this.orderBy +
        "&sorts[0].value=" +
        this.orderType;
      if (this.from) url += "&from=" + this.from;
      if (this.controllerName) url += "&controllerName=" + this.controllerName;
      if (this.actionName) url += "&actionName=" + this.actionName;
      if (this.userName) url += "&userName=" + this.userName;
      if (this.startTime) url += "&startTime=" + this.startTime;
      if (this.endTime) url += "&endTime=" + this.endTime;
      if (this.exception) url += "&exception=" + this.exception;
      return url;
    },
    getFromList() {
      this.$axios.get(this.$urls.log.getfromlist).then(response => {
        if (response.code === 0) this.fromlist = response.result;
      });
    },
    getRowClassName(record, index) {
      if (record.Exception) return "error_tr";
      return "";
    }
  }
};
</script>
<style scoped>
.ant-select-enabled,
.ant-input {
  margin: 10px 0px;
}
.ant-btn-danger {
  margin-left: 0 !important;
}
</style>