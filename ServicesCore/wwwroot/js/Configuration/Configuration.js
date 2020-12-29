
function checkConnection(key) {
    var elements = document.getElementsByClassName(key);
    for (const val of elements) {
        if (val.value != key) {
            var arrValues = {};
            if (val.localName === "input") {
                arrValues.constr = val.value;
                CheckConnectionString(arrValues)
            }
        }
    }

}

function addDbRow(key) {
    var btn = "";
    var eyebtn = document.getElementsByClassName("spaneye");
    for (const val of eyebtn) {
        if (val.id == key || (val.id == "no" + key))
            btn = val;
    }
    var itemsofKey = document.getElementsByClassName(key);
    if (itemsofKey.length == 7)
        return;
    
    var val = "Server = server; Database = db; User id = user; Password = password";

    var ele = `<div class="custom-flex-row">
                            <input class="form-control values custom-flex-100 ${key}" id="${key}" name="${key}"  type="text" value ="${val}"/> 
                                </div>`;
    btn.insertAdjacentHTML("afterend", ele);
}


function addRow(key) {
    var btn = "";
    var eyebtn = document.getElementsByClassName("spaneye");
    for (const val of eyebtn) {
        if (val.id == key || (val.id == "no" + key))
            btn = val;
    }
    var itemsofKey = document.getElementsByClassName(key);
    if (itemsofKey.length == 7)
        return;

        val = "";
   
    var ele = `<div class="custom-flex-row">
                            <input class="form-control values custom-flex-100 ${key}" id="${key}" name="${key}"  type="text" value ="${val}"/> 
                                </div>`;
    btn.insertAdjacentHTML("afterend", ele);
}


function addMpeListRow(key, proteldbs,hotels) {

    var hotels = hotels.split("#");
    var protelsselectbox = ` <div class="resp-textbox custom-flex-100 proteldbsrow">
                                               <div class="select" id="proteldbssubdiv" style="width:400px"  >
                                                <select id="proteldbs"  onchange="selecteddb(this.options[this.selectedIndex])" >
                                              <option selected="selected" disabled value="-1" > Protel Databases </option>
`;
    
    for (const s of hotels) {
        protelsselectbox += `<option id="${key}" onselect="selecteddb(this.value,listOfHotels)" value=${s}> ${s}</option>`;
    }
   
    protelsselectbox += `</select>
    </div > 
        </div > `;
    var btn = "";
    var eyebtn = document.getElementsByClassName("spaneye");
    for (const val of eyebtn) {
        if (val.id == key || (val.id == "no" + key))
            btn = val;
    }
    var itemsofKey = document.getElementsByClassName("proteldbsrow");
    if (itemsofKey.length == 5)
        return;
    
    var val = "Server = server; Database = db; User id = user; Password = password";
    if (key === "ProtelPaths" || key === "PosPaths")
        val = "";


    var ele =     `<input class="form-control values custom-flex-100 ${key}" id="${key}" name="${key}"  type="text" value ="${val}"/> `;
    btn.insertAdjacentHTML("afterend", protelsselectbox);
}

function showPass(id) {
    if (id.includes("no") && id.includes("PosDBs"))
        id = "PosDBs";
    if (id.includes("no") && id.includes("ProtelDBs"))
        id = "ProtelDBs";
    if (id.includes("no") && id.includes("ErmisDB"))
        id = "ErmisDB";

    var x = "";
    x = document.getElementById(id);

    if (id.includes("PosDBs") || id.includes("ProtelDBs") || id.includes("ErmisDB")) {
        var elements = document.getElementsByClassName(id);
        for (const val of elements) {

            if (val.type === "password")
                val.type = "text";
            else
                val.type = "password";
        }
        return;
    }

    if (x.type === "password") {
        x.type = "text";
    } else {
        x.type = "password";
    }
}


function deleteRow(key) {
    var element = "";
    var eyebtn = document.getElementsByClassName("spaneye");
    for (const val of eyebtn) {
        if (val.id == key || (val.id == "no" + key))
            element = val;
    }
    var itemsofKey = document.getElementsByClassName(key);
    if (itemsofKey.length == 2)
        return;

    if (element.tagName == "SPAN")
        element.id = "no" + key;
    if (element.innerHTML == "+" || element.innerHTML == "-" || element.tagName == "SPAN") {
        itemsofKey[2].outerHTML = "";
        var deleteEmptyDivs = document.getElementsByClassName("custom-flex-row");

        for (const val of deleteEmptyDivs) {
            if (val.outerText == "") {
                val.outerHTML = "";
                break;
            }
        }
        return;
    }
    //element.parentNode.removeChild(element);
}


function selectedProtelDb(value) {
    
    var selectedProtelDatabase = document.getElementById('sProtelDB');
    var hotelsSelectDropdown = document.getElementById("hotelsdiv");

    if (selectedProtelDatabase != null && selectedProtelDatabase != undefined && selectedProtelDatabase.value !== "0")
        var databaseIndex = selectedProtelDatabase.selectedIndex;
    var connstr = selectedProtelDatabase[databaseIndex].value + " id=" + selectedProtelDatabase[databaseIndex].id;
    var val = selectedProtelDatabase[databaseIndex].innerText;
    if (connstr.includes("System.Collections.Generic"))
        connstr = document.getElementById(val).value
  
    var options = document.getElementsByClassName("hotels");
  
    for (let item of options) {
        console.log(item.id);
        if (item.id.includes(val)) 
            item.style.visibility = "visible";
        else
            item.style.visibility = "hidden";
    }
    hotelsSelectDropdown.style.visibility = "visible";
}
function selectedDb() {
    
    var selectedDatabase = document.getElementById('sDB');
    if (selectedDatabase != null && selectedDatabase != undefined && selectedDatabase.value !== "0")
        var databaseIndex = document.getElementById('sDB').selectedIndex;
    var connstr = selectedDatabase[databaseIndex].value;
    var connStrParts = connstr.split("#");
    var constrfinal = "Server=" + connStrParts[0] + "; Database=" + connStrParts[1] + "; User id=" + connStrParts[2]+ "; Password=" + connStrParts[3];
    var inputToBeUpdated = document.getElementById('sDB').name;
    document.getElementById(inputToBeUpdated).value = constrfinal;
}
function selectedPlugin() {
    var pluginId = null;
    var selectedPlugin = document.getElementById('selected');
    if (selectedPlugin != null && selectedPlugin != undefined && selectedPlugin.value !== "0")
        pluginId = document.getElementById('selected').value;

    if (pluginId != null) {
        var currentUrl = (window.location.protocol) + "//" + (window.location.hostname) + (window.location.port != "" ? ":" + window.location.port : "") + "/";
        var link = currentUrl + "DataGrid/Configuration?pluginId=" + pluginId;
        window.location.href = link;
    }
}

function getValuesOfPlugin() {

    var selectBox = document.getElementById("selectBox");
    selectBox.style.visibility = "hidden";
    var menu = document.getElementById("hamburgericon");
    menu.style.visibility = "hidden";
    var values = document.getElementsByClassName("values");
    var protelconfig = document.getElementsByClassName("hotelconfig");
    var arrValues = {};
    for (const val of protelconfig) {        
        if (listOfKeys.includes(val.id)) {
            var obj = {};
            var key = val.id;
            obj.key = key;
            obj[key] += val.name + "<" + val.value + "#";
            if (arrValues[key] == undefined) arrValues[key] = "";
            arrValues.key = key;
            arrValues[key] += val.name + "<" + val.value  + "#";
        }
    }
    
    for (const val of values) {
        var key = val.id;

        if (val.hasAttribute("checked")) {
            var value = val.checked;
            var obj = {};
            obj.key = key;
            obj[key] = value;
            Object.assign(arrValues, obj);
        }
        else {
            var value = val.value;
            var obj = {};

            if (listOfKeys.includes(key)) {
                arrValues.Key = key;
                if (arrValues[key] == undefined) arrValues[key] = "";
                arrValues[key] += value + "#";
            } else {
                obj.key = key;
                obj[key] = value;
                Object.assign(arrValues, obj);
            }
        }
    }
    
    SendData(arrValues);

}

function SendData(data) {
    $.ajax({
        url: "/DataGrid/SaveEditedData",
        cache: false,
        type: "POST",
        crossdomain: false,
        dataType: "json",
        ContentType: "application/json; charset=utf-8",
        data: data,
        success: function (response) {
           
        },
        error: function (error) {
            setInterval(function () {
                var selectBox = document.getElementById("selectBox");
                selectBox.style.visibility = "visible";
                var menu = document.getElementById("hamburgericon");
                menu.style.visibility = "visible";
            }, 3000);
           
        }
    });
   
};

function CheckConnectionString(data) {
    $.ajax({
        url: "/DataGrid/CheckConnections",
        cache: false,
        type: "POST",
        crossdomain: false,
        dataType: "json",
        ContentType: "application/json; charset=utf-8",
        data: data,
        success: function (response) {
            if (response === undefined || response === null || !response) {

            }
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
                    message: "Valid Database",
                    //timeout: 2000
                });
  
            }
        }
    });

};

function deleteProtelConfigRow() {
    var elements = document.getElementsByClassName("proteldbsrow");
    if (elements.length == 1)
        return;
    for (const val of elements) {
            val.outerHTML = "";
            break;
    }
}

var currentUrl = (window.location.protocol) + "//" + (window.location.hostname) + (window.location.port != "" ? ":" + window.location.port : "") + "/";
function Post(e) {
    var temp = e.oldData;
    var row = {
        Description: temp.Description,
        ActualValue: e.newData.ActualValue,
        Key: temp.Key,
        ApiVersion: temp.ApiVersion,
        Type: temp.Type,
        DefaultValue: temp.DefaultValue,
        Group: temp.Group,
        PlugInId: temp.PlugInId
    };

    $.ajax({
        url: currentUrl + "FetchDataApi/Post",
        cache: false,
        type: "POST",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        crossdomain: false,
        dataType: "json",
        ContentType: "application/json; ",
        data: JSON.stringify(row),
        success: function (response) {
            console.log("success");
            if (response !== undefined && response !== null) {

            } else {

            }
        },
        error: function (error) {
            console.log(error);
        }
    }).always(function () {

    });

}







