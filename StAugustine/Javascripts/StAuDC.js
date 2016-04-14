
Ext.onReady(function () {
    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ Define the page elements @@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    VT.pageElement.yearSlider = $('#testSlider').slider().on('slideStop', VT.Methods.filerbyYear)
                             .data('slider');
    $("#opacitySlider").slider({
        formater: function (value) {
            return 'Current Opacity: ' + value / 100;
        }
    });
    VT.pageElement.opacitySlider = $("#opacitySlider").slider().on('slideStop', VT.Methods.opacitySet)
                                    .data('slider');

    $('#divOpSlider').hide();
    $('#divAlert').hide();


    Ext.define('HistoricalMap', {
        extend: 'Ext.data.Model',
        fields: [{
            name: 'id'
        },               
        {   name: 'name'
        },
        {
            name: 'bibid'
        },
        {
            name: 'vid'
        }
        ]
    });
    VT.pageElement.imgStore = Ext.create('Ext.data.Store', {
        model: 'HistoricalMap',
        data: VT.pageElement.mapListImg
    });

    VT.pageElement.mapImgView = Ext.create('Ext.view.View', {
        cls: 'patient-view',
        tpl: '<tpl for=".">' +
                '<div class="patient-source"><table id="tblayer_{id}" class="hisDrag"><tbody>' +
                    
                    '<tr><td colspan="2" class="patient-name"><img class="img-thumbnail" width="115" height="70" src="Images/Historic02.jpg" /></td></tr>' +  //<a href="#" id="layer_{id}" onclick="VT.Methods.mapLayerAdd(\'{id}\',\'tblayer_{id}\')"></a>
                    //'<tr><td colspan="2"><input type="button" class="btn btn-info btn-xs pull-left" id="layer_{id}" value="Off" onclick="VT.Methods.mapLayerAdd(\'{id}\',\'tblayer_{id}\')"/></td></tr>' +
                    '<tr><td colspan="2" class="patient-name">{name}</td></tr>' +
                    '<tr><td><button type="button" class="btn btn-info btn-xs pull-left" id="layer_{id}" value="Off" onclick="VT.Methods.mapLayerAdd(\'{id}\',\'tblayer_{id}\',\'{bibid}\',\'{vid}\')"><img src="Images/OFF_s1.jpg" id="layerimg_{id}" /></button>' +
                    '</td><td >{year}</td></tr>' +
                '</tbody></table></div>' +
             '</tpl>',
        itemSelector: 'div.patient-source',
        overItemCls: 'patient-over',
        selectedItemClass: 'patient-selected',
        singleSelect: true,
        store: VT.pageElement.imgStore,

        listeners: {
            render: VT.Methods.initializePatientDragZone

        }
    });

    VT.pageElement.historicalMap = Ext.create('Ext.panel.Panel', {
        //title: 'Historical Map',
        bodyStyle: 'padding: 0px',
        margins: '0 0 0 0',
        width: '145px',

        autoScroll: true,
        items: [VT.pageElement.mapImgView],
        renderTo: document.getElementById('historicalMap')
    });



    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ page initialize @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

    var eleCollection = {
        facetEleID: "ulFacetList",
        listEleID: "divDOListBrif",
        listType: "bgrid",
        detailEleSetting: {
            fullCitationID: "FullCitation",          
            listofFilesID: "tabofContent"
            //elementName: ["Full Citatioin", "Map", "List of Files", "Statistics", "Original Files"],
            //elementID: ["FullCitation", "", "tabofContent","",""]
        }
    };
    
    VT.pageElement.aggCode = VT.pageEvent.GetRequest();
  
    VT.UI.setCollection(eleCollection);
    //VT.Methods.doSearch_basic_collections(VT.pageElement.aggCode, VT.pageElement.condition, '1', '1', '1', '20', VT.Methods.basic_Search_Callback, VT.Methods.basic_Search_Callback_Error);
    VT.pageEvent.Google_init_collections();    
});