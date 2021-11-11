<template>
  <f7-page name="role_add">
    <f7-navbar :title="$t('manage.add_company')" :back-link="$t('common.back')">
      <f7-link @click="saveCompany">{{$t('common.save')}}</f7-link>
    </f7-navbar>
    <CompanyBase :company="company" />
  </f7-page>
</template>

<script>
import CompanyBase from "./company_base";
export default {
  name: "company_add",
  components: { CompanyBase },
  data() {
    return {
      company: {
        code: this.$funtools.randomWord(12,12),
        name: "",
        description: ""
      }
    };
  },
  methods: {
    saveCompany() {
      if (this.company.name.trim() == "") return;
      if (this.company.code.trim() == "") return;
      this.$axios.post(this.$urls.company.add, this.company).then(response => {
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
