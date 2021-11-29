
let path = window.location.pathname;
let page = path.split("/").pop();
document.addEventListener("DOMContentLoaded", () => {
    loadTickets(); });
document.addEventListener("load", () => {
    loadTickets(); });


const ticketsBody = document.querySelector(".paleBlueRows > tbody");
const table = document.querySelector(".paleBlueRows");
const headers = table.querySelectorAll('th');

[].forEach.call(headers, function (header, index) {
    header.addEventListener('click', function () {
        sortColumn(index);
    });
});
let tableBody = null;
let rows = null;

const sortColumn = function (index) {
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
        const a = transform(index, cellA);
        const b = transform(index, cellB);
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

const transform = function (index, content) {
    const type = headers[index].getAttribute('data-type');
    switch (type) {
        case 'number':
            return parseFloat(content);
        case 'string':
        default:
            return content;
    }
};

const directions = Array.from(headers).map(function (header) {
    return '';
});




function loadTickets() {
    const request = new XMLHttpRequest();
    request.open("GET", "api/Tickets");
    request.onload = ()=>{
        try {
            const json = JSON.parse(request.responseText);
            populateTickets(json);
        }
        catch(e) {
            console.warn("Could not load tickets!");
        }
    };
    request.send();
}

function populateTickets(json) {
    while(ticketsBody.firstChild) {
        ticketsBody.removeChild(ticketsBody.firstChild);
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
        ticketsBody.appendChild(tr);
    });
    tableBody = table.querySelector('tbody');
    rows = tableBody.querySelectorAll('tr');
    document.querySelector(".paleBlueRows tbody").addEventListener("click", function(event) {
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
    
    window.location.href = "/update_ticket.html?" + data[0].innerText;
}






    
    
