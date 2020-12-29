function initDB(pluginId) {
    var currentUrl = (window.location.protocol) + "//" + (window.location.hostname) + (window.location.port != "" ? ":" + window.location.port : "") + "/";
    var link = currentUrl + "InitializeDB/Initialize?pluginId="+ pluginId;
    window.location.href = link;
}