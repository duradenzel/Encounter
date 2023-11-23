function Search() {
    var input, filter, table, tr, i, txtValue;
    input = document.getElementById('search');
    filter = input.value.toUpperCase();
    table = document.getElementById("monster-table");
    tr = table.getElementsByTagName('tr');

    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[0];
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}


let monsterData = []; 

async function loadMonsterList() {
    try {
        const response = await fetch('/Encounter/GetMonsterList');
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        monsterData = await response.json();
        loadMonsterPage(currentPage);
    } catch (error) {
        console.error(error);
    }
}


function displayMonsterList(monsters) {
    const tableRow = document.getElementById("monster-list-body");
    tableRow.innerHTML = ''; 

    for (const i of monsters) {
        var row = document.createElement('tr');
        row.innerHTML = `<th scope='row'><button class='add-monster' data-name='${i.name}' data-cr='${i.cr}' data-type='${i.type}' data-size='${i.size}' data-exp='${i.experiencePoints}'>Add</button></th><td>${i.name}</td><td>${i.cr}</td><td>${i.type}</td><td>${i.size}</td><td>${i.experiencePoints}</td>`;
        tableRow.appendChild(row);
        var btns = row.querySelectorAll(".add-monster");
        for (const b of btns) {
            b.addEventListener("click",(e)=>{GetRowMonster(e.target)});
        }
    }
}


function GetRowMonster(row) {
      const name = row.getAttribute('data-name');
      const cr = row.getAttribute('data-cr');
      const type = row.getAttribute('data-type');
      const size = row.getAttribute('data-size');
      const exp = row.getAttribute('data-exp');
  
      const monster = {
          Name: name,
          CR: cr,
          Type: type,
          Size: size,
          Exp: exp
      };
  
      var monsterBlock = document.createElement('div');
      monsterBlock.classList.add('monster');
  
      monsterBlock.innerHTML = `
          <div class="monster-details">
              <h2 class="monster-name">${monster.Name}</h2>
              <p class="monster-type">${monster.Size} ${monster.Type}</p>
          </div>
          <div class="monster-info">
              <p class="monster-cr-exp">CR: <span id="monster-cr">${monster.CR}</span> | Exp: <span id="monster-exp">${monster.Exp}</span></p>
              <div class="monster-quantity">
                  <button class="quantity-increase">+</button>
                  <input type="number" class="quantity-input" value="1">
                  <button class="quantity-decrease">-</button>
              </div>
          </div>
      `;
  
      document.getElementById('monster-list').appendChild(monsterBlock);
      UpdateExp(monsterBlock.children[1], 'increase');
  
}



const monstersPerPage = 10; 
let currentPage = 1; 

$(document).ready(function () {
   
    loadMonsterPage(currentPage);
 
    $('#prev-page').click(function () {
        if (currentPage > 1) {
            currentPage--;
            loadMonsterPage(currentPage);
        }
    });

    $('#next-page').click(function () {
        if (currentPage < Math.ceil(monsterData.length / monstersPerPage)) {
            currentPage++;
            loadMonsterPage(currentPage);
        }
    });

    $('#pagination-list').on('click', 'li', function () {
        currentPage = parseInt($(this).text());
        loadMonsterPage(currentPage);
    });
   
    
});

function loadMonsterPage(page) {
    const startIndex = (page - 1) * monstersPerPage;
    const endIndex = startIndex + monstersPerPage;

    const monstersOnPage = monsterData.slice(startIndex, endIndex);
    displayMonsterList(monstersOnPage);

    updatePaginationButtons(page);
}



function updatePaginationButtons(currentPage) {
    const totalPages = Math.ceil(monsterData.length / monstersPerPage);
    const paginationList = $('#pagination-list');
    paginationList.empty();

    const maxPagesToShow = 5; // Maximum number of page numbers to display

    if (totalPages <= maxPagesToShow) {
        // Display all page numbers if there are fewer than or equal to maxPagesToShow
        for (let i = 1; i <= totalPages; i++) {
            addPageNumber(paginationList, currentPage, i);
        }
    } else {
        // Determine the range of page numbers to display
        let startPage, endPage;

        if (currentPage <= maxPagesToShow - 2) {
            // When you are on the first few pages
            startPage = 1;
            endPage = maxPagesToShow - 1;
        } else if (currentPage >= totalPages - maxPagesToShow + 2) {
            // When you are on the last few pages
            startPage = totalPages - maxPagesToShow + 2;
            endPage = totalPages;
        } else {
            // When you are somewhere in the middle
            startPage = currentPage - 1;
            endPage = currentPage + 1;
        }

        // Always add the first page number
        addPageNumber(paginationList, currentPage, 1);

        if (startPage > 2) {
            // Add dots if not starting from the first page
            paginationList.append('<li>...</li>');
        }

        for (let i = startPage; i <= endPage; i++) {
            // Skip adding the first and last page numbers again
            if (i !== 1 && i !== totalPages) {
                addPageNumber(paginationList, currentPage, i);
            }
        }

        if (endPage < totalPages - 1) {
            // Add dots if not ending at the last page
            paginationList.append('<li>...</li>');
        }

        // Always add the last page number
        addPageNumber(paginationList, currentPage, totalPages);
    }

    // Disable/enable previous and next buttons based on the current page
    $('#prev-page').prop('disabled', currentPage === 1);
    $('#next-page').prop('disabled', currentPage === totalPages);
}


function addPageNumber(paginationList, currentPage, pageNumber) {
    const li = $('<li>');
    li.text(pageNumber);

    if (pageNumber === currentPage) {
        li.addClass('active');
    }

    paginationList.append(li);
}


