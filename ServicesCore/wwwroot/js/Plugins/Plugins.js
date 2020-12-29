function redirectToPlugins() {
    var currentUrl = (window.location.protocol) + "//" + (window.location.hostname) + (window.location.port != "" ? ":" + window.location.port : "") + "/";
    var link = currentUrl + "Plugins/Index";
    window.location.href = link;
}