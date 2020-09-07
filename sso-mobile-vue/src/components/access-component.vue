<template>
  <f7-popup
    class="popup-add-access"
    swipe-to-close
    swipe-handler=".swipe-handler"
    :opened="show"
    @popup:opened="$emit('opened')"
    @popup:closed="$emit('closed')"
  >
    <f7-page>
      <f7-navbar :title="$t('navigator.access_authority')" class="swipe-handler">
        <f7-nav-right>
          <f7-link popup-close>{{$t("common.close")}}</f7-link>
        </f7-nav-right>
      </f7-navbar>
      <f7-list>
        <f7-list-item
          :title="$t('common.company')"
          smart-select
          :smart-select-params="{openIn: 'popover'}"
        >
          <select name="company" @change="changeCompany($event)">
            <option
              v-for="item in companyData"
              :value="item.Code"
              :key="item.Id"
              :selected="item.Code===access.CompanyCode"
            >{{item.Name}}</option>
          </select>
        </f7-list-item>
        <f7-list-item
          link="#"
          :title="$t('common.department')"
          smart-select
          :smart-select-params="{
          formatValueText:formatValueText,
          openIn: 'popover',
        }"
        >
          <select multiple name="department" @change="changeDepartment($event)">
            <option
              :value="item.key"
              :key="item.key"
              v-for="item in departmentData"
              :selected="access.Departments.indexOf(item.key)>-1"
            >{{getDepartmentShow(item)}}</option>
          </select>
        </f7-list-item>
        <div class="list no-hairlines-md">
          <ul>
            <li class="item-content item-input">
              <div class="item-inner">
                <div class="item-title item-label">{{$t('common.user')}}</div>
                <div class="item-input-wrap">
                  <input
                    type="text"
                    :placeholder="$t('common.user')"
                    id="autocomplete-user"
                    :clear="true"
                  />
                </div>
              </div>
            </li>
          </ul>
        </div>
      </f7-list>
    </f7-page>
  </f7-popup>
</template>

<script>
export default {
  name: "access-component",
  props: ["show", "closed", "opened"],
  data() {
    return {
      access: {
        CompanyCode: "",
        Departments: [],
        Users: [],
      },
      companyData: null,
      departmentData: null,
      userData: [],
      pageIndex: 1,
      pageSize: 15,
      userEnd: false,
    };
  },
  created() {
    this.getCompanyData();
    this.getUserData();
  },
  methods: {
    autocompleteUser() {
      var that = this;
      this.$f7.autocomplete.create({
        inputEl: "#autocomplete-user",
        openIn: "dropdown",
        multiple: true,
        source(query, render) {
          const results = [];
          for (let i = 0; i < that.userData.length; i += 1) {
            if (
              that.userData[i].UserName.toLowerCase().indexOf(
                query.toLowerCase()
              ) >= 0
            )
              results.push(that.userData[i].UserName);
          }
          render(results);
        },
        on: {
          change: function (value) {
            console.log(value);
          },
        },
      });
    },
    formatValueText(values) {
      var arr = [];
      for (var i = 0; i < values.length; i++)
        arr.push(values[i].replace(/&nbsp;/gi, ""));
      return arr;
    },
    getDepartmentShow(item) {
      var str = "";
      for (var i = 0; i < item.layer; i++) str += "&nbsp;";
      return str + item.title;
    },
    changeCompany(event) {
      this.access.CompanyCode = event.target.value;
      this.getDepartmentData(event.target.value);
    },
    changeDepartment(event) {
      var depts = [];
      for (var i = 0; i < event.target.length; i++) {
        if (event.target[i].selected) depts.push(event.target[i].value);
      }
      this.access.Departments = depts;
    },
    getCompanyData() {
      this.$axios.get(this.$urls.company.getall).then((response) => {
        if (response.code === 0) {
          this.companyData = response.result;
          if (this.access.CompanyCode === "")
            this.access.CompanyCode = this.companyData[0].Code;
          this.getDepartmentData(this.access.CompanyCode);
        }
      });
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
            this.departmentData = dataList;
          }
        });
    },
    generateDepartmentList(data, dataList) {
      for (let i = 0; i < data.length; i++) {
        dataList.push({
          key: data[i].key,
          title: data[i].title,
          layer: data[i].Layer,
        });
        if (data[i].children) {
          this.generateDepartmentList(data[i].children, dataList);
        }
      }
    },
    getUserData(companyCode, replace) {
      if (!replace && this.userEnd) return;
      var url =
        this.$urls.user.getUrl +
        "?pageIndex=" +
        this.pageIndex +
        "&pageSize=" +
        this.pageSize;
      if (this.access.CompanyCode) {
        url += "&companyCode=" + this.access.CompanyCode;
      }
      this.$axios.get(url).then((data) => {
        if (data.code == 0) {
          if (data.result.length < this.pageSize) this.userEnd = true;
          if (replace) {
            this.userData = data.result;
          } else {
            this.userData = this.userData.concat(data.result);
          }
        }
      });
    },
  },
};
</script>

<style scoped>
.popup-add-access {
  height: 80%;
  bottom: 0px;
  top: auto;
}
</style>
