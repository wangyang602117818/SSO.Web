<template>
  <f7-list>
    <f7-list-input required validate :label="$t('common.department') + $t('common.code') + '*'" type="text"
      :placeholder="$t('common.department') + $t('common.code')" :error-message="$t('valid.department_code_required')"
      :value="department.code" @input="
        ($event) => {
          department.code = $event.target.value;
        }
      " clear-button></f7-list-input>
    <f7-list-input required validate :label="$t('common.department') + $t('common.name') + '*'" type="text"
      :placeholder="$t('common.department') + $t('common.name')" :error-message="$t('valid.department_name_required')"
      :value="department.name" @input="
        ($event) => {
          department.name = $event.target.value;
        }
      " clear-button></f7-list-input>
    <f7-list-item :title="$t('common.company')" smart-select :smart-select-params="{ openIn: 'popover' }"
      v-if="companys.length > 0">
      <select name="company" @change="changeCompany($event)">
        <option :value="item.Code" v-for="item in companys" :key="item.Id"
          :selected="department.companyCode === item.Code">
          {{ item.Name }}
        </option>
      </select>
    </f7-list-item>
    <f7-list-item link="#" :title="$t('manage.sup_department')" v-if="departments.length > 0" smart-select
      :smart-select-params="{
        formatValueText: formatValueText,
        openIn: 'popover',
      }">
      <select name="department" @change="changeDepartment($event)">
        <option value></option>
        <option :value="item.key" :key="item.key" v-for="item in departments"
          :selected="item.key == department.parentCode">
          {{ getDepartmentShow(item) }}
        </option>
      </select>
    </f7-list-item>
    <f7-list-input :label="$t('common.order')" type="text" :placeholder="$t('common.order')" clear-button
      :value="department.order" @input="
        ($event) => {
          department.order = $event.target.value;
        }
      "></f7-list-input>
    <f7-list-input :label="$t('common.department') + $t('common.description')" type="textarea"
      :placeholder="$t('common.department') + $t('common.description')" clear-button :value="department.description"
      @input="
  ($event) => {
    department.description = $event.target.value;
  }
      "></f7-list-input>
  </f7-list>
</template>

<script>
export default {
  name: "department_base",
  props: ["department"],
  data() {
    return {
      companys: [],
      departments: [],
    };
  },
  created() {
    if (this.department.id > 0) this.getCompanyData(); //修改部门才获取
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
      this.$axios.get(this.$urls.company.getall).then((response) => {
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
        .then((response) => {
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
          key: data[i].Code,
          title: data[i].Name,
          layer: data[i].Layer,
        });
        if (data[i].Children) {
          this.generateDepartmentList(data[i].Children, dataList);
        }
      }
    },
  },
};
</script>

<style scoped>

</style>
