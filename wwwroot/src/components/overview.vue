<template>
  <div class="overview">
    <div class="total_con">
      <div class="total_item total_item_80">
        <div class="total_item_txt">公司总数</div>
        <div class="total_item_numb">2</div>
      </div>
      <div class="total_item total_item_80">
           <div class="total_item_txt">部门总数</div>
        <div class="total_item_numb">21</div>
      </div>
      <div class="total_item total_item_80"></div>
      <div class="total_item total_item_80"></div>
    </div>
    <div class="total_con">
      <div class="total_item total_item_240"></div>
      <div class="total_item total_item_240"></div>
    </div>
  </div>
</template>
<script>
export default {
  name: "overview"
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
}
.total_item_80 {
  height: 80px;
  flex-direction: column;
}
.total_item_240 {
  height: 240px;
}
.total_item_txt {
  flex: 2;
  font-size: 14px;
  display: flex;
  align-items: center;
  border-bottom: 1px solid #EFF2F5;
  width: 100%;
  padding-left: 10px;
}
.total_item_numb {
  flex: 3;
  font-size: 16px;
  color: #1890ff;
  
  display: flex;
  align-items: center;
}
</style>