var saveToTableScripts = "";

function SaveToTableScriptViewModel() {
    var self = this;
    self.classDescription = ko.observable("");
    self.classType = ko.observable("");
    self.SourceDB = ko.observable("");
    self.DestinationDB = ko.observable("");
    self.DestinationDBTableName = ko.observable("");
    self.DBOperation = ko.observable("");
    self.DBTransaction  = ko.observable("");
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
    self.SqlDestPreScript = ko.observable ("");
}
$(document).ready(function () {
    GetSaveToTableScripts();
});



function GetSaveToTableScripts() {
    $.ajax({
        url: "/FetchDataApi/dataApi/GetSaveToTable",
        cache: false,
        type: "GET",
        crossdomain: false,
        dataType: "json",
        ContentType: "application/json; charset=utf-8",
        success: function (response) {
            console.log(response);
            saveToTableScripts = response;
            var selectList = document.getElementById("SaveToTable");
            for (var i = 0; i < saveToTableScripts.length; i++) {
                var option = document.createElement("option");
                option.value = saveToTableScripts[i].serviceName;
                option.text = saveToTableScripts[i].serviceName;
                selectList.appendChild(option);
            }
            initializeSelectedScriptModel(saveToTableScripts[0]);

        },
        error: function (error) {
            console.log(error);
        }
    });
};



function selectCurrentScript(sel) {
    var selectedScriptName = sel.options[sel.selectedIndex].text;
    for (var i = 0; i < saveToTableScripts.length; i++) {
        if (saveToTableScripts[i].serviceName == selectedScriptName) {
            {
                initializeSelectedScriptModel(saveToTableScripts[i]);
                break;
            }
        }
    }
}
function initializeSelectedScriptModel(model) {
    if (SaveToTableScriptViewModel != null && SaveToTableScriptViewModel != undefined && model != null) {
        SaveToTableScriptViewModel.classDescription(model.classDescription);
        SaveToTableScriptViewModel.classType(model.classType);
        SaveToTableScriptViewModel.SourceDB(model.sourceDB);
        SaveToTableScriptViewModel.DestinationDB(model.destinationDB);
        SaveToTableScriptViewModel.DestinationDBTableName(model.destinationDBTableName);
        SaveToTableScriptViewModel.DBOperation(model.dbOperation);
        SaveToTableScriptViewModel.DBTransaction(model.dbTransaction);
        SaveToTableScriptViewModel.SqlDestPreScript(model.sqlDestPreScript );
        SaveToTableScriptViewModel.dbTimeout(model.dbTimeout);
        SaveToTableScriptViewModel.fullClassName(model.fullClassName);
        SaveToTableScriptViewModel.sendEmailOnFailure(model.sendEmailOnFailure);
        SaveToTableScriptViewModel.EmailOnSuccess(model.EmailOnSuccess);
        SaveToTableScriptViewModel.sendEmailTo(model.sendEmailTo);
        SaveToTableScriptViewModel.serviceId(model.serviceId);
        SaveToTableScriptViewModel.serviceName(model.serviceName);
        SaveToTableScriptViewModel.serviceType(model.serviceType);
        SaveToTableScriptViewModel.serviceVersion(model.serviceVersion);
        SaveToTableScriptViewModel.sqlParameters(model.sqlParameters);
        SaveToTableScriptViewModel.sqlScript(model.sqlScript);
        SaveToTableScriptViewModel.serviceId(model.serviceId);
        SaveToTableScriptViewModel.sendEmailTo(model.sendEmailTo);
        SaveToTableScriptViewModel.sendEmailOnSuccess(model.sendEmailOnSuccess);
        SaveToTableScriptViewModel.sendEmailOnFailure(model.sendEmailOnFailure);
        SaveToTableScriptViewModel.serviceVersion(model.serviceVersion);
    }
    else { return; }
    var params = {};
    params = SaveToTableScriptViewModel.sqlParameters();
    var paramsbody = document.getElementById("test-body");
    paramsbody.innerHTML = "";

    for (var prop in params) {
        if (Object.prototype.hasOwnProperty.call(params, prop)) {
            var new_row = '<tr id="row' + row + '"><td><input  value="' + prop + ' " name="from_value' + row + ' "    type="text"   class="form-control sqlparamskey" /></td><td><input  value="' + params[prop] + ' "  name="to_value' + row + '" type="text" class="form-control sqlparamsvalue" /></td><td><input class="delete-row btn btn-primary" type="button" value="Delete" /></td></tr>';
            // alert(new_row);
            $('#test-body').append(new_row);
            row++;
        }
    }
}
function selectCurrentDB(sel) {
    var timeoutInputBox = document.getElementById("timeout");
    timeoutInputBox.value = sel.options[sel.selectedIndex].text;
}

function saveNewFile() {
    var data = {};  data.DBTimeout = ""; data.SqlParameters = ""; data.SqlScript = "";
    var newFileName = document.getElementById("newfiletext");
    data.serviceName = newFileName.value;

    var timeout = document.getElementById("timeout");
    data.DBTimeout = timeout.value;

    var script = document.getElementById("scriptbody");
    data.SqlScript = script.value;
    var script = document.getElementById("sqldestprescript");
    data.SqlDestPreScript  = script.value;

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
    data.SourceDB  = document.getElementById("sourcedb").value;
    data.DestinationDB  = document.getElementById("destinationdb").value;
    data.DestinationDBTableName = document.getElementById("destinationtable").value;
    data.DBOperation = operation;
    var transaction = document.getElementById("transaction") ;
    data.DBTransaction = transaction != null ? transaction.checked  : false;
    var newfiletxt = document.getElementById("newfiletext");
    if (newfiletxt.value != "" || newfiletxt.value != null)
        data.serviceName = newfiletxt.value;
    else {
        alert("No service name was selected");
        return;
    }
    var sendEmailTo = document.getElementById("sendEmailTo").value;
    var sendEmailOnSuccess = document.getElementById("sendEmailOnSuccess").checked;
    var sendEmailOnFailure = document.getElementById("sendEmailOnFailure").checked;
    var serviceVersion = document.getElementById("serviceVersion").innerText;

    data.serviceVersion = serviceVersion;
    data.sendEmailTo = sendEmailTo;
    data.sendEmailOnFailure = sendEmailOnFailure;
    data.sendEmailOnSuccess = sendEmailOnSuccess;
   
    $.ajax({
        url: "/SaveToTable/CreateNewSaveToTableFile",
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


function ShowHideDb(elementname) {
    element = document.getElementById(elementname);
    if (element.type === "password")
        element.type = "text";
    else
        element.type = "password";
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

function updateFile() {
    var data = {}; data.DBTimeout = ""; data.SqlParameters = ""; data.SqlScript = "";
    var newFileName = document.getElementById("newfiletext");
    data.serviceName = newFileName.value;

    var timeout = document.getElementById("timeout");
    data.DBTimeout = timeout.value;

    var script = document.getElementById("scriptbody");
    data.SqlScript = script.value;
    var script = document.getElementById("sqldestprescript");
    data.SqlDestPreScript = script.value;

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
    data.SourceDB = document.getElementById("sourcedb").value;
    data.DestinationDB = document.getElementById("destinationdb").value;
    data.DestinationDBTableName = document.getElementById("destinationtable").value;
    data.DBOperation = operation;
    var transaction = document.getElementById("transaction");
    data.DBTransaction = transaction != null ? transaction.checked : false; data.serviceName = SaveToTableScriptViewModel.serviceName();
    data.serviceId = SaveToTableScriptViewModel.serviceId();
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
        url: "/SaveToTable/UpdateSaveToTableFile",
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

var operation = "";
function selectCurrentOperation(sel) {
    operation = sel.options[sel.selectedIndex].text;
}




function showNewFileText() {
    var newfiletxt = document.getElementById("newfiletext");
    newfiletxt.style.visibility = "visible";
    clearValues();
}



function clearValues() {
    var sourcedb = document.getElementById("sourcedb");
    sourcedb.value = "Server=(local)\\SERVER2016; user name=user;password=password; initial catalog=test ";
    var destinationdb = document.getElementById("destinationdb");
    destinationdb.value = "Server=(local)\\SERVER2016; user name=user;password=password; initial catalog=test ";
    var timeout = document.getElementById("timeout");
    timeout.value = "";
    var sqlscriptname = document.getElementById("sqlscriptname");
    if (sqlscriptname != null)
        sqlscriptname.value = "";
    var classtype = document.getElementById("classtype");
    if (classtype != null)
        classtype.value = "";
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
