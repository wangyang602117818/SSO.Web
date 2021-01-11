<template>
  <a-layout>
    <a-layout-sider theme="light">
      <a-input-search
        :placeholder="$t('search')"
        v-model="companySearchValue"
        @search="onSearchCompany"
      />
      <a-menu mode="inline" :defaultSelectedKeys="defaultSelectedCompany" @select="selectCompany">
        <a-spin size="small" v-if="company_loading" style="width:100%" />
        <a-menu-item
          v-for="item in company"
          v-bind:key="item.Code"
          v-bind:code="item.Code"
        >{{item.Name}}</a-menu-item>
      </a-menu>
    </a-layout-sider>
    <a-layout-content>
      <a-input-search
        :placeholder="$t('search')"
        style="width:50%"
        @change="onSearchDepartment"
      />
      <a-button type="default" icon="plus" @click="showDrawer" :title="$t('add_top_dept')"></a-button>
      <a-button
        type="default"
        :icon="expandedAll?'fullscreen-exit':'fullscreen'"
        @click="expandAll"
        :title="$t('expand_collapse')"
      ></a-button>
      <div class="department_wrap">
        <a-spin size="small" v-if="department_loading" style=" width:100%" />
        <a-tree
          :expandedKeys="expandedKeys"
          :autoExpandParent="autoExpandParent"
          @expand="onExpand"
          :treeData="departmentData"
          @select="selectDepartment"
          style="border:0"
        >
          <template slot="custom" slot-scope="{title}">
            <span v-if="title.indexOf(departmentSearchValue) > -1">
              {{title.substr(0, title.indexOf(departmentSearchValue))}}
              <span
                style="color: #f50"
              >{{departmentSearchValue}}</span>
              {{title.substr(title.indexOf(departmentSearchValue) + departmentSearchValue.length)}}
            </span>
            <span v-else>{{title}}</span>
          </template>
        </a-tree>
      </div>
    </a-layout-content>
    <a-layout-sider theme="light" :width="400" :collapsed="collapsedLeft" :collapsedWidth="0">
      <a-tabs defaultActiveKey="1" @change="changeTab">
        <a-tab-pane :tab="$t('add_sub_dept')" key="1" forceRender>
          <a-form :form="addform" @submit.prevent="addSubDept">
            <a-form-item
              :label="$t('code')"
              :label-col="{ span: 6 }"
              :wrapper-col="{ span: 12 }"
            >
              <a-input
                v-decorator="['code', {rules: [{ required: true, message: $t('dept_code_required')}]}]"
              >
                <a-icon slot="addonAfter" type="reload" @click="getRandomCodeSub" />
              </a-input>
            </a-form-item>
            <a-form-item
              :label="$t('name')"
              :label-col="{ span: 6 }"
              :wrapper-col="{ span: 12 }"
            >
              <a-input
                v-decorator="['Name', {rules: [{ required: true, message: $t('dept_name_required') }]}]"
              ></a-input>
            </a-form-item>
            <a-form-item
              :label="$t('order')"
              :label-col="{ span: 6 }"
              :wrapper-col="{ span: 12 }"
            >
              <a-input-number v-decorator="['order', { initialValue: 0 }]" />
            </a-form-item>
            <a-form-item
              :label="$t('description')"
              :label-col="{ span: 6 }"
              :wrapper-col="{ span: 12 }"
            >
              <a-textarea
                :autoSize="{ minRows: 3, maxRows: 5 }"
                v-decorator="['description',{rules: [{ required: false, message: $t('dept_description_required') }]}]"
              />
            </a-form-item>
            <a-form-item :wrapper-col="{ span: 12, offset: 6 }">
              <a-button @click="addform.resetFields();">{{$t('reset')}}</a-button>
              <a-button type="primary" html-type="submit">{{$t('submit')}}</a-button>
            </a-form-item>
          </a-form>
        </a-tab-pane>
        <a-tab-pane :tab="$t('update')" key="2" forceRender>
          <a-form :form="updateform" @submit.prevent="updateSubDept">
            <a-form-item
              :label="$t('code')"
              :label-col="{ span: 8 }"
              :wrapper-col="{ span: 12 }"
            >
              <a-input
                v-decorator="['code', {rules: [{ required: true, message: $t('dept_code_required') }]}]"
              >
                <a-icon slot="addonAfter" type="reload" @click="getRandomCodeUpdate" />
              </a-input>
            </a-form-item>
            <a-form-item
              :label="$t('name')"
              :label-col="{ span: 8 }"
              :wrapper-col="{ span: 12 }"
            >
              <a-input
                v-decorator="['name', {rules: [{ required: true, message: $t('dept_name_required') }]}]"
              ></a-input>
            </a-form-item>
            <a-form-item
              :label="$t('comp')"
              :label-col="{ span: 8 }"
              :wrapper-col="{ span: 12 }"
            >
              <a-select
                v-decorator="[ 'companyCode', {rules: [{ required: true, message: $t('company_required') }]}]"
                @change="changeCompany"
                disabled
              >
                <a-select-option
                  :value="item.Code"
                  v-for="item in company"
                  v-bind:key="item.Id"
                >{{item.Name}}</a-select-option>
              </a-select>
            </a-form-item>
            <a-form-item
              :label="$t('sup_dept')"
              :label-col="{ span: 8}"
              :wrapper-col="{ span: 12 }"
            >
              <a-tree-select
                :treeData="departmentData"
                v-decorator="[ 'parentCode', {rules: [{ required: false }]}]"
                treeDefaultExpandAll
                allowClear
              ></a-tree-select>
            </a-form-item>
            <a-form-item
              :label="$t('order')"
              :label-col="{ span: 8 }"
              :wrapper-col="{ span: 12 }"
            >
              <a-input-number v-decorator="['order', { initialValue: 0 }]" />
            </a-form-item>
            <a-form-item
              :label="$t('description')"
              :label-col="{ span: 8 }"
              :wrapper-col="{ span: 12 }"
            >
              <a-textarea
                :autoSize="{ minRows: 3, maxRows: 5 }"
                v-decorator="['description',{rules: [{ required: false, message: $t('dept_description_required')}]}]"
              />
            </a-form-item>
            <a-form-item :wrapper-col="{ span: 8, offset: 6 }">
              <a-button type="primary" html-type="submit">{{$t('submit')}}</a-button>
            </a-form-item>
          </a-form>
        </a-tab-pane>
        <a-tab-pane :tab="$t('delete')" key="3" forceRender>
          <a-popconfirm
            :title="$t('sure_delete_dept')"
            @confirm="delDept"
            :okText="$t('yes')"
            :cancelText="$t('no')"
          >
            <a-button type="danger">{{$t('delete')}}</a-button>
          </a-popconfirm>
        </a-tab-pane>
      </a-tabs>
    </a-layout-sider>
    <a-drawer
      :title="$t('add_top_dept')"
      :width="360"
      @close="drawerVisible=false"
      :visible="drawerVisible"
    >
      <a-form :form="form" layout="vertical" @submit.prevent="handleTopSubmit">
        <a-row :gutter="0">
          <a-col :span="24">
            <a-form-item :label="$t('code')">
              <a-input
                v-decorator="['code',{rules: [{ required: true, message: $t('dept_code_required') }]}]"
              >
                <a-icon slot="addonAfter" type="reload" @click="getRandomCode" />
              </a-input>
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="0">
          <a-col :span="24">
            <a-form-item :label="$t('name')">
              <a-input
                v-decorator="['name',{rules: [{ required: true, message: $t('dept_name_required')}]}]"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="0">
          <a-col :span="24">
            <a-form-item :label="$t('order')">
              <a-input-number v-decorator="['order', { initialValue: 0 }]" />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="0">
          <a-col :span="24">
            <a-form-item :label="$t('description')">
              <a-textarea
                :autoSize="{ minRows: 4, maxRows: 6 }"
                v-decorator="['description',{rules: [{ required: false, message: $t('dept_description_required') }]}]"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="0">
          <a-col :span="24">
            <a-button @click="form.resetFields();">{{$t('reset')}}</a-button>
            <a-button type="primary" html-type="submit">{{$t('submit')}}</a-button>
          </a-col>
        </a-row>
      </a-form>
    </a-drawer>
  </a-layout>
</template>
<script>
var getParentKey = (key, tree) => {
  let parentKey;
  for (let i = 0; i < tree.length; i++) {
    const node = tree[i];
    if (node.children) {
      if (node.children.some(item => item.key === key)) {
        parentKey = node.key;
      } else if (getParentKey(key, node.children)) {
        parentKey = getParentKey(key, node.children);
      }
    }
  }
  return parentKey;
};
var dataList = [];
var generateList = data => {
  for (let i = 0; i < data.length; i++) {
    const node = data[i];
    const key = node.key;
    dataList.push({ key, title: node.title });
    if (node.children) {
      generateList(node.children, node.key);
    }
  }
};
export default {
  data() {
    return {
      company: [],
      companyPageIndex: 1,
      companySearchValue: "",
      selectedCompany: "",
      defaultSelectedCompany: [],

      id: "", //选中的部门id
      selectedDepartment: "", //选中部门的code
      selectedDepartmentLayer: 0, //选中部门的层级
      defaultExpandDepartment: true,

      form: this.$form.createForm(this),
      addform: this.$form.createForm(this),
      updateform: this.$form.createForm(this),

      departmentData: [],
      departmentSearchValue: "",
      drawerVisible: false,
      company_loading: false,
      department_loading: false,
      expandedAll: false,
      autoExpandParent: false,
      collapsedLeft: true,
      expandedKeys: []
    };
  },
  created() {
    this.getCompanyData();
  },
  methods: {
    getCompanyData() {
      this.company_loading = true;
      this.$axios
        .get(this.$urls.company.getall + "?filter=" + this.companySearchValue)
        .then(response => {
          this.company_loading = false;
          if (response.code == 0) {
            this.company = response.result;
            if (response.count > 0) {
              this.selectedCompany = response.result[0].Code;
              this.defaultSelectedCompany.splice(0, 1, this.selectedCompany);
              this.getDepartmentData(this.selectedCompany);
            }
          }
        });
    },
    onSearchCompany() {
      this.getCompanyData();
    },
    onSearchDepartment(e) {
      const value = e.target.value;
      const expandedKeys = dataList
        .map(item => {
          if (item.title.toLowerCase().indexOf(value.toLowerCase()) > -1) {
            return getParentKey(item.key, this.departmentData);
          }
          return null;
        })
        .filter((item, i, self) => item && self.indexOf(item) === i);
      Object.assign(this, {
        expandedKeys,
        departmentSearchValue: value,
        autoExpandParent: true
      });
    },
    changeCompany(value){
      // this.getDepartmentData(value);
    },
    getDepartmentData(companyCode) {
      this.department_loading = true;
      this.departmentData = [];
      dataList = [];
      this.$axios
        .get(
          this.$urls.department.getdepartments + "?companyCode=" + companyCode
        )
        .then(response => {
          this.department_loading = false;
          if (response.code == 0) {
            if (response.result.length > 0) {
              this.departmentData = response.result;
              generateList(this.departmentData);
            } else {
              this.departmentData = [];
            }
          }
        });
    },
    deepTraversa(datas, list = []) {
      datas.forEach(item => {
        if (item.children.length > 0) {
          list.push(item.key);
          for (let i = 0; i < item.children.length; i++) {
            this.deepTraversa(item.children, list);
          }
        }
      });
      return list;
    },
    expandAll() {
      var keys = this.deepTraversa(this.departmentData);
      this.expandedAll = !this.expandedAll;
      if (this.expandedAll) {
        this.expandedKeys = keys;
      } else {
        this.expandedKeys = [];
      }
    },
    onExpand(expandedKeys, obj) {
      var key = obj.node.value;
      if (this.expandedKeys.indexOf(key) == -1) {
        this.expandedKeys.push(key);
      } else {
        this.$funtools.removeArrayItem(this.expandedKeys, key);
      }
    },
    getRandomCode() {
      this.form.setFieldsValue({ code: this.$funtools.randomWord(12, 12) });
    },
    getRandomCodeSub() {
      this.addform.setFieldsValue({ code: this.$funtools.randomWord(12, 12) });
    },
    getRandomCodeUpdate() {
      this.updateform.setFieldsValue({
        code: this.$funtools.randomWord(12, 12)
      });
    },
    selectCompany(item) {
      this.selectedCompany = item.key;
      this.getDepartmentData(this.selectedCompany);
    },
    selectDepartment(selectedKeys) {
      if (selectedKeys.length > 0) {
        this.getRandomCodeSub();
        this.addform.setFieldsValue({ description: "" });
        this.collapsedLeft = false;
        this.$axios
          .get(this.$urls.department.get + "?code=" + selectedKeys[0])
          .then(response => {
            if (response.code == 0) {
              this.id = response.result.Id;
              this.selectedDepartment = response.result.key;
              this.selectedDepartmentLayer = response.result.Layer;
              this.updateform.setFieldsValue({
                code: response.result.key,
                name: response.result.title,
                order: response.result.Order,
                companyCode: this.selectedCompany,
                parentCode: response.result.ParentCode,
                description: response.result.Description
              });
            }
          });
      } else {
        this.collapsedLeft = true;
        this.id = "";
        this.selectedDepartment = "";
        this.selectedDepartmentLayer = 0;
        this.updateform.setFieldsValue({
          code: "",
          name: "",
          order: "",
          description: ""
        });
      }
    },
    changeTab() {},
    showDrawer() {
      this.drawerVisible = true;
      this.$nextTick(() => {
        this.getRandomCode();
      });
    },
    addSubDept() {
      this.addform.validateFields((error, values) => {
        if (!error) {
          values.companyCode = this.selectedCompany;
          values.parentCode = this.selectedDepartment;
          values.layer = this.selectedDepartmentLayer + 1;
          this.$axios.post(this.$urls.department.add, values).then(response => {
            if (response.code == 0) {
              this.getDepartmentData(this.selectedCompany);
            }
          });
        }
      });
    },
    updateSubDept() {
      this.updateform.validateFields((error, values) => {
        if (!error) {
          values.id = this.id;
          values.companyCode = this.selectedCompany;
          values.parentCode = values.parentCode || "";
          this.$axios
            .post(this.$urls.department.update, values)
            .then(response => {
              if (response.code == 0) {
                this.getDepartmentData(this.selectedCompany);
              }
            });
        }
      });
    },
    delDept() {
      if (this.id) {
        this.$axios
          .get(this.$urls.department.delete + "/" + this.id)
          .then(response => {
            if (response.code == 0) {
              this.id = "";
              this.selectedDepartment = "";
              this.getDepartmentData(this.selectedCompany);
            }
          });
      }
    },
    handleTopSubmit() {
      this.form.validateFields((err, values) => {
        if (!err) {
          values.companyCode = this.selectedCompany;
          values.layer = 0;
          values.parentCode = "";
          this.$axios.post(this.$urls.department.add, values).then(response => {
            if (response.code == 0) {
              this.form.resetFields();
              this.getDepartmentData(this.selectedCompany);
            }
          });
        }
      });
    }
  }
};
</script>
<style scoped>
.ant-layout-sider {
  background-color: transparent;
}
.ant-tree {
  border: 1px solid #e8e8e8;
  background-color: #fff;
}
.anticon-reload {
  cursor: pointer;
}
.anticon-reload:hover {
  color: #000;
}
.ant-tabs {
  background-color: #fff;
  margin-top: 0px;
  padding-bottom: 20px;
}
.department_wrap {
  background-color: #fff;
  min-height: 30px;
  line-height: 30px;
}
.top_layout .ant-layout-content{
  padding-left: 10px;
  border-right:1px solid #e8e8e8;
}
.ant-menu {
  background-color: #fff;
}
</style>