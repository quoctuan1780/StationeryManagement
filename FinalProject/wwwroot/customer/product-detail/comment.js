function addInput(e, commentId, productId, email, userId, userName, imageLink) {
    var node = "<li class='clearfix'><div class='comment-block'>" +
        "<img src='/images/user/" + convertImageLink(imageLink, email) + "' class='avatar' alt=''>" +
        "<div class='post-comments'><textarea contenteditable='true' class='form-control' placeholder='Viết bình luận'></textarea>" +
        "<button type='button' class='btn btn-success' style='margin-top: 5px;'" +
        "onclick='addComment(this," + commentId + "," + productId + "," + '"' + email + '"' + "," + '"' + userId + '"' + "," + '"' + userName + '"' + "," + '"' + imageLink + '"' + ")'>Gửi</button>" +
        "<button type='button' class='btn btn-success' style='margin-top: 5px; margin-left: 5px;' onclick='removeComment(this)'>Hủy</button>" +
        "</div></div></li>";
    $(e).closest('.clearfix').children("ul.comments").append(node);
    $(e).closest('.clearfix').children("ul.comments").children().get($(e).closest('.clearfix').children("ul.comments").children().length - 1).scrollIntoView();
}

function addReplyChildrentInput(e, commentId, productId, email, userId, userName, imageLink) {
    console.log(email)
    var node = "<li class='clearfix'><div class='comment-block'>" +
        "<img src='/images/user/" + convertImageLink(imageLink, email) + "' class='avatar' alt=''>" +
        "<div class='post-comments'><textarea contenteditable='true' class='form-control' placeholder='Viết bình luận'></textarea>" +
        "<button type='button' class='btn btn-success' style='margin-top: 5px;'" +
        "onclick='addReplyChildrentComment(this," + commentId + "," + productId + "," + '"' + email + '"' + "," + '"' + userId + '"' + "," + '"' + userName + '"' + "," + '"' + imageLink + '"' + ")' > Gửi</button >" +
        "<button type='button' class='btn btn-success' style='margin-top: 5px; margin-left: 5px;' onclick='removeComment(this)'>Hủy</button>" +
        "</div></div></li>";

    $(e).closest('.comments').append(node);
    $(e).closest('.comments').children().get($(e).closest('.comments').children().length - 1).scrollIntoView()
}

function removeComment(e) {
    $(e).closest('.clearfix').remove();
}

function repairComment(e, id) {
    Swal.fire({
        title: 'Nhập nội dung',
        html: '<textarea class="form-control" rows="5" id="comment-input"></textarea>',
        showCancelButton: true,
        confirmButtonText: 'Sửa bình luận',
        cancelButtonText: 'Hủy'
    }).then((result) => {
        if (result.isConfirmed) {
            var content = $("#comment-input").val();
            if (content === '') {
                showErrorNullComment();
            }

            $.ajax({
                url: '/Comment/UpdateComment',
                async: true,
                method: 'PUT',
                data: { commentId: id, content: content },
                success: function (data) {
                    switch (data) {
                        case "-4":
                            showError("Lỗi bình luận, vui lòng thử lại sau");
                            break;
                        case "1":

                            $($(e).closest('.post-comments').children().get(1)).text(content);
                            showSuccess('Sửa bình luận thành công');
                            break;
                        case "-99":
                            showErrorSystem();
                            break;
                    }
                },
                error: function (code, err) {
                    showError('Lỗi! vui lòng đăng xuất và đăng nhập lại hệ thống', '460px');
                    console.error(err);
                }
            });
        }
    })
}

function deleteComment(e, id) {
    Swal.fire({
        title: 'Xóa bình luận!',
        text: "Bạn có muốn xóa bình luận này không ?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Xóa',
        cancelButtonText: 'Hủy'
    }).then((result) => {
        if (result.isConfirmed) {
            if (id === null) {
                showErrorSystem();
            }
            else {
                $.ajax({
                    url: '/Comment/DeleteComment',
                    method: 'DELETE',
                    async: true,
                    data: { commentId: id },
                    success: function (data) {
                        switch (data) {
                            case "-4":
                                showError("Không tồn tại bình luận này");
                                break;
                            case "1":
                                removeComment(e);
                                showSuccess('Xóa bình luận thành công');
                                break;
                            case "-99":
                                showErrorSystem();
                                break;
                        }
                    },
                    error: function (code, err) {
                        showError('Lỗi! vui lòng đăng xuất và đăng nhập lại hệ thống', '460px');
                        console.error(err);
                    }
                });
            }
        }
    })
}

function addComment(e, commentId, productId, email, userId, userName, imageLink) {

    var content = $($(e).closest(".post-comments").children("textarea.form-control")).val();

    if (content === null || content.trim() === '') {
        showErrorNullComment();
    }

    $.ajax({
        url: '/Comment/AddChildrentComment',
        method: 'POST',
        async: true,
        data: { userId: userId, content: content, productId: productId, commentId: commentId, userName: userName },
        success: function (data) {
            var json = JSON.parse(data);
            switch (json.Status) {
                case 1:
                    var node = "<div class='comment-block'><img src='/images/user/" + convertImageLink(imageLink, email) + "' class='avatar' alt = ''>" +
                        "<div class='post-comments'>" +
                        "<p class='meta'>" + getDate() + " <a href='#'>" + userName + "</a> nói :<i class='pull-right'>" +
                        "<span style='cursor: pointer;'" +
                        "onclick='addReplyChildrentInput(this," + commentId + "," + productId + "," + '"' + email + '"' + "," + '"' + userId + '"' + "," + '"' + userName + '"' + "," + '"' + imageLink + '"' + ")'>" +
                        "<small>Trả lời</small></span><span> | </span>" +
                        "<span style='cursor: pointer;' onclick='deleteComment(this, " + json.CommentId + ")'><small>Xóa</small></span><span> | </span>" +
                        "<span style='cursor: pointer;' onclick='repairComment(this, " + json.CommentId + ")'><small>Sửa</small></span>" +
                        "</i></p>" +
                        "<p>" + content + "</p>" +
                        "</div></div>";
                    e = $(e).closest('.clearfix');

                    $(e).children('.comment-block').remove();

                    $(e).closest('.clearfix').append(node);

                    //$([document.documentElement, document.body]).animate({
                    //    scrollTop: $(e).offset().top
                    //}, 2000);

                    break;
                case -4:
                    showErrorNullComent();
                    break;
                case -99:
                    showErrorSystem();
                    break;
            }
        },
        error: function (code, err) {
            showError('Lỗi! vui lòng đăng xuất và đăng nhập lại hệ thống', '460px');
            console.error(err);
        }
    });
}

function addReplyChildrentComment(e, commentId, productId, email, userId, userName, imageLink) {

    var content = $($(e).closest(".post-comments").children("textarea.form-control")).val();

    if (content === null || content.trim() === '') {
        showErrorNullComment();
    }

    $.ajax({
        url: '/Comment/AddChildrentReplyComment',
        method: 'POST',
        async: true,
        data: { userId: userId, content: content, productId: productId, commentId: commentId, userName: userName },
        success: function (data) {
            var json = JSON.parse(data)
            switch (json.Status) {
                case 1:
                    var node = "<div class='comment-block'><img src='/images/user/" + convertImageLink(imageLink, email) + "' class='avatar' alt = ''>" +
                        "<div class='post-comments'>" +
                        "<p class='meta'>" + getDate() + " <a href='#'>" + userName + "</a> nói :<i class='pull-right'>" +
                        "<span style='cursor: pointer;'" +
                        "onclick='addReplyChildrentInput(this," + commentId + "," + productId + "," + '"' + email + '"' + "," + '"' + userId + '"' + "," + '"' + userName + '"' + "," + '"' + imageLink + '"' + ")'>" +
                        "<small>Trả lời</small></span><span> | </span>" +
                        "<span style='cursor: pointer;' onclick='deleteComment(this, " + json.CommentId + ")'><small>Xóa</small></span><span> | </span>" +
                        "<span style='cursor: pointer;' onclick='repairComment(this, " + json.CommentId + ")'><small>Sửa</small></span>" +
                        "</i></p>" +
                        "<p>" + content + "</p>" +
                        "</div></div>";

                    e = $(e).closest('.clearfix');

                    $(e).children('.comment-block').remove();

                    $(e).closest('.clearfix').append(node);

                    break;
                case -4:
                    showErrorNullComent();
                    break;
                case -99:
                    showErrorSystem();
                    break;
            }
        },
        error: function (code, err) {
            showError('Lỗi! vui lòng đăng xuất và đăng nhập lại hệ thống', '460px');
            console.error(err);
        }
    });
}

function addOwnerComment(imageLink, userId, isLogin, email, name, productId) {
    var content = $('#comment-content').val();
    if (isLogin === 'False') {
        showErrorLogin();
    }
    else if (content === null || content.trim() === '') {
        showErrorNullComment();
    }
    else {

        $.ajax({
            url: '/Comment/AddParentComment',
            method: 'POST',
            async: true,
            data: { userId: userId, content: content, productId: productId },
            success: function (data) {
                var json = JSON.parse(data);
                switch (json.Status) {
                    case -4:
                        showErrorNullComment();
                        break;
                    case 1:
                        var node = "<li class='clearfix'><div class='comment-block'><img src='/images/user/" + convertImageLink(imageLink, email) + "' class='avatar' alt = ''>" +
                            "<div class='post-comments'>" +
                            "<p class='meta'>" + getDate() + " <a href='#'>" + name + " </a> nói :<i class='pull-right'>" +
                            "<span style='cursor: pointer;' onclick='" +
                            "addInput(this, " + json.CommentId + ", " + productId + ", " + '"' + email + '"' + ", " + '"' + userId + '"' + ", " + '"' + name + '"' + ", " + '"' + imageLink + '"' + ")'>" +
                            "<small>Trả lời</small></span><span> | </span>" +
                            "<span style='cursor: pointer;' onclick='deleteComment(this, " + json.CommentId + ")'><small>Xóa</small></span><span> | </span>" +
                            "<span style='cursor: pointer;' onclick='repairComment(this, " + json.CommentId + ")'><small>Sửa</small></span>" +
                            "</i></p>" +
                            "<p>" + content + "</p>" +
                            "</div></div>" +
                            "<ul class='comments'><li class='clearfix'></li></ul></li>";

                        console.log(node)

                        $('#comment-content').val('');

                        $('#comment').append(node);
                        break;

                    case -99:
                        showErrorSystem();
                        break;
                }
            },
            error: function (code, err) {
                showError('Lỗi! vui lòng đăng xuất và đăng nhập lại hệ thống', '460px');
                console.error(err);
            }
        });
    }
}