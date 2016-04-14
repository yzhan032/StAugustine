// function_GMap.js
/*
Copyright 2012 FIU GIS-RS Center.
*/

/**
* @fileoverview This is a JavaScript library contain Google Map functions
* @Requires: Google Map APIs V3, src="https://maps.googleapis.com/maps/api/js?sensor=false"
* @Requires: function_General.js
* @author Boyuan (Keven) Guan / Huang Huang
* @supported Tested in IE 9 / Firefox / Chrome
*/

DPanther.Gmap = {};
//Function: add info windows to the Map object;
//map: @map; the map container
//mapObj: @type:map object; to add the specific infowindow;
//content: @type:string; html or text to show inside the infowindow;
//lisener_on: @type:string; the liseners to triger the infowinow:'click','drag','mouseover';
//lisener_off: @type:array; the liseners to turn off the infowindow;
DPanther.Gmap.addGmapInfowindow = function (map, mapObj, content, lisener_on, lisener_off) {
    var infoWindow = new google.maps.InfoWindow;
    var infoWinOn = function () {
        infoWindow.setContent(content);
        infoWindow.open(map, mapObj);
    };
    var infoWinOff = function () {
        infoWindow.close(map, mapObj);
    }

    for (var i = 0; i < lisener_on.length; i++) {
        google.maps.event.addListener(mapObj, lisener_on[i], infoWinOn);
    }

    for (var i = 0; i < lisener_off.length; i++) {
        google.maps.event.addListener(mapObj, lisener_off[i], infoWinOff);
    }

    google.maps.event.addListener(map, 'click', function () {
        infoWindow.close(map, mapObj);
    });

}

//add infobox
DPanther.Gmap.addGmapInfobox = function (map, mapObj, content, lisener_on, lisener_off) {

    var myOptions = {
        content: content
                , disableAutoPan: false
                , maxWidth: 0
                , pixelOffset: new google.maps.Size(-140, 0)
                , zIndex: null
                , boxStyle: {
                    background: "white"
                  , opacity: 0.9
                  , width: "280px"
                }
                , closeBoxMargin: "10px 2px 2px 2px"
                , closeBoxURL: "http://www.google.com/intl/en_us/mapfiles/close.gif"
                , infoBoxClearance: new google.maps.Size(1, 1)
                , isHidden: false
                , pane: "floatPane"
                , enableEventPropagation: false
    };

    var mybox = new InfoBox(myOptions);
    //mybox.setContent(content);

   // mybox.setOptions({ maxWidth: "280px" });
    var boxOn = function () {
        mybox.open(map, mapObj);
    };

    var boxOff = function () {
        mybox.close(map, mapObj);
    };


    for (var i = 0; i < lisener_on.length; i++) {
        google.maps.event.addListener(mapObj, lisener_on[i], boxOn);
    }

    for (var i = 0; i < lisener_off.length; i++) {
        google.maps.event.addListener(mapObj, lisener_off[i], boxOff);
    }

    google.maps.event.addListener(map, 'click', function () {
        mybox.close(map, mapObj);
    });

}


/*
 * add file marker on map
 */
DPanther.Gmap.addFileMarkerMap = function (mapObj, fileObj, imageLink) {
    var lat = parseFloat(fileObj['mainLat']);
    var lng = parseFloat(fileObj['mainLng']);
    var myLatLng = new google.maps.LatLng(lat, lng);
    var image = imageLink;


    var marker = new google.maps.Marker({
        position: myLatLng,
        map: mapObj,
        icon: image
    });


    return marker;
    // arrayFileMarker.push(marker);

}