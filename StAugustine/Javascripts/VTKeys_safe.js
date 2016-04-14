// VTKeys.js
/*
    Copyright 2015 FIU GIS-RS Center.
*/

/**
* @fileoverview This is the assembling config javascript file. 
   The links, pathes, enumerate parameters will be define in this file. 
* @author Boyuan (Keven) Guan / Meng Ma
* @supported Tested in IE 11 / Firefox / Chrome
*/

var VT = {};
VT.page = {};
VT.ajax = {};
VT.pageElement = {};
VT.pageEvent = {};
VT.Methods = {};
VT.UI = {};
VT.UI.setting = {};

//#### Server related variables ###############################
VT.dpService = "http://dpanther.fiu.edu/stAugService/";

VT.pageElement.aggCode = 'usach';

switch (top.location.hostname) {
    case "localhost":
        VT.DPantherRestService = "http://localhost:8080/DPantherRESTServices/";
        break;
    default:
        VT.DPantherRestService = "http://localhost/stAugRESTServices/";
        break;
}

VT.DigitalObjRes = "http://ufdcimages.uflib.ufl.edu/";
VT.DigitalObjResNew = "http://ufdcimages.uflib.ufl.edu/imgserver/?image=/";

VT.DigitalObjectServices = VT.DPantherRestService + "DigitalObjectService/";
//#### End of Server related variables ###############################

//#### Map server related variables ##################################
VT.HistoricalMapService = "http://tiles.arcgis.com/tiles/FtJyXM7nBotS8g5t/arcgis/rest/services/Butler_1923/MapServer";
VT.HistoricalMapLayers =
    [   'http://tiles.arcgis.com/tiles/FtJyXM7nBotS8g5t/arcgis/rest/services/boazio_1586/MapServer',
        'http://tiles.arcgis.com/tiles/FtJyXM7nBotS8g5t/arcgis/rest/services/bry_1599_stpln/MapServer',
        'http://tiles.arcgis.com/tiles/FtJyXM7nBotS8g5t/arcgis/rest/services/Arredondo_1737_stpln/MapServer',
        'http://tiles.arcgis.com/tiles/FtJyXM7nBotS8g5t/arcgis/rest/services/silver_1740_stpln/MapServer',
        'http://tiles.arcgis.com/tiles/FtJyXM7nBotS8g5t/arcgis/rest/services/puente_1764_stpln/MapServer',
        'http://tiles.arcgis.com/tiles/FtJyXM7nBotS8g5t/arcgis/rest/services/jefferys_1769_stpln/MapServer',
        'http://tiles.arcgis.com/tiles/FtJyXM7nBotS8g5t/arcgis/rest/services/jefferys_1777_stpln/MapServer',
        'http://tiles.arcgis.com/tiles/FtJyXM7nBotS8g5t/arcgis/rest/services/mariano_1788_stpln/MapServer',
        'http://tiles.arcgis.com/tiles/FtJyXM7nBotS8g5t/arcgis/rest/services/Butler_1923/MapServer',     
        'http://tiles.arcgis.com/tiles/FtJyXM7nBotS8g5t/arcgis/rest/services/butler_1923_stpln/MapServer',
        'http://tiles.arcgis.com/tiles/FtJyXM7nBotS8g5t/arcgis/rest/services/dist_1_1923/MapServer',
        'http://tiles.arcgis.com/tiles/FtJyXM7nBotS8g5t/arcgis/rest/services/dist_2_1923/MapServer',
        'http://tiles.arcgis.com/tiles/FtJyXM7nBotS8g5t/arcgis/rest/services/dist_3_1923/MapServer',
        'http://tiles.arcgis.com/tiles/FtJyXM7nBotS8g5t/arcgis/rest/services/dist_4_1923/MapServer',
        'http://tiles.arcgis.com/tiles/FtJyXM7nBotS8g5t/arcgis/rest/services/dist_5_1923/MapServer',
        'http://tiles.arcgis.com/tiles/FtJyXM7nBotS8g5t/arcgis/rest/services/dist_6_1923/MapServer',
        'http://tiles.arcgis.com/tiles/FtJyXM7nBotS8g5t/arcgis/rest/services/dist_7_1923/MapServer',
        'http://tiles.arcgis.com/tiles/FtJyXM7nBotS8g5t/arcgis/rest/services/dist_9_1923/MapServer',
        'http://tiles.arcgis.com/tiles/FtJyXM7nBotS8g5t/arcgis/rest/services/dist_10_1923/MapServer',
        'http://tiles.arcgis.com/tiles/FtJyXM7nBotS8g5t/arcgis/rest/services/dist_11_1923/MapServer',
        'http://tiles.arcgis.com/tiles/FtJyXM7nBotS8g5t/arcgis/rest/services/dist_12_1923/MapServer',
        'http://tiles.arcgis.com/tiles/FtJyXM7nBotS8g5t/arcgis/rest/services/dist_13_1923/MapServer' 
    ];
VT.pageElement.mapListImg = [{ id: 0, name: "boazio_1586", bibid: "usach00162", vid: "00001" }, { id: 1, name: "bry_1599_stpln", bibid: "", vid: "00001" }, { id: 2, name: "Arredondo_1737_stpln", bibid: "usach00201", vid: "00001" }, { id: 3, name: "silver_1740_stpln", bibid: "USACH00208", vid: "00001" }, { id: 4, name: "puente_1764_stpln", bibid: "usach00236", vid: "00001" },
                             { id: 5, name: "jefferys_1769_stpln", bibid: "usach00259", vid: "00001" }, { id: 6, name: "jefferys_1777_stpln", bibid: "", vid: "00001" }, { id: 7, name: "mariano_1788_stpln", bibid: "", vid: "00001" }, { id: 8, name: "Butler_1923", bibid: "", vid: "00001" }, { id: 9, name: "butler_1923_stpln", bibid: "", vid: "00001" },
                             { id: 10, name: "dist_1_1923", bibid: "USACH00404", vid: "00001" }, { id: 11, name: "dist_2_1923", bibid: "USACH00404", vid: "00001" }, { id: 12, name: "dist_3_1923", bibid: "USACH00404", vid: "00001" }, { id: 13, name: "dist_4_1923", bibid: "USACH00404", vid: "00001" }, { id: 14, name: "dist_5_1923", bibid: "USACH00404", vid: "00001" },
                             { id: 15, name: "dist_6_1923", bibid: "USACH00404", vid: "00001" }, { id: 16, name: "dist_7_1923", bibid: "USACH00404", vid: "00001" }, { id: 17, name: "dist_9_1923", bibid: "USACH00404", vid: "00001" }, { id: 18, name: "dist_10_1923", bibid: "USACH00404", vid: "00001" },
                             { id: 19, name: "dist_11_1923", bibid: "USACH00404", vid: "00001" }, { id: 20, name: "dist_12_1923", bibid: "USACH00404", vid: "00001" }, { id: 21, name: "dist_13_1923", bibid: "USACH00404", vid: "00001" },
]



VT.ZOOM_MIN = 14;
VT.MAP_CENTER_LAT = 29.895054;
VT.MAP_CENTER_LNG = -81.311886;
//#### End of Map server related variables ##################################

