<template>
  <f7-page name="navigation_update">
    <f7-navbar title="导航修改" back-link="返回">
      <f7-link slot="right" @click="saveNavigation">保存</f7-link>
    </f7-navbar>
    <NavigationBase v-if="navigation.id>=0" :navigation="navigation" />
  </f7-page>
</template>

<script>
import NavigationBase from "./navigation_base";
export default {
  name: "navigation_update",
  components: { NavigationBase },
  data() {
    return {
      navigation: {
        id: "",
        title: "",
        baseUrl: "",
        logoUrl: ""
      }
    };
  },
  created() {
    this.getData();
  },
  methods: {
    saveNavigation() {
      if (this.navigation.title.trim() == "") return;
      if (this.navigation.baseUrl.trim() == "") return;
      this.$axios.post(this.$urls.navigation.update, this.navigation).then(response => {
        if (response.code == 0) {
          this.$f7router.back();
          this.showSuccess();
        }
      });
    },
    getData() {
      var id = this.$f7route.params.id;
      this.$axios
        .get(this.$urls.navigation.getbyid + "/" + id)
        .then(response => {
          if (response.code === 0) {
            this.navigation = {
              id: response.result._id,
              title: response.result.Title,
              baseUrl: response.result.BaseUrl,
              logoUrl: response.result.LogoUrl
            };
          }
        });
    }
  }
};
</script>

<style scoped>
</style>
