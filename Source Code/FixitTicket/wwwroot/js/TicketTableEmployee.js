let employeeID = 989306958;
let path = window.location.pathname;
let page = path.split("/").pop();
document.addEventListener("DOMContentLoaded", () => {
    loadTickets(); });
document.addEventListener("load", () => {
    loadTickets(); });


const yourTicketsBody = document.querySelector(".yourpaleBlueRows > tbody");
const allTicketsBody = document.querySelector(".allpaleBlueRows > tbody");
const yourTable = document.querySelector(".yourpaleBlueRows");
const allTable = document.querySelector(".allpaleBlueRows");
const yourHeaders = yourTable.querySelectorAll('th');
const allHeaders = allTable.querySelectorAll('th');
const loading = document.getElementById("loadingMessage");
const noTicketsMsg = document.getElementById("noTicketsMessage");

[].forEach.call(yourHeaders, function (header, index) {
    header.addEventListener('click', function () {
        sortColumn(index, yourHeaders);
    });
});
[].forEach.call(allHeaders, function (header, index) {
    header.addEventListener('click', function () {
        sortColumn(index, allHeaders);
    });
});



let tableBody = null;
let rows = null;

const sortColumn = function (index, headers) {
    const directions = Array.from(headers).map(function (header) {
        return '';
    });
    // Get the current direction
    const direction = directions[index] || 'asc';

    // A factor based on the direction
    const multiplier = (direction === 'asc') ? 1 : -1;
    const newRows = Array.from(rows);
    // Sort rows by the content of cells
    newRows.sort(function (rowA, rowB) {
        // Get the content of cells
        const cellA = rowA.querySelectorAll('td')[index].innerHTML;
        const cellB = rowB.querySelectorAll('td')[index].innerHTML;
        const a = transform(index, cellA, headers);
        const b = transform(index, cellB, headers);
        switch (true) {
            case a > b: return 1 * multiplier;
            case a < b: return -1 * multiplier;
            case a === b: return 0;
        }
    });

    // Remove old rows
    [].forEach.call(rows, function (row) {
        tableBody.removeChild(row);
    });

    // Append new row
    newRows.forEach(function (newRow) {
        tableBody.appendChild(newRow);
    });
    directions[index] = direction === 'asc' ? 'desc' : 'asc';
};

const transform = function (index, content, headers) {
    const type = headers[index].getAttribute('data-type');
    switch (type) {
        case 'number':
            return parseFloat(content);
        case 'string':
        default:
            return content;
    }
};

function loadTickets() {
    const request = new XMLHttpRequest();
    request.open("GET", "api/Tickets");
    request.onload = ()=>{
        try {
            const json = JSON.parse(request.responseText);
            populateTickets(json);
        }
        catch(e) {
            console.log(e);
            console.warn("Could not load tickets!");
        }
    };
    request.send();
}

function populateTickets(json) {
    while(yourTicketsBody.firstChild) {
        yourTicketsBody.removeChild(yourTicketsBody.firstChild);
    }
    while(allTicketsBody.firstChild) {
        allTicketsBody.removeChild(allTicketsBody.firstChild);
    }
    json.forEach((row) => {
        const tr = document.createElement("tr");

        var id = document.createElement("td");
        var residentId = document.createElement("td");
        var repairCategory = document.createElement("td");
        var status = document.createElement("td");
        var creationDate = document.createElement("td");
        var assignedId = document.createElement("td");
        var comments = document.createElement("td");
        id.textContent = row.id;
        residentId.textContent = row.residentId;
        repairCategory.textContent = row.repairCategory;
        status.textContent = row.status;
        comments.textContent = "Click to View";
        var dateDiff = Date.parse(row.creationDate);
        var date = new Date(dateDiff).toLocaleDateString('en-US');
        creationDate.textContent = date;
        assignedId.textContent = row.assignedId ?? "";
        tr.appendChild(id);
        tr.appendChild(residentId);
        tr.appendChild(repairCategory);
        tr.appendChild(status);
        tr.appendChild(creationDate);
        tr.appendChild(assignedId);
        tr.appendChild(comments);
        allTicketsBody.appendChild(tr);
    });
    json.forEach((row) => {
        if (employeeID == row.assignedId) {
            const tr = document.createElement("tr");

            var id = document.createElement("td");
            var residentId = document.createElement("td");
            var repairCategory = document.createElement("td");
            var status = document.createElement("td");
            var creationDate = document.createElement("td");
            var assignedId = document.createElement("td");
            var comments = document.createElement("td");
            id.textContent = row.id;
            residentId.textContent = row.residentId;
            assignedId.textContent = row.assignedId ?? "";
            repairCategory.textContent = row.repairCategory;
            status.textContent = row.status;
            comments.textContent = "Click to View";
            var dateDiff = Date.parse(row.creationDate);
            var date = new Date(dateDiff).toLocaleDateString('en-US');
            creationDate.textContent = date;
            tr.appendChild(id);
            tr.appendChild(residentId);
            tr.appendChild(repairCategory);
            tr.appendChild(status);
            tr.appendChild(creationDate);
            tr.appendChild(assignedId);
            tr.appendChild(comments);
            yourTicketsBody.appendChild(tr);
        }
    });
    
    tableBody = allTable.querySelector('tbody');
    rows = tableBody.querySelectorAll('tr');
    loading.style.opacity = 0;
    if (yourTicketsBody.querySelectorAll('tr').length > 0) {
        noTicketsMsg.style.opacity = 0;
    }
    
    allTicketsBody.addEventListener("click", function(event) {
        var t = event.target;
        console.log("ick")
        if(t.textContent == "Click to View") {
            console.log("BLAH BLAH")
            return;
        }
        while (t !== this && !t.matches("tr")) {
            t = t.parentNode;
        }
        if (t === this) {
            console.log("No table cell found");
        } else {
            GrabUpdateTicket(t);
        }
    });

    yourTicketsBody.addEventListener("click", function(event) {
        var t = event.target;
        if(t.textContent == "Click to View") {
            console.log("BLAH BLAH")
            return;
        }
        while (t !== this && !t.matches("tr")) {
            t = t.parentNode;
        }
        if (t === this) {
            console.log("No table cell found");
        } else {
            GrabUpdateTicket(t);
        }
    });
    
}

function GrabUpdateTicket(row) {
    var data = row.children;
    console.log("hello");
    window.location.href = "/update_ticket.html?" + data[0].innerText + "|employee";
}






    
    
