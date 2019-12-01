<template>
  <div class="overview">
    <div class="total_con">
      <div class="total_item total_item_80">
          <div class="">公司总数</div>
          <div class="">2</div>
      </div>
      <div class="total_item total_item_80"></div>
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
}
.total_item_240 {
  height: 240px;
}
</style>