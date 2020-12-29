function redirectToValidate() {
    var currentUrl = (window.location.protocol) + "//" + (window.location.hostname) + (window.location.port != "" ? ":" + window.location.port : "") + "/";
    var usernameValue = document.getElementById("login__username").value;
    var passwordValue = document.getElementById("login__password").value;
    var link = currentUrl + "Login/Validation?username=" + usernameValue + "&password=" + passwordValue;
    window.location.href = link;
}
function redirectToSMTPConfiguration() {
    var currentUrl = (window.location.protocol) + "//" + (window.location.hostname) + (window.location.port != "" ? ":" + window.location.port : "") + "/";
    var link = currentUrl + "SMTP/Configuration";
    window.location.href = link;
}
function redirectToConfiguration(isAdmin) {
    var currentUrl = (window.location.protocol) + "//" + (window.location.hostname) + (window.location.port != "" ? ":" + window.location.port : "") + "/";
    var link = currentUrl + "DataGrid/Configuration";
    window.location.href = link;
}
function redirectToInitializeDB() {
    var currentUrl = (window.location.protocol) + "//" + (window.location.hostname) + (window.location.port != "" ? ":" + window.location.port : "") + "/";
    var link = currentUrl + "InitializeDB/InitializeDB";
    window.location.href = link;
}    

function redirectToScheduler() {
    var currentUrl = (window.location.protocol) + "//" + (window.location.hostname) + (window.location.port != "" ? ":" + window.location.port : "") + "/";
    var link = currentUrl + "Scheduler";
    window.location.href = link;
}

function redirectToLogin() {
    var currentUrl = (window.location.protocol) + "//" + (window.location.hostname) + (window.location.port != "" ? ":" + window.location.port : "") + "/";
    var link = currentUrl + "Login/Logout?logout=true";
    window.location.href = link;
}
function redirectToServices() {
    var currentUrl = (window.location.protocol) + "//" + (window.location.hostname) + (window.location.port != "" ? ":" + window.location.port : "") + "/";
    var link = currentUrl + "Services/Index";
    window.location.href = link;
}

function redirectToExportDataServices() {
    var currentUrl = (window.location.protocol) + "//" + (window.location.hostname) + (window.location.port != "" ? ":" + window.location.port : "") + "/";
    var link = currentUrl + "ExportData/Index";
    window.location.href = link;
}

function redirectToReadCsvServices() {
    var currentUrl = (window.location.protocol) + "//" + (window.location.hostname) + (window.location.port != "" ? ":" + window.location.port : "") + "/";
    var link = currentUrl + "ReadCsv/Index";
    window.location.href = link;
}

function redirectToSaveToTableServices() {
    var currentUrl = (window.location.protocol) + "//" + (window.location.hostname) + (window.location.port != "" ? ":" + window.location.port : "") + "/";
    var link = currentUrl + "SaveToTable/Index";
    window.location.href = link;
}

function redirectToSqlScriptsServices() {
    var currentUrl = (window.location.protocol) + "//" + (window.location.hostname) + (window.location.port != "" ? ":" + window.location.port : "") + "/";
    var link = currentUrl + "SqlScripts/Index";
    window.location.href = link;
}


