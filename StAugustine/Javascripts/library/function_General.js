// functioni_General.js
/*
Copyright 2012 FIU GIS-RS Center.
*/

/**
* @fileoverview This is a JavaScript library contain general functions
* @author Boyuan (Keven) Guan / Huang Huang
* @supported Tested in IE 9 / Firefox / Chrome
*/

var DPanther = {};
DPanther.ajax = {};

//################################################################# General Page Functions ###############################################################################
//function to initialize the dropdownlist;
//option: @type:control object; the dropdownlist;
//array: the array which stores the location inforamtion;
//firstText: the first element for the dropdownlist, normally will be "Please select..."
DPanther.InitDropDownElements = function (option, array, firstText) {
    var opt = document.createElement("option");
    opt.text = firstText;
    opt.value = 99;
    option.options.add(opt);
    for (i = 0; i < array.length; i++) {
        AddDropDownElement(option, array[i].name, array[i].address);
    }
}


DPanther.InitDropDownElements_Temp = function (option, array, firstText) {
    var opt = document.createElement("option");
    opt.text = firstText;
    opt.value = 99;
    option.options.add(opt);
    for (i = 0; i < array.length; i++) {
        AddDropDownElement(option, array[i][0], array[i][1]);
    }
}


//Function: add an element to the dropdownlist;
//option: @type:control object; the dropdownlist;
//text: @type:string; the display text of the element;
//value: @type:string; the selected value (address) of the element;         
function AddDropDownElement(option, text, value) {
    // Create an Option object
    var opt = document.createElement("option");
    //assign test and values to the Option object
    opt.text = text;
    opt.value = value;

    //add an option object to the dropdown menu
    option.options.add(opt);
}


/**
* Function: Add a SINGLE marker on EARTH
* earthObj: @type: google earth object;
* place: @type: element of arrayPlaces array;
* placeMarker: @type: element of arrayPlacemarks;
* imgLink: @type: string; the URL of the Icon; 
*/
function Add_single_marker_earth(earthObj, place, placeMarker, imgLink) {


   // placeMarker.setName(place.name);

    // Define a custom icon.and set style
    var icon = earthObj.createIcon('');
    icon.setHref(imgLink);
    var style = earthObj.createStyle('');
    style.getIconStyle().setIcon(icon);
    style.getIconStyle().setScale(1.0);    //apply style
    placeMarker.setStyleSelector(style);

    var point = earthObj.createPoint('');


    var lat = parseFloat(place.lat);
    point.setLatitude(lat);

    var lng = parseFloat(place.lng);
    point.setLongitude(lng);

    placeMarker.setGeometry(point);

    earthObj.getFeatures().appendChild(placeMarker);



}

//alternativ: place is a generaic location on earth, not an object of the arrayPlaces
function Add_single_marker_earth_alt(earthObj, place, placeMarker, imgLink) {
    // placeMarker.setName(place.name);



    // Define a custom icon.and set style
    var icon = earthObj.createIcon('');
    icon.setHref(imgLink);
    style = earthObj.createStyle('');
    style.getIconStyle().setIcon(icon);
    style.getIconStyle().setScale(1.0);    //apply style
    placeMarker.setStyleSelector(style);

    var point;

    point = earthObj.createPoint('');
    point.setLatitude(place.lat());
    point.setLongitude(place.lng());

    placeMarker.setGeometry(point);

    earthObj.getFeatures().appendChild(placeMarker);
}

//Function: Add all original sized marker on EARTH
//earthObj: @type:google earth object; 
//arrayPlace: @type array; the array which stores the location information;
//arrayPlacemark: @type array; the array which stores the place mark information;
//arrayPoint: @type array; the array which stores the point information;
function Add_marker_earth(earthObj, arrayPlace, arrayPlacemark, imgLink) {

    for (var i = 0; i < arrayPlace.length; i++) {
        Add_single_marker_earth(earthObj, arrayPlace[i], arrayPlacemark[i], imgLink);


    }

}



//Function: Add unselected  marker on EARTH 
//earthObj: @type:google earth object; 
//idx1, inx2: @type: int; the value specify the index of selected item
//arrayPlace: @type array; the array which stores the location information;
//arrayPlacemark: @type array; the array which stores the place mark information;
//arrayPoint: @type array; the array which stores the point information;
function Add_unselect_marker_earth(idx1, idx2, earthObj, arrayPlace, arrayPlacemark, imgLink) {
    for (var i = 0; i < arrayPlace.length; i++) {
        if ((i != idx1) && (i != idx2)) {
            Add_single_marker_earth(earthObj, arrayPlace[i], arrayPlacemark[i], imgLink);
        }
    }
}

/**Function:  Remove all marker on EARTH
* earthObj: @type:google earth object; 
* arrayPlace: @type array; the array which stores the location information;
* arrayPlacemark: @type array; the array which stores the place mark information;
*/
function Remove_marker_earth(earthObj, arrayPlace, arrayPlacemark) {
    for (var i = 0; i < arrayPlace.length; i++) {
        earthObj.getFeatures().removeChild(arrayPlacemark[i]); //remove a placemarker from GE
    }
}


/**Function:  Remove unselected marker from EARTH
* idx1, inx2: @type: int; the value specify the index of selected item
* earthObj: @type:google earth object; 
* arrayPlace: @type array; the array which stores the location information;
* arrayPlacemark: @type array; the array which stores the place mark information;
*/
function Remove_unselect_marker_earth(idx1, idx2, earthObj, arrayPlace, arrayPlacemark) {
    for (var i = 0; i < arrayPlace.length; i++) {
        if ((i != idx1) && (i != idx2)) {
            earthObj.getFeatures().removeChild(arrayPlacemark[i]); //remove a placemarker from GE
        }
    }
}

/**
* Function: Set single marker on map, V3
* GmapObj: @type:google map object; 
* place: @type: element of arrayPlaces;
* marker: @type: element of arrayMarkersMap;
*/
function Set_single_marker_map(GmapObj, place, marker, imgLink) {
    var LatLng = new google.maps.LatLng(place.lat, place.lng);
//    var image = new google.maps.MarkerImage(imgLink, null, null, null,
//        new google.maps.Size(24, 24));
    var image = imgLink

    marker.setPosition(LatLng);
   // marker.setTitle(place.name);
    marker.setIcon(image);
    marker.setMap(GmapObj);
}

function Change_marker_icon_map(GmapObj, marker, imgLink) {
    marker.setMap(null);
    var LatLng = marker.getPosition();
//    var image = new google.maps.MarkerImage(imgLink, null, null, null,
//        new google.maps.Size(24, 24));

    var image = imgLink
    marker.setPosition(LatLng);
    //marker.setTitle(place.name);
    marker.setIcon(image);
    marker.setMap(GmapObj);
}

//alternative
function Set_single_marker_map_alt(GmapObj, place, marker, imgLink) {
    var LatLng = new google.maps.LatLng(place.lat(), place.lng());
    var image = new google.maps.MarkerImage(imgLink, null, null, null,
        new google.maps.Size(24, 24));

   marker.setPosition(LatLng);
   marker.setTitle(place.name);
    marker.setIcon(image);
    marker.setMap(GmapObj);
}


/**
* Function: Set all markers on map, V3
* GmapObj: @type:google map object; 
* place: @type: element of arrayPlaces;
* marker: @type: element of arrayMarkersMap;
*/
function Set_marker_map(GmapObj, arrayPlace, arrayMarker, imgLink) {
    for (i = 0; i < arrayPlace.length; i++) {
        Set_single_marker_map(GmapObj, arrayPlace[i], arrayMarker[i], imgLink);
    }
}

/**Function: Add all marker on MAP
* GmapObj: @type:google map object; 
* arrayPlace: @type array; the array which stores the location information;
* arrayMark: @type array; the array which stores the place mark information;
* arrayPoint: @type array; the array which stores the point information;
*/
function Add_marker_map(GmapObj, arrayPlace, arrayMarker, imgLink) {
    for (i = 0; i < arrayPlace.length; i++) {
        Add_single_marker_map(GmapObj, arrayPlace[i], arrayMarker[i], imgLink);
    }
}

/**Function: Add un_select  marker on MAP
* idx1, inx2: @type: int; the value specify the index of selected item
* GmapObj: @type:google map object; 
* arrayPlace: @type array; the array which stores the location information;
* arrayMark: @type array; the array which stores the place mark information;
* arrayPoint: @type array; the array which stores the point information;
*/
function Add_unselect_marker_map(idx1, idx2, GmapObj, arrayPlace, arrayMarker, imgLink) {
    for (i = 0; i < arrayPlace.length; i++) {
        if ((i != idx1) && (i != idx2)) {
            Set_single_marker_map(GmapObj, arrayPlace[i], arrayMarker[i], imgLink);
        }
    }
}

/**Function: Remove unselect marker on MAP
* idx1, inx2: @type: int; the value specify the index of selected item
* GmapObj: @type:google map object; 
* rrayPlace: @type array; the array which stores the location information;
* arrayMark: @type array; the array which stores the place mark information;
* arrayPoint: @type array; the array which stores the point information;
*/
function Remove_unselect_marker_map(idx1, idx2, arrayMarker) {
    for (i = 0; i < arrayPlaces.length; i++) {
        if ((i != idx1) && (i != idx2)) {
            arrayMarker[i].setMap(null);
        }
    }
}

/**
* Function:   Change the color of the selected marker when dropdown list changes
*/
function Change_selected_marker(select) {
    //determine  from or to
    if ('from' == select) { drop = $('#from'); }
    if ('to' == select) { drop = $('#to'); }

    var i;
    //search which item in the drop list is selected
    for (var j = 0; j < drop[0].length; j++) {
        if (drop[0].options[j].selected == true) {
            i = j - 1;  //convert to the index of arrayPlaces  
            break;
        }
    }

    if ('from' == select) { Idx_From = i; }
    if ('to' == select) { Idx_To = i; }

    if (j != 0) {
        // refresh the unselected marker on earth and map
        Remove_unselect_marker_earth(Idx_From, Idx_To, my_ge, arrayPlaces, arrayPlacemarks);
        Add_unselect_marker_earth(Idx_From, Idx_To, my_ge, arrayPlaces, arrayPlacemarks, VT.landmark_red_inv);

        Remove_unselect_marker_map(Idx_From, Idx_To, arrayMarkersMap)
        Add_unselect_marker_map(Idx_From, Idx_To, my_map, arrayPlaces, arrayMarkersMap, VT.landmark_red_inv);

        // replace the selected one with different style
        my_ge.getFeatures().removeChild(arrayPlacemarks[i]); //remove select placemarker from GE
        Add_single_marker_earth(my_ge, arrayPlaces[i], arrayPlacemarks[i], VT.landmark_orange); //replace it with a color-changed marker

        arrayMarkersMap[i].setMap(null);
        Set_single_marker_map(my_map, arrayPlaces[i], arrayMarkersMap[i], VT.landmark_orange);
    }
}


/**
* Function:  draw a polygon on earth, the polygons are specified by array of points
*/
function AddPolygonEarth(my_ge, polygonMark, Points) {
    var ge = my_ge;
    var polygonPlacemark = polygonMark;
    var arrayPolyPoints = Points;

    // Create the polygon.
    var polygon = ge.createPolygon('');
    polygon.setAltitudeMode(ge.ALTITUDE_RELATIVE_TO_GROUND);
    polygonPlacemark.setGeometry(polygon);

    // Add points for the outer shape.
    var outer = ge.createLinearRing('');
    outer.setAltitudeMode(ge.ALTITUDE_RELATIVE_TO_GROUND);


    for (var i = 0; i < arrayPolyPoints.length; i++) {

        var lat = parseFloat(arrayPolyPoints[i].lat);
        var lng = parseFloat(arrayPolyPoints[i].lng);
        outer.getCoordinates().pushLatLngAlt(lat, lng, 30);
    }
    polygon.setOuterBoundary(outer);

    //Create a style and set width and color of line
    polygonPlacemark.setStyleSelector(ge.createStyle(''));
    var lineStyle = polygonPlacemark.getStyleSelector().getLineStyle();
    lineStyle.setWidth(2);
    lineStyle.getColor().set('9900ffff');
    var polyStyle = polygonPlacemark.getStyleSelector().getPolyStyle();
    polyStyle.getColor().set('9900ffff');


    // Add the placemark to Earth.
    ge.getFeatures().appendChild(polygonPlacemark);
}


function RemovePolygonEarth(earthObj, polygonMark) {

    earthObj.getFeatures().removeChild(polygonMark); //remove a placemarker from GE

}


function AddPolygonMap(map, PolygonMark, Points) {
    var coords = [];

    for (var i = 0; i < Points.length; i++) {
        coords[i] = new google.maps.LatLng(Points[i].lat, Points[i].lng);
    }

    PolygonMark.setPaths(coords);
    PolygonMark.setOptions({
        strokeColor: "#FF0000",
        strokeOpacity: 0.8,
        strokeWeight: 2,
        fillColor: "#FF0000",
        fillOpacity: 0.35
    })
    PolygonMark.setMap(map);
}


function RemovePolygonMap(PolygonMark) {

    PolygonMark.setMap(null); //remove a placemarker from map

}


function SetAudioElement() {
    audioElement.setAttribute('src', './Audios/music.mp3');

}



//**************************************************************
// waypoints list function

function InitMultiSelectElements(ID, array) {

    //var theSel = document.getElementById(ID);
    for (i = 0; i < array.length; i++) {

       // addOption(ID, array[i].name, array[i].address);
        addOption(ID, array[i].name, array[i].address);
    }
}

function InitLandmarkCheckbox(ID, array) {
    for (i = 0; i < array.length; i++) {

        AddCheckboxEntry(ID, array[i].name, i);
    }
}


function AddCheckboxEntry(ID, theText, index) {
    var target = document.getElementById(ID);
    var html = "<input type='checkbox' id= 'CHK_LM_" + i  +"' onchange='Onchange_Landmark_Check(" + index +")'/>" + theText + "<br>" ;

    target.innerHTML = target.innerHTML + html;
}


function Onchange_Landmark_Check(idx) {

    //close infobox
    VT.Close_LM_Infobox(my_map, arrayMarkersMap, arrayLM_Infobox);
    ////
    var ID = 'CHK_LM_' + idx;
    var checkBox = document.getElementById(ID);
    var ID_Drag = 'DRG_LM_' + idx;
    //change the color of the marker
    
    //bounce the marker and pan map

    //add to the draggable list
    if (checkBox.checked == true) {
        arrayLM_InRoute[idx] = 1;
           //bounce the marker and pan map
            var lat = arrayPlaces[idx].lat;
            var lng = arrayPlaces[idx].lng;
            var LatLng = new google.maps.LatLng(lat, lng);
            var marker = arrayMarkersMap[idx];
     
            var bound = my_map.getBounds();

            if (!bound.contains(LatLng))
                my_map.panTo(LatLng);

            PopMarkerMap(marker);

            // replace the selected one with different style
            my_ge.getFeatures().removeChild(arrayPlacemarks[idx]); //remove select placemarker from GE
            Add_single_marker_earth(my_ge, arrayPlaces[idx], arrayPlacemarks[idx], VT.landmark_orange); //replace it with a color-changed marker

            arrayMarkersMap[idx].setMap(null);
            Set_single_marker_map(my_map, arrayPlaces[idx], arrayMarkersMap[idx], VT.landmark_orange);

            //add element to draggable list
           //Selected Wayponts. Drag to re-order 
            $("#divWaypoints_list").append('<li class="ui-state-default" id=' + ID_Drag +'> ' + arrayPlaces[idx].name + '</li>');

            //update the prompt words
            if ($("#divWaypoints_list li").length == 1){
                $("#divWaypoints_to_instruc").html("<br>Selected Waypoints. Need at least two landmarks to start a tour.");
                ShowDiv('lbl_clearWP');
                
            }
            if ($("#divWaypoints_list li").length > 1){
                $("#divWaypoints_to_instruc").html("<br>Selected Waypoints. Drag to re-order.");
                ShowDiv('btn_go');
            }
            
    }

        else {
            arrayLM_InRoute[idx] = 0;
            //bounce the marker and pan map
            var lat = arrayPlaces[idx].lat;
            var lng = arrayPlaces[idx].lng;
            var LatLng = new google.maps.LatLng(lat, lng);
            var marker = arrayMarkersMap[idx];

            var bound = my_map.getBounds();

            if (!bound.contains(LatLng))
                my_map.panTo(LatLng);

            PopMarkerMap(marker);

            // replace the selected one with different style
            my_ge.getFeatures().removeChild(arrayPlacemarks[idx]); //remove select placemarker from GE
            Add_single_marker_earth(my_ge, arrayPlaces[idx], arrayPlacemarks[idx], VT.landmark_red_inv); //replace it with a color-changed marker

            arrayMarkersMap[idx].setMap(null);
            Set_single_marker_map(my_map, arrayPlaces[idx], arrayMarkersMap[idx], VT.landmark_red_inv);
            //remove element form draggable list
            var elem = document.getElementById(ID_Drag);
            elem.parentNode.removeChild(elem);

            //update the prompt words
            if ($("#divWaypoints_list li").length == 0) {
                $("#divWaypoints_to_instruc").html(null);
                HideDiv('lbl_clearWP');
            }
            if ($("#divWaypoints_list li").length == 1) {
                $("#divWaypoints_to_instruc").html("<br>Selected Waypoints. Need at least two landmarks to start a tour.");
                HideDiv('btn_go');
            }
            if ($("#divWaypoints_list li").length > 1) {
                $("#divWaypoints_to_instruc").html("<br>Selected Waypoints. Drag to re-order.");
            }

        }

        //change back the marker style
        for (var i = 0; i < arrayMarkersMap.length; i++) {
            
            //if added inRoute, put it back to changed one
            if (arrayLM_InRoute[i] == 1) {
                arrayMarkersMap[i].setMap(null);
                Set_single_marker_map(my_map, arrayPlaces[i], arrayMarkersMap[i], VT.landmark_orange);
            }
            //else (not inRoute), put it back to original style
            else {
                arrayMarkersMap[i].setMap(null);
                Set_single_marker_map(my_map, arrayPlaces[i], arrayMarkersMap[i], VT.landmark_red_inv);
            }

        }
        VT.Change_LM_Infobox_Contents(arrayLM_Infobox[idx], idx);
    
}

function ClearWaypoints() {
   // VT.Close_LM_Infobox(my_map, arrayMarkersMap, arrayLM_Infobox);
    
    var y = document.getElementById("divWaypoints_list");
    //var count = y.childElementCount;
    //var arrayChildrenCopy = y.children;

    while (y.childElementCount != 0) {
        var id = y.children[0].id;
        var arrayElem = id.split("_");
        var idx = arrayElem[2];
        var ID = 'CHK_LM_' + idx;
        var checkBox = document.getElementById(ID);
        var ID_Drag = 'DRG_LM_' + idx;

        // replace the selected one with different style
//        my_ge.getFeatures().removeChild(arrayPlacemarks[idx]); //remove select placemarker from GE
//        Add_single_marker_earth(my_ge, arrayPlaces[idx], arrayPlacemarks[idx], VT.landmark_darkgreen); //replace it with a color-changed marker

//        arrayMarkersMap[idx].setMap(null);
//        Set_single_marker_map(my_map, arrayPlaces[idx], arrayMarkersMap[idx], VT.landmark_darkgreen);
//        //remove element form draggable list
//        var elem = document.getElementById(ID_Drag);
//        elem.parentNode.removeChild(elem);

        //
        checkBox.checked = false;
        Onchange_Landmark_Check(idx);
    } 

    //adjust controls
            $("#divWaypoints_to_instruc").html(null);
            HideDiv('lbl_clearWP');
            HideDiv('btn_go');
    //restore map and earth
            //pan map to original center and oringinal zoom
            var LatLng = new google.maps.LatLng(VT.MAP_CENTER_LAT, VT.MAP_CENTER_LNG);
            my_map.panTo(LatLng);
            my_map.setZoom(14);

    //set view to original look at;
    var la = my_ge.createLookAt('');
    la.set(25.7453, -80.2697, 0, my_ge.ALTITUDE_RELATIVE_TO_GROUND, -8.541, 66.213, 4200);
    my_ge.getView().setAbstractView(la);
    //reset bit map
    arrayLM_InRoute = [];
}

function addOption(ID, theText, theValue) {
    var theSel = document.getElementById(ID);
    var newOpt = new Option(theText, theValue);
        newOpt.addEventListener("click", showSelectValue, false);

    var selLength = theSel.length;
    theSel.options[selLength] = newOpt;
}

function deleteOption(ID, theIndex) {
    var theSel = document.getElementById(ID);
    var selLength = theSel.length;
    if (selLength > 0) {
        theSel.options[theIndex] = null;
    }
}

/*
 * move option item formID -> toID
 */
function moveOptions(fromID, toID) {

    var theSelFrom = document.getElementById(fromID);
    var theSelTo = document.getElementById(toID);

    var selLength = theSelFrom.length;
    var selectedText = new Array();
    var selectedValues = new Array();
    var selectedCount = 0;

    var i;

    // Find the selected Options in reverse order
    // and delete them from the 'from' Select.
    for (i = selLength - 1; i >= 0; i--) {
        if (theSelFrom.options[i].selected) {
            selectedText[selectedCount] = theSelFrom.options[i].text;
            selectedValues[selectedCount] = theSelFrom.options[i].value;
            deleteOption(fromID, i);
            selectedCount++;
        }
    }

    // Add the selected text/values in reverse order.
    // This will add the Options to the 'to' Select
    // in the same order as they were in the 'from' Select.
    for (i = selectedCount - 1; i >= 0; i--) {
        addOption(toID, selectedText[i], selectedValues[i]);
    }


}


/*
* move option item fromID -> toID but do not delete form formID
*/
function moveOptionsKeep(fromID, toID) {

    

    var theSelFrom = document.getElementById(fromID);
    var theSelTo = document.getElementById(toID);

    var selLength = theSelFrom.length;
    var toLength = theSelTo.length;
    var selectedText = new Array();
    var selectedValues = new Array();
    var selectedCount = 0;


    arrayWptIdx = [];
    //add the existing landmark into the index array
    for (var j = 0; j < toLength; j++) {
        for (var i = selLength - 1; i >= 0; i--) {
            if (theSelFrom.options[i].text == theSelTo.options[j].text) {
                arrayWptIdx.push(i);
            }
        }
    }
    // Find the selected Options in reverse order
    // and not delete them from the 'from' Select.
    
    for (var i = selLength - 1; i >= 0; i--) {
        if (theSelFrom.options[i].selected) {
            var count = 0; //unmatched item count
            
            for (var j = 0; j < toLength; j++ ) {
                if (theSelFrom.options[i].text != theSelTo.options[j].text) 
                count++;
            }

            if(count == toLength){
                selectedText[selectedCount] = theSelFrom.options[i].text;
                selectedValues[selectedCount] = theSelFrom.options[i].value;

                selectedCount++;
                arrayWptIdx.push(i); //update the index: used when change markers.
                }
        }

       
    }

    //change the marker of the landmarks on the route
    for (var i = 0; i < arrayWptIdx.length; i++ ) {

        var idx = arrayWptIdx[i];

        // replace the selected one with different style
        my_ge.getFeatures().removeChild(arrayPlacemarks[idx]); //remove select placemarker from GE
        Add_single_marker_earth(my_ge, arrayPlaces[idx], arrayPlacemarks[idx], VT.landmark_orange); //replace it with a color-changed marker

        arrayMarkersMap[idx].setMap(null);
        Set_single_marker_map(my_map, arrayPlaces[idx], arrayMarkersMap[idx], VT.landmark_orange);

    }

    // Add the selected text/values in reverse order.
    // This will add the Options to the 'to' Select
    // in the same order as they were in the 'from' Select.
    for (var i = selectedCount - 1; i >= 0; i--) {
        addOption(toID, selectedText[i], selectedValues[i]);
    }

   
}


/*
* move option item fromID 
*/
function moveOptionsEmpty(fromID) {

    var theSelFrom = document.getElementById(fromID);
    

    var selLength = theSelFrom.length;
    var selectedText = new Array();
    var selectedValues = new Array();
    var selectedCount = 0;

    

    // Find the selected Options in reverse order
    // and delete them from the 'from' Select.
    for (var i = selLength - 1; i >= 0; i--) {
        if (theSelFrom.options[i].selected) {

            for (var j = 0; j < arrayPlaces.length; j++) {
                if (arrayPlaces[j].name == theSelFrom.options[i].text) {
                // selectedText[selectedCount] = theSelFrom.options[i].text;
                //  selectedValues[selectedCount] = theSelFrom.options[i].value;

                // replace the selected one with different style
                    my_ge.getFeatures().removeChild(arrayPlacemarks[j]); //remove select placemarker from GE
                    Add_single_marker_earth(my_ge, arrayPlaces[j], arrayPlacemarks[j], VT.landmark_red_inv); //replace it with a color-changed marker

                    arrayMarkersMap[j].setMap(null);
                    Set_single_marker_map(my_map, arrayPlaces[j], arrayMarkersMap[j], VT.landmark_red_inv);
                break;
               
                }
            }
        //delete option
        deleteOption(fromID, i);
        selectedCount++;
        }

     }

 }

 function moveOptionsEmpty_All(fromID) {

     var theSelFrom = document.getElementById(fromID);


     var selLength = theSelFrom.length;
     var selectedText = new Array();
     var selectedValues = new Array();
     var selectedCount = 0;

     // and delete them from the 'from' Select.
     for (var i = selLength - 1; i >= 0; i--) {
        
             //delete option
             deleteOption(fromID, i);
             selectedCount++;
         }

     

 }

function showSelectValue(e) {
    if (e.target.id != 'select') {
        document.getElementById('test').innerHTML = e.target.value;
    }
}

function AddOnclick(selectID) {
    var ele = document.getElementById(selectID);
  // ele.addEventListener('mouseover', showSelectValue, false);
   
    for (var i = 0; i < ele.length; i++){
//        ele[i].onclick = function () {
//            alert('a');
//        };
        ele[i].addEventListener("click", ClickLandMark, false);
    }
 //  ele.addEventListener("click", showSelectValue, false);
}
//***************************************************************



//###########################################################################################################################################################################

//################################################### General Page Function ###################################################################################
function clearInputBox() {
    if ($("#txtSearchField")[0].value == "Please enter the conditon here.") {
        this.$("#txtSearchField")[0].value = "";
    }
}


//######################################### Code segment for the server side access and functioinalities ######################################################
//Function: method to send request to the server
var http_request = false;

//************************************************* Same domain request by using jQuery *************************************************
//send request function for AJAX method;
//callBackFun: name of the call back function/object;
//callBackType: symcro or not/bool;
//callBackMethod: post or get / string 
//url: url for the backend function / string
//strPost:Parameter for the backend function / string
//timeOut:the tolerance time for time out;
//contentType: application/x-www-form-urlencoded, application/json...
//dataType:xml. html, script, json, jsonp, text...
DPanther.ajax.send_request = function (callBackFun, callBackType, HTTPMethod, url, strPost, timeOut, contentType, dataType, errorFun) {//initialize   
    if (timeOut == "" || timeOut == undefined) {
        timeOut = 10000;
    }
    if (contentType == "" || contentType == undefined) {
        contentType = "application/json";
    }
    if (dataType == "" || dataType == undefined) {
        dataType = "";
    }
    if (errorFun == "" || errorFun == undefined) {
        errorFun = DPanther.ajax.GetError;
    }
    $.ajax({
        url: url, //Url of the Service
        type: HTTPMethod,  //Type of ajax call
        data: strPost,  //Data to send        
        processData: callBackType,
        //crossDomain: true,
        contentType: contentType, //Type of data being sent
        timeout: timeOut,
        dataType: dataType,
        success: callBackFun,
        error: errorFun
    });


}
//***************************************************************************************************************************************************

//************************************************* Cross domain call back *************************************************
//send request function for AJAX method;
//callBackFun: name of the call back function/object;
//callBackType: symcro or not/bool;
//callBackMethod: post or get / string 
//url: url for the backend function / string
//strPost:Parameter for the backend function / string
DPanther.ajax.send_request_cros = function (callBackFun, callBackType, HTTPMethod, url, strPost, timeOut, contentType, dataType, errorFun) {//initialize 
    if (timeOut == "" || timeOut == undefined) {
        timeOut = 10000;
    }
    if (contentType == "" || contentType == undefined) {
        contentType = "application/json";
    }
    if (dataType == "" || dataType == undefined) {
        dataType = "";
    }
    if (errorFun == "" || errorFun == undefined) {
        errorFun = DPanther.ajax.GetError;
    }
    $.ajax({
        url: url,
        type: HTTPMethod,  //Type of ajax call
        data: strPost,
        contentType: contentType,
        crossDomain: true,
        dataType: dataType,
        jsonp: "callback",
        //jsonpCallback: "?",
        success: callBackFun,
        error: errorFun
    });
    //    $.getJSON(url+"?callback=?", null, function (data) {
    //        alert(data);

    //    });
}
//***************************************************************************************************************************************************

DPanther.ajax.GetError = function (request, textStatus, error) {
    alert(request);
    alert(textStatus);
    alert(error);
}

//************************************************* Old method to create a HttpXMLRequest *************************************************
//send request function for AJAX method by using HttpXMLRequest;
//callBackFun: name of the call back function/object;
//callBackType: symcro or not/bool;
//callBackMethod: post or get / string 
//url: url for the backend function / string
//strPost:Parameter for the backend function / string
DPanther.ajax.send_request_httpXML = function (callBackFun, callBackType, callBackMethod, url, strPost) {//initialize    
    //initialize the XMLHttpRequest object
    if (window.XMLHttpRequest) { //Mozilla 
        http_request = new XMLHttpRequest();
        if (http_request.overrideMimeType) {//set MiME type
            http_request.overrideMimeType("text/xml");
        }
    }
    else if (window.ActiveXObject) {
        try {
            http_request = new ActiveXObject("Msxml2.XMLHTTP");
        } catch (e) {
            try {
                http_request = new ActiveXObject("Microsoft.XMLHTTP");
            } catch (e) { }
        }
    }
    if (!http_request) {
        window.alert("can not create XMLHttpRequest for the current browser.");
        return false;
    }

    http_request.onreadystatechange = callBackFun;
    http_request.open(callBackMethod, url, callBackType);
    http_request.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    http_request.send(strPost);
}
//******************************************************************************************************************************************


//#####################################################################################################################################################


