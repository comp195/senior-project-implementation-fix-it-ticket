
async function UpdateTicketStart() {
    
    var residentId = document.getElementById("resid");
    const repairCategory = document.getElementById("Select_an_option");
    const status = document.getElementById("Select_an_option_ew");
    const assID = document.getElementById("assignedid")
    const description = document.getElementById("description");
    var ticketNum = document.getElementById("ticketNum");
    let json = null;
    let id = location.search.substring(1).split("|")[0];
    const request = new XMLHttpRequest();
    request.open("GET", "api/Tickets/" + id);
    request.onload = ()=>{
        try {
            json = JSON.parse(request.responseText);
            ticketNum.textContent = json.id;
            residentId.value = json.residentId;
            repairCategory.textContent = json.repairCategory;
            status.textContent = json.repairStatus;
            assID.value = json.assignedId;
            description.textContent = json.description;
            console.log(ticketNum.textContent);
        }
        catch(e) {
            console.warn("Could not load ticket!");
        }
    };
    request.send();
}