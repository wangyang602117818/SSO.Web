var Suggest = {
    data() {
        return {
            suggests: [],
            cancelToken: null,
        }
    },
    methods: {
        cancelRequest() {
            if (typeof this.cancelToken === "function") {
                this.cancelToken();
            }
        },
        emptySuggests() {
            this.suggests = [];
        },
        suggestInput(e) {
            var value = e.target.value;
            if (!value) {
                this.suggests = [];
                return;
            }
            this.cancelRequest();
            this.loadSuggest(value);
        },
        suggestFocus(e) {
            var value = e.target.value;
            if (!value) return;
            this.loadSuggest(value);
        },
        loadSuggest(value) {
            var that = this;
            let CancelToken = this.$axios.CancelToken;
            this.$axios
                .get(this.$urls.search.suggest + "?word=" + value, {
                    cancelToken: new CancelToken((c) => {
                        that.cancelToken = c;
                    }),
                })
                .then((response) => {
                    if (response.code == 0) {
                        this.suggests = response.result;
                    }
                });
        },
    }
}

export default Suggest