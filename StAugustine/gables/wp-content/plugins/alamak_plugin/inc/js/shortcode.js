jQuery(document).ready(function($) {
	//ACHIEVEMENT SHORTCODE
    tinymce.create('tinymce.plugins.achievement_plugin', {
        init : function(ed, url) {
                // Register command for when button is clicked
                ed.addCommand('achievement_insert_shortcode', function() {
                        content =  "[awards]\
						<br>[aw_head grey='Insert your grey text here' black='Insert your black text here']\
						<br>[aw_text text='Insert your description here']\
						<br>[aw_head grey='Insert your grey text here' black='Insert your black text here']\
						<br>[aw_text text='Insert your description here']\
						<br>[aw_head grey='Insert your grey text here' black='Insert your black text here']\
						<br>[aw_text text='Insert your description here']\
						<br>[aw_head grey='Insert your grey text here' black='Insert your black text here']\
						<br>[aw_text text='Insert your description here']\
						<br>[/awards]";
                    

                    tinymce.execCommand('mceInsertContent', false, content);
                });

            // Register buttons - trigger above command when clicked
            ed.addButton('achievement_button', {title : 'Achievement List Shortcode', cmd : 'achievement_insert_shortcode', image: url + '/trophy.png' });
        },   
    });

    // Register our TinyMCE plugin
    // first parameter is the button ID1
    // second parameter must match the first parameter of the tinymce.create() function above
    tinymce.PluginManager.add('achievement_button', tinymce.plugins.achievement_plugin);
	
	//ADDRESS SHORTCODE
	 tinymce.create('tinymce.plugins.address_plugin', {
        init : function(ed, url) {
                // Register command for when button is clicked
                ed.addCommand('address_insert_shortcode', function() {
                        content =  "[address]\
						<br>[add_list icon='map-marker' text='Insert Your Description Here']\
						<br>[add_list icon='phone' text='Insert Your Description Here']\
						<br>[add_list icon='suitcase' text='Insert Your Description Here']\
						<br>[address_link icon='envelope' link='Your Link' text='Insert Your Text Link Here']\
						<br>[/address]";
                    

                    tinymce.execCommand('mceInsertContent', false, content);
                });

            // Register buttons - trigger above command when clicked
            ed.addButton('address_button', {title : 'Address List Shortcode', cmd : 'address_insert_shortcode', image: url + '/marker.png' });
        },   
    });

    // Register our TinyMCE plugin
    // first parameter is the button ID1
    // second parameter must match the first parameter of the tinymce.create() function above
    tinymce.PluginManager.add('address_button', tinymce.plugins.address_plugin);
});