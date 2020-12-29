var sqlScripts = "";
var db1 = document.getElementById("db1");
var timeout = document.getElementById("timeout");
var sqlscriptname = document.getElementById("sqlscriptname");
var classtype = document.getElementById("classtype");
var settings = document.getElementById("settings");
var classdescription = document.getElementById("classdescription");
var classname = document.getElementById("classname");
var script = document.getElementById("scriptbody");

function SqlScriptViewModel() {
    var self = this;
    self.classDescription = ko.observable("");
    self.classType = ko.observable("");
    self.custom1DB = ko.observable("");
    self.dbTimeout = ko.observable("");
    self.fullClassName = ko.observable("");
    self.sendEmailOnFailure = ko.observable("");
    self.EmailOnSuccess = ko.observable("");
    self.sendEmailTo = ko.observable("");
    self.serviceId = ko.observable("");
    self.serviceName = ko.observable("");
    self.serviceType = ko.observable("");
    self.serviceVersion = ko.observable("");
    self.sqlParameters = ko.observable("");
    self.sqlScript = ko.observable("");
    self.serviceId = ko.observable("");
    self.sendEmailTo = ko.observable("");
    self.sendEmailOnFailure = ko.observable("");
    self.serviceVersion = ko.observable("");
    self.sendEmailOnSuccess = ko.observable("");
}

function selectCurrentScript(sel) {
    var selectedScriptName = sel.options[sel.selectedIndex].text;
    for (var i = 0; i < sqlScripts.length; i++) {
        if (sqlScripts[i].serviceName == selectedScriptName) {
            {
                initializeSelectedScriptModel(sqlScripts[i]);
                break;
            }
        }
    }
}
function initializeSelectedScriptModel(model) {
    if (SqlScriptViewModel != null && SqlScriptViewModel != undefined && model !=null) {
        SqlScriptViewModel.classDescription(model.classDescription);
        SqlScriptViewModel.classType(model.classType);
        SqlScriptViewModel.custom1DB(model.custom1DB);
        SqlScriptViewModel.dbTimeout(model.dbTimeout);
        SqlScriptViewModel.fullClassName(model.fullClassName);
        SqlScriptViewModel.sendEmailOnFailure(model.sendEmailOnFailure);
        SqlScriptViewModel.EmailOnSuccess(model.EmailOnSuccess);
        SqlScriptViewModel.sendEmailTo(model.sendEmailTo);
        SqlScriptViewModel.serviceId(model.serviceId);
        SqlScriptViewModel.serviceName(model.serviceName);
        SqlScriptViewModel.serviceType(model.serviceType);
        SqlScriptViewModel.serviceVersion(model.serviceVersion);
        SqlScriptViewModel.sqlParameters(model.sqlParameters);
        SqlScriptViewModel.sqlScript(model.sqlScript);
        SqlScriptViewModel.serviceId(model.serviceId);
        SqlScriptViewModel.sendEmailTo(model.sendEmailTo);
        SqlScriptViewModel.sendEmailOnSuccess(model.sendEmailOnSuccess);
        SqlScriptViewModel.sendEmailOnFailure(model.sendEmailOnFailure);
        SqlScriptViewModel.serviceVersion(model.serviceVersion);
    }
    else { return;}
    var params = {};
    params = SqlScriptViewModel.sqlParameters();
    var paramsbody = document.getElementById("test-body");
    paramsbody.innerHTML = "";
    
    for (var prop in params) {
        if (Object.prototype.hasOwnProperty.call(params, prop)) {
            var new_row = '<tr id="row' + row + '"><td><input  value="' + prop + ' " name="from_value' + row + ' "    type="text"   class="form-control sqlparamskey" /></td><td><input  value="' + params[prop] +  ' "  name="to_value' + row + '" type="text" class="form-control sqlparamsvalue" /></td><td><input class="delete-row btn btn-primary" type="button" value="Delete" /></td></tr>';
            // alert(new_row);
            $('#test-body').append(new_row);
            row++;
        }
    }
}
$(document).ready(function () {
    GetSqlScripts();
});
function selectCurrentDB(sel) {
    var timeoutInputBox = document.getElementById("timeout");
    timeoutInputBox.value = sel.options[sel.selectedIndex].text;
}


function ShowHideDb() {
    var database = document.getElementById("db1");

    if (database.type === "password")
        database.type = "text";
    else
        database.type = "password";
}
function showNewFileText() {
    var newfiletxt = document.getElementById("newfiletext");
    newfiletxt.style.visibility = "visible";
    clearValues();
}

function GetSqlScripts() {
    $.ajax({
        url: "/FetchDataApi/dataApi/GetSqlScripts" ,
        cache: false,
        type: "GET",
        crossdomain: false,
        dataType: "json",
        ContentType: "application/json; charset=utf-8",
        success: function (response) {
            console.log(response);
            sqlScripts = response;
            var selectList = document.getElementById("SqlScripts");
            for (var i = 0; i < sqlScripts.length; i++) {
                var option = document.createElement("option");                
                option.value = sqlScripts[i].serviceName;
                option.text = sqlScripts[i].serviceName;
                selectList.appendChild(option);
            }
            initializeSelectedScriptModel(sqlScripts[0]);

        },
        error: function (error) {
            console.log(error);
        }
    });
};

function clearValues() {
    var db1= document.getElementById("db1");
    db1.value = "Server=(local)\\SERVER2016; user name=user;password=password; initial catalog=test ";
    var timeout = document.getElementById("timeout");
    timeout.value = "";
    var sqlscriptname = document.getElementById("sqlscriptname");
    if (sqlscriptname !=null)
    sqlscriptname.value = "";  
    var classtype = document.getElementById("classtype");
    if (classtype !=null)
    classtype.value = ""; 
    //var settings = document.getElementById("settings");
    //settings.value = ""; 
    var classdescription = document.getElementById("classdescription");
    if (classdescription != null)
    classdescription.value = ""; 
    var classname = document.getElementById("classname");
    if (classname != null)
    classname.value = ""; 
    var scriptbody = document.getElementById("scriptbody");
    if (scriptbody != null)
    scriptbody.value = "";  

}
function updateFile() {
    var data = {}; data.Custom1DB = ""; data.DBTimeout = ""; data.SqlParameters = ""; data.SqlScript = "";
    
    var timeout = document.getElementById("timeout");
    data.DBTimeout = timeout.value;
    db1 = document.getElementById("db1");
    data.Custom1DB = db1.value;
    var script = document.getElementById("scriptbody");
    data.SqlScript = script.value;

    data.classdescription = descstr;
    var sqlparamskeys = document.getElementsByClassName("sqlparamskey");
    var sqlparamsvalues = document.getElementsByClassName("sqlparamsvalue");
    var i;
    var paramsObj = {};
    var allParams = [];
    for (i = 0; i < sqlparamskeys.length; i++) {
        paramsObj.key = sqlparamskeys[i].value;
        paramsObj.value = sqlparamsvalues[i].value;
        allParams.push(paramsObj)
        paramsObj = {};
    }
    data.SqlParameters = allParams;
    data.serviceName = SqlScriptViewModel.serviceName();
    data.serviceId = SqlScriptViewModel.serviceId();
    var sendEmailTo = document.getElementById("sendEmailTo").value;
    var sendEmailOnSuccess = document.getElementById("sendEmailOnSuccess").checked;
    var sendEmailOnFailure = document.getElementById("sendEmailOnFailure").checked;
    var serviceVersion = document.getElementById("serviceVersion").innerText;
    
    data.serviceVersion = serviceVersion;
    data.sendEmailTo = sendEmailTo;
    data.sendEmailOnFailure = sendEmailOnFailure;
    data.sendEmailOnSuccess = sendEmailOnSuccess;
    
    updateExistingFile(data);
}
function updateExistingFile(data) {
    $.ajax({
        url: "/SqlScripts/UpdateExistingSqlScript",
        cache: false,
        type: "POST",
        crossdomain: false,
        dataType: "json",
        ContentType: "application/json;  charset=utf-8",
        data: data,
        success: function (response) {

        },
        error: function (error) {
        }
    });
 
    window.location.reload();
}
function saveNewFile() {
    var data = {}; data.Custom1DB = ""; data.DBTimeout = ""; data.SqlParameters = ""; data.SqlScript = "";
    var newFileName = document.getElementById("newfiletext");
    data.serviceName = newFileName.value;

    var timeout = document.getElementById("timeout");
    data.DBTimeout = timeout.value;

    db1 = document.getElementById("db1");
        data.Custom1DB = db1.value;

    var script = document.getElementById("scriptbody");
    data.SqlScript = script.value;

    var sqlparamskeys = document.getElementsByClassName("sqlparamskey");
    var sqlparamsvalues = document.getElementsByClassName("sqlparamsvalue");
    var i;
    var paramsObj = {};
    var allParams = [];
    for (i = 0; i < sqlparamskeys.length; i++) {
        paramsObj.key = sqlparamskeys[i].value;
        paramsObj.value = sqlparamsvalues[i].value;
        allParams.push(paramsObj)
        paramsObj = {};
        
    }
    data.SqlParameters = allParams;

    var newfiletxt = document.getElementById("newfiletext");
    if (newfiletxt.value != "" || newfiletxt.value != null)
        data.serviceName = newfiletxt.value;
    else {
        alert("No service name was selected");
        return;
    }

    //alert(data.serviceName);
    $.ajax({
        url: "/SqlScripts/CreateNewFile",
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
    window.location.reload();
    
}
// Add row
var row = 1;
$(document).on("click", "#add-row", function () {
    var new_row = '<tr id="row' + row + '"><td><input name="from_value' + row + '" type="text" class="form-control sqlparamskey" /></td><td><input name="to_value' + row + '" type="text" class="form-control sqlparamsvalue" /></td><td><input class="delete-row btn btn-primary" type="button" value="Delete" /></td></tr>';
   // alert(new_row);
    $('#test-body').append(new_row);
    row++;
    return false;
});

// Remove criterion
$(document).on("click", ".delete-row", function () {
    //  alert("deleting row#"+row);
    if (row > 1) {
        $(this).closest('tr').remove();
        row--;
    }
    return false;
});
