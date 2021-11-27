const ticketsBody = document.querySelector(".paleBlueRows > tbody");
document.addEventListener("DOMContentLoaded", () => {
    loadTickets(); });
document.addEventListener("load", () => {
    loadTickets(); });
const tempDataHolder = "http://localhost:8000/temp-data.json"

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
    console.log(json);
    json.forEach((row) => {
        const tr = document.createElement("tr");

        var id = document.createElement("td");
        var residentId = document.createElement("td");
        var repairCategory = document.createElement("td");
        var status = document.createElement("td");
        var creationDate = document.createElement("td");
        var assignedId = document.createElement("td");
        id.textContent = row.id;
        residentId.textContent = row.residentId;
        repairCategory.textContent = row.repairCategory;
        status.textContent = row.status;
        var dateDiff = Date.parse(row.creationDate);
        var date = new Date(dateDiff);
        creationDate.textContent = date;
        assignedId.textContent = row.assignedId ?? "";
        tr.appendChild(id);
        tr.appendChild(residentId);
        tr.appendChild(repairCategory);
        tr.appendChild(status);
        tr.appendChild(creationDate);
        tr.appendChild(assignedId);

        ticketsBody.appendChild(tr);
    });
} 
