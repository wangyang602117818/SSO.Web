///////////////验证中心方法 authorize(baseUrl)////////////////////////////////////
//********************************************************************* */

function setCookie(cname, cvalue, exdays) {
    var cookies = cname + "=" + cvalue + ";path=/";
    if (exdays) {
        var d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        cookies += ";expires=" + d.toGMTString();
    }
    document.cookie = cookies;
}
function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i].trim();
        if (c.indexOf(name) == 0) { return c.substring(name.length, c.length); }
    }
    return "";
}
function geAuthCookie() {
    var cookieName = window.location.hostname + ".auth";
    return getCookie(cookieName);
}
function parseUrlString(str) {
    var result = str.replace('-', '+').replace('_', '/');
    switch (result.length % 4) {
        case 2:
            result += "==";
            break;
        case 3:
            result += "=";
            break;
    }
    return result;
}
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
function getTokenByTicket(url, success, error) {
    var xhr = new XMLHttpRequest();
    xhr.onload = function (event) {
        var target = event.srcElement || event.target;
        success(JSON.parse(target.responseText));
    };
    if (error) xhr.onerror = error;
    xhr.open('get', url, false);
    xhr.send();
}
function authorize(baseUrl) {
    var loginUrl = baseUrl + "sso/login";
    var indexUrl = baseUrl + "sso/index";
    var getTokenUrl = baseUrl + "sso/gettoken";
    var cookieName = window.location.hostname + ".auth";
    var ssourl = getQueryString("ssourls");
    //debugger;
    //sso退出
    if (ssourl) {
        ////////清除本站cookie
        var ssoUrls = JSON.parse(window.atob(parseUrlString(ssourl)));
        var cookieValue = getCookie(cookieName);
        if (cookieValue) {
            setCookie(cookieName, cookieValue, -1);
        }
        /////////////////////
        var index = 0;
        for (var i = 0; i < ssoUrls.length; i++) if (window.location.href.indexOf(ssoUrls[i]) > -1) index = i;
        if (index < ssoUrls.length - 1) {
            window.location.href = ssoUrls[index + 1] + "?ssourls=" + ssourl;
        }
        else //最后一个
        {
            window.location.href = indexUrl;
        }
        return;
    }
    var authorization = getCookie(cookieName);
    var ticket = getQueryString("ticket");
    //cookie不可用的时候
    if (!authorization) {
        //cookie和ticket都不可用的时候
        if (!ticket) {
            window.location.href = loginUrl + "?returnUrl=" + window.location.href;
            return;
        }
        //cookie不可用,但是有ticket
        else {
            getTokenByTicket(getTokenUrl + "?ticket=" + ticket, function (result) {
                if (result.code == 0 && result.result) {
                    //通过ticket获取到了token,一般发生在首次登陆
                    setCookie(cookieName, result.result);
                } else {
                    //两者都不可用
                    window.location.href = loginUrl + "?returnUrl=" + window.location.href;
                }
            });
        }
    }
}

export default {
    authorize: authorize,
    geAuthCookie: geAuthCookie
}