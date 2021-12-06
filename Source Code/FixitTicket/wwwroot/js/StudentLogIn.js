
const user = document.getElementById("username-field");
const pass = document.getElementById("password-field")
const loginButton = document.getElementById("login-form-submit");
const loginErrorMsgText = document.getElementById("Your_username_andor_password__");

loginButton.addEventListener("click", (e) => {
    e.preventDefault();
    const username = user.value.trim(" ");
    const password = pass.value.trim(" ");
    Token(username, password);
})

document.addEventListener("submit", (e) => {
    e.preventDefault();
    const username = user.value.trim(" ");
    const password = pass.value.trim(" ");

    Token(username, password);
    /*if (username in studentIDs && password === studentIDs[username]) {
        window.location.href = "resident_landing_page.html";
    }
    else if (username in employeeIDs && password === employeeIDs[username]) {
        window.location.href = "employee_landing_page.html";
    }
    else {
        loginErrorMsg.opacity = 1;
        loginErrorMsgText.style.opacity = 1;
    }*/
})

function Token(username, password) {
    const uri = 'api/Token';
    const log = {
        Email: username,
        Password: password
    }
    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(log)
    })
        .then(response => response.json())
        .then((token) => StoreToken(token));
}

function StoreToken(token) {
    if(token.token === undefined) {
        loginErrorMsgText.style.opacity = 1;
    }
    else {
        const ur = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        loginErrorMsgText.style.opacity = 0;
        localStorage.setItem("token", token.token);
        var decoded = parseJwt(token.token);
        window.location.href = decoded[ur].toLowerCase() + "_landing_page.html";
    }
}

function parseJwt (token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}
