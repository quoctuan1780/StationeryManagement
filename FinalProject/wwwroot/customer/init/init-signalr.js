"use strict";

let connection = new signalR.HubConnectionBuilder()
    .withUrl("/signalServer")
    .withAutomaticReconnect()
    .build();

connection.on("rejectNotificationCustomer", function () {
    loadNewNotify('Thông báo đơn hàng đã được hủy');
})

connection.on("newSaleNotificationCustomer", function () {
    loadNewNotify('Thông báo có khuyến mại mới');
});

connection.start().then(function () {
    connection.invoke("CustomerJoinGroup", "Customer");
}).catch(function (err) {
    return console.error(err);
})

function loadNewNotify(content) {
    var number = parseInt($('#count-notification').text());
    number++;
    if (number > 99) {
        $('#count-notification').text('+99')
    }
    else {
        $('#count-notification').text(number.toString())
    }

    $.notiny({
        position: 'right-top',
        theme: 'success',
        template: '<div class="notiny-base"><div class="MuiPaper-root MuiAlert-root MuiAlert-filledSuccess MuiPaper-elevation6"><div class="MuiAlert-icon"><svg class="MuiSvgIcon-root MuiSvgIcon-fontSizeInherit" focusable="false" viewBox="0 0 24 24" aria-hidden="true"><path d="M20,12A8,8 0 0,1 12,20A8,8 0 0,1 4,12A8,8 0 0,1 12,4C12.76,4 13.5,4.11 14.2, 4.31L15.77,2.74C14.61,2.26 13.34,2 12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0, 0 22,12M7.91,10.08L6.5,11.5L11,16L21,6L19.59,4.58L11,13.17L7.91,10.08Z"></path></svg></div><div class="notiny-text MuiAlert-message"></div></div></div>',
        text: content,
    });
}