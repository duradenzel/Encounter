$(document).ready(function () {
    $('.quantity-increase').click(function () {
        var input = $(this).siblings('.quantity-input');
        input.val(parseInt(input.val()) + 1);
        UpdateExp(this.parentElement.parentElement, "increase");
    });

    $('.quantity-decrease').click(function () {
        var input = $(this).siblings('.quantity-input');
        if (parseInt(input.val()) > 1) {
            input.val(parseInt(input.val()) - 1);
        }
        else if (parseInt(input.val()) == 1) {
            this.parentElement.parentElement.parentElement.remove()
        }
        UpdateExp(this.parentElement.parentElement, "decrease");
    });


    loadMonsterList();
});




function UpdateExp(data, type) {

    var exp = data.children[0].children[1].textContent;
    var totalexp = document.getElementById('exp-total');
    console.log(totalexp);

    if (type == "increase") {
        totalexp.textContent = parseInt(totalexp.textContent) + parseInt(exp);
    }
    else {
        totalexp.textContent = parseInt(totalexp.textContent) - parseInt(exp);
    }
}