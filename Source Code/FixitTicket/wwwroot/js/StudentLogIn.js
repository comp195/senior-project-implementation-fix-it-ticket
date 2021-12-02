const user = document.getElementById("username-field");
const pass = document.getElementById("password-field")
const loginButton = document.getElementById("login-form-submit");
const loginErrorMsg = document.getElementById("Rectangle_1095");
const loginErrorMsgText = document.getElementById("Your_username_andor_password__")
let employeeIDs = {"employee": "Arshita"};
let studentIDs = {"user": "web_dev"};

loginButton.addEventListener("click", (e) => {
    e.preventDefault();
    const username = user.value.trim(" ");
    const password = pass.value.trim(" ");

    if (username in studentIDs && password === studentIDs[username]) {
        window.location.href = "resident_landing_page.html";
    } 
    else if (username in employeeIDs && password === employeeIDs[username]) {
        window.location.href = "employee_landing_page.html";
    }
    else {
        loginErrorMsg.opacity = 1;
        loginErrorMsgText.style.opacity = 1;
    }
})

document.addEventListener("submit", (e) => {
    e.preventDefault();
    const username = user.value.trim(" ");
    const password = pass.value.trim(" ");

    if (username in studentIDs && password === studentIDs[username]) {
        window.location.href = "resident_landing_page.html";
    }
    else if (username in employeeIDs && password === employeeIDs[username]) {
        window.location.href = "employee_landing_page.html";
    }
    else {
        loginErrorMsg.opacity = 1;
        loginErrorMsgText.style.opacity = 1;
    }
})