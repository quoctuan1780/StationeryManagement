var skip = 10;
var idleTime = 0;
$(document).ready(function () {
    //Increment the idle time counter every minute.
    //var idleInterval =
    setInterval(timerIncrement, 60000); // 1 minute

    //Zero the idle timer on mouse movement.
    $(this).mousemove(function (e) {
        idleTime = 0;
    });
    $(this).keypress(function (e) {
        idleTime = 0;
    });

    $('#show-notification').on('scroll', function () {
        if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {
            $(this).addClass('loading');
            $('#load-component').removeClass('hidden-table');
            $.ajax({
                url: '/Admin/Notification/GetMoreNotification',
                method: 'GET',
                async: true,
                data: { skip: skip },
                success: function (data) {
                    $("#show-notification").removeClass('loading');
                    $('#load-component').addClass('hidden-table');
                    if (data !== 'NULL') {
                        var json = JSON.parse(data);
                        var node = ``;
                        $.each(json, function (k, v) {
                            var linkSeen = "seenNotify(" + v.Id + ")";
                            if (v.Status === 'Đã xem') {
                                linkSeen = 'Javascript:void(0)';
                            }
                            switch (json.Type) {
                                case "Thông báo đặt hàng":
                                    node += `
                                                <a onclick="`+ linkSeen + `" class="dropdown-item d-flex align-items-center" href="` + v.Link + `">
                                                    <div class="mr-3">
                                                        <div class="icon-circle bg-primary">
                                                            <i class="fas fa-file-alt text-white"></i>
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <div class="small text-gray-500">`+ v.CreatedDate + `</div>
                                                        <span class="font-weight-bold">`+ v.Content + `</span>
                                                    </div>`;
                                    if (v.Status === 'Chưa xem') {
                                        node += `
                                                        <div>
                                                            <img src="/images/background/icon-status.png" width="20" height="20" />
                                                        </div>`;
                                    }
                                    node += `</a>`;
                                    break;

                                case "Thông báo hủy đơn hàng":
                                    node += `
                                                <a onclick="`+ linkSeen + `" class="dropdown-item d-flex align-items-center" href="` + v.Link + `">
                                                    <div class="mr-3">
                                                        <div class="icon-circle bg-warning">
                                                            <i class="fas fa-exclamation-triangle text-white"></i>
                                                        </div>
                                                    </div>
                                                    <div>
                                                    <div class="small text-gray-500">`+ v.CreatedDate + `</div>
                                                    <span class="font-weight-bold">`+ v.Content + `</span>
                                                </div>`;
                                    if (v.Status === 'Chưa xem') {
                                        node += `
                                                    <div>
                                                        <img src="/images/background/icon-status.png" width="20" height="20" />
                                                    </div>`;
                                    }
                                    break;

                                default:
                                    node += `
                                                <a onclick="`+ linkSeen + `" class="dropdown-item d-flex align-items-center" href="` + v.Link + `">
                                                    <div class="mr-3">
                                                        <div class="icon-circle bg-success">
                                                            <i class="fas fa-warehouse text-white"></i>
                                                        </div>
                                                    </div>
                                                    <div>
                                                    <div class="small text-gray-500">`+ v.CreatedDate + `</div>
                                                    <span class="font-weight-bold">`+ v.Content + `</span>
                                                </div>`;
                                    if (v.Status === 'Chưa xem') {
                                        node += `
                                                    <div>
                                                        <img src="/images/background/icon-status.png" width="20" height="20" />
                                                    </div>`;
                                    }
                                    break;
                            }

                            $('#show-notification').append(node);

                            skip += 10;
                        });
                    }
                    else {
                        $.notiny({
                            position: 'right-top',
                            theme: 'success',
                            template: '<div class="notiny-base"><div class="MuiPaper-root MuiAlert-root MuiAlert-filledSuccess MuiPaper-elevation6"><div class="MuiAlert-icon"><svg class="MuiSvgIcon-root MuiSvgIcon-fontSizeInherit" focusable="false" viewBox="0 0 24 24" aria-hidden="true"><path d="M20,12A8,8 0 0,1 12,20A8,8 0 0,1 4,12A8,8 0 0,1 12,4C12.76,4 13.5,4.11 14.2, 4.31L15.77,2.74C14.61,2.26 13.34,2 12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0, 0 22,12M7.91,10.08L6.5,11.5L11,16L21,6L19.59,4.58L11,13.17L7.91,10.08Z"></path></svg></div><div class="notiny-text MuiAlert-message"></div></div></div>',
                            text: 'Đã tải hết thông báo',
                        });
                    }
                },
                error: function (code, err) {
                    $("#show-notification").removeClass('loading');
                    $('#load-component').addClass('hidden-table');
                    $.notiny({
                        position: 'right-top',
                        theme: 'danger',
                        template: '<div class="notiny-base"><div class="MuiPaper-root MuiAlert-root MuiAlert-filledSuccess MuiPaper-elevation6"><div class="MuiAlert-icon"><svg class="MuiSvgIcon-root MuiSvgIcon-fontSizeInherit" focusable="false" viewBox="0 0 24 24" aria-hidden="true"><path d="M20,12A8,8 0 0,1 12,20A8,8 0 0,1 4,12A8,8 0 0,1 12,4C12.76,4 13.5,4.11 14.2, 4.31L15.77,2.74C14.61,2.26 13.34,2 12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0, 0 22,12M7.91,10.08L6.5,11.5L11,16L21,6L19.59,4.58L11,13.17L7.91,10.08Z"></path></svg></div><div class="notiny-text MuiAlert-message"></div></div></div>',
                        text: 'Lỗi không tải thêm được thông báo',
                    });
                }
            });
        }
    });
});

function timerIncrement() {
    idleTime = idleTime + 1;
    if (idleTime > 29) { // 29 minutes
        window.location.href = "/Admin/Account/Logout"
    }
}

function seenNotify(id) {
    $.ajax({
        url: '/Admin/Notification/SeenNotify',
        method: 'PUT',
        async: true,
        data: { notificationId: id },
        success: function (data) {

        },
        error: function (code, err) {

        }
    });
}