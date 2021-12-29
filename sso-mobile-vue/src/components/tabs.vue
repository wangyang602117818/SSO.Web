<template>
  <f7-page :page-content="false">
    <f7-toolbar tabbar bottom>
      <f7-link
        tab-link="#navigator"
        tab-link-active
        :text="$t('common.navigator')"
        icon-f7="flag_circle"
        icon-size="24px"
      >
      </f7-link>
      <f7-link
        tab-link="#manage"
        :text="$t('common.manage')"
        icon-f7="square_grid_2x2_fill"
        icon-size="24px"
      ></f7-link>
      <f7-link
        tab-link="#me"
        :text="$t('common.me')"
        icon-f7="person_crop_circle"
        icon-size="24px"
      ></f7-link>
    </f7-toolbar>
    <f7-tabs @click="pageClick">
      <f7-tab id="navigator" tab-active>
        <Navigator
          :suggests="suggests"
          @suggestInput="suggestInput"
          @suggestFocus="suggestFocus"
          @search="search"
        />
      </f7-tab>
      <f7-tab id="manage">
        <Manage />
      </f7-tab>
      <f7-tab id="me">
        <Me />
      </f7-tab>
    </f7-tabs>
  </f7-page>
</template>

<script>
import Navigator from "./navigator";
import Manage from "./manage";
import Me from "./me";
export default {
  name: "tabs",
  components: { Navigator, Manage, Me },
  props: {
    f7router: Object,
  },
  data() {
    return {
      suggests: [],
      cancelToken:null
    };
  },
  computed: {},
  methods: {
    pageClick() {
      this.suggests = [];
    },
    search(e) {
      var value = e.target.value;
      this.f7router.navigate("/search/" + value);
    },
    cancelRequest() {
      if (typeof this.cancelToken === "function") {
        this.cancelToken();
      }
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
  },
};
</script>

<style scoped>
#me {
  height: 100%;
  background-color: #fff;
}
</style>
