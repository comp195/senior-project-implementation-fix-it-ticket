const ticketsBody = document.querySelector(".paleBlueRows > tbody");
document.addEventListener("DOMContentLoaded", () => {
    loadTickets(); });
document.addEventListener("load", () => {
    loadTickets(); });
function loadTickets() {
    const request = new XMLHttpRequest();
    request.open("GET", "http://localhost:8000/temp-data.json");
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
        
        row.forEach((cell) => {
            const td = document.createElement("td");
            td.textContent = cell;
            tr.appendChild(td);
        })
        ticketsBody.appendChild(tr);
    });
} 
