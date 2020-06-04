<template>
  <div class="manage">
    <div class="man_top">
      <div class="man_title">管理中心</div>
      <div class="man_news">xx新增了一条用户信息</div>
    </div>
    <div class="man_record">
      <div class="man_record_item">
        <div class="man_record_num">12243</div>
        <div class="man_record_txt">总操作数</div>
      </div>
      <div class="man_record_item">
        <div class="man_record_num">123</div>
        <div class="man_record_txt">上月操作数</div>
      </div>
      <div class="man_record_item">
        <div class="man_record_num">13</div>
        <div class="man_record_txt">昨日操作数</div>
      </div>
    </div>
    <div class="man_content">
      <f7-row no-gap>
        <f7-col>
          <div class="nav_wrap" @click='$f7router.navigate("/users")'>
            <f7-icon f7="person_2_fill" color="blue"></f7-icon>
            <div class="nav_title">
              <span>用户管理</span>
            </div>
          </div>
        </f7-col>
        <f7-col>
          <div class="nav_wrap" @click='$f7router.navigate("/roles")'>
            <f7-icon f7="lock_circle" color="blue"></f7-icon>
            <div class="nav_title">
              <span>角色管理</span>
            </div>
          </div>
        </f7-col>
        <f7-col>
          <div class="nav_wrap" @click='$f7router.navigate("/company")'>
            <f7-icon f7="building_2_fill" color="blue"></f7-icon>
            <div class="nav_title">
              <span>公司管理</span>
            </div>
          </div>
        </f7-col>
        <f7-col>
          <div class="nav_wrap">
            <f7-icon f7="rectangle_3_offgrid_fill" color="blue"></f7-icon>
            <div class="nav_title">
              <span>部门管理</span>
            </div>
          </div>
        </f7-col>
      </f7-row>
      <f7-row no-gap>
        <f7-col>
          <div class="nav_wrap" @click='$f7router.navigate("/navigation")'>
            <f7-icon f7="flag_circle" color="blue"></f7-icon>
            <div class="nav_title">
              <span>导航管理</span>
            </div>
          </div>
        </f7-col>
        <f7-col>
          <div class="nav_wrap" @click='$f7router.navigate("/logs")'>
            <f7-icon f7="doc_text" color="blue"></f7-icon>
            <div class="nav_title">
              <span>日志列表</span>
            </div>
          </div>
        </f7-col>
        <f7-col></f7-col>
        <f7-col></f7-col>
      </f7-row>
    </div>
  </div>
</template>

<script>
export default {
  name: "manage",
  data() {
    return {
      datas: []
    };
  },
  computed: {},
  methods: {
    
  }
};
</script>

<style scoped>
.manage {
  height: 100%;
  display: flex;
  flex-direction: column;
  align-items: center;
}
.man_top {
  width: 90%;
}

.man_title {
  font-size: 16px;
  height: 35px;
  line-height: 35px;
}

.man_news {
  height: 40px;
  line-height: 40px;
  background-color: #fff;
  border-radius: 4px;
  padding-left: 6px;
}
.man_record {
  width: 90%;
  height: 80px;
  display: flex;
  background-color: #fff;
  border-radius: 4px;
  margin-top: 5px;
}

.man_record .man_record_item {
  flex: 1;
  display: flex;
  flex-direction: column;
}

.man_record .man_record_num {
  flex: 1;
  display: flex;
  align-items: flex-end;
  justify-content: center;
  font-size: 20px;
  font-weight: 600;
  padding: 2px;
}

.man_record .man_record_txt {
  flex: 1;
  display: flex;
  align-items: flex-start;
  justify-content: center;
  color: #949494;
  padding: 2px;
}

.man_content {
  width: 90%;
  margin-top: 5px;
  border-radius: 4px;
  overflow: hidden;
  background-color: #fff;
}
.nav_wrap {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 10px;
}
.nav_wrap:active {
  background-color: #ddd;
}
.nav_wrap .nav_title {
  height: 30px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 12px;
}
</style>
