<?php 

//Achievement list shortcode
function awards($attr,$content){
          return '<ul class="achievement-list">'.do_shortcode($content).'</ul>';
}
add_shortcode("awards","awards");

function aw_head( $atts) {
	extract( shortcode_atts( array(
		'grey' => 'grey heading',
		'black' => 'black heading',
	), $atts ) );

	return '<li><p class="strong"><span>' . esc_attr($grey) . '</span> ' . esc_attr($black) . '</p>';
}
add_shortcode("aw_head","aw_head");

function aw_text( $atts) {
	extract( shortcode_atts( array(
		'text' => 'description'
	), $atts ) );

	return '<p>' . esc_attr($text) . '</p></li>';
}
add_shortcode("aw_text","aw_text");

//Google Map shortcode
function g_maps( $atts) {
	extract( shortcode_atts( array(
		'width' => '100%',
		'height' => '450px'
	), $atts ) );

	return '<div class="map_canvas" style="width:' . esc_attr($width) . ';height:' . esc_attr($height) . ';"></div>' ;
}
add_shortcode("g_maps","g_maps");

//flickr feed shortcode
function rdn_flickr_feed() {

	return '<ul class="flickr-feed clearfix"></ul>' ;
}
add_shortcode("rdn_flickr_feed","rdn_flickr_feed");



//address list shortcode
function address($attr,$content){
          return '<ul class="address-list">'.do_shortcode($content).'</ul>';
}
add_shortcode("address","address");

function add_list( $atts) {
	extract( shortcode_atts( array(
		'icon' => 'map-marker',
		'text' => 'Your Description',
	), $atts ) );

	return '<li><i class="fa fa-' . esc_attr($icon) . '"></i> ' . esc_attr($text) . '</li>';
}
add_shortcode("add_list","add_list");

function address_link( $atts) {
	extract( shortcode_atts( array(
		'icon' => 'map-marker',
		'text' => 'Your Description',
		'link'=> '#'
	), $atts ) );

	return '<li><i class="fa fa-' . esc_attr($icon) . '"></i> <a href="' . esc_attr($link) . '">' . esc_attr($text) . '</a></li>';
}
add_shortcode("address_link","address_link");


//remove unwanted br from the shortcode above
add_filter("the_content", "the_content_filter");
 
function the_content_filter($content) {
 
// array of custom shortcodes requiring the fix
$block = join("|",array("aw_text","aw_head","add_list","address_link","awards","address"));
 
// opening tag
$rep = preg_replace("/(<p>)?\[($block)(\s[^\]]+)?\](<\/p>|<br \/>)?/","[$2$3]",$content);
// closing tag
$rep = preg_replace("/(<p>)?\[\/($block)](<\/p>|<br \/>)?/","[/$2]",$rep);
 
return $rep;
 
}



//tinymce
 // init process for registering achievement button
 add_action('init', 'achievement_shortcode_button_init');
 function achievement_shortcode_button_init() {

      //Abort early if the user will never see TinyMCE
      if ( ! current_user_can('edit_posts') && ! current_user_can('edit_pages') && get_user_option('rich_editing') == 'true')
           return;

      //Add a callback to regiser our tinymce plugin   
      add_filter("mce_external_plugins", "achievement_register_tinymce_plugin"); 

      // Add a callback to add our button to the TinyMCE toolbar
      add_filter('mce_buttons', 'achievement_add_tinymce_button');
}


//This callback registers our plug-in
function achievement_register_tinymce_plugin($plugin_array) {
    $plugin_array['achievement_button'] =  plugins_url( '/js/shortcode.js' , __FILE__ );
    return $plugin_array;
}

//This callback adds our button to the toolbar
function achievement_add_tinymce_button($buttons) {
            //Add the button ID to the $button array
    $buttons[] = "achievement_button";
    return $buttons;
}


 // init process for registering address button
 add_action('init', 'address_shortcode_button_init');
 function address_shortcode_button_init() {

      //Abort early if the user will never see TinyMCE
      if ( ! current_user_can('edit_posts') && ! current_user_can('edit_pages') && get_user_option('rich_editing') == 'true')
           return;

      //Add a callback to regiser our tinymce plugin   
      add_filter("mce_external_plugins", "address_register_tinymce_plugin"); 

      // Add a callback to add our button to the TinyMCE toolbar
      add_filter('mce_buttons', 'address_add_tinymce_button');
}


//This callback registers our plug-in
function address_register_tinymce_plugin($plugin_array) {
    $plugin_array['address_button'] =  plugins_url( '/js/shortcode.js' , __FILE__ );
    return $plugin_array;
}

//This callback adds our button to the toolbar
function address_add_tinymce_button($buttons) {
            //Add the button ID to the $button array
    $buttons[] = "address_button";
    return $buttons;
}

