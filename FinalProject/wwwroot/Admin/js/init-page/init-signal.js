"use strict";

let connection = new signalR.HubConnectionBuilder()
    .withUrl("/signalServer")
    .withAutomaticReconnect()
    .build();

connection.on("newNotification", function (link, createdDate, content) {
    loadNewNotification(link, createdDate, content);
});

connection.on("rejectNotification", function (link, createdDate, content) {
    loadRejectNotification(link, createdDate, content);
});

connection.on("newReceiptRequestNotification", function (link, createdDate, content) {
    loadReceiptRequestNotification(link, createdDate, content);
});

connection.start().then(function () {
    connection.invoke("AdminJoinGroup", "Admin");
}).catch(function (err) {
    return console.error(err);
})

function loadReceiptRequestNotification(link, createdDate, content) {
    var node = `<a class="dropdown-item d-flex align-items-center" href="` + link + `">
                            <div class="mr-3">
                                <div class="icon-circle bg-success">
                                    <i class="fas fa-warehouse text-white"></i>
                                </div>
                            </div>
                            <div>
                                <div class="small text-gray-500">`+ createdDate + `</div>
                                `+ content + `
                            </div>
                            <div>
                                <img src="/images/background/icon-status.png" width="20" height="20" />
                            </div>
                        </a>`;

    var nodeShowAllNotification = `<a id="show-add-notification" class="dropdown-item text-center small text-gray-500"
                                            href="/Admin/Notification/Notifications">Xem tất cả thông báo</a>`;

    var number = parseInt($('#notification-count').text());

    number++;

    $('#notification-count').text(number + "")


    $('#show-notification').append(node);
    $('#show-add-notification').remove();
    $('#show-notification').append(nodeShowAllNotification);

    $.notiny({
        position: 'right-top',
        theme: 'success',
        template: '<div class="notiny-base"><div class="MuiPaper-root MuiAlert-root MuiAlert-filledSuccess MuiPaper-elevation6"><div class="MuiAlert-icon"><svg class="MuiSvgIcon-root MuiSvgIcon-fontSizeInherit" focusable="false" viewBox="0 0 24 24" aria-hidden="true"><path d="M20,12A8,8 0 0,1 12,20A8,8 0 0,1 4,12A8,8 0 0,1 12,4C12.76,4 13.5,4.11 14.2, 4.31L15.77,2.74C14.61,2.26 13.34,2 12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0, 0 22,12M7.91,10.08L6.5,11.5L11,16L21,6L19.59,4.58L11,13.17L7.91,10.08Z"></path></svg></div><div class="notiny-text MuiAlert-message"></div></div></div>',
        text: 'Thông báo yêu cầu nhập kho mới',
    });
}

function loadNewNotification(link, createdDate, content) {
    var node = `<a class="dropdown-item d-flex align-items-center" href="` + link + `">
                    <div class="mr-3">
                        <div class="icon-circle bg-primary">
                            <i class="fas fa-file-alt text-white"></i>
                        </div>
                    </div>
                    <div>
                        <div class="small text-gray-500">`+ createdDate + `</div>
                        <span class="font-weight-bold">`+ content + `</span>
                    </div>
                    <div>
                        <img src="/images/background/icon-status.png" width="20" height="20" />
                    </div>
                </a>`;

    var nodeShowAllNotification = `<a id="show-add-notification" class="dropdown-item text-center small text-gray-500"
                                            href="/Admin/Notification/Notifications">Xem tất cả thông báo</a>`;

    var number = parseInt($('#notification-count').text());

    number++;

    $('#notification-count').text(number + "")


    $('#show-notification').prepend(node);
    $('#show-add-notification').remove();
    $('#show-notification').append(nodeShowAllNotification);

    $.notiny({
        position: 'right-top',
        theme: 'success',
        template: '<div class="notiny-base"><div class="MuiPaper-root MuiAlert-root MuiAlert-filledSuccess MuiPaper-elevation6"><div class="MuiAlert-icon"><svg class="MuiSvgIcon-root MuiSvgIcon-fontSizeInherit" focusable="false" viewBox="0 0 24 24" aria-hidden="true"><path d="M20,12A8,8 0 0,1 12,20A8,8 0 0,1 4,12A8,8 0 0,1 12,4C12.76,4 13.5,4.11 14.2, 4.31L15.77,2.74C14.61,2.26 13.34,2 12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0, 0 22,12M7.91,10.08L6.5,11.5L11,16L21,6L19.59,4.58L11,13.17L7.91,10.08Z"></path></svg></div><div class="notiny-text MuiAlert-message"></div></div></div>',
        text: 'Thông báo đơn hàng mới',
    });

}

function loadRejectNotification(link, createdDate, content) {
    var node = `<a class="dropdown-item d-flex align-items-center" href="` + link + `">
                    <div class="mr-3">
                        <div class="icon-circle bg-warning">
                            <i class="fas fa-exclamation-triangle text-white"></i>
                        </div>
                    </div>
                    <div>
                        <div class="small text-gray-500">`+ createdDate + `</div>
                        <span class="font-weight-bold">`+ content + `</span>
                    </div>
                    <div>
                        <img src="/images/background/icon-status.png" width="20" height="20" />
                    </div>
                </a>`;

    var nodeShowAllNotification = `<a id="show-add-notification" class="dropdown-item text-center small text-gray-500"
                                            href="/Admin/Notification/Notifications">Xem tất cả thông báo</a>`;

    var number = parseInt($('#notification-count').text());

    number++;

    $('#notification-count').text(number + "")


    $('#show-notification').prepend(node);
    $('#show-add-notification').remove();
    $('#show-notification').append(nodeShowAllNotification);

    $.notiny({
        position: 'right-top',
        theme: 'danger',
        template: '<div class="notiny-base"><div class="MuiPaper-root MuiAlert-root MuiAlert-filledSuccess MuiPaper-elevation6"><div class="MuiAlert-icon"><svg class="MuiSvgIcon-root MuiSvgIcon-fontSizeInherit" focusable="false" viewBox="0 0 24 24" aria-hidden="true"><path d="M20,12A8,8 0 0,1 12,20A8,8 0 0,1 4,12A8,8 0 0,1 12,4C12.76,4 13.5,4.11 14.2, 4.31L15.77,2.74C14.61,2.26 13.34,2 12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0, 0 22,12M7.91,10.08L6.5,11.5L11,16L21,6L19.59,4.58L11,13.17L7.91,10.08Z"></path></svg></div><div class="notiny-text MuiAlert-message"></div></div></div>',
        text: 'Thông báo có đơn hàng chờ hủy',
    });
}