// JavaScript Document
(function() {
    // Creates a new plugin class and a custom listbox
    tinymce.create('tinymce.plugins.bs_collapse', {
        createControl: function(n, cm) {
            switch (n) {                
                case 'bs_collapse':
                var c = cm.createSplitButton('bs_collapse', {
                    title : 'Collapse',
                    onclick : function() {

                    }
                    //'class':'mceListBoxMenu'
                });

                c.onRenderMenu.add(function(c, m) {
                    m.onShowMenu.add(function(c,m){
                        jQuery('#menu_'+c.id).height('auto').width('auto');
                        jQuery('#menu_'+c.id+'_co').height('auto').addClass('mceListBoxMenu'); 
                        var $menu = jQuery('#menu_'+c.id+'_co').find('tbody:first');
                        if($menu.data('added')) return;
                        $menu.append('');
                        $menu.append('<div style="padding:0 10px 10px">\
                        <label>Number of items<br />\
                        <input type="text" name="itemnum" value="3" onclick="this.select()"  /></label>\
                        </div>');

                        jQuery('<input type="button" class="button" value="Insert" />').appendTo($menu)
                                .click(function(){
                         /**
                          * Shortcode markup
                          * -----------------------
                          *      [bs_collapse id="#"]
                          *         [bs_citem title="" id="" parent=""]
                          *         [/bs_citem]
                          *     [/bs_collapse]
                          *  -----------------------
                          */
                                var uID =  Math.floor((Math.random()*100)+1);
                                var shortcode = '[bs_collapse id="collapse_'+uID+'"]<br class="nc"/>';
                                var num = $menu.find('input[name=itemnum]').val();
                                    for(i=0;i<num;i++){
                                        var id = Math.floor((Math.random()*100)+1);
                                        var title = 'Collapsible Group Item '+(i+1);
                                        shortcode+= '[bs_citem title="'+title+'" id="citem_'+id+'" parent="collapse_'+uID+'"]<br class="nc"/>';
                                        shortcode += 'Collapse content goes here....<br class="nc"/>';
                                        shortcode += '[/bs_citem]<br class="nc"/>';
                                    }

                                shortcode+= '[/bs_collapse]';

                                    tinymce.activeEditor.execCommand('mceInsertContent',false,shortcode);
                                    c.hideMenu();
                                }).wrap('<div style="padding: 0 10px 10px"></div>')

                        $menu.data('added',true); 

                    });

                   // XSmall
                    m.add({title : 'Collapse', 'class' : 'mceMenuItemTitle'}).setDisabled(1);

                 });
                // Return the new splitbutton instance
                return c;
                
            }
            return null;
        }
    });
    tinymce.PluginManager.add('bs_collapse', tinymce.plugins.bs_collapse);
})();