<template>
  <f7-page>
    <f7-navbar :back-link="filename"> </f7-navbar>
    <embed  :src="pdfurl" type="application/pdf" height="100%" width="100%">
  </f7-page>
</template>

<script>
export default {
  name: "pdfviewer",
  data() {
    return {};
  },
  props: {
    fileid: String,
    filename: String,
  },
  computed: {
    pdfurl() {
      return this.$urls.preview + "/" + this.fileid + "/" + this.filename;
    },
  },
  created() {},
  mounted() {
    //pdf.embed(this.pdfurl, "#pdf-content",{height: "50rem",fallbackLink: false});
  },
  methods: {},
};
</script>

<style scoped>

</style>