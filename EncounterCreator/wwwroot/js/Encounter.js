$(document).ready(function () {
       
       $(document).on('click', '.quantity-increase', function () {
        var input = $(this).siblings('.quantity-input');
        input.val(parseInt(input.val()) + 1);
        UpdateExp(this.parentElement.parentElement, "increase");
    });

    
    $(document).on('click', '.quantity-decrease', function () {
        var input = $(this).siblings('.quantity-input');
        if (parseInt(input.val()) > 1) {
            input.val(parseInt(input.val()) - 1);
        } else if (parseInt(input.val()) === 1) {
            this.parentElement.parentElement.parentElement.remove();
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
    var monsters = [];

    $('.monster').each(function () {
        var monsterName = $(this).find('.monster-name').text();
        var monsterSizeType = $(this).find('.monster-type').text().split(' ');
        var monsterSize = monsterSizeType[0];
        var monsterType = monsterSizeType[1];
        var monsterCR = $(this).find('.monster-cr-exp #monster-cr').text();
        var monsterExp = $(this).find('.monster-cr-exp #monster-exp').text();

        var monsterQuantity = parseInt($(this).find('.quantity-input').val()) || 1;

        for (var i = 0; i < monsterQuantity; i++) {
            var monster = {
                Name: monsterName,
                Size: monsterSize,
                Type: monsterType,
                CR: monsterCR,
                ExperiencePoints: monsterExp
            };
            monsters.push(monster);
        }
    });

    var encounterData = {
        Monsters: monsters,
        Difficulty: $('#encounter-difficulty').text(), 
        TotalExp: parseInt($('#exp-total').text()) || 0,
        AdjustedExp: parseInt($('#exp-adjusted').text()) || 0,
        XpSums: null
    };

    console.log(encounterData);
    var encounterResult = JSON.stringify(encounterData);

    $.ajax({
        type: 'POST',
        url: '/Encounter/SaveEncounter',
        contentType: 'application/json',
        data: encounterResult,
        success: function (response) {
            console.log(response);
            // Handle success
        },
        error: function (error) {
            console.error(error);
            // Handle error
        }
    });
}

