(function(t){function e(e){for(var a,i,o=e[0],l=e[1],u=e[2],c=0,p=[];c<o.length;c++)i=o[c],s[i]&&p.push(s[i][0]),s[i]=0;for(a in l)Object.prototype.hasOwnProperty.call(l,a)&&(t[a]=l[a]);d&&d(e);while(p.length)p.shift()();return n.push.apply(n,u||[]),r()}function r(){for(var t,e=0;e<n.length;e++){for(var r=n[e],a=!0,o=1;o<r.length;o++){var l=r[o];0!==s[l]&&(a=!1)}a&&(n.splice(e--,1),t=i(i.s=r[0]))}return t}var a={},s={index:0},n=[];function i(e){if(a[e])return a[e].exports;var r=a[e]={i:e,l:!1,exports:{}};return t[e].call(r.exports,r,r.exports,i),r.l=!0,r.exports}i.m=t,i.c=a,i.d=function(t,e,r){i.o(t,e)||Object.defineProperty(t,e,{enumerable:!0,get:r})},i.r=function(t){"undefined"!==typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(t,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(t,"__esModule",{value:!0})},i.t=function(t,e){if(1&e&&(t=i(t)),8&e)return t;if(4&e&&"object"===typeof t&&t&&t.__esModule)return t;var r=Object.create(null);if(i.r(r),Object.defineProperty(r,"default",{enumerable:!0,value:t}),2&e&&"string"!=typeof t)for(var a in t)i.d(r,a,function(e){return t[e]}.bind(null,a));return r},i.n=function(t){var e=t&&t.__esModule?function(){return t["default"]}:function(){return t};return i.d(e,"a",e),e},i.o=function(t,e){return Object.prototype.hasOwnProperty.call(t,e)},i.p="";var o=window["webpackJsonp"]=window["webpackJsonp"]||[],l=o.push.bind(o);o.push=e,o=o.slice();for(var u=0;u<o.length;u++)e(o[u]);var d=l;n.push([0,"chunk-vendors"]),r()})({0:function(t,e,r){t.exports=r("7d36")},1:function(t,e){},"7d36":function(t,e,r){"use strict";r.r(e);r("cadf"),r("551c"),r("f751"),r("097d");var a=r("a026"),s=r("28dd"),n=function(){var t=this,e=this,r=e.$createElement,a=e._self._c||r;return a("div",{staticClass:"container"},[a("div",{staticClass:"top"},[a("a-row",{attrs:{type:"flex",justify:"center",align:"middle"}},[a("a-col",{staticStyle:{"text-align":"right","padding-right":"20px"},attrs:{span:24}},[a("a-dropdown",[e.user?a("a",{staticClass:"ant-dropdown-link",attrs:{href:"#"}},[e._v("\n            "+e._s(e.user)+"\n            "),a("a-icon",{attrs:{type:"down"}})],1):a("a",{staticClass:"ant-dropdown-link",attrs:{href:"#"},on:{click:function(){return t.login_visible=!0}}},[e._v("登录")]),e.user?a("a-menu",{attrs:{slot:"overlay"},slot:"overlay"},[a("a-menu-item",{key:"0"},[a("a",{attrs:{target:"_self",href:this.$urls.logout}},[e._v("退出")])])],1):e._e()],1)],1)],1)],1),a("div",{staticClass:"main"},[a("a-row",{attrs:{justify:"center",align:"middle"}},[a("a-col",{attrs:{span:8,offset:8}},[a("a-tabs",{attrs:{defaultActiveKey:"1"}},[a("a-tab-pane",{key:"1",attrs:{closable:!1}},[a("span",{attrs:{slot:"tab"},slot:"tab"},[a("a-icon",{attrs:{type:"home"}}),e._v("导航\n            ")],1),a("a-row",{attrs:{type:"flex",justify:"space-between"}},e._l(e.urls,function(t){return a("a",{key:t.Title,staticClass:"site",attrs:{target:"_blank",href:t.BaseUrl,title:t.BaseUrl}},[a("div",{staticClass:"logo"},[t.IconUrl?a("img",{attrs:{alt:"icon",src:t.IconUrl}}):a("span",[a("a-avatar",{staticStyle:{backgroundColor:"#3498DB"},attrs:{size:"large"}},[e._v(e._s(t.Title.getFileName(1).trimEnd(".")))])],1)]),a("div",{staticClass:"title",attrs:{title:t.Title}},[e._v(e._s(t.Title.getFileName(3)))])])}),0)],1),a("a-icon",{style:{fontSize:"16px",cursor:"pointer"},attrs:{slot:"tabBarExtraContent",type:"plus"},on:{click:function(){return t.addurl_visible=!t.addurl_visible}},slot:"tabBarExtraContent"})],1),e.addurl_visible?a("div",{staticClass:"addurl"},[a("a-form",{attrs:{form:e.addUrl_form,layout:"inline"},on:{submit:function(t){return t.preventDefault(),e.addUrl(t)}}},[a("a-form-item",[a("a-input",{directives:[{name:"decorator",rawName:"v-decorator",value:["title",{rules:[{required:!0,message:"Please input your title!"}]}],expression:"['title',{ rules: [{ required: true, message: 'Please input your title!' }] }]"}],staticStyle:{width:"120px"},attrs:{placeholder:"名称"}})],1),a("a-form-item",[a("a-input",{directives:[{name:"decorator",rawName:"v-decorator",value:["baseUrl",{rules:[{required:!0,message:"Please input your url!"}]}],expression:"['baseUrl',{ rules: [{ required: true, message: 'Please input your url!' }]}]"}],staticStyle:{width:"180px"},attrs:{placeholder:"网址"}})],1),a("a-form-item",[a("a-button",{attrs:{type:"primary","html-type":"submit"}},[e._v("确定")])],1)],1)],1):e._e()],1)],1)],1),a("a-modal",{attrs:{title:"登录",visible:e.login_visible,maskClosable:!1,confirmLoading:!1},on:{ok:e.login,cancel:function(){return t.login_visible=!1}}},[a("a-form",{attrs:{form:e.form},on:{submit:function(t){return t.preventDefault(),e.handleSubmit(t)}}},[a("a-form-item",[a("a-input",{directives:[{name:"decorator",rawName:"v-decorator",value:["userId",{rules:[{required:!0,message:"Please input your userId!"}]}],expression:"['userId',{ rules: [{ required: true, message: 'Please input your userId!' }] }]"}],attrs:{placeholder:"UserId",size:"large"}},[a("a-icon",{staticStyle:{color:"rgba(0,0,0,.25)"},attrs:{slot:"prefix",type:"user"},slot:"prefix"})],1)],1),a("a-form-item",[a("a-input",{directives:[{name:"decorator",rawName:"v-decorator",value:["password",{rules:[{required:!0,message:"Please input your Password!"}]}],expression:"['password',{ rules: [{ required: true, message: 'Please input your Password!' }]}]"}],attrs:{type:"password",placeholder:"Password",size:"large"}},[a("a-icon",{staticStyle:{color:"rgba(0,0,0,.25)"},attrs:{slot:"prefix",type:"lock"},slot:"prefix"})],1)],1)],1)],1)],1)},i=[],o={name:"app",data:function(){return{urls:[],user:"",login_visible:!1,addurl_visible:!1,form:this.$form.createForm(this),addUrl_form:this.$form.createForm(this)}},created:function(){this.getData()},methods:{getData:function(){var t=this;this.$http.get(this.$urls.geturlmeta).then(function(e){0==e.body.code&&(t.urls=e.body.result)}),this.$http.get(this.$urls.getuser).then(function(e){0==e.body.code&&e.body.result&&(t.user=e.body.result)})},login:function(){var t=this;this.form.validateFields(function(e,r){e||t.$http.post(t.$urls.login,r).then(function(e){0==e.body.code?t.login_visible=!1:t.$message.warning("用户名或密码不正确!")})})},addUrl:function(){var t=this;this.addUrl_form.validateFields(function(e,r){e||t.$http.post(t.$urls.addnavigation,r).then(function(e){0==e.body.code?t.login_visible=!1:t.$message.warning("添加失败!")})})}}},l=o,u=(r("bb23"),r("2877")),d=Object(u["a"])(l,n,i,!1,null,"51a0a624",null),c=d.exports;r("a481"),r("3b2b"),r("386d"),r("6b54");function p(t){var e=new Date(0);return e.setMilliseconds(t),e.getFullYear()+"-"+f(e.getMonth()+1)+"-"+f(e.getDate())+" "+f(e.getHours())+":"+f(e.getMinutes())+":"+f(e.getSeconds())}function f(t){return 1==t.toString().length?"0"+t:t}function g(t,e){var r="",a=t,s=["0","1","2","3","4","5","6","7","8","9","a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z","A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"];a=Math.round(Math.random()*(e-t))+t;for(var n=0;n<a;n++){var i=Math.round(Math.random()*(s.length-1));r+=s[i]}return r}function m(t,e){var r=t.indexOf(e);r>-1&&t.splice(r,1)}function h(t){var e=window.location.search.indexOf(t),r=window.location.search.substring(e+t.length+1);return r}String.prototype.trim=function(t){var e=new RegExp("^"+(t||" ")+"+|"+(t||" ")+"+$","gi");return this.replace(e,"")},String.prototype.trimEnd=function(t){var e=new RegExp(("\\"+t||!1)+"+$","gi");return this.replace(e,"")},String.prototype.trimStart=function(t){var e=new RegExp("^"+(t||" ")+"+","gi");return this.replace(e,"")},String.prototype.getFileName=function(t){if(this.indexOf('<span class="search_word">')>-1){var e=this.indexOf('<span class="search_word">'),r=this.indexOf("</span>"),a=e-t/2,s=r+7+t/2;a<0&&(s+=Math.abs(a));var n=this.substring(a,s);return this.length>n.length?n+"...":n}for(var i=0,o=0;o<this.length;o++){if(o==t)break;/^[\u4E00-\u9FA5]+$/.test(this[o])?i+=1:i+=2}return this.length>i?this.substring(0,i)+"...":this.substring(0,i)};var v={parseBsonTime:p,removeArrayItem:m,randomWord:g,getReturnUrl:h},b=r("5efb"),y=r("9a63"),w=r("e32c"),_=r("a79d"),x=r("ccb9e"),S=r("cdeb"),$=r("27fd"),P=r("9839"),k=r("0c63"),O=r("a600"),C=r("ed3b"),U=r("3af3"),j=r("b558"),M=r("55f1"),E=r("f64c");r("202f");a["a"].use(b["a"]),a["a"].use(y["a"]),a["a"].use(w["a"]),a["a"].use(_["a"]),a["a"].use(x["a"]),a["a"].use(S["a"]),a["a"].use($["a"]),a["a"].use(P["a"]),a["a"].use(k["a"]),a["a"].use(O["a"]),a["a"].use(C["a"]),a["a"].use(U["a"]),a["a"].use(j["a"]),a["a"].use(M["a"]),a["a"].prototype.$message=E["a"],a["a"].use(s["a"]),a["a"].prototype.$common=v,a["a"].http.options.root="";var F={geturlmeta:"sso/geturlmeta",login:"sso/login",logout:"sso/logout",getuser:"sso/getuser",addnavigation:"sso/addnavigation"};a["a"].prototype.$urls=F,new a["a"]({render:function(t){return t(c)}}).$mount("#app")},bb23:function(t,e,r){"use strict";var a=r("d467"),s=r.n(a);s.a},d467:function(t,e,r){}});
//# sourceMappingURL=index.js.map