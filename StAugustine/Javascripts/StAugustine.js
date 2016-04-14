VT.Methods.doSearch_basic_collections = function (aggregation, condition, startYear, endYear, page, pagelimit, callBack, errorFun) {
    VT.pageElement.loadMask.show();
    var url = VT.DigitalObjectServices + "AggSearchPage/" + aggregation + "/" + condition + "/" + startYear + "/" + endYear + "/" + page + "/" + pagelimit;
    Ajaxsend_request_cros(callBack,
                            true, "get", url,
                            {}, 100000, "application/javascript", "jsonp",
                            errorFun);
};

VT.Methods.doSearch_byLocation_collections = function (aggregation, lat, lng, page, pagelimit, callBack, errorFun) {
    VT.pageElement.loadMask.show();
    var url = VT.DigitalObjectServices + "AggSearchByLocation/" + aggregation + "/" + lat + "/" + lng + "/" + page + "/" + pagelimit;
    Ajaxsend_request_cros(callBack,
                            true, "get",url,
                            {},100000, "application/javascript","jsonp",
                            errorFun);
}

VT.Methods.doSearch_get_historicalMaps = function (callBack, errorFun) {
    var url = "http://tiles.arcgis.com/tiles/FtJyXM7nBotS8g5t/arcgis/rest/services/Butler_1923/MapServer?f=pjson";
    Ajaxsend_request_cros(callBack,
                            true,"get",url,
                            {}, 100000,"application/javascript","jsonp",
                            errorFun);
};

VT.Methods.doSearch_single_Item = function (bibid, vid, callback,errorFun) {

    var url = VT.dpService + "dpCollectionService/collections/getDetailByFI/" + bibid + "/" + vid;
    Ajaxsend_request_cros(callback,
                            true, "get",url,
                            {},100000,"application/javascript", "jsonp",
                            errorFun);
}
//************************************************* Cross domain call back *************************************************
//send request function for AJAX method;
//callBackFun: name of the call back function/object;
//callBackType: symcro or not/bool;
//callBackMethod: post or get / string 
//url: url for the backend function / string
//strPost:Parameter for the backend function / string
function Ajaxsend_request_cros(callBackFun, callBackType, HTTPMethod, url, strPost, timeOut, contentType, dataType, errorFun) {//initialize 
    if (timeOut == "" || timeOut == undefined) {
        timeOut = 10000;
    }
    if (contentType == "" || contentType == undefined) {
        contentType = "application/json";
    }
    if (dataType == "" || dataType == undefined) {
        dataType = "jsonp";
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
        //jsonp: "callback",
        //jsonpCallback: "?",
        success: callBackFun,
        error: errorFun
    });

}