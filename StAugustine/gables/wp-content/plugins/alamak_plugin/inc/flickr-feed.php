<?php 

// create custom plugin settings menu
add_action('admin_menu', 'rdn_flickr_menu');

function rdn_flickr_menu() {

	//create new top-level menu
	add_menu_page('Flickr Feed Setting', 'Flickr Feed Setting', 'administrator', __FILE__, 'rdn_flickr_page',plugins_url('/flickr.png', __FILE__));

	//call register settings function
	add_action( 'admin_init', 'register_rdn_flickr_feed' );
}


function register_rdn_flickr_feed() {
	//register our settings
	register_setting( 'rdn-flickr-feed-setting-group', 'rdn_flickr_id' );
	register_setting( 'rdn-flickr-feed-setting-group', 'rdn_flickr_img');
}

function plugin_options_validate($input) {
$newinput['text_string'] = trim($input['text_string']);
if(!preg_match('/^[a-z0-9]{32}$/i', $newinput['text_string'])) {
$newinput['text_string'] = '';
}
return $newinput;
}
function rdn_flickr_page() {
?>
<div class="wrap">
<h2>Flickr Feed Setting</h2>

<form method="post" action="options.php">
    <?php settings_fields( 'rdn-flickr-feed-setting-group' ); ?>
    <?php do_settings_sections( 'rdn-flickr-feed-setting-group' ); ?>
    <table class="form-table">
        <tr valign="top">
        <th scope="row">1. Flickr ID<br/> <small>eg: 52617155@N08</small></th>
        <td><input type="text" name="rdn_flickr_id" value="<?php echo get_option('rdn_flickr_id'); ?>" /></td>
        </tr>
         
        <tr valign="top">
        <th scope="row">2. Show Flickr Image.<br/> <small>(default value 9)</small></th>
        <td><input type="text" name="rdn_flickr_img" value="<?php echo get_option('rdn_flickr_img'); ?>" /></td>
        </tr>
        
        
    </table>
    
    <?php submit_button(); ?>

</form>
		<h4>Explanation:</h4>
		<ul>
        	<li><p>You can check your Flickr ID <a href="http://idgettr.com/" target="_blank">here</a></p></li> 
        	<li><p>You can decide how many images will be displayed. Default value is 9 </p></li>
            <li><p>After you finish set it up, you can use the shortcode to display the flickr feed in post/text widget.
            <br />The shortcode is <b>[rdn_flickr_feed]</b>.
            </p></li>
        </ul>
</div>
<?php } 



//load all script for flickr feed
function rdn_flickr_load() {
	wp_enqueue_script( 'flickr-feed',plugins_url( '/js/jflickrfeed.min.js' , __FILE__ ),array(),'', 'in_footer');
	wp_register_style('flickr_feed_style', plugins_url( '/css/flickr.css' , __FILE__ ), array(), '1','all' );
	wp_enqueue_style('flickr_feed_style');
} 
//only load script if flickr feed shortcode exist
function rdn_flickr_feed_shortcode_exist() {
		add_action( 'wp_enqueue_scripts', 'rdn_flickr_load' );
		add_action( 'wp_footer', 'rdn_flickr_feed_start',102 );
}
add_action( 'init', 'rdn_flickr_feed_shortcode_exist' );

//flickr feed function script
function rdn_flickr_feed_start() { ?>
	<script type="text/javascript">
	(function ($) {
	'use strict';
	//script for flickr feed
	$('.flickr-feed').jflickrfeed({
		limit: <?php if  ( get_option('rdn_flickr_img') != '') { echo get_option('rdn_flickr_img');} else { echo "9";} ?>,
		qstrings: {
			id: '<?php if  ( get_option('rdn_flickr_id') != '') { echo get_option('rdn_flickr_id');} else { echo "52617155@N08";} ?>'
		},
		itemTemplate: '<li>' + '<a href="{{image_b}}" data-rel="prettyPhoto"><img src="{{image_s}}" alt="{{title}}" /></a>' + '</li>'
	});
	// script prettyphoto
    $( window ).load(function() {
        $(".flickr-feed a").prettyPhoto({
            social_tools: false,
            deeplinking: false
        });
    });
	})(jQuery);
	</script> 
<?php }