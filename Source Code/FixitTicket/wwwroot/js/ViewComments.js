document.addEventListener("DOMContentLoaded", () => {
    loadComments(); });
document.addEventListener("load", () => {
    loadComments(); });

function loadComments() {
    const request = new XMLHttpRequest();
    request.setRequestHeader("Authorization", "Bearer " + localStorage.getItem("token"));
    request.open("GET", "api/Tickets");
    request.onload = ()=>{
        try {
            const json = JSON.parse(request.responseText);
            populateComments(json);
        }
        catch(e) {
            console.warn("Could not load Comments!");
        }
    };
    request.send();
}

function populateComments(json) {
    while(ticketsBody.firstChild) {
        ticketsBody.removeChild(ticketsBody.firstChild);
    }
    json.forEach((row) => {
        if(row.residentId === residentID) {
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
        }
    });
    tableBody = table.querySelector('tbody');
    rows = tableBody.querySelectorAll('tr');
    loading.style.opacity = 0;
    if (tableBody.querySelectorAll('tr').length === 0) {
        noTicketsMsg.style.opacity = 1;
    }

    document.querySelector(".paleBlueRows tbody").addEventListener("click", function(event) {
        var t = event.target;
        if(t.textContent === "Click to View") {
            window.location.href = "/view_comments.html?" + t.parentNode.children[0].innerText + "|resident";
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