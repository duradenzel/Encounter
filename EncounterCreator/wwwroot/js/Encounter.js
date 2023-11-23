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

function SaveEncounter() {
    // Create an array to store the monsters
    var monsters = [];

    // Iterate through each monster in the encounter
    $('.monster').each(function () {
        var monsterName = $(this).find('.monster-name').text();
        var monsterQuantity = parseInt($(this).find('.quantity-input').val());

        // Add the monster to the array as a nested object
        monsters.push({
            Name: monsterName,
            Quantity: monsterQuantity
        });
    });

    // Create an object to send to the controller
    var encounterData = {
        Monsters: monsters,
        // Add any other data you want to send
    };

    // Use jQuery AJAX to send the data to the controller
    $.ajax({
        type: 'POST',
        url: '/Encounter/SaveEncounter', // Replace with your controller and action names
        contentType: 'application/json',
        data: JSON.stringify(encounterData),
        success: function (data) {
            // Handle the success response
            console.log('Encounter saved successfully!');
        },
        error: function (error) {
            // Handle the error response
            console.error('Error saving encounter:', error);
        }
    });
}