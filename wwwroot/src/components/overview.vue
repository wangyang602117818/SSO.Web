<template>
  <div class="overview">
    <div class="total_con">
      <div class="total_item total_item_80">
        <div class="total_item_txt">公司总数</div>
        <div class="total_item_numb">
          <a-spin size="small" v-if="total_loading" />
          {{total.Companys}}
        </div>
      </div>
      <div class="total_item total_item_80">
        <div class="total_item_txt">部门总数</div>
        <div class="total_item_numb">
          <a-spin size="small" v-if="total_loading" />
          <span>{{total.Departments}}</span>
        </div>
      </div>
      <div class="total_item total_item_80">
        <div class="total_item_txt">角色总数</div>
        <div class="total_item_numb">
          <a-spin size="small" v-if="total_loading" />
          <span>{{total.Roles}}</span>
        </div>
      </div>
      <div class="total_item total_item_80">
        <div class="total_item_txt">用户总数</div>
        <div class="total_item_numb">
          <a-spin size="small" v-if="total_loading" />
          <span>{{total.Users}}</span>
        </div>
      </div>
    </div>
    <div class="total_con">
      <div class="total_item total_item_240">
        <div class="total_item_txt">操作记录</div>
        <div class="total_item_data" id="op_record">
          <a-spin size="small" v-if="op_record_loading" />
        </div>
      </div>
      <div class="total_item total_item_240">
        <div class="total_item_txt">用户录入</div>
        <div class="total_item_data" id="user_record">
          <a-spin size="small" v-if="op_record_loading" />
        </div>
      </div>
    </div>
  </div>
</template>
<script>
export default {
  name: "overview",
  data() {
    return {
      total_loading: false,
      op_record_loading: false,
      user_record_loading: false,
      total: {}
    };
  },
  created() {
    this.getData();
    this.getOpRecord();
    this.getUserRecord();
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
      this.op_record_loading = true;
      this.$http.get(this.$urls.overview.opRecord).then(response => {
        this.op_record_loading = false;
        if (response.body.code == 0) {
        }
      });
    },
    getUserRecord() {
      this.user_record_loading = true;
      this.$http.get(this.$urls.overview.userRecord).then(response => {
        this.user_record_loading = false;
        if (response.body.code == 0) {
        }
      });
    }
  }
};
</script>
<style scoped>
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
}
.total_item_txt {
  height: 30px;
  font-size: 14px;
  display: flex;
  align-items: center;
  border-bottom: 1px solid #eff2f5;
  width: 100%;
  padding-left: 10px;
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
</style>