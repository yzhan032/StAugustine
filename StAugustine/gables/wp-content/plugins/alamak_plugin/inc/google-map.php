<?php
// create custom plugin settings menu
add_action('admin_menu', 'google_map_menu');

function google_map_menu() {

	//create new top-level menu
	add_menu_page('Google Map Setting', 'Google Map Setting', 'administrator', __FILE__, 'google_map_page',plugins_url('/map.png', __FILE__));

	//call register settings function
	add_action( 'admin_init', 'register_mysettings' );
}


function register_mysettings() {
	//register our settings
	register_setting( 'google-map-settings-group', 'map_coordinate' );
	register_setting( 'google-map-settings-group', 'map_zoom' );
	register_setting( 'google-map-settings-group', 'map_lightness' );
	register_setting( 'google-map-settings-group', 'map_saturation' );
	register_setting( 'google-map-settings-group', 'map_image' );
	register_setting( 'google-map-settings-group', 'marker_content' );
	register_setting( 'google-map-settings-group', 'gmap_display' );
}

function google_map_page() {
?>
<div class="wrap">
<h2>Google Map Setting</h2>

<form method="post" action="options.php">
    <?php settings_fields( 'google-map-settings-group' ); ?>
    <?php do_settings_sections( 'google-map-settings-group' ); ?>
    <table class="form-table">
        <tr valign="top">
        <th scope="row">1. Google Map Latitude and Longitude</th>
        <td><input type="text" name="map_coordinate" value="<?php echo get_option('map_coordinate'); ?>" /></td>
        </tr>
         
        <tr valign="top">
        <th scope="row">2. Google Map Zoom Value</th>
        <td><input type="text" name="map_zoom" value="<?php echo get_option('map_zoom'); ?>" /></td>
        </tr>
        
        <tr valign="top">
        <th scope="row">3. Google Map Lightness Value</th>
        <td><input type="text" name="map_lightness" value="<?php echo get_option('map_lightness'); ?>" /></td>
        </tr>
        
        <tr valign="top">
        <th scope="row">4. Google Map Saturation Value</th>
        <td><input type="text" name="map_saturation" value="<?php echo get_option('map_saturation'); ?>" /></td>
        </tr>
        <tr valign="top">
        <th scope="row">5. Google Map Marker Icon</th>
        <td>
        <label for="upload_image">
            <input id="map_image" type="text" size="36" name="map_image" value="<?php echo get_option('map_image'); ?>" />
            <input id="upload_image_button" class="button" type="button" value="Upload Image" />
            <br />Enter a URL or upload an image (recommended size 37x37px)
        </label>
        </td>
        </tr>
        
        <tr valign="top">
        <th scope="row">7. Google Map Marker Content</th>
        <td><textarea id="marker_content" cols="30" rows="5" name="marker_content"><?php echo get_option('marker_content'); ?></textarea></td>
        </tr>
        
        <tr valign="top">
        <th scope="row">8. Use shortcode in sidebar</th>
        <td>
        <select name="gmap_display" id="gmap_display">
        	<?php $display = get_option('gmap_display'); ?>
          	<option value="yes" <?php if ($display =='yes'){ echo 'selected="selected"';}?>>Yes</option>
          	<option value="no" <?php if ($display =='no'){ echo 'selected="selected"';}?>>No</option>
        </select> 
        </td>
        </tr>
        
        
    </table>
    
    <?php submit_button(); ?>

</form>
		<h4>Explanation:</h4>
		<ol>
        	<li><p>You can check your latitude and longitude in <a href="http://universimmedia.pagesperso-orange.fr/geo/loc.htm">here</a>. example value: -6.94010,107.62575 </p></li>
            <li><p>Input your zoom level in here. example value: 15</p></li>
            <li><p>Input your value for map lightness here. example value: 7</p></li>
            <li><p>Input your value for map saturation here. example value :0 (for making normal map, default map is "monochrome").</p></li>
            <li><p>You can upload your icon image there.</p></li>
            <li><p>The content will be appear if the marker is clicked. You should use &lt;br /&gt; tag instead of "enter/return" key for line breaks.</p></li>
            <li><p>Choose yes if you want the shortcode in sidebar, choose no if you only want the shortcode only inside the post. <br />
            Map shortcode is <b>[ g_maps width='your map width' height='your map height' ]</b></p></li>
        </ol>
</div>
<?php } 

add_action('admin_enqueue_scripts', 'my_admin_scripts');
 
function my_admin_scripts() {
	if (isset($_GET['page']) && $_GET['page'] == 'alamak_plugin/inc/google-map.php') {
        wp_enqueue_media();
        wp_enqueue_script('my-admin-js',plugins_url( '/js/gmap.js' , __FILE__ ) , array('jquery'),'', true);
	}
}

function google_map_start() { ?> 
			<script type="text/javascript">
			(function ($) {
				'use strict';
				
				//google map
				$(document).ready(function () {
					<?php if  ( get_option('map_image') != '') { ?> var icons = '<?php echo get_option('map_image'); ?>'; 
					<?php } else {?> var icons ='<?php echo plugins_url( '/office-building.png' , __FILE__ ) ?>' <?php }?>
					
					$('.map_canvas').gmap({
						'center': '<?php if  ( get_option('map_coordinate') != '') { echo get_option('map_coordinate');} else { echo "-6.94010,107.62575";} ?>',
						'zoom': <?php if  ( get_option('map_zoom') != '') { echo get_option('map_zoom');} else { echo "15";} ?> ,
						scrollwheel: false,
						'disableDefaultUI': false,
						'styles': [{
							stylers: [{
								lightness: <?php if  ( get_option('map_lightness') != '') { echo get_option('map_lightness');} else { echo "7";} ?>
							}, {
								saturation: <?php if  ( get_option('map_saturation') != '') { echo get_option('map_saturation');} else { echo "-100";} ?>
							}]
						}],
						'callback': function () {
							var self = this;
							
							self.addMarker({
								'position': this.get('map').getCenter(),
								icon: icons,
							}).click(function () {
								self.openInfoWindow({
									<?php $string = get_option('marker_content'); $output = preg_replace('!\s+!m', ' ', $string); ?>
									'content': '<?php if  ( get_option('marker_content') != '') {   echo $output;
									;} else { 
									echo "Visit Us <br> Address: 11231 Buah Batu Bandung <br> Phone: 1233-2324-2324 <br>Email: company-email@email.com"; } ?>'
								}, this);
							});
			
						}
			
					});
				});
			
				
			
			
			})(jQuery);
			</script>

<?php
}

function g_maps_load_home() {
	if (have_posts()){
		global $post; /* load script only on homepage and if the post has google map shortcode*/
		if( is_page_template( 'homepage-slider.php' ) || is_page_template( 'homepage-video.php' ) || is_page_template( 'homepage-youtube.php' ) || is_page_template( 'homepage-custom.php' ) ||  has_shortcode( $post->post_content, 'g_maps' ))  {
		wp_enqueue_script( 'ui_map',plugins_url( '/js/jquery.ui.map.js' , __FILE__ ),array(),'', 'in_footer');
		wp_enqueue_script( 'map_js', plugins_url( '/js/map.js' , __FILE__ ),array(),'', 'in_footer');
		wp_enqueue_script( 'gmap','https://maps.google.com/maps/api/js?sensor=true',array(),'', 'in_footer');
		add_action( 'wp_footer', 'google_map_start',102 );
		}
	}
} 
function g_maps_load_sidebar() {  
	wp_enqueue_script( 'ui_map',plugins_url( '/js/jquery.ui.map.js' , __FILE__ ),array(),'', 'in_footer');
	wp_enqueue_script( 'map_js', plugins_url( '/js/map.js' , __FILE__ ),array(),'', 'in_footer');
	wp_enqueue_script( 'gmap','https://maps.google.com/maps/api/js?sensor=true',array(),'', 'in_footer');
	add_action( 'wp_footer', 'google_map_start',102 );
} 




function rdn_gmap() {
			add_action( 'wp_enqueue_scripts', 'g_maps_load_home' );
			if (get_option('gmap_display')== 'yes'){
				add_action( 'wp_enqueue_scripts', 'g_maps_load_sidebar' );
			}
}    
add_action( 'init', 'rdn_gmap' );


