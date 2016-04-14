VT.UI.setting.eleCollection = {
    facetEleID: "ulFacetList",//facet container id
    facetType:"",//facet display type
    listEleID: "divDOListBrif",//list container id
    listType:"bgrid",//list display type
    detailEleID: "",//detail container id
    detailEleSetting: {
        fullCitationID: "FullCitation",
        mapID:"",
        listofFilesID: "tabofContent",
        statisticsID:"",
        OriginalFilesID:""
        //elementName: ["Full Citatioin", "Map", "List of Files", "Statistics", "Original Files"],
        //elementID: ["FullCitation", "", "tabofContent","",""]
    }
};

VT.UI.setCollection = function (setting, callBack)
{
    //get the customize setting from front end
    if (setting)
        for (var property in VT.UI.setting.eleCollection) {
            
            if (setting.hasOwnProperty(property)) {
                if (property == "detailEleSetting")
                    for (var detailProperty in setting[property]) {
                        VT.UI.setting.eleCollection[property][detailProperty] = setting[property][detailProperty];
                    }
                else {
                    VT.UI.setting.eleCollection[property] = setting[property];
                }
            }
        }
        
    //check if callback function has been customized;
    if(!callBack)
        VT.Methods.doSearch_basic_collections(VT.pageElement.aggCode, VT.pageElement.condition, '1', '1', '1', '20', VT.Methods.basic_Search_Callback, VT.Methods.basic_Search_Callback_Error);
    else
        VT.Methods.doSearch_basic_collections(VT.pageElement.aggCode, VT.pageElement.condition, '1', '1', '1', '20', callBack, VT.Methods.basic_Search_Callback_Error);
};

