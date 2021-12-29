<template>
  <f7-page>
    <f7-navbar :back-link="$funtools.getFileName(filename, 10)"> </f7-navbar>
      <iframe
        :src="pdfurl"
        title="SOME_TITLE"
        width="100%"
        height="100%"
        border="0"
        style="border:0"
      />
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
      return (
        this.$urls.preview +
        "/" +
        this.fileid +
        "/" +
        this.filename +
        "#toolbar=0"
      );
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