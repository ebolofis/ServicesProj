var currentUrl = (window.location.protocol) + "//" + (window.location.hostname) + (window.location.port != "" ? ":" + window.location.port : "") + "/";
function SaveConfiguration() {
    var usrname = document.getElementById("username").value;
    var password = document.getElementById("password").value;
    var sender = document.getElementById("sender").value;
    var port      = document.getElementById("port").value;
    var smtp = document.getElementById("smtp").value;
    var ssl = document.getElementById("ssl");
    if (ssl.checked == true)
        ssl = "true";
    else
        ssl = false;
    
    var row = {
        username: usrname,
        password: password,
        sender: sender,
        port: port,
        smtp: smtp,
        ssl: ssl,
    };

    SendData(row) 
}

function testEmail() {
    var testemail = document.getElementById("testemail").value;
    var usrname = document.getElementById("username").value;
    var password = document.getElementById("password").value;
    var sender = document.getElementById("sender").value;
    var port = document.getElementById("port").value;
    var smtp = document.getElementById("smtp").value;
    var ssl = document.getElementById("ssl");
    
    if (ssl.checked == true)
        ssl = "true";
    else
        ssl = false;
    var row = {
        username: usrname,
        password: password,
        sender: sender,
        port: port,
        smtp: smtp,
        ssl: ssl,
        testemail: testemail
    };
    Test(row);
}

function SendData(data) {
    $.ajax({
        url: "/SMTP/SaveConfiguration",
        cache: false,
        type: "POST",
        crossdomain: false,
        dataType: "json",
        ContentType: "application/json; charset=utf-8",
        data: data,
        success: function (response) {
        },
        error: function (error) {
            
        }
    });

};



function Test(data) {
    $.ajax({
        url: "/SMTP/TestEmail",
        cache: false,
        type: "POST",
        crossdomain: false,
        dataType: "json",
        ContentType: "application/json; charset=utf-8",
        data: data,
        success: function (response) {
        },
        error: function (error) {
            if (error.responseText != "") {

                iziToast.error({
                    title: "Error",
                    message: error.responseText
                });
            }
            else {
                iziToast.success({
                    title: "Success",
                    message: "Valid Email Test",
                    //timeout: 2000
                });

            }
        }
    });

};
