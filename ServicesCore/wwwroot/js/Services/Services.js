function redirectToScheduledTasks(serviceId) {
    if (serviceId == "null") {
        alert("The current Service is incative");
    return;
}
    var currentUrl = (window.location.protocol) + "//" + (window.location.hostname) + (window.location.port != "" ? ":" + window.location.port : "") + "/";
    var link = currentUrl + "ScheduledTasks/Index?serviceId=" + serviceId;
    window.location.href = link;
}

function redirectToScheduler() {
    var currentUrl = (window.location.protocol) + "//" + (window.location.hostname) + (window.location.port != "" ? ":" + window.location.port : "") + "/";
    var link = currentUrl + "Scheduler";
    window.location.href = link;
}



function fireForget(serviceId) {
    if (serviceId == "null") {
        alert("The current Service is inactive");
        return;
    }
    var data = {};
    data.serviceId = serviceId;
    FireAndForget(data);
}


function FireAndForget(data) {

    $.ajax({
        url: "/Services/FireAndForget",
        cache: false,
        type: "POST",
        crossdomain: false,
        dataType: "json",
        data: data,
        ContentType: "application/json; charset=utf-8",
        success: function (response) {
        },
        error: function (error) {
            if (error.responseText != "OK") {

                iziToast.error({
                    title: "Error",
                    message: error.responseText
                });
            }
            else {
                iziToast.success({
                    title: "Success",
                    message: "Fire and Forget Initiated",
                    //timeout: 2000
                });
            }
        }
    });

};


function changeStatusToActive(serviceId) {
    var link = currentUrl + "Services/ChangeStatusToActive?serviceId=" + serviceId;
    window.location.href = link;
}


function changeStatusToInActive(serviceId) {
   
    var link = currentUrl + "Services/ChangeStatusToInActive?serviceId=" + serviceId;
    window.location.href = link;
}



