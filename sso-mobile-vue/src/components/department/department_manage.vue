<template>
  <f7-page name="departmentmanage" ptr @ptr:refresh="getDepartment">
    <f7-navbar :title="companyName" :back-link="$t('common.back')">
      <f7-nav-right>
        <!--传一个空的父id-->
        <f7-link icon-f7="plus_circle" :href="'/departmentadd/'+companyCode+'/ '"></f7-link>
      </f7-nav-right>
    </f7-navbar>
    <f7-list media-list>
      <f7-list-item
        v-for="item in dataList"
        :title="item.title"
        :subtitle="item.desc||' '"
        :key="item.key"
        :link="'/departmentupdate/'+companyCode+'/'+item.key"
        swipeout
      >
        <f7-icon slot="media" v-for="layer in item.layer" :key="layer"></f7-icon>
        <f7-swipeout-actions right>
          <f7-swipeout-button
            color="blue"
            @click="addSubDepartment(companyCode,item.key)"
          >{{$t('manage.add_sub_department')}}</f7-swipeout-button>
          <f7-swipeout-button color="red" @click="delDeptartment(item.id)">{{$t('common.delete')}}</f7-swipeout-button>
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
      companyName: "",
      companyCode: ""
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
          order: node.Order.toString(),
          id: node.Id
        });
        if (node.children) {
          this.generateList(node.children);
        }
      }
    },
    delDeptartment(id) {
      var that = this;
      this.$f7.dialog.confirm(
        this.$t("confirm.sure_delete"),
        this.$t("common.tips"),
        function() {
          that.$axios
            .get(that.$urls.department.delete + "/" + id)
            .then(response => {
              if (response.code === 0) {
                for (var i = 0; i < that.dataList.length; i++) {
                  if (that.dataList[i].id == id) {
                    that.dataList.splice(i, 1);
                    return;
                  }
                }
              }
            });
        }
      );
    },
    addSubDepartment(companyCode, departmentCode) {
      this.$f7router.navigate(
        "/departmentadd/" + companyCode + "/" + departmentCode
      );
    },
    getDepartment(done) {
      var companyCode = this.$f7route.params.id;
      var companyName = this.$f7route.params.name;
      this.companyName = companyName;
      this.companyCode = companyCode;
      this.dataList = [];
      this.$axios
        .get(
          this.$urls.department.getdepartments + "/?companyCode=" + companyCode
        )
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
