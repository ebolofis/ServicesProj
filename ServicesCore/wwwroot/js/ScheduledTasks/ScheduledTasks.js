

function ScheduledTasks() {
    var self = this;
    self.cronMin = ko.observable("*");
    self.cronMin = "*";
    self.cronHour = ko.observable("*");
    self.cronHour = "*";
    self.cronDay = ko.observable("*");
    self.cronDay = "*";
    self.cronMonth = ko.observable("*");
    self.cronMonth = "*";
    self.cronWeekday = ko.observable("*");
    self.cronWeekday = "*";
    self.cron = ko.observable("N/A");
    self.evaluateState = ko.observable("");
    self.xminute = ko.observable("");
    self.mminute = ko.observable("");
    self.xhour = ko.observable("");
    self.mhour = ko.observable("");
    self.xday = ko.observable("");
    self.mday = ko.observable("");
    self.xmonth = ko.observable("");
    self.mmonth = ko.observable("");
    self.xweekday = ko.observable("");
    self.mweekday = ko.observable("");

    self.EveryMinuteSet = function () {      IsDisabled = true;      self.cronMin = "*";     assignToCron();     self.EvaluateState();       return true;    }
    self.EvenMinuteSet = function () {       IsDisabled = true;      self.cronMin = "*/2";       assignToCron();     self.EvaluateState();       return true;    }
    self.OddMinute = function () {      IsDisabled = true;          self.cronMin = "1-59/2";        assignToCron();     self.EvaluateState();   return true;    }
    self.EveryXMinute = function () {
        IsDisabled = true; var xminute = document.getElementById("xminute").value;
        
        if ((parseInt(xminute) < 1 || parseInt(xminute) > 59) && xminute !== "")
            alert("Invalid values for minutes")
        self.xminute(xminute); self.cronMin = "*/" + xminute; assignToCron(); self.EvaluateState(); return true;
    }
    self.ManualMinuteSet = function () {
        self.cronMin = "";
        var mminute = "";
        var values = document.getElementById('manualminute');
        if (values.innerHTML == "") return;
        mminute = values.innerHTML;
        self.cronMin = "*/" + mminute;
        self.mminute(mminute);
        assignToCron();
        self.EvaluateState(); 
    }
    self.EveryHour = function () { IsDisabled = true; self.cronHour = "*"; assignToCron(); self.EvaluateState(); return true; }
    self.EvenHour = function () { IsDisabled = true; self.cronHour = "*/2"; assignToCron(); self.EvaluateState(); return true; }
    self.OddHour = function () {    IsDisabled = true;   self.cronHour = "1-23/2";      assignToCron();      self.EvaluateState();      return true;}
    self.EveryXHour = function () {
        IsDisabled = true; var xhour = document.getElementById("xhour").value;
        if ((parseInt(xhour) < 1 || parseInt(xhour) > 23) && xhour !== "")
            alert("Invalid values for hours")
        self.xhour(xhour); self.cronHour = "*/" + xhour; assignToCron(); self.EvaluateState(); return true;
    }
    self.ManualHourSet = function () {
        self.cronHour = "";
        var mhour = "";
        var values = document.getElementById('manualhour');
        if (values.innerHTML == "") return;
        mhour = values.innerHTML;
        self.cronHour = "*/" + mhour;
        self.mhour(mhour);
        assignToCron();
        self.EvaluateState(); 
    }
    self.EveryDay = function () { IsDisabled = true; self.cronDay = "*"; assignToCron(); self.EvaluateState(); return true; }
    self.EvenDay = function () { IsDisabled = true; self.cronDay = "*/2"; assignToCron(); self.EvaluateState(); return true; }
    self.OddDay = function () { IsDisabled = true; self.cronDay = "1-31/2"; assignToCron(); self.EvaluateState(); return true; }
    self.EveryXDay = function () {
        IsDisabled = true; var xday = document.getElementById("xday").value;
        if ((parseInt(xday) < 1 || parseInt(xday) > 31) && xday !== "")
            alert("Invalid values for days")
        self.xday(xday); self.cronDay = "*/" + xday; assignToCron(); self.EvaluateState(); return true;
    }
    self.ManualDaySet = function () {
        self.cronDay = "";
        var mday = "";
        var values = document.getElementById('manualday');
        if (values.innerHTML == "") return;
        mday = values.innerHTML;
        self.cronDay = "*/" + mday;
        self.mday(mday);
        assignToCron();
        self.EvaluateState(); 
    }
    self.EveryMonth = function () { IsDisabled = true; self.cronMonth = "*"; assignToCron(); self.EvaluateState(); return true; }
    self.EvenMonth = function () { IsDisabled = true; self.cronMonth = "*/2"; assignToCron(); self.EvaluateState(); return true; }
    self.OddMonth = function () { IsDisabled = true; self.cronMonth = "1-12/2"; assignToCron(); self.EvaluateState(); return true; }
    self.EveryXMonth = function () {
        IsDisabled = true; var xmonth = document.getElementById("xmonth").value;
        if ((parseInt(xmonth) < 1 || parseInt(xmonth) > 12) && xmonth !== "")
            alert("Invalid values for months")
        self.xmonth(xmonth); self.cronMonth = "*/" + xmonth; assignToCron(); self.EvaluateState(); return true;
    }
    self.ManualMonthSet = function () {
        self.cronMonth = "";
        var mmonth = "";
        var values = document.getElementById('manualmonth');
        if (values.innerHTML == "") return;
        mmonth = values.innerHTML;
        self.cronMonth = "*/" + mmonth;
        self.mmonth(mmonth);
        assignToCron();
        self.EvaluateState(); 
    }
    self.EveryWeekday = function () { IsDisabled = true; self.cronWeekday = "*"; assignToCron(); self.EvaluateState(); return true; }
    self.EvenWeekday = function () { IsDisabled = true; self.cronWeekday = "*/2"; assignToCron(); self.EvaluateState(); return true; }
    self.OddWeekday = function () { IsDisabled = true; self.cronWeekday = "0-6/2"; assignToCron(); self.EvaluateState(); return true; }
    self.EveryXWeekday = function () {
        IsDisabled = true;
        var xweekday = document.getElementById("xweekday").value;
        if ((parseInt(xweekday) < 1 ||  parseInt(xweekday) > 7) && xweekday !=="")
            alert("Invalid values for weekdays")
        self.xweekday(xweekday);
        self.cronWeekday = "*/" + xweekday;
        assignToCron();
        self.EvaluateState();
        return true;
    }
    self.ManualWeekdaySet = function () {
        self.cronWeekday = "";
        var mweekday = "";
        var values = document.getElementById('manualweekday');
        if (values.innerHTML < 1 || values.innerHTML > 12) alert("Error")
        mweekday = values.innerHTML;
        self.cronWeekday = "*/" + mweekday;
        self.mweekday(mweekday);
        assignToCron();
        self.EvaluateState(); 
    }
    self.EvaluateState = function () {
        var minuteState = "";
        var hourState = "";
        var daysState = "";
        var monthsState = "";
        var weekdayState = "";
        if (self.cronMin == "*") minuteState = "At every minute";
        if (self.cronMin == "*/2") minuteState = "At every 2nd minute";
        if (self.cronMin == "1-59/2") minuteState = "At every 2nd minute from 1 through 59";
        if (self.cronMin == "*/" + self.xminute() && self.xminute() != undefined) minuteState = "At every " + self.xminute() + " minute(s) ";
        if (self.cronMin == "*/" + self.mminute() && self.mminute() != undefined) minuteState = " At every " + self.mminute() + " minute(s) ";

        if (self.cronHour == "*") hourState = "past every hour";
        if (self.cronHour == "*/2") hourState = "past every two hours";
        if (self.cronHour == "1-23/2") hourState = "past every 2nd hour from 1 through 23";
        if (self.cronHour == "*/" + self.xhour() && self.xhour() != undefined) hourState = "past every " + self.xhour() + " hour(s) ";
        if (self.cronHour == "*/" + self.mhour() && self.mhour() != undefined) hourState = "past every " + self.mhour() + " hour(s) ";

        if (self.cronDay == "*") daysState = "on every day";
        if (self.cronDay == "*/2") daysState = "on 2nd day";
        if (self.cronDay == "1-31/2") daysState = "on every 2nd day-of-month from 1 through 31";
        if (self.cronDay == "*/" + self.xday() && self.xday() != undefined) daysState = "on every " + self.xday() + " day(s) ";
        if (self.cronDay == "*/" + self.mday() && self.mday() != undefined) daysState = "on every " + self.mday() + " day(s) ";

        if (self.cronMonth == "*") monthsState = "in every month";
        if (self.cronMonth == "*/2") monthsState = "in every 2nd month";
        if (self.cronMonth == "1-12/2") monthsState = "in every 2nd month from January through December";
        if (self.cronMonth == "*/" + self.xmonth() && self.xmonth() != undefined) monthsState = "in every " + self.xmonth() + " month(s) ";
        if (self.cronMonth == "*/" + self.mmonth() && self.mmonth() != undefined) monthsState = "in every " + self.mmonth() + " month(s) ";

        if (self.cronWeekday == "*") weekdayState = "on every weekday";
        if (self.cronWeekday == "*/2") weekdayState = "on every 2nd weekday";
        if (self.cronWeekday == "0-6/2") weekdayState = "on every 2nd day-of-week from Monday through Sunday";
        if (self.cronWeekday == "*/" + self.xweekday() && self.xweekday() != undefined) weekdayState = "on every " + self.xweekday() + " weekday(s) ";
        if (self.cronWeekday == "*/" + self.mweekday() && self.mweekday() != undefined) weekdayState = "on every " + self.mweekday() + " weekday(s) ";

        self.evaluateState( minuteState + ", " + hourState + ", " + daysState + ", " + monthsState + ", " + weekdayState);
    }
    function assignToCron() {

        self.cron(self.cronMin + " " + self.cronHour + " " + self.cronDay + " " + self.cronMonth + " " +self.cronWeekday);
    }
    self.saveScheduledTask= function() {
        var scheduledTaskObj = {};
        if (self.cron() == "N/A") self.cron( "* * * * *");
        if (self.EvaluateState() == undefined) self.EvaluateState ("Every minute, Every hour, Every day, Every month, Every weekday");
        scheduledTaskObj.stars = self.cron();
        scheduledTaskObj.starsDesc = self.evaluateState();
        SendData(scheduledTaskObj);
    }
    function SendData(data) {
        $.ajax({
            url: "/ScheduledTasks/ScheduleJob",
            cache: false,
            type: "POST",
            crossdomain: false,
            dataType: "json",
            ContentType: "application/json; charset=utf-8",
            data: (data),
            success: function (response) {
            },
            error: function (error) {
                var currentUrl = (window.location.protocol) + "//" + (window.location.hostname) + (window.location.port != "" ? ":" + window.location.port : "") + "/";
                var link = currentUrl + "Services/Index";
                window.location.href = link;
            }
        });

    };
}
function selectedService() {
    var serviceId = null;
    var serviceObj = document.getElementById("selectedService");
    if (serviceObj != null && serviceObj != undefined)
        serviceId = serviceObj.value;

    if (serviceId != null) {
        var currentUrl = (window.location.protocol) + "//" + (window.location.hostname) + (window.location.port != "" ? ":" + window.location.port : "") + "/";
        var link = currentUrl + "ScheduledTasks/Index?serviceId=" + serviceId;
        window.location.href = link;
    }
}
function radioGroupItemTemplate(itemData, _, itemElement) {
    itemElement
        .parent().addClass(itemData.toLowerCase())
        .text(itemData);
}
function radioGroup_valueChanged(e) {
    $("#list").children().remove();
    var priorities = e.component.option("items");

    $.each([{
        subject: "Choose between PPO and HMO Health Plan",
        priority: "High"
    }, {
        subject: "Non-Compete Agreements",
        priority: "Low"
    }, {
        subject: "Comment on Revenue Projections",
        priority: "Normal"
    }, {
        subject: "Sign Updated NDA",
        priority: "Urgent"
    }, {
        subject: "Submit Questions Regarding New NDA",
        priority: "High"
    }, {
        subject: "Rollout of New Website and Marketing Brochures",
        priority: "High"
    }], function (_, item) {
        if (item.priority === e.value) {
            $("#list").append($("<li/>").text(item.subject));
        }
    });
}
function enableEveryXMinute() {
     document.getElementById("xminute").disabled = false;
}
function enableEveryXHour() {
   document.getElementById("xhour").disabled = false;
}
function enableEveryXDay() {
   document.getElementById("xday").disabled = false;
}
function enableEveryXMonth() {
 document.getElementById("xmonth").disabled = false;
}
function enableEveryXWeekday() {
    document.getElementById("xweekday").disabled = false;
}









