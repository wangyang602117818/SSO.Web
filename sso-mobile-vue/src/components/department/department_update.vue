<template>
  <f7-page name="department_add">
    <f7-navbar :title="$t('manage.update_department')" :back-link="$t('common.back')">
      <f7-link slot="right" @click="saveDepartment">{{$t('common.save')}}</f7-link>
    </f7-navbar>
    <DepartmentBase :department="department" v-if="department.id>0" />
    <f7-block class="text-align-center" v-else>
      <f7-preloader></f7-preloader>
    </f7-block>
  </f7-page>
</template>

<script>
import DepartmentBase from "./department_base";
export default {
  name: "department_update",
  components: { DepartmentBase },
  data() {
    return {
      department: {
        id: 0,
        code: "",
        name: "",
        companyCode: "",
        description: "",
        order: 0,
        parentCode: ""
      }
    };
  },
  created() {
    this.getData();
  },
  methods: {
    saveDepartment() {
      if (this.department.name.trim() == "") return;
      if (this.department.code.trim() == "") return;
      this.$axios
        .post(this.$urls.department.update, this.department)
        .then(response => {
          if (response.code == 0) {
            this.$f7router.back("", { force: true });
            this.showSuccess();
          }
        });
    },
    getData() {
      var companyCode = this.$f7route.params.companyCode;
      this.$axios
        .get(this.$urls.department.get + "?code=" + this.$f7route.params.code)
        .then(response => {
          if (response.code === 0) {
            this.department = {
              id: response.result.Id,
              code: response.result.key,
              name: response.result.title,
              companyCode: companyCode,
              description: response.result.Description || "",
              order: response.result.Order,
              parentCode: response.result.ParentCode
            };
          }
        });
    }
  }
};
</script>

<style scoped>
</style>
