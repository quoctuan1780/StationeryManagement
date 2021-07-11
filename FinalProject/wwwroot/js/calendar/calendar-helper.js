﻿const startDateComponent = MCDatepicker.create({
    dateFormat: 'mm-dd-yyyy',
    selectedDate: new Date(),
    closeOndblclick: false,
    closeOnBlur: false,
    customOkBTN: 'Chọn',
    customClearBTN: 'Xóa',
    customCancelBTN: 'Hủy',
});

const endDateComponent = MCDatepicker.create({
    dateFormat: 'mm-dd-yyyy',
    selectedDate: new Date(),
    closeOndblclick: false,
    closeOnBlur: false,
    customOkBTN: 'Chọn',
    customClearBTN: 'Xóa',
    customCancelBTN: 'Hủy',
});




const startDateButton = document.querySelector('#start-date-button');
const startDate = document.querySelector('#start-date');
const endDateButton = document.querySelector('#end-date-button');
const endDate = document.querySelector('#end-date');


startDateComponent.onSelect((date, formatedDate) => {
    startDate.value = formatedDate;
});
startDateButton.onclick = () => {
    startDateComponent.open();
};
endDateComponent.onSelect((date, formatedDate) => {
    endDate.value = formatedDate;
});
endDateButton.onclick = () => {
    endDateComponent.open();
};