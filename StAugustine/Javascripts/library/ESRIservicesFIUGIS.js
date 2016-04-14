// ESRIservicesFIUGIS.js
/*
Copyright 2012 FIU GIS-RS Center.
*/

/**
* @fileoverview This is a JavaScript library for muniplulating the REST Services of ArcGIS Server
* @Requires: Google Map APIs V3, src="https://maps.googleapis.com/maps/api/js?sensor=false"
* @Requires: function_General.js
* @author Boyuan (Keven) Guan 
* @supported Tested in IE 9 / Firefox / Chrome
*/

//######################################## Route Tasks Service ###################################################
var FIUGISServices = {};
FIUGISServices.routeTask = {};
FIUGISServices.routeTask.map;
/**
Default Network model URL, Http method, passing parameters;
*/
FIUGISServices.routeTask.Network = "http://maps.fiu.edu/ArcGIS_FIU_GISRS/rest/services/MPOBike/Route/NAServer/Route/solve";
FIUGISServices.routeTask.HttpMethod = "post";
FIUGISServices.routeTask.parameters = "stops=-80.24070739746094%2C26.160060382323596%3B%0D%0A-80.2001953125%2C26.13941218371873"
                                         + "&outSR=4326&ignoreInvalidLocations=true&accumulateAttributeNames=timeslow%2Cdist"
                                        + "&impedanceAttributeName=dist&restrictionAttributeNames=restriction%2COneway"
                                        + "&restrictUTurns=esriNFSBNoBacktrack&useHierarchy=false&returnDirections=true&returnRoutes=true"
                                        + "&returnStops=true&returnBarriers=false&returnPolylineBarriers=false&returnPolygonBarriers=false"
                                        + "&directionsLanguage=en-US&directionsStyleName=NA+Desktop&outputLines=esriNAOutputLineTrueShape"
                                        + "&findBestSequence=false&preserveFirstStop=true&preserveLastStop=true&useTimeWindows=false"
                                        + "&outputGeometryPrecisionUnits=esriFeet&directionsTimeAttributeName=timeslow"
                                        + "&directionsLengthUnits=esriNAUMiles&f=pjson";

//Call back funtion, overwite in client is needed;
FIUGISServices.routeTask.addRoute = function (results, status) {
    if (status == "success") {        
        if (results.routes) {
            var r = results.routes.features;

            //add result to google;
            var flightPlanCoordinates = new Array(r[0].geometry.paths[0].length);
            for (var i = 0, I = r[0].geometry.paths[0].length; i < I; i++) {
                var p = r[0].geometry.paths[0][i];
                flightPlanCoordinates[i] = new google.maps.LatLng(p[1], p[0]);
            }

            var flightPath = new google.maps.Polyline({
                path: flightPlanCoordinates,
                strokeColor: "#FF0000",
                strokeOpacity: 1.0,
                strokeWeight: 2
            });

            flightPath.setMap(FIUGISServices.routeTask.map);
        }
    }
};

//Request sending function.
FIUGISServices.routeTask.solve = function () {
//    DPanther.ajax.send_request(FIUGISServices.routeTask.addRoute,
//                                true, FIUGISServices.routeTask.HttpMethod,
//                                FIUGISServices.routeTask.Network,
//                                FIUGISServices.routeTask.parameters);
    DPanther.ajax.send_request_cros(FIUGISServices.routeTask.addRoute,
                                true, FIUGISServices.routeTask.HttpMethod,
                                FIUGISServices.routeTask.Network,
                                FIUGISServices.routeTask.parameters);
};
//######################################## Route Tasks Service End ###################################################