const id = location.search.substring(1).split("|")[0];
document.addEventListener("DOMContentLoaded", () => {
    loadComments(); });
document.addEventListener("load", () => {
    loadComments(); });

const ticketsBody = document.querySelector(".paleBlueRows > tbody");
const table = document.querySelector(".paleBlueRows");

function loadComments() {
    const request = new XMLHttpRequest();
    request.open("GET", "api/Tickets/" + id + "/updates");
    request.setRequestHeader("Authorization", "Bearer " + localStorage.getItem("token"));
    request.onload = ()=>{
        try {
            const json = JSON.parse(request.responseText);
            console.log(json);
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
        console.log(row);
        const tr = document.createElement("tr");

        var author = document.createElement("td");
        var creationDate = document.createElement("td");
        var comment = document.createElement("td");

        author.textContent = row.updaterId;
        comment.textContent = row.description;
        var dateDiff = Date.parse(row.creationDate);
        var date = new Date(dateDiff).toLocaleDateString('en-US');
        creationDate.textContent = date;

        tr.appendChild(author);
        tr.appendChild(creationDate);
        tr.appendChild(comment);
        ticketsBody.appendChild(tr);
    });
    tableBody = table.querySelector('tbody');
    rows = tableBody.querySelectorAll('tr');
    //loading.style.opacity = 0;
    //if (tableBody.querySelectorAll('tr').length === 0) {
        //noTicketsMsg.style.opacity = 1;
   // }

}