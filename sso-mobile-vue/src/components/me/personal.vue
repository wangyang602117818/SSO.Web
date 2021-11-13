<template>
  <f7-page name="personal">
    <f7-navbar
      :title="$t('manage.personal_info')"
      :back-link="$t('common.back')"
    >
      <f7-nav-right>
        <f7-link @click="save">{{ $t("common.save") }}</f7-link>
      </f7-nav-right>
    </f7-navbar>
    <f7-list inline-labels v-if="user.UserId && companyData && departmentData">
      <f7-list-item
        :title="$t('manage.user_id')"
        :after="user.UserId"
      ></f7-list-item>
      <f7-list-input
        :label="$t('manage.user_name') + '*'"
        type="text"
        :placeholder="$t('manage.user_name')"
        :error-message="$t('valid.user_name_required')"
        required
        validate
        clear-button
        :value="user.UserName"
        @input="
          ($event) => {
            user.UserName = $event.target.value;
          }
        "
      ></f7-list-input>
      <f7-list-item
        :title="$t('common.sex')"
        smart-select
        :smart-select-params="{ openIn: 'popover' }"
      >
        <select
          name="sex"
          @change="
            ($event) => {
              user.Sex = $event.target.value;
            }
          "
        >
          <option value="M" :selected="user.Sex === 'M'">
            {{ $t("common.male") }}
          </option>
          <option value="F" :selected="user.Sex === 'F'">
            {{ $t("common.female") }}
          </option>
        </select>
      </f7-list-item>
      <f7-list-input
        :label="$t('common.mobile')"
        type="text"
        validate
        pattern="[0-9]*"
        :placeholder="$t('common.mobile')"
        :value="user.Mobile"
        @input="
          ($event) => {
            user.Mobile = $event.target.value;
          }
        "
      ></f7-list-input>
      <f7-list-input
        :label="$t('common.email')"
        type="email"
        validate
        :placeholder="$t('common.email')"
        :error-message="$t('valid.email_format_invalid')"
        :value="user.Email"
        @input="
          ($event) => {
            user.Email = $event.target.value;
          }
        "
      ></f7-list-input>
      <f7-list-input
        :label="$t('common.idCard')"
        :placeholder="$t('common.idCard')"
        type="text"
        :value="user.IdCard"
        @input="
          ($event) => {
            user.IdCard = $event.target.value;
          }
        "
      ></f7-list-input>
      <f7-list-item
        :title="$t('common.company')"
        smart-select
        :smart-select-params="{ openIn: 'popover' }"
      >
        <select name="company" @change="changeCompany($event)">
          <option
            v-for="item in companyData"
            :value="item.Code"
            :key="item.Id"
            :selected="item.Code === user.CompanyCode"
          >
            {{ item.Name }}
          </option>
        </select>
      </f7-list-item>
      <f7-list-item
        link="#"
        :title="$t('common.department')"
        smart-select
        :smart-select-params="{
          formatValueText: formatValueText,
          openIn: 'popover',
        }"
      >
        <select multiple name="department" @change="changeDepartment($event)">
          <option
            :value="item.key"
            :key="item.key"
            v-for="item in departmentData"
            :selected="user.Departments.indexOf(item.key) > -1"
          >
            {{ getDepartmentShow(item) }}
          </option>
        </select>
      </f7-list-item>
      <f7-list-item
        :title="$t('common.role')"
        :after="user.Role.join(',')"
      ></f7-list-item>
      <f7-list-item
        :title="$t('manage.last_modified_time')"
        :after="user.UpdateTime"
      ></f7-list-item>
    </f7-list>
    <f7-block class="text-align-center" v-else>
      <f7-preloader></f7-preloader>
    </f7-block>
  </f7-page>
</template>

<script>
export default {
  name: "personal",
   props: {
    f7router: Object
  },
  data() {
    return {
      user: {
        id: this.$store.state.currentUser.Id,
        UserId: this.$store.state.currentUser.UserId,
        UserName: this.$store.state.currentUser.UserName,
        Sex: this.$store.state.currentUser.Sex,
        Mobile: this.$store.state.currentUser.Mobile,
        Email: this.$store.state.currentUser.Email,
        IdCard: this.$store.state.currentUser.IdCard,
        CompanyCode: this.$store.state.currentUser.CompanyCode,
        Departments: this.$store.state.currentUser.DepartmentCode,
        UpdateTime: this.$store.state.currentUser.UpdateTime,
        Role: this.$store.state.currentUser.Role,
      },
      companyData: null,
      departmentData: null,
    };
  },
  created() {
    this.getCompanyData();
  },
  methods: {
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
      this.user.CompanyCode = event.target.value;
      this.getDepartmentData(event.target.value);
    },
    changeDepartment(event) {
      var depts = [];
      for (var i = 0; i < event.target.length; i++) {
        if (event.target[i].selected) depts.push(event.target[i].value);
      }
      this.user.Departments = depts;
    },
    getCompanyData() {
      this.$axios.get(this.$urls.company.getall).then((response) => {
        if (response.code === 0) {
          this.companyData = response.result;
          if (this.user.CompanyCode && this.user.CompanyCode.trim() === "")
            this.user.CompanyCode = this.companyData[0].Code;
          this.getDepartmentData(this.user.CompanyCode);
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
    save() {
      if (this.user.UserId.trim() == "") return;
      if (this.user.UserName.trim() == "") return;
      this.$axios
        .post(this.$urls.user.updatebasicsetting, this.user)
        .then((response) => {
          if (response.code === 0) {
            this.$store.commit("getUser");
            this.f7router.back();
            this.showSuccess();
          }
        });
    },
  },
};
</script>

<style scoped>
.me_content {
  width: 90%;
  margin: 0 auto;
  overflow-y: scroll;
  padding-bottom: 30px;
}
</style>
