function parseBsonTime(value) {
    var date = new Date(0);
    date.setMilliseconds(value);
    return date.getFullYear() + "-" + formatMonth((date.getMonth() + 1)) + "-" + formatMonth(date.getDate()) + " " + formatMonth(date.getHours()) + ":" + formatMonth(date.getMinutes()) + ":" + formatMonth(date.getSeconds());
}
function formatMonth(month) {
    return month.toString().length == 1 ? "0" + month : month;
}
function randomWord(min, max) {
    var str = "",
        range = min,
        arr = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
    // 随机产生
    range = Math.round(Math.random() * (max - min)) + min;
    for (var i = 0; i < range; i++) {
        var pos = Math.round(Math.random() * (arr.length - 1));
        str += arr[pos];
    }
    return str;
}
function removeArrayItem(array, val) {
    var index = array.indexOf(val);
    if (index > -1) {
        array.splice(index, 1);
    }
}
function getReturnUrl(name) {
    var index = window.location.search.indexOf(name);
    var returnUrl = window.location.search.substring(index + name.length + 1);
    return returnUrl;
}
String.prototype.getFileName = function (length) {
    if (this.indexOf("<span class=\"search_word\">") > -1) {
        var startIndex = this.indexOf("<span class=\"search_word\">"),
            endIndex = this.indexOf("</span>"),
            startPos = startIndex - length / 2,
            endPos = endIndex + 7 + length / 2;
        if (startPos < 0) endPos = endPos + Math.abs(startPos);
        var newfilename = this.substring(startPos, endPos);
        if (this.length > newfilename.length) return newfilename + "...";
        return newfilename;
    } else {
        var len = 0;
        for (var i = 0; i < this.length; i++) {
            if (i == length) break;
            /^[\u4E00-\u9FA5]+$/.test(this[i]) ? len += 1 : len += 2;
        }
        if (this.length > len) return this.substring(0, len) + "...";
        return this.substring(0, len);
    }
}
export default {
    parseBsonTime: parseBsonTime,
    removeArrayItem: removeArrayItem,
    randomWord: randomWord,
    getReturnUrl: getReturnUrl
}