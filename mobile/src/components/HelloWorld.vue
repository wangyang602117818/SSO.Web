<template>
  <div></div>
</template>

<script>
export default {
  name: "",
  data() {
    return {};
  },
  computed: {},
  created() {
     this.$http
        .get('role/getlist')
        .then(response => {
          window.console.log( response.body.result);
        });
  },
  methods: {}
};
</script>

<style scoped>
</style>
