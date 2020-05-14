var BaseComponent = {
  data() {
    return {
      data: [],
      searchValue: "",
      loading: false,
      pagination: { current: 1, pageSize: 10, size: "small" },
      selectedRowKeys: [],
      drawerVisible: false,
      isUpdate: false,
      form: this.$form.createForm(this)
    }
  },
  created() {
    this.getData();
  },
  methods: {
    onSearch: function () {
      this.pagination.current = 1;
      this.selectedRowKeys = [];
      this.getData();
    },
    showDrawer() {
      this.drawerVisible = true;
      this.isUpdate = false;
    },
    getData() {
      this.loading = true;
      this.$http
        .get(this.getlist + "?pageIndex=" + this.pagination.current + "&pageSize=" + this.pagination.pageSize + "&filter=" + this.searchValue
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
    onSelectChange(selectedRowKeys) {
      this.selectedRowKeys = selectedRowKeys;
    },
    handleTableChange(pagination) {
      this.pagination.current = pagination.current;
      this.getData();
    },
  }
}
export default BaseComponent