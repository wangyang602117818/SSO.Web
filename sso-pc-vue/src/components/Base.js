var BaseComponent = {
  data() {
    return {
      data: [],
      searchValue: "",
      loading: false,
      pagination: { current: 1, pageSize: 15, size: "small" },
      selectedRowKeys: [],
      selectedRows: [],
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
      this.$axios
        .get(this.getlist + this.getQuerystring())
        .then(response => {
          this.loading = false;
          const pagination = { ...this.pagination };
          pagination.total = response.count;
          pagination.showTotal = () => {
            return this.pagination.total;
          };
          this.pagination = pagination;
          if (response.code == 0) this.data = response.result;
        });
    },
    reload() {
      this.selectedRowKeys = [];
      this.getData();
    },
    onSelectChange(selectedRowKeys, selectedRows) {
      this.selectedRowKeys = selectedRowKeys;
      this.selectedRows = selectedRows;
    },
    handleTableChange(pagination) {
      this.pagination.current = pagination.current;
      this.getData();
    },
  }
}
export default BaseComponent