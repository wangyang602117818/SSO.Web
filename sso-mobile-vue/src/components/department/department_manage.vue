<template>
  <f7-page name="departmentmanage" ptr @ptr:refresh="getDepartment">
    <f7-navbar :title="companyName" back-link="返回">
      <f7-nav-right>
        <f7-link icon-f7="plus_circle" href="/departmentadd/"></f7-link>
      </f7-nav-right>
    </f7-navbar>
    <f7-list media-list>
      <f7-list-item
        v-for="item in dataList"
        :title="item.title"
        :subtitle="item.desc||' '"
        :key="item.key"
        :link="'/departmentupdate/'+item.id"
        swipeout
      >
        <f7-icon slot="media" v-for="layer in item.layer" :key="layer"></f7-icon>
        <f7-swipeout-actions right>
          <f7-swipeout-button color="blue" @click="addSubDepartment(item.id)">addSub</f7-swipeout-button>
          <f7-swipeout-button color="green" @click="departmentUpdate(item.id)">Update</f7-swipeout-button>
          <f7-swipeout-button color="red" @click="delDeptartment(item.id)">Delete</f7-swipeout-button>
        </f7-swipeout-actions>
      </f7-list-item>
    </f7-list>
  </f7-page>
</template>

<script>
export default {
  name: "departmentmanage",
  data() {
    return {
      dataList: [],
      companyName: ""
    };
  },
  created() {
    this.getDepartment();
  },
  methods: {
    generateList(list) {
      for (let i = 0; i < list.length; i++) {
        const node = list[i];
        const key = node.key;
        this.dataList.push({
          key: key,
          title: node.title,
          desc: node.Description,
          layer: node.Layer,
          id: node.Id
        });
        if (node.children) {
          this.generateList(node.children);
        }
      }
    },
    delDeptartment(id) {},
    departmentUpdate(id){
      window.console.log(id);
    },
    addSubDepartment(id){

    },
    getDepartment(done) {
      var code = this.$f7route.params.id;
      var name = this.$f7route.params.name;
      this.companyName = name;
      this.dataList = [];
      this.$axios
        .get(this.$urls.department.getdepartments + "/?companyCode=" + code)
        .then(response => {
          if (done) done();
          if (response.code === 0) {
            this.generateList(response.result);
          }
        });
    }
  }
};
</script>

<style scoped>
</style>
