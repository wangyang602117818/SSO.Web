<template>
  <div>
    <a-select v-model="id" style="width: 180px" @change="nameChange">
      <a-select-option :value="0">{{ $t("all") }}</a-select-option>
      <a-select-option v-for="item in names" :value="item.Id" :key="item.Id">{{
        item.Name
      }}</a-select-option> </a-select
    >&nbsp;

    <a-date-picker
      valueFormat="YYYY-MM-DD"
      v-model="startTime"
      @change="onStartChange"
      :placeholder="$t('start_run_time')"
      style="width: 150px"
    />&nbsp;
    <a-date-picker
      valueFormat="YYYY-MM-DD"
      v-model="endTime"
      @change="onEndChange"
      :placeholder="$t('start_end_time')"
      style="width: 150px"
    />&nbsp;
    <a-button icon="redo" @click="reload"></a-button>
    <a-table
      :columns="columns"
      :rowKey="(record) => record.Id"
      :dataSource="data"
      :loading="loading"
      :pagination="pagination"
      @change="handleTableChange"
    >
    </a-table>
  </div>
</template>

<script>
import base from "./Base";
export default {
  name: "task_scheduling_history",
  mixins: [base],
  data() {
    return {
      id: 0,
      names: [],
      startTime: "",
      endTime: "",
      getlist: this.$urls.taskscheduling.getSchedulingHistory,
    };
  },
  computed: {
    columns: function () {
      return [
        {
          title: this.$t("id"),
          dataIndex: "Id",
          width: "5%",
        },
        {
          title: this.$t("scheduling_id"),
          dataIndex: "SchedulingId",
          width: "5%",
        },
        {
          title: this.$t("scheduling_name"),
          dataIndex: "SchedulingName",
          width: "15%",
        },
        {
          title: this.$t("run_time"),
          dataIndex: "RunTime",
          width: "15%",
          ellipsis: true,
        },
        {
          title: this.$t("run_result"),
          dataIndex: "RunResult",
          width: "50%",
          ellipsis: true,
        },
      ];
    },
  },
  created() {
    this.getDistinctNames();
  },
  methods: {
    getQuerystring() {
      var url =
        "?pageIndex=" +
        this.pagination.current +
        "&pageSize=" +
        this.pagination.pageSize;
      if (this.startTime) url += "&startTime=" + this.startTime;
      if (this.endTime) url += "&endTime=" + this.endTime;
      if (this.id > 0) url += "&id=" + this.id;
      return url;
    },
    getDistinctNames() {
      this.$axios
        .get(this.$urls.taskscheduling.getSchedulingNames)
        .then((response) => {
          if (response.code === 0) this.names = response.result;
        });
    },
    nameChange(e) {
      this.getData();
    },
    onStartChange() {
      this.getData();
    },
    onEndChange() {
      this.getData();
    },
  },
};
</script>

<style scoped>
</style>
