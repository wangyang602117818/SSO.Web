<template>
  <div class="me">
    <div class="me_text">我的</div>
    <div class="me_title" @click="$f7router.navigate('/personal/9')">
      <div class="ico">
        <f7-icon f7="person" color="blue" size="35"></f7-icon>
      </div>
      <div class="desc">
        <div class="name">Yang Xa Wang</div>
        <div class="detail">reader | admin</div>
      </div>
      <div class="arrow">
        <f7-icon f7="chevron_right" color="gray" size="25"></f7-icon>
      </div>
    </div>
    <div class="me_settings">
      <f7-list media-list>
        <f7-list-item title="Yellow Submarine" subtitle="Beatles">
          <img
            slot="media"
            src="https://cdn.framework7.io/placeholder/fashion-88x88-1.jpg"
            width="44"
          />
        </f7-list-item>
        <f7-list-item link="#" title="Don't Stop Me Now" subtitle="Queen">
          <img
            slot="media"
            src="https://cdn.framework7.io/placeholder/fashion-88x88-2.jpg"
            width="44"
          />
        </f7-list-item>
        <f7-list-item title="Billie Jean" subtitle="Michael Jackson">
          <img
            slot="media"
            src="https://cdn.framework7.io/placeholder/fashion-88x88-3.jpg"
            width="44"
          />
        </f7-list-item>
      </f7-list>
    </div>
  </div>
</template>

<script>
export default {
  name: "me",
  data() {
    return {};
  },
  methods: {}
};
</script>

<style scoped>
.me {
  margin: 0 auto;
  width: 90%;
  align-items: center;
  /* border: 1px solid transparent; */
}
.me_text {
  font-size: 16px;
  height: 35px;
  line-height: 35px;
  padding-left: 5px;
}
.me_title {
  height: 50px;
  margin-top: 5px;
  overflow: hidden;
  display: flex;
  flex-direction: row;
  border-bottom: 1px solid #e3e4e4;
}
.me_title:active {
  background-color: #ddd;
}
.me_title .ico {
  width: 45px;
  display: flex;
  align-items: center;
  justify-content: flex-start;
}
.me_title .desc {
  flex: 1;
  display: flex;
  flex-direction: column;
}

.me_title .arrow {
  display: flex;
  justify-content: flex-start;
  align-items: center;
  width: 30px;
}
.desc .name {
  height: 25px;
  line-height: 25px;
  font-weight: bold;
}
.desc .detail {
  flex: 1;
  display: flex;
}
.media-list .item-inner {
  border-bottom: 1px solid red;
}
.list li {
  /* border-bottom: 1px solid #e3e4e4; */
}
</style>
