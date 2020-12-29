var readFromCsvScripts = "";

function ReadFromCsvViewModel() {
    var self = this;
    self.classDescription = ko.observable("");
    self.classType = ko.observable("");
    self.SourceDB = ko.observable("");
    self.DestinationDB = ko.observable("");
    self.DestinationDBTableName = ko.observable("");
    self.DBOperation = ko.observable("");
    self.DBTransaction = ko.observable("");
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
    self.SqlDestPreScript = ko.observable("");
    self.CsvFileHeaders = ko.observable("");
    self.CsvFileHeader = ko.observable("");
    self.CsvFilePath = ko.observable("");
    self.CsvDelimenter = ko.observable("");
    self.CsvEncoding = ko.observable("");
}
$(document).ready(function () {
    GetReadFromCsvScripts();
});



function GetReadFromCsvScripts() {
    $.ajax({
        url: "/FetchDataApi/dataApi/GetReadFromCsv",
        cache: false,
        type: "GET",
        crossdomain: false,
        dataType: "json",
        ContentType: "application/json; charset=utf-8",
        success: function (response) {
            console.log(response);
            readFromCsvScripts = response;
            var selectList = document.getElementById("ReadFromCsv");
            for (var i = 0; i < readFromCsvScripts.length; i++) {
                var option = document.createElement("option");
                option.value = readFromCsvScripts[i].serviceName;
                option.text = readFromCsvScripts[i].serviceName;
                selectList.appendChild(option);
            }
            initializeSelectedScriptModel(readFromCsvScripts[0]);

        },
        error: function (error) {
            console.log(error);
        }
    });
};



function selectCurrentScript(sel) {
    var selectedScriptName = sel.options[sel.selectedIndex].text;
    for (var i = 0; i < readFromCsvScripts.length; i++) {
        if (readFromCsvScripts[i].serviceName == selectedScriptName) {
            {
                initializeSelectedScriptModel(readFromCsvScripts[i]);
                break;
            }
        }
    }
}
function initializeSelectedScriptModel(model) {
    
    if (ReadFromCsvViewModel != null && ReadFromCsvViewModel != undefined && model != null) {
        ReadFromCsvViewModel.classDescription(model.classDescription);
       ReadFromCsvViewModel.classType(model.classType);
       ReadFromCsvViewModel.DestinationDB(model.destinationDB);
       ReadFromCsvViewModel.DestinationDBTableName(model.destinationDBTableName);
       ReadFromCsvViewModel.DBOperation(model.dbOperation);
       ReadFromCsvViewModel.DBTransaction(model.dbTransaction);
       ReadFromCsvViewModel.SqlDestPreScript(model.sqlDestPreScript);
       ReadFromCsvViewModel.dbTimeout(model.dbTimeout);
       ReadFromCsvViewModel.fullClassName(model.fullClassName);
       ReadFromCsvViewModel.sendEmailOnFailure(model.sendEmailOnFailure);
       ReadFromCsvViewModel.EmailOnSuccess(model.EmailOnSuccess);
       ReadFromCsvViewModel.sendEmailTo(model.sendEmailTo);
       ReadFromCsvViewModel.serviceId(model.serviceId);
       ReadFromCsvViewModel.serviceName(model.serviceName);
       ReadFromCsvViewModel.serviceType(model.serviceType);
       ReadFromCsvViewModel.serviceVersion(model.serviceVersion);
       ReadFromCsvViewModel.sqlParameters(model.sqlParameters);
       ReadFromCsvViewModel.sqlScript(model.sqlScript);
       ReadFromCsvViewModel.serviceId(model.serviceId);
       ReadFromCsvViewModel.sendEmailTo(model.sendEmailTo);
       ReadFromCsvViewModel.sendEmailOnSuccess(model.sendEmailOnSuccess);
       ReadFromCsvViewModel.sendEmailOnFailure(model.sendEmailOnFailure);
        ReadFromCsvViewModel.serviceVersion(model.serviceVersion);
        ReadFromCsvViewModel.CsvFileHeaders(model.csvFileHeaders);
        ReadFromCsvViewModel.CsvFileHeader(model.csvFileHeader);
        ReadFromCsvViewModel.CsvFilePath(model.csvFilePath);
        ReadFromCsvViewModel.CsvDelimenter(model.csvDelimenter);
        ReadFromCsvViewModel.CsvEncoding(model.csvEncoding);
    }
    else { return; }
    var params = {};
    params = ReadFromCsvViewModel.CsvFileHeaders();
    var paramsbody = document.getElementById("test-body");
    paramsbody.innerHTML = "";

    for (var prop in params) {
        if (Object.prototype.hasOwnProperty.call(params, prop)) {
            var new_row = '<tr id="row' + row + '"><td><input  value="' + prop + ' " name="from_value' + row + ' "    type="text"   class="form-control sqlparamskey" /></td><td><input class="delete-row btn btn-primary" type="button" value="Delete" /></td></tr>';
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
    var data = {}; data.DBTimeout = ""; data.SqlParameters = ""; 
    var newFileName = document.getElementById("newfiletext");
    data.serviceName = newFileName.value;

    var timeout = document.getElementById("timeout");
    data.DBTimeout = timeout.value;


    var script = document.getElementById("sqldestprescript");
    data.SqlDestPreScript = script.value;

    var sqlparamskeys = document.getElementsByClassName("sqlparamskey");
      var i;
    
    var allParams = [];
    for (i = 0; i < sqlparamskeys.length; i++) {
        allParams[i] = sqlparamskeys[i].value;
    }
    
    data.CsvFileHeaders = allParams;
    var csvpath = document.getElementById("csvpath");
    data.CsvFilePath = csvpath.value;

    var fheader = document.getElementById("fileheader");
    data.CsvFileHeader = fheader != null ? fheader.checked : false;

    var csvdelim = document.getElementById("csvdelim");
    var csvencod = document.getElementById("csvencod");

    data.CsvDelimenter = csvdelim.options[csvdelim.selectedIndex].text;
    data.CsvEncoding = csvencod.options[csvencod.selectedIndex].text;
        
    data.DestinationDB = document.getElementById("destinationdb").value;
    data.DestinationDBTableName = document.getElementById("destinationdb").value;
    data.DBOperation = operation;
    var transaction = document.getElementById("transaction");
    data.DBTransaction = transaction != null ? transaction.checked : false;
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
        url: "/ReadCsv/CreateNewReadCsvFile",
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
    var new_row = '<tr id="row' + row + '"><td><input name="from_value' + row + '" type="text" class="form-control sqlparamskey" /></td><td><input class="delete-row btn btn-primary" type="button" value="Delete" /></td></tr>';
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
    var data = {}; data.DBTimeout = ""; data.SqlParameters = ""; 
    var newFileName = document.getElementById("newfiletext");
    data.serviceName = newFileName.value;

    var timeout = document.getElementById("timeout");
    data.DBTimeout = timeout.value;

    
    var script = document.getElementById("sqldestprescript");
    data.SqlDestPreScript = script.value;

    var sqlparamskeys = document.getElementsByClassName("sqlparamskey");
   
    var i;
    var paramsObj = {};
    var allParams = [];
    for (i = 0; i < sqlparamskeys.length; i++) {
        paramsObj = sqlparamskeys[i].value;
        allParams.push(paramsObj)
        paramsObj = {};
    }
    data.CsvFileHeaders = allParams;
    var csvpath = document.getElementById("csvpath");
    data.CsvFilePath = csvpath.value;

    var fheader = document.getElementById("fileheader");
    data.CsvFileHeader = fheader != null ? fheader.checked : false;

    var csvdelim = document.getElementById("csvdelim");
    var csvencod = document.getElementById("csvencod");

    data.CsvDelimenter = csvdelim.options[csvdelim.selectedIndex].text;
    data.CsvEncoding = csvencod.options[csvencod.selectedIndex].text;    
    data.DestinationDB = document.getElementById("destinationdb").value;
    data.DestinationDBTableName = document.getElementById("destinationtable").value;
    data.DBOperation = operation;
    var transaction = document.getElementById("transaction");
    data.DBTransaction = transaction != null ? transaction.checked : false; data.serviceName = ReadFromCsvViewModel.serviceName();
    data.serviceId = ReadFromCsvViewModel.serviceId();
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
        url: "/ReadCsv/UpdateExistingReadFromCsvScript",
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
