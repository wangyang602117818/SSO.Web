<template>
  <f7-list v-if="company">
    <f7-list-input
      required
      validate
      label="公司编号*"
      type="text"
      placeholder="公司编号"
      error-message="公司编号是必填项!"
      clear-button
      :value="company.code"
      @input="($event)=>{company.code=$event.target.value}"
    ></f7-list-input>
    <f7-list-input
      required
      validate
      label="公司名称*"
      type="text"
      placeholder="公司名称"
      error-message="公司名称是必填项!"
      clear-button
      :value="company.name"
      @input="($event)=>{company.name=$event.target.value}"
    ></f7-list-input>
    <f7-list-input
      label="公司描述"
      type="textarea"
      placeholder="公司描述"
      clear-button
      :value="company.description"
      @input="($event)=>{company.description=$event.target.value}"
    ></f7-list-input>
  </f7-list>
</template>

<script>
export default {
  name: "company_base",
  props: ["company"],
  data() {
    return {};
  },
  methods: {}
};
</script>

<style scoped>
</style>
