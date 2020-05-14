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
    />
  </div>
</template>
<script>
import base from "./Base";
export default {
  name:"log",
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
          title: this.$lang.source,
          dataIndex: "From",
          width: "15%"
        },
        {
          title: this.$lang.type,
          dataIndex: "Type",
          width: "5%",
          customRender: val => {
            if (val == 1) return "detail";
            if (val == 2) return "warning";
            if (val == 3) return "error";
          }
        },
        {
          title:this.$lang.content,
          dataIndex: "Content",
          width: "15%"
        },
        {
          title: this.$lang.remark,
          dataIndex: "Remark",
          width: "15%"
        },
        {
          title: this.$lang.us,
          dataIndex: "UserId",
          width: "8%"
        },
        {
          title: this.$lang.ip,
          dataIndex: "UserHost",
          width: "10%"
        },
        {
          title: this.$lang.agent,
          dataIndex: "UserAgent",
          width: "8%",
          customRender: val => {
            return this.$funtools.getDeviceType(val);
          }
        },
        {
          title: this.$lang.create_time,
          dataIndex: "CreateTime.$date",
          width: "19%",
          customRender: val => {
            return this.$funtools.parseBsonTime(val);
          }
        }
      ],
      getlist: this.$urls.log.getlist
    };
  },
  methods: {
 
  }
};
</script>
<style scoped>
</style>