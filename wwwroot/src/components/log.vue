<template>
  <div>
    <a-input-search placeholder="内容" style="width: 200px" @search="onSearch" v-model="searchValue" />
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
export default {
  data() {
    return {
      data: [],
      columns: [
        {
          title: "编号",
          dataIndex: "_id",
          width: "5%"
        },
        {
          title: "来源",
          dataIndex: "From",
          width: "15%"
        },
        {
          title: "类型",
          dataIndex: "Type",
          width: "5%",
          customRender: val => {
            if (val == 1) return "详情";
            if (val == 2) return "警告";
            if (val == 3) return "错误";
          }
        },
        {
          title: "内容",
          dataIndex: "Content",
          width: "15%"
        },
        {
          title: "备注",
          dataIndex: "Remark",
          width: "15%"
        },
        {
          title: "用户",
          dataIndex: "UserId",
          width: "8%"
        },
        {
          title: "用户Ip",
          dataIndex: "UserHost",
          width: "10%"
        },
        {
          title: "用户代理",
          dataIndex: "UserAgent",
          width: "7%",
          customRender: val => {
            return this.$common.getAgent(val);
          }
        },
        {
          title: "生成时间",
          dataIndex: "CreateTime.$date",
          width: "20%",
          customRender: val => {
            return this.$common.parseBsonTime(val);
          }
        }
      ],
      searchValue: "",
      loading: false,
      pagination: { current: 1, pageSize: 10, size: "small" }
    };
  },
  created() {
    this.getData();
  },
  methods: {
    onSearch() {
      this.pagination.current = 1;
      this.getData();
    },
    getData() {
      this.loading = true;
      this.$http
        .get(
          this.$urls.log.getlist +
            "?pageIndex=" +
            this.pagination.current +
            "&pageSize=" +
            this.pagination.pageSize +
            "&filter=" +
            this.searchValue
        )
        .then(response => {
          this.loading = false;
          const pagination = { ...this.pagination };
          pagination.total = response.body.count;
          pagination.showTotal = () => {
            return this.pagination.total;
          };
          this.pagination = pagination;
          if (response.body.code == 0) this.data = response.body.result;
        });
    },
    reload() {
      this.selectedRowKeys = [];
      this.getData();
    },
    handleTableChange(pagination) {
      this.pagination.current = pagination.current;
      this.getData();
    }
  }
};
</script>
<style scoped>
</style>