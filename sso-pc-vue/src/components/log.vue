<template>
  <div>
    <a-select
      :default-value="from"
      v-model="from"
      style="width: 180px"
      @change="fromChange"
    >
      <a-select-option value>{{ $t("source") }}</a-select-option>
      <a-select-option
        v-for="item in fromlist"
        :value="item.from || 'anonymous'"
        :key="item.from || 'anonymous'"
        >{{ item.from || "anonymous" }}</a-select-option
      > </a-select
    >&nbsp;
    <a-select
      :default-value="to"
      v-model="to"
      style="width: 180px"
      @change="toChange"
    >
      <a-select-option value>{{ $t("to") }}</a-select-option>
      <a-select-option v-for="item in tolist" :value="item.to" :key="item.to">{{
        item.to
      }}</a-select-option> </a-select
    >&nbsp;
    <a-select
      :default-value="from"
      v-model="controllerName"
      style="width: 150px"
      @change="controllerChange"
    >
      <a-select-option value>controller</a-select-option>
      <a-select-option
        v-for="item in controllers"
        :value="item.controller"
        :key="item.controller"
        >{{ item.controller }}</a-select-option
      > </a-select
    >&nbsp;
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
        >{{ item.action }}</a-select-option
      > </a-select
    >&nbsp;
    <a-select v-model="orderBy" style="width: 100px">
      <a-select-option value="Time" @click="orderChange">{{
        $t("response_time") + this.getOrderSymbol("Time")
      }}</a-select-option>
      <a-select-option value="CreateTime" @click="orderChange">{{
        $t("create_time") + this.getOrderSymbol("CreateTime")
      }}</a-select-option> </a-select
    >&nbsp;
    <a-input
      :placeholder="$t('us')"
      style="width: 100px"
      v-on:keyup.enter="onSearch"
      v-model="userName"
    />&nbsp;
    <a-date-picker
      valueFormat="YYYY-MM-DD"
      :value="startTime"
      @change="onStartChange"
      :placeholder="$t('start_time')"
      style="width: 120px"
    />&nbsp;
    <a-date-picker
      valueFormat="YYYY-MM-DD"
      :value="endTime"
      @change="onEndChange"
      :placeholder="$t('end_time')"
      style="width: 120px"
    />&nbsp; <a-button @click="handleReset">Clear</a-button>&nbsp;
    <a-button v-bind:type="exception ? 'danger' : ''" @click="showError"
      >Error</a-button
    >&nbsp;
    <a-button icon="redo" @click="reload"></a-button>
    <a-table
      :columns="columns"
      :rowKey="(record) => record._id"
      :dataSource="data"
      :loading="loading"
      :pagination="pagination"
      :rowClassName="getRowClassName"
      @change="handleTableChange"
    >
      <span slot="RoleName" slot-scope="RoleName" v-if="RoleName">
        <a-tag v-for="tag in RoleName.split(',')" :key="tag">{{ tag }}</a-tag>
      </span>
      <span slot="path" slot-scope="text, record">{{
        record.Controller + "/" + record.Action
      }}</span>
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
          title: this.$t("source"),
          dataIndex: "From",
          width: "10%",
          ellipsis: true,
        },
        {
          title: this.$t("to"),
          dataIndex: "To",
          width: "10%",
          ellipsis: true,
        },
        {
          title: "controller",
          dataIndex: "Controller",
          width: "8%",
          ellipsis: true,
        },
        {
          title: "action",
          dataIndex: "Action",
          width: "8%",
          ellipsis: true,
        },
        {
          title: this.$t("response_time") + "ms",
          dataIndex: "Time",
          width: "6%",
          ellipsis: true,
        },
        {
          title: this.$t("query"),
          dataIndex: "QueryString",
          width: "10%",
          ellipsis: true,
        },
        {
          title: this.$t("request_content"),
          dataIndex: "Content",
          width: "10%",
          ellipsis: true,
        },
        {
          title: this.$t("response_content"),
          dataIndex: "Response",
          width: "10%",
          ellipsis: true,
        },
        {
          title: this.$t("us"),
          dataIndex: "UserName",
          width: "8%",
        },
        {
          title: this.$t("count") + "/" + this.$t("minute"),
          dataIndex: "CountPerMinute",
          width: "5%",
          ellipsis: true,
        },
        {
          title: this.$t("agent"),
          dataIndex: "UserAgent",
          width: "5%",
          ellipsis: true,
          customRender: (val) => {
            return this.$funtools.getDeviceType(val);
          },
        },
        {
          title: this.$t("create_time"),
          dataIndex: "CreateTime",
          width: "10%",
          ellipsis: true,
          customRender: (val) => {
            return this.$funtools.parseIsoDateTime(val);
          },
        },
      ],
      fromlist: [],
      from: "",
      tolist: [],
      to: "",
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
      getlist: this.$urls.log.getlist,
    };
  },
  mounted() {
    this.getFromList();
    this.getToList();
  },
  methods: {
    getOrderSymbol: function (orderBy) {
      if (orderBy == this.orderBy) {
        if (this.orderType == "asc") return "↑";
        if (this.orderType == "desc") return "↓";
      }
      return "";
    },
    handleReset() {
      this.pagination.current = 1;
      this.from = "";
      this.to = "";
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
    toChange(item) {
      this.controllerName = "";
      this.actionName = "";
      this.to = item;
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
        .get(this.$urls.log.getControllersByTo + "?to=" + this.to)
        .then((response) => {
          if (response.code == 0) {
            this.controllers = response.result;
          }
        });
    },
    getActions() {
      this.$axios
        .get(
          this.$urls.log.getActionsByController +
            "?to=" +
            this.to +
            "&controllerName=" +
            this.controllerName
        )
        .then((response) => {
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
      if (this.to) url += "&to=" + this.to;
      if (this.controllerName) url += "&controllerName=" + this.controllerName;
      if (this.actionName) url += "&actionName=" + this.actionName;
      if (this.userName) url += "&userName=" + this.userName;
      if (this.startTime) url += "&startTime=" + this.startTime;
      if (this.endTime) url += "&endTime=" + this.endTime;
      if (this.exception) url += "&exception=" + this.exception;
      return url;
    },
    getFromList() {
      this.$axios.get(this.$urls.log.getfromlist).then((response) => {
        if (response.code === 0) this.fromlist = response.result;
      });
    },
    getToList() {
      this.$axios.get(this.$urls.log.gettolist).then((response) => {
        if (response.code === 0) this.tolist = response.result;
      });
    },
    getRowClassName(record, index) {
      if (record.Exception) return "error_tr";
      return "";
    },
  },
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