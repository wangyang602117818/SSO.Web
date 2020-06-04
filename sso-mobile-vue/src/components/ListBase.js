var ListBase = {
    data() {
        return {
            datas: [],
            filter: "",
            isEnd: false,
            loading: true,
            pageIndex: 1,
            pageSize: 15
        }
    },
    created() {
        this.getData();
    },
    methods: {
        onSearch(event) {
            var value = event.target.value;
            this.filter = value;
            this.pageIndex = 1;
            this.isEnd = false;
            this.getData(null, true);
        },
        refresh(done) {
            this.pageIndex = 1;
            this.getData(done, true);
        },
        loadMore() {
            //正在loading数据或者所有数据已经加载完成
            if (this.loading || this.isEnd) return;
            this.pageIndex = this.pageIndex + 1;
            this.getData(null, false);
        },
        removeItem(id) {
            var index = -1;
            for (var i = 0; i < this.datas.length; i++) {
                if (this.datas[i].Id == id) index = i;
            }
            this.datas.splice(index,1);
        },
        getData(callback, replace) {
            this.loading = true;
            this.$axios
                .get(this.getlist +
                    "?pageIndex=" +
                    this.pageIndex +
                    "&pageSize=" +
                    this.pageSize +
                    "&filter=" +
                    this.filter
                )
                .then(response => {
                    if (callback) callback();
                    this.loading = false;
                    if (response.code === 0) {
                        if (response.result.length < this.pageSize) this.isEnd = true;
                        if (replace) {
                            this.datas = response.result;
                        } else {
                            this.datas = this.datas.concat(response.result);
                        }
                    }
                });
        }
    }
}

export default ListBase