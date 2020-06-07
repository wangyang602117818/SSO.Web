<template>
  <f7-page name="department_add">
    <f7-navbar title="添加部门" back-link="返回">
      <f7-link slot="right" @click="saveDepartment">保存</f7-link>
    </f7-navbar>
    <DepartmentBase :department="department" />
  </f7-page>
</template>

<script>
import DepartmentBase from "./department_base";
export default {
  name: "department_add",
  components: { DepartmentBase },
  data() {
    return {
      department: {
        code: this.$funtools.randomWord(12, 12),
        name: "",
        companyCode: this.$f7route.params.companyCode.trim(),
        description: "",
        order: 0,
        parentCode: this.$f7route.params.parentCode.trim()
      }
    };
  },
  created() {},
  methods: {
    saveDepartment() {
      if (this.department.name.trim() == "") return;
      if (this.department.code.trim() == "") return;
      this.$axios.post(this.$urls.department.add, this.department).then(response => {
        if (response.code == 0) {
          this.$f7router.back("", { force: true });
          this.showSuccess();
        }
      });
    }
  }
};
</script>

<style scoped>
</style>
