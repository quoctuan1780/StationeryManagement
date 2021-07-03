function showErrorSystem() {
    $.notiny({
        position: 'right-top',
        theme: 'danger',
        template: '<div class="notiny-base"><div class="MuiPaper-root MuiAlert-root MuiAlert-filledError MuiPaper-elevation6"><div class="MuiAlert-icon"><svg class="MuiSvgIcon-root MuiSvgIcon-fontSizeInherit" focusable="false" viewBox="0 0 24 24" aria-hidden="true"><path d="M11 15h2v2h-2zm0-8h2v6h-2zm.99-5C6.47 2 2 6.48 2 12s4.47 10 9.99 10C17.52 22 22 17.52 22 12S17.52 2 11.99 2zM12 20c-4.42 0-8-3.58-8-8s3.58-8 8-8 8 3.58 8 8-3.58 8-8 8z"></path></svg></div><div class="notiny-text MuiAlert-message"></div></div></div>',
        text: 'Lỗi hệ thống vui lòng thử lại sau',
    });
}

function showErrorNullComment() {
    $.notiny({
        position: 'right-top',
        theme: 'danger',
        template: '<div class="notiny-base"><div class="MuiPaper-root MuiAlert-root MuiAlert-filledError MuiPaper-elevation6"><div class="MuiAlert-icon"><svg class="MuiSvgIcon-root MuiSvgIcon-fontSizeInherit" focusable="false" viewBox="0 0 24 24" aria-hidden="true"><path d="M11 15h2v2h-2zm0-8h2v6h-2zm.99-5C6.47 2 2 6.48 2 12s4.47 10 9.99 10C17.52 22 22 17.52 22 12S17.52 2 11.99 2zM12 20c-4.42 0-8-3.58-8-8s3.58-8 8-8 8 3.58 8 8-3.58 8-8 8z"></path></svg></div><div class="notiny-text MuiAlert-message"></div></div></div>',
        text: 'Bạn chưa nhập bình luận',
    });
}

function showErrorLogin() {
    $.notiny({
        position: 'right-top',
        theme: 'danger',
        template: '<div class="notiny-base"><div class="MuiPaper-root MuiAlert-root MuiAlert-filledError MuiPaper-elevation6"><div class="MuiAlert-icon"><svg class="MuiSvgIcon-root MuiSvgIcon-fontSizeInherit" focusable="false" viewBox="0 0 24 24" aria-hidden="true"><path d="M11 15h2v2h-2zm0-8h2v6h-2zm.99-5C6.47 2 2 6.48 2 12s4.47 10 9.99 10C17.52 22 22 17.52 22 12S17.52 2 11.99 2zM12 20c-4.42 0-8-3.58-8-8s3.58-8 8-8 8 3.58 8 8-3.58 8-8 8z"></path></svg></div><div class="notiny-text MuiAlert-message"></div></div></div>',
        text: 'Bạn chưa đăng nhập vào hệ thống',
    });
}

function convertImageLink(imageLink, email) {
    var imageShow = '';
    if (imageLink === '') {
        imageShow = 'avatar.png';
    }
    else {
        imageShow = email + '/' + imageLink;
    }

    return imageShow;
}

function getDate() {
    const monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"];
    const dateObj = new Date();
    const month = monthNames[dateObj.getMonth()];
    const day = String(dateObj.getDate()).padStart(2, '0');
    const year = dateObj.getFullYear();

    return month + ' ' + day + ',' + year;
}

function showError(content) {
    $.notiny({
        position: 'right-top',
        theme: 'danger',
        template: '<div class="notiny-base"><div class="MuiPaper-root MuiAlert-root MuiAlert-filledError MuiPaper-elevation6"><div class="MuiAlert-icon"><svg class="MuiSvgIcon-root MuiSvgIcon-fontSizeInherit" focusable="false" viewBox="0 0 24 24" aria-hidden="true"><path d="M11 15h2v2h-2zm0-8h2v6h-2zm.99-5C6.47 2 2 6.48 2 12s4.47 10 9.99 10C17.52 22 22 17.52 22 12S17.52 2 11.99 2zM12 20c-4.42 0-8-3.58-8-8s3.58-8 8-8 8 3.58 8 8-3.58 8-8 8z"></path></svg></div><div class="notiny-text MuiAlert-message"></div></div></div>',
        text: content,
    });
}

function showSuccess(content) {
    $.notiny({
        position: 'right-top',
        theme: 'success',
        template: '<div class="notiny-base"><div class="MuiPaper-root MuiAlert-root MuiAlert-filledSuccess MuiPaper-elevation6"><div class="MuiAlert-icon"><svg class="MuiSvgIcon-root MuiSvgIcon-fontSizeInherit" focusable="false" viewBox="0 0 24 24" aria-hidden="true"><path d="M20,12A8,8 0 0,1 12,20A8,8 0 0,1 4,12A8,8 0 0,1 12,4C12.76,4 13.5,4.11 14.2, 4.31L15.77,2.74C14.61,2.26 13.34,2 12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0, 0 22,12M7.91,10.08L6.5,11.5L11,16L21,6L19.59,4.58L11,13.17L7.91,10.08Z"></path></svg></div><div class="notiny-text MuiAlert-message"></div></div></div>',
        text: content,
    });
}

function showError(content, width) {
    $.notiny({
        position: 'right-top',
        theme: 'danger',
        width: width,
        template: '<div class="notiny-base"><div class="MuiPaper-root MuiAlert-root MuiAlert-filledError MuiPaper-elevation6"><div class="MuiAlert-icon"><svg class="MuiSvgIcon-root MuiSvgIcon-fontSizeInherit" focusable="false" viewBox="0 0 24 24" aria-hidden="true"><path d="M11 15h2v2h-2zm0-8h2v6h-2zm.99-5C6.47 2 2 6.48 2 12s4.47 10 9.99 10C17.52 22 22 17.52 22 12S17.52 2 11.99 2zM12 20c-4.42 0-8-3.58-8-8s3.58-8 8-8 8 3.58 8 8-3.58 8-8 8z"></path></svg></div><div class="notiny-text MuiAlert-message"></div></div></div>',
        text: content,
    });
}

function showSuccess(content, width) {
    $.notiny({
        position: 'right-top',
        theme: 'success',
        width: width,
        template: '<div class="notiny-base"><div class="MuiPaper-root MuiAlert-root MuiAlert-filledSuccess MuiPaper-elevation6"><div class="MuiAlert-icon"><svg class="MuiSvgIcon-root MuiSvgIcon-fontSizeInherit" focusable="false" viewBox="0 0 24 24" aria-hidden="true"><path d="M20,12A8,8 0 0,1 12,20A8,8 0 0,1 4,12A8,8 0 0,1 12,4C12.76,4 13.5,4.11 14.2, 4.31L15.77,2.74C14.61,2.26 13.34,2 12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0, 0 22,12M7.91,10.08L6.5,11.5L11,16L21,6L19.59,4.58L11,13.17L7.91,10.08Z"></path></svg></div><div class="notiny-text MuiAlert-message"></div></div></div>',
        text: content,
    });
}