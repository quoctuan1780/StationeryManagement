const dateComponent = MCDatepicker.create({
    dateFormat: 'mm-dd-yyyy',
    selectedDate: new Date(),
    closeOndblclick: false,
    closeOnBlur: false,
    customOkBTN: 'Chọn',
    customClearBTN: 'Xóa',
    customCancelBTN: 'Hủy',
});

const dateOfBirthButton = document.querySelector('#date-of-birth-button');
const dateOfBirth = document.querySelector('#date-of-birth');

dateComponent.onSelect((date, formatedDate) => {
    dateOfBirth.value = formatedDate;
});
dateOfBirthButton.onclick = () => {
    dateComponent.open();
};