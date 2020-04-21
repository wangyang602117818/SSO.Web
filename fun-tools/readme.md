# fun-tools fun-tools

## install
``` 
npm install fun-tools
```
## usage
```
import funtools from 'fun-tools'
```
## function list
** 前缀 funtools. 省略
```
//sso验证方法(window.token_jwt_data)
authorize(baseUrl, cookieName)

//获取cookie
var value = getCookie(cname)

//设置cookie
setCookie(cname, cvalue, exdays)

//获取url上面的参数
var result = getQueryString(name)

//还原从url上面传过来的base64字符串
var result = parseBase64String(str)

//解析从mongo中传出来的 date.$date
var result = parseBsonTime(str)

//格式化成2位的时间格式
var result = formatMonth(month)

//取随机字符串 min:最小位数,max:最大位数
var result = randomWord(min,max)

//随机数字 min:最小值,max:最大值
var result = randomNumber(min,max)

//移除数组[a,b,c,d]中某一项c,不包含数组对象
removeArrayItem(array, val)

//获取url上面name参数之后所有部分,这和getQueryString有所不同,因为returnUrl后面可能还有参数
var result = getReturnUrl(name)

//根据userAgent获取设备类型 mobile|pc
var device = getDeviceType(userAgent)

//获取长文件名的前几个字符串
var result = getFileName(length)

//去除字符串首尾char字符串
var result = trimChar(char)
var result = trimEndChar(char)
var result = trimStartChar(char)
```