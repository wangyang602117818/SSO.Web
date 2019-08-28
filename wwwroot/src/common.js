function parseBsonTime(value) {
    var date = new Date(0);
    date.setMilliseconds(value);
    return date.getFullYear() + "-" + formatMonth((date.getMonth() + 1)) + "-" + formatMonth(date.getDate()) + " " + formatMonth(date.getHours()) + ":" + formatMonth(date.getMinutes()) + ":" + formatMonth(date.getSeconds());
}
function formatMonth(month) {
    return month.toString().length == 1 ? "0" + month : month;
}
export default{
    parseBsonTime:parseBsonTime
}