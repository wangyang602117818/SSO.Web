<template>
  <f7-page name="company_update">
    <f7-navbar title="公司修改" back-link="返回">
      <f7-link slot="right" @click="saveCompany">保存</f7-link>
    </f7-navbar>
    <CompanyBase v-if="company.id>=0" :company="company" />
  </f7-page>
</template>

<script>
import CompanyBase from "./company_base";
export default {
  name: "company_update",
  components: { CompanyBase },
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
            this.$f7router.back();
            this.showSuccess();
          }
        });
    },
    getData() {
      var id = this.$f7route.params.id;
      this.$axios.get(this.$urls.company.getById + "/" + id).then(response => {
        if (response.code === 0) {
          this.company = {
            id: response.result._id,
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
