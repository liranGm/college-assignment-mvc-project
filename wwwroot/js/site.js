// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var BingMapKey = "AlNATsB8l5skcoutGNA8P9tN0RobQpXYatlSexjFkbmEoxV-Nm87VRhW2SqhVqDW";

var renderRequestsMap = function (divIdForMap, requestData) {
    if (requestData) {
        var bingMap = createBingMap(divIdForMap);
        addRequestPins(bingMap, requestData);
    }
}

function createBingMap(divIdForMap) {
    return new Microsoft.Maps.Map(
        document.getElementById(divIdForMap), {
            credentials: BingMapKey
        });
}

function addRequestPins(bingMap, requestData) {
    bingMap.setView(
        {
            center: new Microsoft.Maps.Location(data.lat, data.long),
            zoom: 15
        }
    );
}