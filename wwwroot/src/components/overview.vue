<template>
  <div class="overview">
    <div class="total_con">
      <div class="total_item total_item_80">
        <div class="total_item_wrap">
          <div class="total_item_txt">{{this.$lang.companys}}</div>
          <div class="total_item_title"></div>
          <div class="total_item_op">
            <a-icon type="sync" size="small" @click="getData" />
          </div>
        </div>
        <div class="total_item_numb">
          <a-spin size="small" v-if="total_loading" />
          <span v-else>{{total.Companys}}</span>
        </div>
      </div>
      <div class="total_item total_item_80">
        <div class="total_item_wrap">
          <div class="total_item_txt">{{this.$lang.departments}}</div>
          <div class="total_item_title"></div>
          <div class="total_item_op">
            <a-icon type="sync" size="small" @click="getData" />
          </div>
        </div>
        <div class="total_item_numb">
          <a-spin size="small" v-if="total_loading" />
          <span v-else>{{total.Departments}}</span>
        </div>
      </div>
      <div class="total_item total_item_80">
        <div class="total_item_wrap">
          <div class="total_item_txt">{{this.$lang.roles}}</div>
          <div class="total_item_title"></div>
          <div class="total_item_op">
            <a-icon type="sync" size="small" @click="getData" />
          </div>
        </div>
        <div class="total_item_numb">
          <a-spin size="small" v-if="total_loading" />
          <span v-else>{{total.Roles}}</span>
        </div>
      </div>
      <div class="total_item total_item_80">
        <div class="total_item_wrap">
          <div class="total_item_txt">{{this.$lang.users}}</div>
          <div class="total_item_title"></div>
          <div class="total_item_op">
            <a-icon type="sync" size="small" @click="getData" />
          </div>
        </div>
        <div class="total_item_numb">
          <a-spin size="small" v-if="total_loading" />
          <span v-else>{{total.Users}}</span>
        </div>
      </div>
    </div>
    <div class="total_con">
      <div class="total_item total_item_240">
        <div class="total_item_wrap">
          <div class="total_item_txt">{{this.$lang.op_record}}</div>
          <div class="total_item_title"></div>
          <div class="total_item_op">
            <a-icon type="sync" size="small" @click="getOpRecord" />
          </div>
        </div>
        <a-spin size="small" v-if="op_record_loading" />
        <div class="total_item_data" ref="op_record"></div>
      </div>
      <div class="total_item total_item_240">
        <div class="total_item_wrap">
          <div class="total_item_txt">{{this.$lang.user_record}}</div>
          <div class="total_item_title">
            <span class="line line_0053FE"></span> {{this.$lang.input}}
            <span class="line line_00C782"></span> {{this.$lang.delete}}
          </div>
          <div class="total_item_op">
            <a-icon type="sync" size="small" @click="getUserRecord" />
          </div>
        </div>
        <a-spin size="small" v-if="user_record_loading" />
        <div class="total_item_data" ref="user_record"></div>
      </div>
    </div>
    <div class="total_con">
      <div class="total_item total_item_260">
        <div class="total_item_wrap">
          <div class="total_item_txt">{{this.$lang.sex_ratio}}</div>
          <div class="total_item_title"></div>
          <div class="total_item_op">
            <a-icon type="sync" size="small" @click="getUserRatio" />
          </div>
        </div>
        <a-spin size="small" v-if="user_ratio_loading" />
        <div class="total_item_data" ref="user_ratio"></div>
      </div>
      <div class="total_item total_item_260">
        <div class="total_item_wrap">
          <div class="total_item_txt">{{this.$lang.company_ratio}}</div>
          <div class="total_item_title"></div>
          <div class="total_item_op">
            <a-icon type="sync" size="small" @click="getUserCompanyRatio" />
          </div>
        </div>
        <a-spin size="small" v-if="user_company_loading" />
        <div class="total_item_data" ref="user_company_ratio"></div>
      </div>
      <div class="total_item total_item_260">
        <div class="total_item_wrap">
          <div class="total_item_txt">{{this.$lang.department_ratio}}</div>
          <div class="total_item_title"></div>
          <div class="total_item_op">
            <a-icon type="sync" size="small" @click="getUserDepartmentRatio" />
          </div>
        </div>
        <a-spin size="small" v-if="user_department_loading" />
        <div class="total_item_data" ref="user_department_ratio"></div>
      </div>
    </div>
  </div>
</template>
<script>
import echarts from "echarts";
export default {
  name: "overview",
  data() {
    return {
      total_loading: false,
      op_record_loading:false,
      user_record_loading:false,
      user_ratio_loading:false,
      user_company_loading:false,
      user_department_loading:false,
      total: {},
      opRecordChart: null,
      userRecordChart: null,
      userRatioChart: null,
      userCompanyChart: null,
      userDepartmentChart: null
    };
  },
  created() {},
  mounted() {
    this.getData();
    this.getOpRecord();
    this.getUserRecord();
    this.getUserRatio();
    this.getUserCompanyRatio();
    this.getUserDepartmentRatio();
  },
  methods: {
    getData() {
      this.total_loading = true;
      this.$http.get(this.$urls.overview.total).then(response => {
        this.total_loading = false;
        if (response.body.code == 0) this.total = response.body.result;
      });
    },
    getOpRecord() {
      this.op_record_loading=true;
      this.$http.get(this.$urls.overview.opRecord).then(response => {
        this.op_record_loading=false;
        if (!this.opRecordChart) {
          this.opRecordChart = echarts.init(this.$refs.op_record);
        }
        if (response.body.code == 0) {
          var dateList = response.body.result.map(function(item) {
            return item["date"];
          });
          var countList = response.body.result.map(function(item) {
            return item["count"];
          });
          // 绘制图表。
          var options = this.$common.echartOptionsLine(dateList);
          options.series = [
            {
              data: countList,
              type: "line",
              symbol: "circle",
              lineStyle: {
                width: 1
              }
            }
          ];
          this.opRecordChart.clear();
          this.opRecordChart.setOption(options);
        }
      });
    },
    getUserRecord() {
      this.user_record_loading=true;
      this.$http.get(this.$urls.overview.userRecord).then(response => {
        this.user_record_loading=false;
        if (!this.userRecordChart) {
          this.userRecordChart = echarts.init(this.$refs.user_record);
        }
        if (response.body.code == 0) {
          var dateList = Array.from(
            new Set(
              response.body.result.map(function(item) {
                return item["date"];
              })
            )
          );
          var addList = [];
          var delList = [];
          response.body.result.forEach(function(currentValue) {
            if (currentValue["type"] == "insert")
              addList.push([currentValue["date"], currentValue["count"]]);
            if (currentValue["type"] == "delete")
              delList.push([currentValue["date"], currentValue["count"]]);
          });
          var options = this.$common.echartOptionsLine(dateList);
          options.series = [
            {
              name: this.$lang.input,
              type: "line",
              symbol: "circle",
              lineStyle: {
                width: 1
              },
              data: addList
            },
            {
              name: this.$lang.delete,
              type: "line",
              symbol: "circle",
              lineStyle: {
                width: 1
              },
              data: delList
            }
          ];
          this.userRecordChart.clear();
          this.userRecordChart.setOption(options);
        }
      });
    },
    getUserRatio() {
      this.user_ratio_loading=true;
      this.$http.get(this.$urls.overview.userRatio).then(response => {
        this.user_ratio_loading=false;
        if (!this.userRatioChart) {
          this.userRatioChart = echarts.init(this.$refs.user_ratio);
        }
        if (response.body.code == 0) {
          var options = this.$common.echartOptionsPie();
          var data = [];
          var that =this;
          response.body.result.forEach(function(currentValue) {
            if (currentValue["type"] == "M") {
              data.push({ value: currentValue.count, name: that.$lang.male});
            } else {
              data.push({ value: currentValue.count, name: that.$lang.female });
            }
          });
          options.series = [
            {
              type: "pie",
              data: data
            }
          ];
          this.userRatioChart.clear();
          this.userRatioChart.setOption(options);
        }
      });
    },
    getUserCompanyRatio() {
      this.user_company_loading=true;
      this.$http.get(this.$urls.overview.userCompanyRatio).then(response => {
        this.user_company_loading=false;
        if (!this.userCompanyChart) {
          this.userCompanyChart = echarts.init(this.$refs.user_company_ratio);
        }
        if (response.body.code == 0) {
          var options = this.$common.echartOptionsPie();
          var data = [];
          response.body.result.forEach(function(currentValue) {
            data.push({ value: currentValue.count, name: currentValue.type });
          });
          options.series = [
            {
              type: "pie",
              data: data
            }
          ];
          this.userCompanyChart.clear();
          this.userCompanyChart.setOption(options);
        }
      });
    },
    getUserDepartmentRatio() {
      this.user_department_loading=true;
      this.$http.get(this.$urls.overview.userDepartmentRatio).then(response => {
        this.user_department_loading=false;
        if (!this.userDepartmentChart) {
          this.userDepartmentChart = echarts.init(
            this.$refs.user_department_ratio
          );
        }
        if (response.body.code == 0) {
          var options = this.$common.echartOptionsPie();
          var data = [];
          response.body.result.forEach(function(currentValue) {
            data.push({ value: currentValue.count, name: currentValue.type });
          });
          options.series = [
            {
              type: "pie",
              data: data
            }
          ];
          this.userDepartmentChart.clear();
          this.userDepartmentChart.setOption(options);
        }
      });
    }
  }
};
</script>
<style scoped>
.overview {
  margin-bottom: 50px;
}
.total_con {
  display: flex;
  margin-top: 10px;
  margin-left: -10px;
}
.total_item {
  flex: 1;
  background-color: #fff;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-left: 10px;
  border: 1px solid #eff2f5;
}
.total_item_80 {
  height: 80px;
  flex-direction: column;
}
.total_item_240 {
  height: 240px;
  flex-direction: column;
  position: relative;
}
.total_item_260 {
  height: 260px;
  flex-direction: column;
}
.total_item_wrap {
  height: 30px;
  font-size: 14px;
  display: flex;
  align-items: center;
  border-bottom: 1px solid #eff2f5;
  width: 100%;
  padding-left: 10px;
  padding-right: 10px;
}
.total_item_wrap .total_item_txt {
  flex: 1;
}
.total_item_wrap .total_item_title {
  display: flex;
  justify-content: center;
  align-items: center;
  font-size: 11px;
}
.total_item_wrap .total_item_op {
  flex: 1;
  text-align: right;
}
.anticon {
  font-size: 12px;
}
.anticon:hover {
  cursor: pointer;
  color: #000;
}
.line {
  display: inline-block;
  border: 0;
  height: 2px;
  width: 20px;
  margin-left: 5px;
  margin-right: 2px;
}
.line_0053FE {
  background-color: #0053fe;
}
.line_00C782 {
  background-color: #00c782;
}
.total_item_numb {
  flex: 1;
  font-size: 16px;
  color: #1890ff;
  display: flex;
  align-items: center;
}
.total_item_data {
  flex: 1;
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
}
.ant-spin-spinning {
  position: absolute;
}
</style>