<template>
  <div>
    <a-input-search
      placeholder="input search text"
      style="width: 200px"
      @search="onSearch"
      v-model="searchValue"
    />
  </div>
</template>
<script>
export default {
  data() {
    return {
      data: [],
      searchValue: "",
      loading: false,

    }
  },
  methods:{
    onSearch() {
      this.pagination.current = 1;
      this.selectedRowKeys = [];
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
          pagination.showTotal=()=>{return this.pagination.total;};
          this.pagination = pagination;
          if (response.body.code == 0) this.data = response.body.result;
        });
    },
  }
}
</script>
<style scoped>

</style>