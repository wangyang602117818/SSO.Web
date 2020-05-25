<template>
  <f7-list v-if="role">
    <f7-list-input
      required
      validate
      label="角色名称*"
      type="text"
      placeholder="角色名称"
      error-message="角色名称是必填项!"
      clear-button
      :value="role.name"
      @input="($event)=>{role.name=$event.target.value}"
    ></f7-list-input>
    <f7-list-input
      required
      validate
      label="角色描述"
      type="textarea"
      placeholder="角色描述"
      error-message="角色描述是必填项!"
      clear-button
      :value="role.description"
      @input="($event)=>{role.description=$event.target.value}"
    ></f7-list-input>
  </f7-list>
</template>

<script>
export default {
  name: "role_base",
  props: ["role"],
  data() {
    return {};
  },
  methods: {}
};
</script>

<style scoped>
</style>
