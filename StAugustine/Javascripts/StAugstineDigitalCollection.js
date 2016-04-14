Ext.Loader.setConfig({
    enabled: true
});
Ext.Loader.setPath('Ext.ux', 'Resources/ux');

Ext.require(['*', 'Ext.ux.Spotlight', 'Ext.slider.*']);
VT.pageElement.loadMask = new Ext.LoadMask(Ext.getBody(), { msg: "Please wait..." });

VT.pageElement.FacetObjectList = [];
VT.pageElement.DigitalObjectList = [];
VT.pageElement.DetailItem = null;
VT.pageElement.geoObjectList = [];
VT.pageElement.MaxYear = '1';
VT.pageElement.MinYear = '1';
VT.pageElement.Pager = '1';
VT.pageElement.Total = '';
VT.pageElement.condition = '1';
VT.pageElement.FacetCondition = "";
VT.pageElement.AdvSearchCondition = "";
VT.pageElement.facedNumberBrief = 5;

VT.pageElement.opacityHistoricalMap = 0.3;
VT.pageElement.currentLayer = null;
VT.lastSelectedElementLength = 0;
VT.lastSelectedElementID = -1;
VT.geoSelection = false;
VT.arrayMarker = [];
VT.pageElement.markerLat = '';
VT.pageElement.markerLng = '';
VT.pageElement.selectedMarker = null;
VT.pageElement.KmlLayer = null;
VT.pageElement.iniSlider = false;



//get page parameters
VT.pageEvent.GetRequest = function () {   
    var url = location.search;
    var theRequest = "usach";
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            if (strs[i].split("=")[0] == 'id') {
                theRequest = strs[i].split("=")[1];
            }
            else {
                $('#txtSearchCritera').val(strs[i].split("=")[1].replace(/%22/g, "").replace(/%20/g, "  "));
                VT.pageElement.AdvSearchCondition = "~" + strs[i].split("=")[1] + "^Full Citation";
                VT.pageEvent.AdvancedSearchClick();
                VT.pageElement.condition = VT.pageElement.FacetCondition + VT.pageElement.AdvSearchCondition;

            }

        }
    }
    return theRequest;
};

//Ext.onReady(function () {


//    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ Define the page elements @@@@@@@@@@@@@@@@@@@@@@@@@@@@@
//    VT.pageElement.yearSlider = $('#testSlider').slider().on('slideStop', VT.Methods.filerbyYear)
//                             .data('slider');
//    $("#opacitySlider").slider({
//        formater: function (value) {
//            return 'Current Opacity: ' + value / 100;
//        }
//    });
//    VT.pageElement.opacitySlider = $("#opacitySlider").slider().on('slideStop', VT.Methods.opacitySet)
//                                    .data('slider');

//    $('#divOpSlider').hide();
//    $('#divAlert').hide();    

//    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ page initialize @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

//    VT.pageElement.aggCode = VT.pageEvent.GetRequest();
//    VT.Methods.doSearch_basic_collections(VT.pageElement.aggCode, VT.pageElement.condition, '1', '1', '1', '20', VT.Methods.basic_Search_Callback, VT.Methods.basic_Search_Callback_Error);
//    VT.pageEvent.Google_init_collections();
//    VT.Methods.doSearch_get_historicalMaps(VT.Methods.historicalMaps_Search_Callback, VT.Methods.basic_Search_Callback_Error);
   
//});

//@@@@@@@@@@@@@@@@ page event call back functions  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

VT.Methods.basic_Search_Callback = function (response, status) {    
    if (status == "success") {

        if ($("#collapseOne").attr('class') != "panel-collapse collapse in") {
            $("#collapseOne").collapse('show');
            $("#collapseTwo").collapse('hide');
        }
        //
        if (VT.pageElement.KmlLayer) {
            VT.pageElement.KmlLayer.setMap(null);
            VT.pageEvent.Google_init_collections();
        }
        var returnObj = response;

        //##################################################################
        //1. load faced content into faced panel 
        //2. load digital collection list into list panel        
        //3. initial map    
        if (response.FacetObjectList != null) {
            VT.pageElement.FacetObjectList = returnObj.FacetObjectList;
            VT.pageElement.DigitalObjectList = returnObj.DigitalObjectList;
            VT.pageElement.geoObjectList = returnObj.geoObjectList;
            VT.Methods.getcollectionsLocations(VT.pageElement.geoObjectList, VT.geoSelection);

            VT.pageElement.MaxYear = returnObj.MaxYear;
            VT.pageElement.MinYear = returnObj.MinYear;
            $("#startYear").html(parseInt(VT.pageElement.MinYear));
            $("#endYear").html(parseInt(VT.pageElement.MaxYear));
            if (!VT.pageElement.iniSlider) {
                $("#testSlider").slider({});
                $("#testSlider").slider('setValue', [VT.pageElement.MinYear, VT.pageElement.MaxYear]);
            }
            VT.pageElement.Pager = returnObj.Pager;
            VT.pageElement.Total = returnObj.Total;

            if (VT.geoSelection) {
                var contentInfo = VT.pageElement.Total + " items for this location.";
                VT.Methods.addGmapInfobox(VT.pageElement.selectedMarker, contentInfo);
            }
           
           
            var start = 20 * (VT.pageElement.Pager - 1) + 1;
            var end = 20 * VT.pageElement.Pager;
            if (end >= parseInt(VT.pageElement.Total)) {
                end = parseInt(VT.pageElement.Total);
            }

            var pagerText = start.toString() + "-" + end + " of " + VT.pageElement.Total;

            document.getElementById("divPagerHeader").innerHTML = pagerText;
            document.getElementById("divPagerFooter").innerHTML = pagerText;
            //change button sytle based on the current page;
            if (VT.pageElement.Total < 20) {
                $('button[class*="btn btn-primary"]').addClass('disabled');
            }
            else {
                if (start.toString() == "1") {
                    $('#btn_first1').addClass('disabled');
                    $('#btn_down1').addClass('disabled');
                    $('#btn_up1').removeClass('disabled');
                    $('#btn_last1').removeClass('disabled');
                    $('#btn_first2').addClass('disabled');
                    $('#btn_down2').addClass('disabled');
                    $('#btn_up2').removeClass('disabled');
                    $('#btn_last2').removeClass('disabled');

                }
                else {
                    if (end.toString() == VT.pageElement.Total) {
                        $('#btn_first1').removeClass('disabled');
                        $('#btn_down1').removeClass('disabled');
                        $('#btn_up1').addClass('disabled');
                        $('#btn_last1').addClass('disabled');
                        $('#btn_first2').removeClass('disabled');
                        $('#btn_down2').removeClass('disabled');
                        $('#btn_up2').addClass('disabled');
                        $('#btn_last2').addClass('disabled');

                    }
                    else {
                        $('button[class*="btn btn-primary"]').removeClass('disabled');
                    }
                }
            }
            if (VT.UI.setting.eleCollection.listEleID!="")
                VT.UI.getBriefList(VT.UI.setting.eleCollection.listEleID, VT.UI.setting.eleCollection.listType);
            if (VT.UI.setting.eleCollection.facetEleID!="")
            VT.UI.getFacetList(VT.UI.setting.eleCollection.facetEleID);
        }
            //no result 
        else {
            document.getElementById("divPagerHeader").innerHTML = "0";
            document.getElementById("divPagerFooter").innerHTML = "0";
            document.getElementById("divDOListBrif").innerHTML = "No Result";

            document.getElementById("FullCitation").innerHTML = "No Result";
            var strFacetNoResult = "<ul class='nav nav-list facetList'><li class='facetTitile'>LANGUAGE</li>" +
            "<li class='facetTitile'>PUBLISHER</li>" +
            "<li class='facetTitile'>SUBJECT: TOPIC</li>" +           
            "<li class='facetTitile'>AUTHOR / CREATOR</li></ul>";
            $('#ulFacetList').html(strFacetNoResult);
            //grey out markers;
            VT.Methods.greyMarkers();

        }
        VT.pageElement.loadMask.hide();
        //###################################################################
    }
};

VT.Methods.basic_Search_Callback_Error = function (request, textStatus, error) {
    alert("error");
    VT.pageElement.loadMask.hide();
};
VT.Methods.SingleItem_Search_Callback = function (response, status) {
    if (status == "success") {
        VT.pageElement.DetailItem = response;

        if (response != null) {
           
            //change background color;
            var contentList = $("li[class*='media']");

            for (var i = 0; i < contentList.length; i++) {
                if (contentList[i].id == "li_" + response.BibID + "_" + response.VID) {
                    contentList[i].className = "media activeDiv";
                }
                else {
                    contentList[i].className = "media";
                }
            }
            $("#collapseOne").collapse('hide');
            $("#collapseTwo").collapse('show');

            for (var detailProperty in VT.UI.setting.eleCollection.detailEleSetting) {
                if(VT.UI.setting.eleCollection.detailEleSetting[detailProperty] !="")
                    VT.UI.getDetailItem(VT.UI.setting.eleCollection.detailEleSetting[detailProperty], detailProperty);
            }

            //VT.UI.getDetailItem("FullCitation", "fullCitation");
            //VT.UI.getDetailItem("tabofContent", "fileList");
            var tempfilelists = VT.Methods.GetDownloadLink(response);
            $('#uldownloadTree a').each(function (index, value) {
                $(this).click(function () {

                    VT.pageEvent.SpotlightDownloadWindow(tempfilelists[index]);
                });
            });
           
            ////set markers
            for (var i = 0; i < VT.arrayMarker.length; i++) {
                if (VT.arrayMarker[i].lat != response.mainLat || VT.arrayMarker[i].lng != response.mainLng) {
                    VT.arrayMarker[i].setIcon('Images/Loc_Icon-Off_s1.png');
                }

            }
        }
    }
};

//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ google map section  @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

VT.pageEvent.Google_init_collections = function () {
    //Start set up google maps;

    var mapOptions = {
        center: new google.maps.LatLng(VT.MAP_CENTER_LAT, VT.MAP_CENTER_LNG),
        zoom: 17,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        mapTypeControlOptions: {
            position: google.maps.ControlPosition.TOP_LEFT
        },

    };
    VT.my_map = new google.maps.Map(document.getElementById("map"), mapOptions);
    


   /* //set up dynamic map layers;
    VT.dynamap = new gmaps.ags.MapOverlay(VT.HistoricalMapService, {
        opacity: 0.8
    });
    //VT.dynamap.setMap(VT.my_map);
    google.maps.event.addListenerOnce(VT.dynamap.getMapService(), 'load', function () {

        VT.dynamap.setMap(VT.my_map);

        VT.service = VT.dynamap.getMapService();
        for (var i = 0; i < VT.service.layers.length; i++) {
            VT.service.layers[i].visible = false;
        }
    });
    */
    //set map min zoom
    google.maps.event.addListener(VT.my_map, 'zoom_changed', function () {
        var zoomLevel = VT.my_map.getZoom();
        //if (zoomLevel < VT.ZOOM_MIN) {
        //    VT.my_map.setZoom(VT.ZOOM_MIN);
        //}
    });

    var homeControlDiv = document.createElement('div');
    var homeControl = new VT.Methods.resetMap(homeControlDiv);

    homeControlDiv.index = 1;
    VT.my_map.controls[google.maps.ControlPosition.TOP_LEFT].push(homeControlDiv);

};

VT.Methods.addGmapInfobox = function (marker, content) {
    if (VT.mybox)
        VT.mybox.close(VT.my_map, marker);
    var myOptions = {
        content: content
                , disableAutoPan: false
                , maxWidth: 0
                , pixelOffset: new google.maps.Size(-150, 0)
                , zIndex: null
                , boxStyle: {
                    background: "white"
                  , opacity: 0.9
                  , padding: "4px"
                }
                , closeBoxMargin: "2px 2px 2px 2px"
                , closeBoxURL: "Images/close.gif"
                , infoBoxClearance: new google.maps.Size(1, 1)
                , isHidden: false
                , pane: "floatPane"
                , enableEventPropagation: false
    };

    VT.mybox = new InfoBox(myOptions);
    VT.mybox.open(VT.my_map, marker);
    google.maps.event.addListener(VT.my_map, 'click', function () {
        VT.mybox.close(VT.my_map, marker);

    });

};

VT.Methods.greyMarkers = function () {
    for (var i = 0; i < VT.arrayMarker.length; i++) {

        VT.arrayMarker[i].setIcon('Images/Loc_Icon-Off_s1.png');
    }
};

VT.Methods.checkMarker = function (latlng) {
    var marker = new google.maps.Marker({
        icon: 'Images/Loc_01_s1.png',
        map: VT.my_map,
        position: latlng
    });
    marker.exist = false;
    for (var i = 0; i < VT.arrayMarker.length; i++) {
        if (VT.arrayMarker[i].getPosition().lat() == latlng.lat() && VT.arrayMarker[i].getPosition().lng() == latlng.lng()) {
            VT.arrayMarker[i].exist = true;
            marker.setMap(null);
            marker = VT.arrayMarker[i];
            marker.setIcon('Images/Loc_01_s1.png');
            break;
        }

    }

    return marker;
};
//refresh button on the map
VT.Methods.resetMap = function (controlDiv) {

    controlDiv.style.padding = '5px';
    // Set CSS for the control border
    var controlUI = document.createElement('div');
    controlUI.style.backgroundColor = 'white';
    controlUI.style.borderStyle = 'solid';
    controlUI.style.borderWidth = '1px';
    controlUI.style.cursor = 'pointer';
    controlUI.style.textAlign = 'center';
    controlUI.title = 'Click to reset the map';
    controlDiv.appendChild(controlUI);
    // Set CSS for the control interior
    var controlText = document.createElement('div');
    controlText.style.fontFamily = 'Arial,sans-serif';
    controlText.style.fontSize = '12px';
    controlText.style.paddingLeft = '4px';
    controlText.style.paddingRight = '4px';
    controlText.innerHTML = '<span class="glyphicon glyphicon-refresh"></span> Reset Map';
    controlUI.appendChild(controlText);

    google.maps.event.addDomListener(controlUI, 'click', function () {
        VT.my_map.setZoom(17);
        VT.my_map.setCenter(new google.maps.LatLng(VT.MAP_CENTER_LAT, VT.MAP_CENTER_LNG));
        //remove all historical map layers
        var i, o = VT.my_map.overlayMapTypes;      
        for (i = o.getLength() - 1; i >= 0; i--) {
            o.removeAt(i);          
           
        }
        
        $('#historicalMap').find('button').each(function (i) {
            var layer_id = "layer_" + i;
            var img_id = "layerimg_" + i;
            if (document.getElementById(layer_id).value == "On") {
                document.getElementById(layer_id).value = "Off";
                $('#' + img_id).attr('src', 'Images/OFF_s1.jpg');
            }
               // $('#' + img_id).attr('src', 'Images/OFF_s1.jpg');
            });
        VT.lastSelectedElementLength = 0;
      
    });

};


VT.Methods.removeOverlayMapType=function (m, layerid) {
    var i,
		o = VT.my_map.overlayMapTypes;
    var layername = "layer_" + layerid;
    for (i = o.getLength() - 1; i >= 0; i--) {
        if (o.getAt(i).name == layername) {
            o.removeAt(i);
        }
    }
}

//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ Historical Map section @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
//update 10/702015 by Meng, only allow the user to open one historical map at a time
//when a historical map is selected, display the related item detail info on the left panel

VT.Methods.mapLayerAdd = function (layerid, tableid,bibid,vid) {
    var layer_id = "layer_" + layerid;
    var img_id = "layerimg_" + layerid;

    if (document.getElementById(layer_id).value == "Off") {
        document.getElementById(layer_id).value = "On";
        $('#' + img_id).attr('src', 'Images/ON_s1.jpg');
        VT.Methods.historicalMap(layerid, VT.pageElement.opacityHistoricalMap);
        //remove other historal map layer
        if (VT.lastSelectedElementID != -1)
        {
            document.getElementById("layer_" + VT.lastSelectedElementID).value = "Off";
            $('#' + "layerimg_" + VT.lastSelectedElementID).attr('src', 'Images/OFF_s1.jpg');
            VT.Methods.removeOverlayMapType(VT.my_map, VT.lastSelectedElementID);
        }

        //VT.lastSelectedElementLength++;
        VT.lastSelectedElementID = layerid;
        //display the related item detail info on the left panel
        if (bibid != "") {
            VT.Methods.doSearch_single_Item(bibid, vid, VT.Methods.SingleItem_Search_Callback, VT.Methods.basic_Search_Callback_Error);
        }
        else {
            VT.Methods.doSearch_basic_collections(VT.pageElement.aggCode, VT.pageElement.condition, '1', '1', '1', '20', VT.Methods.basic_Search_Callback, VT.Methods.basic_Search_Callback_Error);
        }
        $('#divOpSlider').show();

    }
    else if (document.getElementById(layer_id).value == "On") {
        document.getElementById(layer_id).value = "Off";
        $('#' + img_id).attr('src', 'Images/OFF_s1.jpg');
        VT.lastSelectedElementID =-1;
        //VT.lastSelectedElementLength--
        
        VT.Methods.removeOverlayMapType(VT.my_map, layerid);
        VT.Methods.doSearch_basic_collections(VT.pageElement.aggCode, VT.pageElement.condition, '1', '1', '1', '20', VT.Methods.basic_Search_Callback, VT.Methods.basic_Search_Callback_Error);

        //if (VT.lastSelectedElementLength == 0)
        $('#divOpSlider').hide(); 
    }
   
};

VT.Methods.historicalMap = function (layer, opacity) {
  
    var layer_id = "layer_" + layer;
   
    var agsType = new gmaps.ags.MapType(VT.HistoricalMapLayers[layer], {
        opacity: 0.8,
        name: layer_id
    });

    VT.my_map.overlayMapTypes.insertAt(0, agsType);
       
    agsType.setOpacity(opacity);
       
  
};

VT.Methods.historicalMapOpacityChange = function (opacity) {
    var i,
      o = VT.my_map.overlayMapTypes;  
    for (i = o.getLength() - 1; i >= 0; i--) {
        o.getAt(i).setOpacity(opacity);
    }
};


VT.Methods.initializePatientDragZone = function (v) {
    v.dragZone = Ext.create('Ext.dd.DragZone', v.getEl(), {

        getDragData: function (e) {
            var sourceEl = e.getTarget(v.itemSelector, 10), d;
            if (sourceEl) {
                d = sourceEl.cloneNode(true);
                d.id = Ext.id();
                return v.dragData = {
                    sourceEl: sourceEl,
                    repairXY: Ext.fly(sourceEl).getXY(),
                    ddel: d,
                    patientData: v.getRecord(sourceEl).data
                };
            }
        },

        getRepairXY: function () {
            return this.dragData.repairXY;
        }
    });
};

//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ page event section @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

VT.pageEvent.Facet_Item_Click = function (term, attribute) {
    VT.pageEvent.clearGeoSelectiong();
    var eleUl = $('#ulsearchCritera li');
    if (eleUl.length == 0) {
        $("#lisearchTitle").remove();
        $('#ulsearchTitle').append('<li class="active" id="lisearchTitle">Search results are based on the following criteria:</li>');
    }
    if (eleUl.length == 5) {
        alert('You can only add 5 search critera!');
        return;
    }
   
    var eleUlDContain = $('#ulsearchCritera li:contains("' + attribute + ":" + term + '")');
    if (eleUlDContain.length > 0) {
        alert("This criteria is already added.")
        return;
    }

    var strsearchCritera = attribute + ":" + term.replace(/\:/g, " ").replace(/\*/g, "'");
    $('#ulsearchCritera').append('<li class="alert alert-warning facetSearchCriteria">' + strsearchCritera + '<a class="close" data-dismiss="alert" href="#" aria-hidden="true">&times;</a></li>');

    VT.pageElement.FacetCondition += "~" + term + "^" + attribute;

    VT.pageElement.FacetCondition = VT.pageElement.FacetCondition.trim();
    VT.pageElement.FacetCondition = VT.pageElement.FacetCondition.replace(/\//g, "$");
    VT.pageElement.FacetCondition = VT.pageElement.FacetCondition.replace(/\:/g, " ");
    VT.pageElement.FacetCondition = VT.pageElement.FacetCondition.replace(/\-/g, " ");
    VT.pageElement.FacetCondition = VT.pageElement.FacetCondition.replace(/\&/g, "@");
    VT.pageElement.FacetCondition = VT.pageElement.FacetCondition.replace(/\,/g, " ");
    VT.pageElement.FacetCondition = VT.pageElement.FacetCondition.replace(/\./g, " ");
    VT.pageElement.FacetCondition = VT.pageElement.FacetCondition.replace(/\?/g, "|");
    VT.pageElement.FacetCondition = VT.pageElement.FacetCondition.replace(/\*/g, "'");
    VT.pageElement.condition = VT.pageElement.FacetCondition + VT.pageElement.AdvSearchCondition;

    VT.Methods.doSearch_basic_collections(VT.pageElement.aggCode, VT.pageElement.condition, '1', '1', '1', '20', VT.Methods.basic_Search_Callback, VT.Methods.basic_Search_Callback_Error);


    $('.facetSearchCriteria').bind('closed.bs.alert', function () {
        var eleUl = $('#ulsearchCritera li');
        if (eleUl.length == 1) {
            $("#lisearchTitle").remove();
            $('#ulsearchTitle').append('<li class="active" id="lisearchTitle">FULL DATASET HAS BEEN RETURNED:</li>');
            $("#txtSearchCritera").val('');

            //reset page criteria;
            VT.pageElement.FacetCondition = "";
            VT.pageElement.condition = "1";

            VT.Methods.doSearch_basic_collections(VT.pageElement.aggCode, VT.pageElement.condition, '1', '1', '1', '20', VT.Methods.basic_Search_Callback, VT.Methods.basic_Search_Callback_Error);
        }
        else {
            var searchCriteraArray = $(this).text();
            var removeCritera = VT.Methods.SearchCriteraStringClose(searchCriteraArray.split(':')[0], searchCriteraArray.split(':')[1]);

            VT.pageElement.FacetCondition = VT.pageElement.FacetCondition.replace(removeCritera, '');
            VT.pageElement.condition = VT.pageElement.FacetCondition + VT.pageElement.AdvSearchCondition;
            //VT.pageElement.condition = VT.pageElement.condition.replace(removeCritera, '');
            if (VT.pageElement.condition == "")
            { VT.pageElement.condition = "1"; }

            VT.Methods.doSearch_basic_collections(VT.pageElement.aggCode, VT.pageElement.condition, '1', '1', '1', '20', VT.Methods.basic_Search_Callback, VT.Methods.basic_Search_Callback_Error);
        }

    });

};

VT.pageEvent.AdvancedItemSelect = function (attribute, term) {

    var eleUl = $('#ulsearchCritera li');

    if (eleUl.length == 0) {
        $("#lisearchTitle").remove();
        $('#ulsearchTitle').append('<li class="active" id="lisearchTitle">Search results are based on the following criteria:</li>');
    }

    if (eleUl.length == 5) {
        alert('You can only add 5 search critera!');
        return;
    }
    if ($("#liAdvSearch").length > 0) {
        $("#liAdvSearch").remove();
    }


    var strsearchCritera = attribute + ":" + $("#txtSearchCritera").val();

    $('#ulsearchCritera').prepend('<li class="alert alert-success advSearchType" id="liAdvSearch">' + strsearchCritera + '<a class="close" data-dismiss="alert" href="#" aria-hidden="true">&times;</a></li>');

};

VT.pageEvent.AdvancedSearchKeyup = function () {
    var strsearchCritera = "Full Citation" + ":";
    var eleUl = $('#ulsearchCritera li');
    if (eleUl.length == 0) {
        $("#lisearchTitle").remove();
        $('#ulsearchTitle').append('<li class="active" id="lisearchTitle">Search results are based on the following criteria:</li>');
    }
    if ($("#liAdvSearch").length > 0) {

        strsearchCritera = $("#liAdvSearch").text().split(":")[0] + ":" + $("#txtSearchCritera").val();
        $("#liAdvSearch").remove();


    }
    else {
        strsearchCritera += $("#txtSearchCritera").val();
    }
    $('#ulsearchCritera').prepend('<li class="alert alert-success advKeyUp" id="liAdvSearch">' + strsearchCritera
        + '<a class="close" data-dismiss="alert" href="#" aria-hidden="true">&times;</a></li>');

    $('.advKeyUp').bind('closed.bs.alert', function () {
        $("#txtSearchCritera").val('');
    });

};

VT.pageEvent.AdvancedSearchClick = function () {
    VT.pageEvent.clearGeoSelectiong();
    var txtSearchCriteraFormat = $("#txtSearchCritera").val();
    var strsearchCritera = "Full Citation" + ":";
    var eleUl = $('#ulsearchCritera li');
    //if it is blank search all
    if (!$("#txtSearchCritera").val()) {        
        VT.pageElement.AdvSearchCondition = "";
    }
    else {
        //change the search title
        if (eleUl.length == 0) {
            $("#lisearchTitle").remove();
            $('#ulsearchTitle').append('<li class="active" id="lisearchTitle">Search results are based on the following criteria:</li>');
        }
        //if the search criteria is already exsit
        if ($("#liAdvSearch").length > 0) {
            strsearchCritera = $("#liAdvSearch").text().split(":")[0] + ":" + $("#txtSearchCritera").val();
            $("#liAdvSearch").remove();
        }
        //search in full cititaion
        else {
            strsearchCritera += txtSearchCriteraFormat;
        }       
        VT.pageElement.AdvSearchCondition = VT.Methods.SearchCriteraStringClose(strsearchCritera.split(':')[0], strsearchCritera.split(':')[1]);
        VT.pageElement.AdvSearchCondition = VT.pageElement.AdvSearchCondition.trim();
        VT.pageElement.AdvSearchCondition = VT.pageElement.AdvSearchCondition.replace(/\//g, "$");
        VT.pageElement.AdvSearchCondition = VT.pageElement.AdvSearchCondition.replace(/\:/g, " ");
        VT.pageElement.AdvSearchCondition = VT.pageElement.AdvSearchCondition.replace(/\-/g, " ");
        VT.pageElement.AdvSearchCondition = VT.pageElement.AdvSearchCondition.replace(/\&/g, "@");

        $('#ulsearchCritera').prepend('<li class="alert alert-success advSearchCriteria" id="liAdvSearch">' + strsearchCritera + '<a class="close" data-dismiss="alert" href="#" aria-hidden="true">&times;</a></li>');

    }
    VT.pageElement.condition = VT.pageElement.FacetCondition + VT.pageElement.AdvSearchCondition;

    VT.pageElement.condition = VT.pageElement.condition.trim();
    VT.pageElement.condition = VT.pageElement.condition.replace(/\//g, "$");
    if (VT.pageElement.condition == '')
    { VT.pageElement.condition = '1'; }
    VT.Methods.doSearch_basic_collections(VT.pageElement.aggCode, VT.pageElement.condition, '1', '1', '1', '20',VT.Methods.basic_Search_Callback, VT.Methods.basic_Search_Callback_Error);

    $('.advSearchCriteria').bind('closed.bs.alert', function () {
        var eleUl = $('#ulsearchCritera li');
        $("#txtSearchCritera").val('');
        if (eleUl.length == 1) {
            $("#lisearchTitle").remove();
            $('#ulsearchTitle').append('<li class="active" id="lisearchTitle">FULL DATASET HAS BEEN RETURNED:</li>');

        }
        var searchCriteraArray = $(this).text();
        var removeCritera = VT.Methods.SearchCriteraStringClose(searchCriteraArray.split(':')[0], searchCriteraArray.split(':')[1]);

        VT.pageElement.condition = VT.pageElement.FacetCondition;

        if (VT.pageElement.condition == "")
        { VT.pageElement.condition = "1"; }

        VT.Methods.doSearch_basic_collections(VT.pageElement.aggCode, VT.pageElement.condition, '1', '1', '1', '20', VT.Methods.basic_Search_Callback, VT.Methods.basic_Search_Callback_Error);

    });
    //    }
};

VT.pageEvent.BacktoListClick = function () {
    $("#collapseOne").collapse('show');
    $("#collapseTwo").collapse('hide');
    if (VT.pageElement.KmlLayer) {

        VT.pageElement.KmlLayer.setMap(null);
        VT.pageElement.KmlLayer = null;
        VT.my_map.setZoom(14);
        VT.my_map.setCenter(new google.maps.LatLng(VT.MAP_CENTER_LAT, VT.MAP_CENTER_LNG));
    }
    //reset marker
    for (var i = 0; i < VT.arrayMarker.length; i++) {

        VT.arrayMarker[i].setIcon('Images/Loc_01_s1.png');

    }
};

VT.Methods.Pagedown = function (down) {
    var maxPage = Math.floor(VT.pageElement.Total / 20);
    var rem = VT.pageElement.Total / 20;
    var pager = parseInt(VT.pageElement.Pager);
    if (rem != 0) {
        maxPage = maxPage + 1;
    }
    if (!VT.geoSelection) {
        if (down == "up") {
            if (VT.pageElement.Pager < maxPage)
            { VT.Methods.doSearch_basic_collections(VT.pageElement.aggCode, VT.pageElement.condition, '1', '1', (pager + 1).toString(), '20', VT.Methods.basic_Search_Callback, VT.Methods.basic_Search_Callback_Error); }
        }
        else {
            if (VT.pageElement.Pager > 1)
            { VT.Methods.doSearch_basic_collections(VT.pageElement.aggCode, VT.pageElement.condition, '1', '1', (pager - 1).toString(), '20', VT.Methods.basic_Search_Callback, VT.Methods.basic_Search_Callback_Error); }
        }
    }
    else {
        if (down == "up") {
            VT.Methods.doSearch_byLocation_collections(VT.pageElement.aggCode, VT.pageElement.markerLat, VT.pageElement.markerLng, (pager + 1).toString(), "20", VT.Methods.basic_Search_Callback, VT.Methods.basic_Search_Callback_Error);

        }
        else {
            VT.Methods.doSearch_byLocation_collections(VT.pageElement.aggCode, VT.pageElement.markerLat, VT.pageElement.markerLng, (pager - 1).toString(), "20", VT.Methods.basic_Search_Callback, VT.Methods.basic_Search_Callback_Error);
        }
    }

};

VT.Methods.PageFirstLast = function (first) {
    var maxPage = Math.floor(VT.pageElement.Total / 20);
    var rem = VT.pageElement.Total % 20;

    if (rem != 0) {
        maxPage = maxPage + 1;
    }
    if (!VT.geoSelection) {
        if (first == "first") {
            VT.Methods.doSearch_basic_collections(VT.pageElement.aggCode, VT.pageElement.condition, '1', '1', '1', '20', VT.Methods.basic_Search_Callback, VT.Methods.basic_Search_Callback_Error);
        }
        else {
            VT.Methods.doSearch_basic_collections(VT.pageElement.aggCode, VT.pageElement.condition, '1', '1', maxPage, '20', VT.Methods.basic_Search_Callback, VT.Methods.basic_Search_Callback_Error);
        }
    }
    else {
        if (first == "first") {
            VT.Methods.doSearch_byLocation_collections(VT.pageElement.aggCode, VT.pageElement.markerLat, VT.pageElement.markerLng, "1", "20", VT.Methods.basic_Search_Callback, VT.Methods.basic_Search_Callback_Error);

        }
        else {
            VT.Methods.doSearch_byLocation_collections(VT.pageElement.aggCode, VT.pageElement.markerLat, VT.pageElement.markerLng, maxPage, "20", VT.Methods.basic_Search_Callback, VT.Methods.basic_Search_Callback_Error);
        }
    }

};

VT.Methods.filerbyYear = function () {
    VT.pageElement.iniSlider = true;
    var YearStart = VT.pageElement.yearSlider.getValue()[0];
    var YearEnd = VT.pageElement.yearSlider.getValue()[1];
    //include records without published date info
    if (YearStart == VT.pageElement.yearSlider.min)
    { YearStart = '1'; }
    if (YearEnd == VT.pageElement.yearSlider.max)
    { YearEnd = '1'; }
    VT.pageEvent.clearGeoSelectiong();
    VT.Methods.doSearch_basic_collections(VT.pageElement.aggCode, VT.pageElement.condition, YearStart, YearEnd, '1', '20', VT.Methods.basic_Search_Callback, VT.Methods.basic_Search_Callback_Error);
};

VT.Methods.opacitySet = function () {
    var newVal = VT.pageElement.opacitySlider.getValue();
    var opacity = newVal / 100;
    //VT.Methods.historicalMap(99, opacity);
    VT.Methods.historicalMapOpacityChange(opacity);
    VT.pageElement.opacityHistoricalMap = opacity;
};

VT.pageEvent.Facet_Expand = function (iIdex) {
    var eleUl = $('#faced_' + iIdex + ' li[class="eleHide"]');
    for (var i = 0; i < eleUl.length; i++) {
        eleUl[i].className = "eleShow";
    }

    $('#faced_' + iIdex + ' li[class="more"]')[0].innerHTML = "<span><a href='#' onClick='VT.pageEvent.Facet_Fold(" + iIdex + ")'>SHOW LESS...</a></span>"
};

VT.pageEvent.Facet_Fold = function (iIdex) {
    var eleUl = $('#faced_' + iIdex + ' li[class="eleShow"]');
    for (var i = 0; i < eleUl.length; i++) {
        eleUl[i].className = "eleHide";
    }

    $('#faced_' + iIdex + ' li[class="more"]')[0].innerHTML = "<span><a href='#' onClick='VT.pageEvent.Facet_Expand(" + iIdex + ")'>SHOW MORE...</a></span>"
};

VT.pageElement.dropZone = new Ext.dd.DropTarget("map", {
    notifyDrop: function (dd, e, data) {
        VT.Methods.historicalMap(data.patientData.id, VT.pageElement.opacityHistoricalMap);

        VT.lastSelectedElementLength++;
        var layer_id = "layer_" + (data.sourceEl.viewIndex);
        var tableid = "tblayer_" + (data.sourceEl.viewIndex);
        var img_id = "layerimg_" + (data.sourceEl.viewIndex);
        document.getElementById(layer_id).value = "On";
        $('#' + img_id).attr('src', 'Images/ON_s1.jpg');
        //$("#" + tableid).css("background-color", "#E6E6E6");
        //VT.pageElement.optSliderContainer.show();
        $('#divOpSlider').show();
        return true;
    }
});

VT.Methods.SearchCriteraStringClose = function (attribute, term) {
    term = term.replace('×', '');
    term = term.replace(/\//g, "$");
    term = term.replace(/\:/g, " ");
    term = term.replace(/\-/g, " ");
    term = term.replace(/\&/g, "@");
    term = term.replace(/\,/g, " ");
    //term = term.replace(/\./g, " ");
    term = term.replace(/\?/g, "|");
    term = term.replace(/\*/g, "'");

    attribute = attribute.replace(/\//g, "$");
    attribute = "~" + term + "^" + attribute;
    return attribute;

};

$('#txtSearchCritera').keypress(function (e) {

    var code = e.which; // recommended to use e.which, it's normalized across browsers
    if (code == 13)
        e.preventDefault();
    if (code == 186 || code == 187 || code == 188 || code == 189 || code == 191 || code == 192 || code == 220 || code == 222) {
        var backspace = $('#txtSearchCritera').val();
        $('#txtSearchCritera').val(backspace.substring(0, backspace.length - 1));
        alert("Invalid Character.");
        return;
    }
    if (code == 13) {
        VT.pageEvent.AdvancedSearchClick();
    }
    else {
        VT.pageEvent.AdvancedSearchKeyup();
    }
});

//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ function and method section @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

VT.Methods.getcollectionsLocations = function (response, selected) {
    var alertMessage = "";

    if (!selected) {
        var returnObj = response;

        //grey out existing markers;
        VT.Methods.greyMarkers();

        $('#divAlert').hide();

        for (var i = 0; i < returnObj.length; i++) {
            var lat = returnObj[i].Lat;
            var lng = returnObj[i].Log;
            var title = returnObj[i].Address;
            if (lat == "" || lng == "") {
                alertMessage = "One or more selected collections do not have geo-reference information."
            }
            else {
                var latlng = new google.maps.LatLng(lat, lng);

                //check if the marker already display on the map
                var marker = VT.Methods.checkMarker(latlng);

                if (!marker.exist) {

                    marker.title = title;
                    marker.lat = returnObj[i].Lat;
                    marker.lng = returnObj[i].Log;
                    marker.exist = true;

                    VT.arrayMarker.push(marker);
                    google.maps.event.addListener(marker, 'click', function () {
                        $('#divAlert').hide();
                        $('#facetListDis').hide();
                        $("#txtSearchCritera").val('');
                        $("#collectionListDis").addClass('markerActive');
                        for (var i = 0; i < VT.arrayMarker.length; i++) {
                            if (VT.arrayMarker[i].getIcon() == "Images/Loc_01_s1.png") {
                                VT.arrayMarker[i].setIcon('Images/Loc_Icon-Off_s1.png');
                                //break;
                            }
                        }

                        VT.pageElement.selectedMarker = this;
                        VT.pageElement.markerLat = VT.pageElement.selectedMarker.lat;
                        VT.pageElement.markerLng = VT.pageElement.selectedMarker.lng;
                        VT.Methods.doSearch_byLocation_collections(VT.pageElement.aggCode, VT.pageElement.markerLat, VT.pageElement.markerLng, "1", "20", VT.Methods.basic_Search_Callback, VT.Methods.basic_Search_Callback_Error);
                        VT.geoSelection = true;
                        VT.pageElement.selectedMarker.setIcon("Images/Loc_01_s1.png");
                        //clear the existing critea
                        VT.pageElement.condition = '1';
                        VT.pageElement.FacetCondition = "";
                        VT.pageElement.AdvSearchCondition = "";
                        $('#ulsearchCritera li').remove();
                        $("#lisearchTitle").remove();
                        $('#ulsearchTitle').append('<li class="alert alert-info geoSearchCriteria" id="lisearchTitle">Search Result Based on the Selected Location.<a class="close" data-dismiss="alert" href="#" aria-hidden="true">&times;</a></li>');
                        $('.geoSearchCriteria').bind('closed.bs.alert', function () {

                            VT.pageEvent.clearGeoSelectiong();
                            if (VT.pageElement.condition == "")
                            { VT.pageElement.condition = "1"; }

                            VT.Methods.doSearch_basic_collections(VT.pageElement.aggCode, VT.pageElement.condition, '1', '1', '1', '20', VT.Methods.basic_Search_Callback, VT.Methods.basic_Search_Callback_Error);

                        });

                    });

                }

            }
        }

        if (alertMessage != "") {
            $('#divAlert').show();
        }
        VT.pageElement.loadMask.hide();
    }
    //###################################################################

};

VT.UI.getFacetList = function (eleID) {
    var arrayObj = VT.pageElement.FacetObjectList;
    if (arrayObj != null) {
        var len_Obj = arrayObj.length;

        var arrayAttribute = ["LANGUAGE-Language", "PUBLISHER-Publisher", "SUBJECT: TOPIC-SubjectTopic", "AUTHOR / CREATOR-Author"];
        var len_Attribute = arrayAttribute.length;

        //start ul
        var ulHtml = "";

        //attribute li
        for (var i = 0; i < len_Attribute; i++) {

            ulHtml += "<ul id='faced_" + i + "' class='nav nav-list facetList'><li class='facetTitile'>";
            ulHtml = ulHtml + arrayAttribute[i].split('-')[0]
                + "</li>";

            var strAttribute = arrayAttribute[i].split('-')[1];


            for (var j = 0; j < arrayObj[strAttribute].length; j++) {
                var strValue = "";
                strValue = strValue + arrayObj[strAttribute][j]["Attribute"] + "<span class='badge'>" + arrayObj[strAttribute][j]["Count"] + "</span>";
                if (j < VT.pageElement.facedNumberBrief) {
                    ulHtml = ulHtml + "<li ><a onclick='VT.pageEvent.Facet_Item_Click(" + "\x22" + arrayObj[strAttribute][j]["Attribute"].replace(/\"/g, " ").replace(/\'/g, "*") + "\x22" + "," + "\x22" + strAttribute + "\x22" + ")'>" + strValue + "</a></li>";
                }
                else {
                    ulHtml = ulHtml + "<li class='eleHide'><a onclick='VT.pageEvent.Facet_Item_Click(" + "\x22" + arrayObj[strAttribute][j]["Attribute"].replace(/\"/g, " ").replace(/\'/g, "*") + "\x22" + "," + "\x22" + strAttribute + "\x22" + ")'>" + strValue + "</a></li>";
                }

                if ((j == arrayObj[strAttribute].length - 1) && j >= VT.pageElement.facedNumberBrief) {
                    ulHtml = ulHtml + "<li class='more'><span><a href='#' onClick='VT.pageEvent.Facet_Expand(" + i + ")'>SHOW MORE...</a></span></li>"
                }
            }
            ulHtml = ulHtml + "</ul>";
        }

        $('#' + eleID).html(ulHtml);
    }
    else $('#' + eleID).html('');

};

VT.UI.getBriefList = function (eleID, listType) {
    var arrayObj = VT.pageElement.DigitalObjectList;
    if (arrayObj != null) {
        var divHtml = "";
        var len_Obj = arrayObj.length;
        if (listType == 'bgrid') {

            var arrayAttribute = ["Title","Volume", "Author", "Year", "Type"]; //attribute in data.
            var len_Attribute = arrayAttribute.length;
            //start div

            divHtml += "<ul class='media-list'>";

            for (var i = 0; i < len_Obj; i++) {

                divHtml = divHtml + "<li id='li_" + arrayObj[i].BibID + "_" + arrayObj[i].VID + "' class='media'  onclick='VT.Methods.doSearch_single_Item(" + "\x22" + arrayObj[i].BibID + "\x22" + "," + "\x22" + arrayObj[i].VID + "\x22" + ",VT.Methods.SingleItem_Search_Callback, VT.Methods.basic_Search_Callback_Error)'>";
                //img div
                var thumbnailPath = VT.Methods.GetImgLink(arrayObj[i]);

                divHtml = divHtml + "<img class='img-thumbnail pull-left' onerror='this.src=\"Images/no_img.png\"' src='" + thumbnailPath + "' />" + "<div class='media-body'>";

                //attribute div
                for (var j = 0; j < len_Attribute; j++) {

                    divHtml = divHtml + "<dl class='dl-horizontal'>";
                    divHtml = divHtml + "<dt>" + arrayAttribute[j] + ":</dt>";
                    var strValue = "";
                    var strAttribute = arrayAttribute[j];
                   
                    if (arrayAttribute[j] == 'Year')
                    { strAttribute = "PublishYear"; }
                    if (arrayAttribute[j] == 'Volume') {
                        strValue = arrayObj[i]['BibID'] + " / <span class='spanddVID'>" + arrayObj[i]['VID'] + "</span>";
                    }
                    else {
                        if ($.isArray(arrayObj[i][strAttribute])) {

                            for (var k = 0; k < arrayObj[i][strAttribute].length; k++) {
                                strValue = strValue + arrayObj[i][strAttribute][k];
                            }

                        }
                        else {
                            strValue = arrayObj[i][strAttribute];
                        }
                    }
                    divHtml = divHtml + "<dd>" + strValue + "</dd>";
                    divHtml = divHtml + "</dl>";
                }
                divHtml = divHtml + "</div></li>";

            }
        }
        else if (listType == 'list') {
            divHtml += '<table class="table table-striped"><thead> <tr> <th>No.</th> <th>First Name</th> </tr></thead><tbody>';
            for (var i = 0; i < len_Obj; i++) {
                divHtml += "<tr><th scope='row'>" + i + "<td><a class='media'  onclick='VT.Methods.doSearch_single_Item(" + "\x22" + arrayObj[i].BibID + "\x22" + "," + "\x22" + arrayObj[i].VID + "\x22" + ",VT.Methods.SingleItem_Search_Callback, VT.Methods.basic_Search_Callback_Error)'>";
                divHtml += arrayObj[i]["Title"] + "</a></td></tr>"
            }
            divHtml = divHtml + "</tbody></table>";
        }
        else if (listType == 'thumbNail') {
            for (var i = 0; i < len_Obj; i++) {
                var thumbnailPath = VT.Methods.GetImgLink(arrayObj[i]);

                divHtml += "<div class='col-lg-3 col-md-4 col-xs-6 thumb'><a class='img-thumbnail' href='#' onclick='VT.Methods.doSearch_single_Item(" + "\x22" + arrayObj[i].BibID + "\x22" + "," + "\x22" + arrayObj[i].VID + "\x22" + ",VT.Methods.SingleItem_Search_Callback, VT.Methods.basic_Search_Callback_Error)'>" +
                             "<img class='img-responsive img-thumbnail' onerror='this.src=\"Images/noThumb.jpg\"' src='" + thumbnailPath + "')'>"
                             + "<lable>" + arrayObj[i]["Title"] + "</label></a></div>";

            }

        }
        $('#' + eleID).html(divHtml);
    }
    else $('#' + eleID).html('');
};
//===contentType: fullCitation; map; fileList
VT.UI.getDetailItem = function (eleID, contentType) {
    var docObj = VT.pageElement.DetailItem;
    //var FInum = docObj.BibID;
    //VT.Methods.doSearch_byFInum_getKML(FInum);
    switch (contentType) {
        case "fullCitationID":
            $('#' + eleID).html(docObj.htmlContent.replace(/DPanther/g, 'VT'));
            break;
        case "listofFilesID":
            var temp = VT.Methods.GetThumbNailLink(docObj);
            var fileListHtml = "<div><div>" + docObj.downloadTree + "</div>";
            fileListHtml += "<div>" + temp + "</div></div>";
            $('#' + eleID).html(fileListHtml);

            break;
        case "mapID":
            break;
    }

};

VT.Methods.GetImgLink = function (docObj) {
    var link;
    var fileName = docObj.Thumbnail;
    var vid = docObj.VID;
    var bibID = docObj.BibID;  
    bibID = bibID.replace(/(\S{2})/g, "$1/");
    return link = VT.DigitalObjRes + bibID + vid + "/" + fileName;
};

VT.Methods.GetThumbNailLink = function (docObj) {
    var arrayLink = [];
    var arrayFilename = docObj.DownloadLink;
    var vid = docObj.VID;
    var bibID = docObj.BibID;

    bibID = bibID.replace(/(\S{2})/g, "$1/");
    var thumHtml = "";
    for (var i = 0; i < arrayFilename.length; i++) {
        var fileName = arrayFilename[i].substr(0, arrayFilename[i].lastIndexOf(".")) + "thm.jpg";
        var downloadLink = "";
        fileName = VT.DigitalObjRes + bibID + vid + "/" + fileName;

        if (arrayFilename[i].indexOf(".jp2") != -1 || arrayFilename[i].indexOf(".JP2") != -1) {
            downloadLink = VT.DigitalObjResNew + bibID + vid + "/" + arrayFilename[i];
        }
        else {
            downloadLink = VT.DigitalObjRes + bibID + vid + "/" + arrayFilename[i];
        }

        thumHtml += "<div class='col-lg-3 col-md-4 col-xs-6 thumb'><a class='img-thumbnail' href='#' onclick='VT.pageEvent.SpotlightDownloadWindow(" + "\x22" + downloadLink + "\x22" + ")'>" +
        "<img class='img-responsive img-thumbnail' onerror='this.src=\"Images/noThumb.jpg\"' src=" + "\x22" + fileName + "\x22" + ")'></a></div>"

        //arrayLink.push(VT.DigitalObjRes + bibID + vid + "/" + fileName);
    }

    return thumHtml;
};

VT.Methods.GetDownloadLink = function (docObj) {
    var arrayLink = [];
    var arrayFilename = docObj.DownloadLink;
    var vid = docObj.VID;
    var bibID = docObj.BibID;

    bibID = bibID.replace(/(\S{2})/g, "$1/");

    for (var i = 0; i < arrayFilename.length; i++) {
        if (arrayFilename[i].indexOf(".jp2") != -1 || arrayFilename[i].indexOf(".JP2") != -1) {
            arrayLink.push(VT.DigitalObjResNew + bibID + vid + "/" + arrayFilename[i]);
        }
        else
            arrayLink.push(VT.DigitalObjRes + bibID + vid + "/" + arrayFilename[i]);
    }

    return arrayLink;
};

VT.pageEvent.SpotlightDownloadWindow = function (html) {
    VT.pageElement.SpotlightDownloadWindow = Ext.create('Ext.window.Window',
        {
            title: "dPanther Window Viewer",
            layout: 'fit',
            width: 800,
            height: 820,
            minWidth: 390,
            minHeight: 280,
            constrain: true,
            modal: true,
            items: [{
                xtype: "component",
                autoEl: {
                    tag: "iframe",
                    src: html
                }
            }]
        });
    VT.pageElement.SpotlightDownloadWindow.show();

};

VT.pageEvent.clearGeoSelectiong = function () {
    if (VT.geoSelection) {
        $('#facetListDis').show();
        $("#collectionListDis").removeClass('markerActive');
        VT.geoSelection = false;
        if (VT.mybox) {
            VT.mybox.close(VT.my_map, VT.pageElement.selectedMarker);
            VT.pageElement.selectedMarker = null;
        }
        for (var i = 0; i < VT.arrayMarker.length; i++) {
            if (VT.arrayMarker[i].getIcon() == "Images/Loc_01_s1.png") {
                VT.arrayMarker[i].setIcon('Images/Loc_Icon-Off_s1.png');
                break;
            }
        }

        $("#lisearchTitle").remove();
        $('#ulsearchTitle').append('<li class="active" id="lisearchTitle">Entire Collection List Has Been Returned</li>');
    }


}

VT.pageEvent.Para_term_search = function (searchContent, code) {
    //change "condition" to "FacetCondition" to enable facet function after page term click
    //reset facet condition to start a new search
    VT.pageElement.FacetCondition = "~" + searchContent + "^" + code;

    VT.pageElement.FacetCondition = VT.pageElement.FacetCondition.trim();
    VT.pageElement.FacetCondition = VT.pageElement.FacetCondition.replace(/\+/g, " ");
    VT.pageElement.FacetCondition = VT.pageElement.FacetCondition.replace(/\//g, "$");
    VT.pageElement.FacetCondition = VT.pageElement.FacetCondition.replace(/\:/g, " ");
    VT.pageElement.FacetCondition = VT.pageElement.FacetCondition.replace(/\-/g, " ");
    VT.pageElement.FacetCondition = VT.pageElement.FacetCondition.replace(/\&/g, "@");
    VT.pageElement.FacetCondition = VT.pageElement.FacetCondition.replace(/\,/g, " ");
    //VT.pageElement.FacetCondition = VT.language.spanish.encode(DPanther.pageElement.FacetCondition);
    VT.pageEvent.showPara_term_search(code, searchContent);
    VT.Methods.doSearch_basic_collections(VT.pageElement.aggCode, VT.pageElement.FacetCondition, '1', '1', '1', '20', VT.Methods.basic_Search_Callback, VT.Methods.basic_Search_Callback_Error);
}

VT.pageEvent.showPara_term_search = function (code, searchContent) {
    var showTerm = VT.pageEvent.showPara_code(code);
    if ($("#liAdvSearch").length > 0) {
        $("#liAdvSearch").remove();
    }
    searchContent = searchContent.replace(/\+/g, " ");
    //searchContent = DPanther.language.spanish.encode(searchContent);
    document.getElementById("ulsearchCritera").innerHTML = "";
    var eleUl = $('#ulsearchCritera li');
    if (eleUl.length == 0) {
        $("#lisearchTitle").remove();
        $('#ulsearchTitle').append('<li class="active" id="lisearchTitle">Refine By:</li>');
    }
    //add a class "liPageTerm to distinguish the criteria from pagge term to facet"
    $('#ulsearchCritera').append('<li class="alert alert-block liPageTerm">' + showTerm + ":" + searchContent + '<button type="button" class="close" data-dismiss="alert">&times;</button></li>');
   
    VT.Methods.closeOnCriteria();
}


VT.Methods.closeOnCriteria = function () {
    $('.alert').on('closed.bs.alert', function () {
        //$('.alert').bind('close', function () {
       
            var eleUl = $('#ulsearchCritera li');
            if (eleUl.length == 1) {
                $("#lisearchTitle").remove();
                $('#ulsearchTitle').append('<li class="active" id="lisearchTitle">Entire Collection List Has Been Returned</li>');
                $("#txtSearchCritera").val('');
            }
            var searchCriteraArray = $(this).text();
            var removeCritera = VT.Methods.SearchCriteraStringClose(VT.pageEvent.showPara_decode(searchCriteraArray.split(':')[0]), searchCriteraArray.split(':')[1]);

            VT.pageElement.FacetCondition = VT.pageElement.FacetCondition.replace(removeCritera, '');
            VT.pageElement.condition = VT.pageElement.FacetCondition + VT.pageElement.AdvSearchCondition;

            if (VT.pageElement.condition == "")
            { VT.pageElement.condition = "1"; }

            VT.Methods.doSearch_basic_collections(VT.pageElement.aggCode, VT.pageElement.condition, '1', '1', '1', '20', VT.Methods.basic_Search_Callback, VT.Methods.basic_Search_Callback_Error);
          
    });

};

VT.pageEvent.showPara_code = function (code) {
    var return_code = code;
    switch (code) {
        case "BI":
            return_code = "BibID";
            break;
        case "TI":
            return_code = "Title";
            break;
        case "AU":
            return_code = "Author";
            break;
        case "SU":
            return_code = "Subject Keywords";
            break;
        case "BI":
            return_code = "BibID";
            break;
        case "CO":
            return_code = "Country";
            break;
        case "ST":
            return_code = "State";
            break;
        case "CT":
            return_code = "County";
            break;
        case "CI":
            return_code = "City";
            break;
        case "PP":
            return_code = "Place of Publication";
            break;
        case "SP":
            return_code = "Spatial Coverage";
            break;
        case "TY":
            return_code = "Type";
            break;
        case "LA":
            return_code = "Language";
            break;
        case "PU":
            return_code = "Publisher";
            break;
        case "TY":
            return_code = "Type";
            break;
        case "LA":
            return_code = "Language";
            break;
        case "GE":
            return_code = "Genre";
            break;
        case "TA":
            return_code = "Target Audience";
            break;
        case "AT":
            return_code = "Attribution";
            break;
        case "TL":
            return_code = "Tickler";
            break;
        case "NO":
            return_code = "Notes";
            break;
        case "ID":
            return_code = "Identifier";
            break;
        case "FR":
            return_code = "Frequency";
            break;
        case "TB":
            return_code = "Tracking Box";
            break;

    }
    return return_code;

}

VT.pageEvent.showPara_decode = function (term) {
    var return_code = term;
    switch (term) {
        case "BibID":
            return_code = "BI";
            break;
        case "Title":
            return_code = "TI";
            break;
        case "Author":
            return_code = "AU";
            break;
        case "Subject Keywords":
            return_code = "SU";
            break;
        case "BibID":
            return_code = "BI";
            break;
        case "Country":
            return_code = "CO";
            break;
        case "State":
            return_code = "ST";
            break;
        case "County":
            return_code = "CT";
            break;
        case "City":
            return_code = "CI";
            break;
        case "Place of Publication":
            return_code = "PP";
            break;
        case "Spatial Coverage":
            return_code = "SP";
            break;
        case "Type":
            return_code = "TY";
            break;
        case "Language":
            return_code = "LA";
            break;
        case "Publisher":
            return_code = "PU";
            break;
        case "Type":
            return_code = "TY";
            break;
        case "Language":
            return_code = "LA";
            break;
        case "Genre":
            return_code = "GE";
            break;
        case "Target Audience":
            return_code = "TA";
            break;
        case "Attribution":
            return_code = "AT";
            break;
        case "Tickler":
            return_code = "TL";
            break;
        case "Notes":
            return_code = "NO";
            break;
        case "Identifier":
            return_code = "ID";
            break;
        case "Frequency":
            return_code = "FR";
            break;
        case "Tracking Box":
            return_code = "TB";
            break;

    }
    return return_code;

};

function printDiv(divId) {
    if (divId) {
        var printDivCSS = '<link href="Styles/print.css" rel="stylesheet" />';
        window.frames["print_frame"].document.clear();
        window.frames["print_frame"].document.write(printDivCSS);
        for (var i = 0; i < divId.length; i++) {
            window.frames["print_frame"].document.write('<div id="'+divId[i]+'">'+document.getElementById(divId[i]).innerHTML+'</div>');
        }
        //window.frames["print_frame"].window.focus();
        //window.frames["print_frame"].window.print();
       
        setTimeout(function () {
            window.frames["print_frame"].focus();
            window.frames["print_frame"].print();
            frame1.remove();
        }, 500);
       
    }
    else {
        window.print();
    }
      
    window.frames["print_frame"].document.close();
    
}
//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ page UI function section @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

