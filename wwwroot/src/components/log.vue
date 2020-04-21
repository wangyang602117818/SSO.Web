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
export default {
  data() {
    return {
      data: [],
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