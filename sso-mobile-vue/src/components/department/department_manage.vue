<template>
  <f7-page name="departmentmanage" ptr @ptr:refresh="getDepartment">
    <f7-navbar :title="companyName" :back-link="$t('common.back')">
      <f7-nav-right>
        <!--传一个空的父id-->
        <f7-link
          icon-f7="plus_circle"
          :href="'/departmentadd/' + companyCode + '/ '"
        ></f7-link>
      </f7-nav-right>
    </f7-navbar>
    <f7-list media-list>
      <f7-list-item
        v-for="item in dataList"
        :title="item.title"
        :subtitle="item.desc || ' '"
        :key="item.key"
        :link="'/departmentupdate/' + companyCode + '/' + item.key"
        swipeout
        @swipeout:delete="delDeptartment(item.id)"
      >
        <template #media v-if="item.layer > 0">
          <f7-icon v-for="layer in item.layer" :key="layer" width="7"
            >·</f7-icon
          >
        </template>
        <f7-swipeout-actions right>
          <f7-swipeout-button
            color="blue"
            @click="addSubDepartment(companyCode, item.key)"
            >{{ $t("manage.add_sub_department") }}</f7-swipeout-button
          >
          <f7-swipeout-button
            color="red"
            delete
            :confirm-title="$t('common.tips')"
            :confirm-text="$t('confirm.sure_delete')"
            >{{ $t("common.delete") }}</f7-swipeout-button
          >
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
      companyCode: "",
    };
  },
  props: {
    f7router: Object,
    id: String,
    name: String,
  },
  created() {
    this.getDepartment();
     this.$eventbus.on("departentrefresh", this.getDepartment);
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
          id: node.Id,
        });
        if (node.children) {
          this.generateList(node.children);
        }
      }
    },
    delDeptartment(id) {
      this.$axios
        .get(this.$urls.department.delete + "/" + id)
        .then((response) => {
          if (response.code != 0) {
            this.f7router.refreshPage();
          }
        });
    },
    addSubDepartment(companyCode, departmentCode) {
      this.f7router.navigate(
        "/departmentadd/" + companyCode + "/" + departmentCode
      );
    },
    getDepartment(done) {
      var companyCode = this.id;
      var companyName = this.name;
      this.companyName = companyName;
      this.companyCode = companyCode;
      this.dataList = [];
      this.$axios
        .get(
          this.$urls.department.getdepartments + "/?companyCode=" + companyCode
        )
        .then((response) => {
          if (done) done();
          if (response.code === 0) {
            this.generateList(response.result);
          }
        });
    },
  },
};
</script>

<style scoped>
</style>
