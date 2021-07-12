const dateButton = document.querySelector('#date-button');
const dateInput = document.querySelector('#date-input');

const dateComponent = MCDatepicker.create({
    dateFormat: 'mm-dd-yyyy',
    selectedDate: new Date(),
    closeOndblclick: false,
    closeOnBlur: false,
    customOkBTN: 'Chọn',
    customClearBTN: 'Xóa',
    customCancelBTN: 'Hủy',
});

dateComponent.onSelect((date, formatedDate) => {
    dateInput.value = formatedDate;
});
dateButton.onclick = () => {
    dateComponent.open();
};