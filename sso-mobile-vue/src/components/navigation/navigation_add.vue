<template>
  <f7-page name="navigation_add">
    <f7-navbar :title="$t('manage.add_navigator')" :back-link="$t('common.back')">
      <f7-link slot="right" @click="saveNavigation">{{$t('common.save')}}</f7-link>
    </f7-navbar>
    <NavigationBase :navigation="navigation" />
  </f7-page>
</template>

<script>
import NavigationBase from "./navigation_base";
export default {
  name: "navigation_add",
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
  methods: {
    saveNavigation(){
      if (this.navigation.title.trim() == "") return;
      if (this.navigation.baseUrl.trim() == "") return;
      this.$axios.post(this.$urls.navigation.add, this.navigation).then(response => {
        if (response.code == 0) {
          this.$f7router.back("",{force:true});
          this.showSuccess();
        }
      });
    }
  }
};
</script>

<style scoped>
</style>
