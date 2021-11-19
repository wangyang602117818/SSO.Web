<template>
  <f7-page name="company_update">
    <f7-navbar :title="$t('manage.update_company')" :back-link="$t('common.back')">
      <f7-nav-right>
      <f7-link @click="saveCompany">{{$t('common.save')}}</f7-link>
      </f7-nav-right>
    </f7-navbar>
    <CompanyBase v-if="company.id>=0" :company="company" />
  </f7-page>
</template>

<script>
import CompanyBase from "./company_base";
export default {
  name: "company_update",
  components: { CompanyBase },
   props: {
    f7router: Object,
    id: String,
  },
  data() {
    return {
      company: {
        id: "",
        code: "",
        name: "",
        description: ""
      }
    };
  },
  created() {
    this.getData();
  },
  methods: {
    saveCompany() {
      if (this.company.code.trim() == "") return;
      if (this.company.name.trim() == "") return;
      this.$axios
        .post(this.$urls.company.update, this.company)
        .then(response => {
          if (response.code == 0) {
             this.$eventbus.emit('companyupdate',this.company.id);
            this.f7router.back();
            this.showSuccess();
          }
        });
    },
    getData() {
      var id = this.id;
      this.$axios.get(this.$urls.company.getById + "/" + id).then(response => {
        if (response.code === 0) {
          this.company = {
            id: response.result.Id,
            code: response.result.Code,
            name: response.result.Name,
            description: response.result.Description
          };
        }
      });
    }
  }
};
</script>

<style scoped>
</style>
