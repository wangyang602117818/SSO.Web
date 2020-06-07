<template>
  <f7-list>
    <f7-list-input
      required
      validate
      label="部门编号*"
      type="text"
      placeholder="部门编号"
      error-message="部门编号是必填项!"
      :value="department.code"
      @input="($event)=>{department.code=$event.target.value}"
      clear-button
    ></f7-list-input>
    <f7-list-input
      required
      validate
      label="部门名称*"
      type="text"
      placeholder="部门名称"
      error-message="部门名称是必填项!"
      :value="department.name"
      @input="($event)=>{department.name=$event.target.value}"
      clear-button
    ></f7-list-input>
    <f7-list-item
      title="公司"
      smart-select
      :smart-select-params="{openIn: 'popover'}"
      v-if="companys.length>0"
    >
      <select name="company" @change="changeCompany($event)">
        <option
          :value="item.Code"
          v-for="item in companys"
          :key="item.Id"
          :selected="department.companyCode===item.Code"
        >{{item.Name}}</option>
      </select>
    </f7-list-item>
    <f7-list-item
      link="#"
      title="上级部门"
      v-if="departments.length>0"
      smart-select
      :smart-select-params="{
          formatValueText:formatValueText,
          openIn: 'popover',
        }"
    >
      <select name="department" @change="changeDepartment($event)">
        <option value></option>
        <option
          :value="item.key"
          :key="item.key"
          v-for="item in departments"
          :selected="item.key==department.parentCode"
        >{{getDepartmentShow(item)}}</option>
      </select>
    </f7-list-item>
    <f7-list-input
      label="顺序"
      type="text"
      placeholder="顺序"
      clear-button
      :value="department.order"
      @input="($event)=>{department.order=$event.target.value}"
    ></f7-list-input>
    <f7-list-input
      label="部门描述"
      type="textarea"
      placeholder="部门描述"
      clear-button
      :value="department.description"
      @input="($event)=>{department.description=$event.target.value}"
    ></f7-list-input>
  </f7-list>
</template>

<script>
export default {
  name: "department_base",
  props: ["department"],
  data() {
    return {
      companys: [],
      departments: []
    };
  },
  created() {
    if (this.department.id > 0) this.getCompanyData();  //修改部门才获取
  },
  methods: {
    formatValueText(values) {
      var arr = [];
      for (var i = 0; i < values.length; i++)
        arr.push(values[i].replace(/&nbsp;/gi, ""));
      return arr;
    },
    changeCompany(event) {
      this.department.companyCode = event.target.value;
      this.getDepartmentData(event.target.value);
    },
    changeDepartment(event) {
      this.department.parentCode = event.target.value;
    },
    getCompanyData() {
      this.$axios.get(this.$urls.company.getall).then(response => {
        if (response.code === 0) {
          this.companys = response.result;
          this.getDepartmentData(this.department.companyCode);
        }
      });
    },
    getDepartmentShow(item) {
      var str = "";
      for (var i = 0; i < item.layer; i++) str += "&nbsp;";
      return str + item.title;
    },
    getDepartmentData(companyCode) {
      this.$axios
        .get(
          this.$urls.department.getdepartments + "?companyCode=" + companyCode
        )
        .then(response => {
          if (response.code === 0) {
            var dataList = [];
            this.generateDepartmentList(response.result, dataList);
            this.departments = dataList;
          }
        });
    },
    generateDepartmentList(data, dataList) {
      for (let i = 0; i < data.length; i++) {
        dataList.push({
          key: data[i].key,
          title: data[i].title,
          layer: data[i].Layer
        });
        if (data[i].children) {
          this.generateDepartmentList(data[i].children, dataList);
        }
      }
    }
  }
};
</script>

<style scoped>
</style>
