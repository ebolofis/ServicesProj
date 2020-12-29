// tabs
$(document).ready(function () {

    var tabLinks = document.querySelectorAll(".tablinks");
    var tabContent = document.querySelectorAll(".tabcontent");

    GetExportDataScripts();

    tabLinks.forEach(function (el) {
        el.addEventListener("click", openTabs);
    });


    function openTabs(el) {
        var btnTarget = el.currentTarget;
        var country = btnTarget.dataset.country;

        tabContent.forEach(function (el) {
            el.classList.remove("active");
        });

        tabLinks.forEach(function (el) {
            el.classList.remove("active");
        });

        document.querySelector("#" + country).classList.add("active");

        btnTarget.classList.add("active");
    }

});

function ExportDataViewModel() {
    var self = this;
    self.sqlParameters = ko.observable("");
    self.classDescription = ko.observable("");
    self.classType = ko.observable("");
    self.SourceDB = ko.observable("");
    self.DestinationDB = ko.observable("");
    self.custom1DB = ko.observable("");
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
    self.SqlScript = ko.observable("");
    self.CsvFileHeaders = ko.observable("");
    self.CsvFileHeader = ko.observable("");
    self.CsvFilePath = ko.observable("");
    self.CsvDelimenter = ko.observable("");
    self.timeStamp = ko.observable("");
    self.cultureInfo = ko.observable("");
    self.formater = ko.observable("");
    self.xmlElement = ko.observable("");
    self.xmlFilePath = ko.observable("");
    self.xmlRootElement = ko.observable("");
    self.jsonFilePath = ko.observable("");
    self.fixedLenghtFileHeader = ko.observable("");
    self.fixedLenghtAlignRight = ko.observable("");
    self.fixedLengths = ko.observable("");
    self.fixedLenghtFilePath = ko.observable("");
    self.htmlHeader = ko.observable("");
    self.htmlTitle = ko.observable("");
    self.htmlcss = ko.observable("");
    self.pdfcss = ko.observable("");

    self.restServerAuthenticationHeader = ko.observable("");
    self.restServerAuthenticationType = ko.observable("");
    self.restServerHttpMethod = ko.observable("");
    self.restServerMediaType = ko.observable("");
    self.restServerUrl = ko.observable("");
    self.restServerCustomHeaders = ko.observable("");

}

var exportDataScripts = "";

function GetExportDataScripts() {
    $.ajax({
        url: "/FetchDataApi/dataApi/GetExportData",
        cache: false,
        type: "GET",
        crossdomain: false,
        dataType: "json",
        ContentType: "application/json; charset=utf-8",
        success: function (response) {
            console.log(response);
            exportDataScripts = response;
            var selectList = document.getElementById("ExportData");
            for (var i = 0; i < exportDataScripts.length; i++) {
                var option = document.createElement("option");
                option.value = exportDataScripts[i].serviceName;
                option.text = exportDataScripts[i].serviceName;
                selectList.appendChild(option);
            }
            initializeSelectedScriptModel(exportDataScripts[0]);

        },
        error: function (error) {
            console.log(error);
        }
    });
};




function selectCurrentScript(sel) {
    var selectedScriptName = sel.options[sel.selectedIndex].text;
    for (var i = 0; i < exportDataScripts.length; i++) {
        if (exportDataScripts[i].serviceName == selectedScriptName) {
            {
                initializeSelectedScriptModel(exportDataScripts[i]);
                break;
            }
        }
    }
}
function initializeSelectedScriptModel(model) {

    if (ExportDataViewModel != null && ExportDataViewModel != undefined && model != null) {
        ExportDataViewModel.classDescription(model.classDescription);
        ExportDataViewModel.classType(model.classType);
        ExportDataViewModel.DestinationDB(model.destinationDB);
        ExportDataViewModel.custom1DB(model.custom1DB);
        ExportDataViewModel.DestinationDBTableName(model.destinationDBTableName);
        ExportDataViewModel.DBOperation(model.dbOperation);
        ExportDataViewModel.DBTransaction(model.dbTransaction);
        ExportDataViewModel.SqlScript(model.sqlScript);
        ExportDataViewModel.dbTimeout(model.dbTimeout);
        ExportDataViewModel.fullClassName(model.fullClassName);
        ExportDataViewModel.sendEmailOnFailure(model.sendEmailOnFailure);
        ExportDataViewModel.EmailOnSuccess(model.EmailOnSuccess);
        ExportDataViewModel.sendEmailTo(model.sendEmailTo);
        ExportDataViewModel.serviceId(model.serviceId);
        ExportDataViewModel.serviceName(model.serviceName);
        ExportDataViewModel.serviceType(model.serviceType);
        ExportDataViewModel.serviceVersion(model.serviceVersion);
        ExportDataViewModel.sqlParameters(model.sqlParameters);
        ExportDataViewModel.sqlScript(model.sqlScript);
        ExportDataViewModel.serviceId(model.serviceId);
        ExportDataViewModel.sendEmailTo(model.sendEmailTo);
        ExportDataViewModel.sendEmailOnSuccess(model.sendEmailOnSuccess);
        ExportDataViewModel.sendEmailOnFailure(model.sendEmailOnFailure);
        ExportDataViewModel.serviceVersion(model.serviceVersion);
        ExportDataViewModel.CsvFileHeaders(model.csvFileHeaders);
        ExportDataViewModel.CsvFileHeader(model.csvFileHeader);
        ExportDataViewModel.CsvFilePath(model.csvFilePath);
        ExportDataViewModel.CsvDelimenter(model.csvDelimenter);
        ExportDataViewModel.sqlParameters(model.sqlParameters);
        ExportDataViewModel.timeStamp(model.timeStamp);
        ExportDataViewModel.cultureInfo(model.cultureInfo);
        ExportDataViewModel.formater(model.formater);
        ExportDataViewModel.xmlElement(model.xmlElement);
        ExportDataViewModel.xmlFilePath(model.xmlFilePath);
        ExportDataViewModel.xmlRootElement(model.xmlRootElement);
        ExportDataViewModel.jsonFilePath(model.jsonFilePath);
        ExportDataViewModel.fixedLenghtFileHeader(model.fixedLenghtFileHeader);
        ExportDataViewModel.fixedLenghtAlignRight(model.fixedLenghtAlignRight);
        ExportDataViewModel.fixedLengths(model.fixedLengths);
        ExportDataViewModel.fixedLenghtFilePath(model.fixedLenghtFilePath);
        ExportDataViewModel.htmlHeader(model.htmlHeader);
        ExportDataViewModel.htmlTitle(model.htmlTitle);
        ExportDataViewModel.htmlcss(model.htmlcss);
        ExportDataViewModel.pdfcss(model.pdfcss);
        ExportDataViewModel.restServerAuthenticationHeader(model.restServerAuthenticationHeader);
        ExportDataViewModel.restServerAuthenticationType(model.restServerAuthenticationType);
        ExportDataViewModel.restServerHttpMethod(model.restServerHttpMethod);
        ExportDataViewModel.restServerMediaType(model.restServerMediaType);
        ExportDataViewModel.restServerUrl(model.restServerUrl);
        ExportDataViewModel.restServerCustomHeaders(model.restServerCustomHeaders);

    }
    else { return; }
    var params = {};
    params = ExportDataViewModel.sqlParameters();
    var paramsbody = document.getElementById("test-body");
    paramsbody.innerHTML = "";

    for (var prop in params) {
        if (Object.prototype.hasOwnProperty.call(params, prop)) {
            var new_row = '<tr id="row' + row + '"><td><input  value="' + prop + ' " name="from_value' + row + ' "    type="text"   class="form-control sqlparamskey" /></td><td><input  value="' + params[prop] + ' "  name="to_value' + row + '" type="text" class="form-control sqlparamsvalue" /></td><td><input class="delete-row btn btn-primary" type="button" value="Delete" /></td></tr>';
            $('#test-body').append(new_row);
            row++;
        }
    }


    var exparams = {};
    exparams = ExportDataViewModel.formater();
    var exparamsbody = document.getElementById("test-body-export");
    exparamsbody.innerHTML = "";

    for (var prop in exparams) {
        if (Object.prototype.hasOwnProperty.call(exparams, prop)) {
            var new_row = '<tr id="row' + row + '"><td><input  value="' + prop + ' " name="from_value' + row + ' "    type="text"   class="form-control exporttofileparamskey" /></td><td><input  value="' + exparams[prop] + ' "  name="to_value' + row + '" type="text" class="form-control exporttofileparamsvalue" /></td><td><input class="delete-row btn btn-primary" type="button" value="Delete" /></td></tr>';
            $('#test-body-export').append(new_row);
            row++;
        }

    }

    var apiparams = {};
    apiparams = ExportDataViewModel.restServerCustomHeaders();
    var apiparamsbody = document.getElementById("test-body-api");
    apiparamsbody.innerHTML = "";

    for (var prop in apiparams) {
        if (Object.prototype.hasOwnProperty.call(apiparams, prop)) {
            var new_row = '<tr id="row' + row + '"><td><input  value="' + prop + ' " name="from_value' + row + ' "    type="text"   class="form-control apiparamskey" /></td><td><input  value="' + apiparams[prop] + ' "  name="to_value' + row + '" type="text" class="form-control apiparamsvalue" /></td><td><input class="delete-row btn btn-primary" type="button" value="Delete" /></td></tr>';
            $('#test-body-api').append(new_row);
            row++;
        }

    }


}

function selectCurrentDB(sel) {
    var timeoutInputBox = document.getElementById("timeout");
    timeoutInputBox.value = sel.options[sel.selectedIndex].text;
}

function selectCultureInfo(sel) {
    var cultureInfoBox = document.getElementById("cultureinfo");
    cultureInfoBox.value = sel.options[sel.selectedIndex].innerText;
}


function saveNewFile() {
    var data = {}; data.DBTimeout = ""; data.SqlParameters = "";
    var newFileName = document.getElementById("newfiletext");
    data.serviceName = newFileName.value;

    var timeout = document.getElementById("timeout");
    data.DBTimeout = timeout.value;


    var script = document.getElementById("scriptbody");
    data.SqlScript = script.value;


    var cultureInfo = document.getElementById("cultureinfo");
    data.CultureInfo = cultureInfo.value;

    var timeStamp = document.getElementById("timestamp");
    data.TimeStamp = timeStamp.value;

    var xmlFilePath = document.getElementById("xmlFilePath");
    data.XmlFilePath = xmlFilePath.value;
    var xmlRootElement = document.getElementById("xmlRootElement");
    data.XmlRootElement = xmlRootElement.value;
    var xmlElement = document.getElementById("xmlElement");
    data.XmlElement = xmlElement.value;
    var jsonFilePath = document.getElementById("jsonFilePath");
    data.JsonFilePath = jsonFilePath.value;
    var fixedLenghtFileHeader = document.getElementById("fixedLenghtFileHeader");
    data.FixedLenghtFileHeader = fixedLenghtFileHeader.checked;
    var fixedLenghtFilePath = document.getElementById("fixedLenghtFilePath");
    data.FixedLenghtFilePath = fixedLenghtFilePath.value;


    var pdfcss = document.getElementById("pdfcss");
    data.Pdfcss = pdfcss.value;
    var fixedLenghtAlignRight = document.getElementById("fixedLenghtAlignRight");
    data.FixedLenghtAlignRight = fixedLenghtAlignRight.checked;

    var fixedLengths = document.getElementById("fixedLengths");
    data.FixedLengths = fixedLengths.value;

    var htmlHeader = document.getElementById("htmlHeader");
    data.HtmlHeader = htmlHeader.checked;

    var htmlTitle = document.getElementById("htmlTitle");
    data.HtmlTitle = htmlTitle.value;
    var htmlcss = document.getElementById("htmlcss");
    data.HtmlCss = htmlcss.value;

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


    var exportparamskeys = document.getElementsByClassName("exporttofileparamskey");
    var exportparamsvalues = document.getElementsByClassName("exporttofileparamsvalue");
    var i;
    var exparamsObj = {};
    var exallParams = [];
    for (i = 0; i < exportparamskeys.length; i++) {
        exparamsObj.key = exportparamskeys[i].value;
        exparamsObj.value = exportparamsvalues[i].value;
        exallParams.push(exparamsObj)
        exparamsObj = {};
    }
    data.Formater = exallParams;


    var restServerMediaType = document.getElementById("restServerMediaType");
    data.RestServerMediaType = restServerMediaType.value;

    var restServerAuthenticationType = document.getElementById("restServerAuthenticationType");
    data.RestServerAuthenticationType = restServerAuthenticationType.value;

    var restServerHttpMethod = document.getElementById("restServerHttpMethod");
    data.RestServerHttpMethod = restServerHttpMethod.value;

    var restServerAuthenticationHeader = document.getElementById("restServerAuthenticationHeader");
    data.RestServerAuthenticationHeader = restServerAuthenticationHeader.value;

    var restServerUrl = document.getElementById("restServerUrl");
    data.RestServerUrl = restServerUrl.value;

    var apiparamskeys = document.getElementsByClassName("apiparamskey");
    var apiparamsvalues = document.getElementsByClassName("apiparamsvalue");
    var i;
    var apiparamsObj = {};
    var apiallParams = [];
    for (i = 0; i < apiparamskeys.length; i++) {
        apiparamsObj.key = apiparamskeys[i].value;
        apiparamsObj.value = apiparamsvalues[i].value;
        apiallParams.push(apiparamsObj)
        apiparamsObj = {};
    }
    data.restServerCustomHeaders = apiallParams;


    var csvpath = document.getElementById("csvpath");
    data.CsvFilePath = csvpath.value;

    var fheader = document.getElementById("fileheader");
    data.CsvFileHeader = fheader != null ? fheader.checked : false;

    var csvdelim = document.getElementById("csvdelim");

    data.CsvDelimenter = csvdelim.options[csvdelim.selectedIndex].text;




    db1 = document.getElementById("db1");
    data.Custom1DB = db1.value;
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
        url: "/ExportData/CreateNewExportDataFile",
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

var exportfilerow = 1;
$(document).on("click", "#add-row-export", function () {
    var new_row = '<tr id="row' + row + '"><td><input name="from_value' + row + '" type="text" class="form-control exporttofileparamskey" /></td><td><input name="to_value' + row + '" type="text" class="form-control exporttofileparamsvalue" /></td><td><input class="delete-row btn btn-primary" type="button" value="Delete" /></td></tr>';
    // alert(new_row);
    $('#test-body-export').append(new_row);
    exportfilerow++;
    return false;
});

$(document).on("click", "#add-row-api", function () {
    var new_row = '<tr id="row' + row + '"><td><input name="from_value' + row + '" type="text" class="form-control apiparamskey" /></td><td><input name="to_value' + row + '" type="text" class="form-control apiparamsvalue" /></td><td><input class="delete-row btn btn-primary" type="button" value="Delete" /></td></tr>';
    // alert(new_row);
    $('#test-body-api').append(new_row);
    exportfilerow++;
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


function showNewFileText() {
    var newfiletxt = document.getElementById("newfiletext");
    newfiletxt.style.visibility = "visible";
    clearValues();
}


function updateFile() {
    var data = {}; data.DBTimeout = ""; data.SqlParameters = "";
    var newFileName = document.getElementById("newfiletext");
    data.serviceName = newFileName.value;

    var timeout = document.getElementById("timeout");
    data.DBTimeout = timeout.value;


    var script = document.getElementById("scriptbody");
    data.SqlScript = script.value;

    var cultureInfo = document.getElementById("cultureinfo");
    data.CultureInfo = cultureInfo.value;

    var timeStamp = document.getElementById("timestamp");
    data.TimeStamp = timeStamp.value;

    var jsonFilePath = document.getElementById("jsonFilePath");
    data.JsonFilePath = jsonFilePath.value;

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
    var pdfcss = document.getElementById("pdfcss");
    data.Pdfcss = pdfcss.value;
    var fixedLenghtAlignRight = document.getElementById("fixedLenghtAlignRight");
    data.FixedLenghtAlignRight = fixedLenghtAlignRight.checked;
    var xmlFilePath = document.getElementById("xmlFilePath");
    data.XmlFilePath = xmlFilePath.value;
    var xmlRootElement = document.getElementById("xmlRootElement");
    data.XmlRootElement = xmlRootElement.value;
    var xmlElement = document.getElementById("xmlElement");
    data.XmlElement = xmlElement.value;
    var fixedLenghtFilePath = document.getElementById("fixedLenghtFilePath");
    data.FixedLenghtFilePath = fixedLenghtFilePath.value;

    var fixedLenghtFileHeader = document.getElementById("fixedLenghtFileHeader");
    data.FixedLenghtFileHeader = fixedLenghtFileHeader.checked;

    var exportparamskeys = document.getElementsByClassName("exporttofileparamskey");
    var exportparamsvalues = document.getElementsByClassName("exporttofileparamsvalue");
    var fixedLengths = document.getElementById("fixedLengths");
    data.FixedLengths = fixedLengths.value;

    var htmlHeader = document.getElementById("htmlHeader");
    data.HtmlHeader = htmlHeader.checked;

    var htmlTitle = document.getElementById("htmlTitle");
    data.HtmlTitle = htmlTitle.value;
    var htmlcss = document.getElementById("htmlcss");
    data.HtmlCss = htmlcss.value;

    var i;
    var exparamsObj = {};
    var exallParams = [];
    for (i = 0; i < exportparamskeys.length; i++) {
        exparamsObj.key = exportparamskeys[i].value;
        exparamsObj.value = exportparamsvalues[i].value;
        exallParams.push(exparamsObj)
        exparamsObj = {};
    }
    data.Formater = exallParams;
    // data.CsvFileHeaders = allParams;
    var csvpath = document.getElementById("csvpath");
    data.CsvFilePath = csvpath.value;

    var fheader = document.getElementById("fileheader");
    data.CsvFileHeader = fheader != null ? fheader.checked : false;

    var csvdelim = document.getElementById("csvdelim");
    var csvencod = document.getElementById("csvencod");

    data.CsvDelimenter = csvdelim.options[csvdelim.selectedIndex].text;

    db1 = document.getElementById("db1");
    data.Custom1DB = db1.value;


    var restServerMediaType = document.getElementById("restServerMediaType");
    data.RestServerMediaType = restServerMediaType.value;

    var restServerAuthenticationType = document.getElementById("restServerAuthenticationType");
    data.RestServerAuthenticationType = restServerAuthenticationType.value;

    var restServerHttpMethod = document.getElementById("restServerHttpMethod");
    data.RestServerHttpMethod = restServerHttpMethod.value;

    var restServerAuthenticationHeader = document.getElementById("restServerAuthenticationHeader");
    data.RestServerAuthenticationHeader = restServerAuthenticationHeader.value;

    var restServerUrl = document.getElementById("restServerUrl");
    data.RestServerUrl = restServerUrl.value;

    var apiparamskeys = document.getElementsByClassName("apiparamskey");
    var apiparamsvalues = document.getElementsByClassName("apiparamsvalue");
    var i;
    var apiparamsObj = {};
    var apiallParams = [];
    for (i = 0; i < apiparamskeys.length; i++) {
        apiparamsObj.key = apiparamskeys[i].value;
        apiparamsObj.value = apiparamsvalues[i].value;
        apiallParams.push(apiparamsObj)
        apiparamsObj = {};
    }
    data.restServerCustomHeaders = apiallParams;


    data.DBOperation = operation;
    var transaction = document.getElementById("transaction");
    data.DBTransaction = transaction != null ? transaction.checked : false; data.serviceName = ExportDataViewModel.serviceName();
    data.serviceId = ExportDataViewModel.serviceId();
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
        url: "/ExportData/UpdateExistingExportDataScript",
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
function selectHttpMethod(sel) {
    document.getElementById("restServerHttpMethod").value = sel.options[sel.selectedIndex].text;
}
function selectMediaType(sel) {
    document.getElementById("restServerMediaType").value = sel.options[sel.selectedIndex].text;
}

function selectFixedLength(sel) {
    document.getElementById("fixedLengths").value = sel.options[sel.selectedIndex].text;
}

function selectAuthenticationType(sel) {
    document.getElementById("restServerAuthenticationType").value = sel.options[sel.selectedIndex].text;
}

function showNewFileText() {
    var newfiletxt = document.getElementById("newfiletext");
    newfiletxt.style.visibility = "visible";
    clearValues();
}

function clearValues() {

    var db1 = document.getElementById("db1");
    db1.value = "Server=(local)\\SERVER2016; user name=user;password=password; initial catalog=test";
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