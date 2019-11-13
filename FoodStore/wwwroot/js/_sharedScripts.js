window.EventListeners = [
    {
        name: 'onlyNumeric',
        listeners: [
            {
                name: 'keydown',
                func: 'onlyNumericKeyDown'
            },
            {
                name: 'keyup',
                func: 'onlyNumericKeyUp'
            },
        ]
    }
];
var onlyNumericKeyDown = function (e) {
    if ([46, 8, 9, 27, 13].indexOf(e.keyCode) !== -1 ||
        // Allow: Ctrl+A
        (e.keyCode === 65 && e.ctrlKey === true) ||
        // Allow: Ctrl+C
        (e.keyCode === 67 && e.ctrlKey === true) ||
        // Allow: Ctrl+V
        (e.keyCode === 86 && e.ctrlKey === true) ||
        // Allow: Ctrl+X
        (e.keyCode === 88 && e.ctrlKey === true) ||
        // Allow: home, end, left, right
        (e.keyCode >= 35 && e.keyCode <= 39)) {
        // let it happen, don't do anything
        return;
    }
    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {

        e.preventDefault();

    }
    if (e.target && e.target.maxLength) {
        const maxlength = +e.target.maxLength;
        if (e.target.value && (e.target.value + '').length > maxlength) {
            e.preventDefault();
        }
    }
}
var onlyNumericKeyUp = function (e) {
    if (e.target.value) {
        e.target.value = e.target.value.replace(/[^0-9]+/g, '');
    }
}