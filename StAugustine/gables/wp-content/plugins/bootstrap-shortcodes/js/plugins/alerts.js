// JavaScript Document
(function() {
    // Creates a new plugin class and a custom listbox
    tinymce.create('tinymce.plugins.bs_alerts', {
        createControl: function(n, cm) {
            switch (n) {                
                case 'bs_alerts':
                var c = cm.createSplitButton('bs_alerts', {
                    title : 'Notification',
                    onclick : function() {
                    }
                });

                c.onRenderMenu.add(function(c, m) {
                    // Boxes & frames
                    m.add({title : 'Alerts', 'class' : 'mceMenuItemTitle'}).setDisabled(1);
                    m.add({title : 'Success notification', onclick : function() {
                        tinyMCE.activeEditor.execCommand( 'mceInsertContent', false, '[bs_notification type="success"]<strong>Well done!</strong>   You successfully read <a href="#" class="alert-link">this important alert message</a>.  [/bs_notification]' );
                    }});   
                    m.add({title : 'Info notification', onclick : function() {
                        tinyMCE.activeEditor.execCommand( 'mceInsertContent', false, '[bs_notification type="info"]<strong>Heads up!</strong>   This <a href="#" class="alert-link">alert needs your attention</a>, but it\'s not super important.  [/bs_notification]' );
                    }});  
                    m.add({title : 'Warning notification', onclick : function() {
                        tinyMCE.activeEditor.execCommand( 'mceInsertContent', false, '[bs_notification type="warning"]<strong>Warning!</strong>  Best check yo self, you\'re <a href="#" class="alert-link">not looking too good</a>. [/bs_notification]' );
                    }});  
                    m.add({title : 'Error notification', onclick : function() {
                        tinyMCE.activeEditor.execCommand( 'mceInsertContent', false, '[bs_notification type="danger"]<strong>Oh snap!</strong> <a href="#" class="alert-link">Change a few things</a> up and try submitting again.[/bs_notification]' );
                    }});  
                   
                });

                // Return the new splitbutton instance
                return c;
                
            }
            return null;
        }
    });
    tinymce.PluginManager.add('bs_alerts', tinymce.plugins.bs_alerts);
})();