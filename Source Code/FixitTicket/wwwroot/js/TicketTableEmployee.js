let employeeID = 989271234;
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
const loading2 = document.getElementById("loadingMessage2");
const noTicketsMsg = document.getElementById("noTicketsMessage");
const noTicketsMsg2 = document.getElementById("noTicketsMessage2");

[].forEach.call(yourHeaders, function (header, index) {
    header.addEventListener('click', function () {
        sortColumn(index, yourHeaders, yourTicketsBody);
    });
});
[].forEach.call(allHeaders, function (header, index) {
    header.addEventListener('click', function () {
        sortColumn(index, allHeaders, allTicketsBody);
    });
});


const sortColumn = function (index, headers, body) {
    // Get the current direction
    let direction;
    if (headers === allHeaders) {
        direction = directions2[index] || 'asc';
    }
    else if(headers === yourHeaders) {
        direction = directions1[index] || 'asc';
    }
    

    // A factor based on the direction
    const multiplier = (direction === 'asc') ? 1 : -1;
    const newRows = Array.from(body.querySelectorAll('tr'));
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
    [].forEach.call(body.querySelectorAll('tr'), function (row) {
        body.removeChild(row);
    });

    // Append new row
    newRows.forEach(function (newRow) {
        body.appendChild(newRow);
    });
    if (headers === allHeaders) {
        directions2[index] = direction === 'asc' ? 'desc' : 'asc';
    }
    else if(headers === yourHeaders) {
        directions1[index] = direction === 'asc' ? 'desc' : 'asc';
    }
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

const directions1 = Array.from(yourHeaders).map(function (header) {
    return '';
});
const directions2 = Array.from(allHeaders).map(function (header) {
    return '';
});

function loadTickets() {
    const request = new XMLHttpRequest();
    request.open("GET", "api/Tickets");
    request.setRequestHeader("Authorization", "Bearer " + localStorage.getItem("token"));
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
        var location = document.createElement("td");
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
        tr.appendChild(location);
        tr.appendChild(repairCategory);
        tr.appendChild(status);
        tr.appendChild(creationDate);
        tr.appendChild(assignedId);
        tr.appendChild(comments);
        GetTicketLocation(row.id, tr);
        allTicketsBody.appendChild(tr);
    });
    loading.style.opacity = 0;
    json.forEach((row) => {
        if (employeeID === row.assignedId) {
            const tr = document.createElement("tr");

            var id = document.createElement("td");
            var residentId = document.createElement("td");
            var location = document.createElement("td");
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
            tr.appendChild(location);
            tr.appendChild(repairCategory);
            tr.appendChild(status);
            tr.appendChild(creationDate);
            tr.appendChild(assignedId);
            tr.appendChild(comments);
            GetTicketLocation(row.id, tr);
            yourTicketsBody.appendChild(tr);
        }
    });
    loading2.style.opacity = 0;
    if (yourTicketsBody.querySelectorAll('tr').length === 0) {
        noTicketsMsg.style.opacity = 1;
    }
    if (allTicketsBody.querySelectorAll('tr').length === 0) {
        noTicketsMsg2.style.opacity = 1;
    }
    
    allTicketsBody.addEventListener("click", function(event) {
        var t = event.target;
        if(t.textContent === "Click to View") {
            window.location.href = "/view_comments.html?" + t.parentNode.children[0].innerText + "|employee";
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
        if(t.textContent === "Click to View") {
            window.location.href = "/view_comments.html?" + t.parentNode.children[0].innerText + "|employee";
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
    window.location.href = "/update_ticket.html?" + data[0].innerText + "|employee";
}

function GetTicketLocation(ticketId, tr) {
    const request = new XMLHttpRequest();
    request.open("GET", "api/Tickets/" + ticketId + "/location");
    request.setRequestHeader("Authorization", "Bearer " + localStorage.getItem("token"));
    request.onload = () => {
        try {
            const json = JSON.parse(request.responseText);
            tr.children[2].textContent = json.building;
        }
        catch (e) {
            console.warn("Could not load tickets!");
        }
    };
    request.send();
    return location;
}






    
    
