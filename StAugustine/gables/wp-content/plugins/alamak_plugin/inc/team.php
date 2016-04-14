<?php
// Registers team custom post type  and taxonomy

function wpt_team_posttype() {
	register_post_type( 'team',
		array(
			'labels' => array(
				'name' => __( 'Team' ),
				'singular_name' => __( 'Team' ),
				'add_new' => __( 'Add New Team' ),
				'add_new_item' => __( 'Add New Team' ),
				'edit_item' => __( 'Edit Team' ),
				'new_item' => __( 'Add New Team' ),
				'view_item' => __( 'View Team' ),
				'search_items' => __( 'Search Team' ),
				'not_found' => __( 'No Team found' ),
				'not_found_in_trash' => __( 'No Team found in trash' )
			),
			'public' => true,
			'supports' => array( 'title', 'editor', 'thumbnail', 'comments', 'post-formats' , 'excerpt'),
			'capability_type' => 'post',
			'rewrite' => array("slug" => "team"), // Permalinks format
			'menu_position' => 5,
			'register_meta_box_cb' => 'team_metabox_add',
			'exclude_from_search' => true 
		)
	);

}

add_action( 'init', 'wpt_team_posttype' );

function my_taxonomies_team() {
	$labels = array(
		'name'              => _x( 'Team Favorite', 'taxonomy general name' ),
		'singular_name'     => _x( 'Team Favorite', 'taxonomy singular name' ),
		'search_items'      => __( 'Search Team Favorite' ),
		'all_items'         => __( 'All Team Favorite' ),
		'parent_item'       => __( 'Parent Team Favorite' ),
		'parent_item_colon' => __( 'Parent Team Favorite:' ),
		'edit_item'         => __( 'Edit Team Favorite' ), 
		'update_item'       => __( 'Update Team Favorite' ),
		'add_new_item'      => __( 'Add New Team Favorite' ),
		'new_item_name'     => __( 'New Team Favorite' ),
		'menu_name'         => __( 'Team Favorite' ),
	);
	$args = array(
		'labels' => $labels,
		'hierarchical' => true,
	);
	register_taxonomy( 'team_favorite', 'team', $args );
}
add_action( 'init', 'my_taxonomies_team', 0 );

//metabox-custom script

function team_metabox() {
    wp_enqueue_script('custom-meta-boxes', plugins_url( '/js/custom-meta-boxes.js' , __FILE__ ) , array('jquery'),'', true);
}    

add_action('admin_enqueue_scripts', 'team_metabox');

//add Post Details metabox
add_action( 'add_meta_boxes', 'team_metabox_add' );
function team_metabox_add()
{
    add_meta_box( 'team-details', 'Post Details', 'metabox_team_form', 'team', 'normal', 'high' );
}

//add Team Social Network metabox
add_action( 'add_meta_boxes', 'teamsc_metabox_add' );
function teamsc_metabox_add()
{
    add_meta_box( 'team-sc', 'Team Social Network', 'metabox_teamsc_form', 'team', 'side', 'low' );
}
function metabox_team_form( $post )
{
    $values = get_post_custom( $post->ID );
	$black_text = isset( $values['black_text'] ) ? esc_attr( $values['black_text'][0] ) : '';
	$grey_text = isset( $values['grey_text'] ) ? esc_attr( $values['grey_text'][0] ) : '';
	$gallery_sport = isset( $values['gallery_port'] ) ? esc_attr( $values['gallery_port'][0] ) : '';
	$embed_link = isset( $values['embed_post'] ) ? esc_attr( $values['embed_post'][0] ) : '';
	$team_icon = isset( $values['team_icon'] ) ? esc_attr( $values['team_icon'][0] ) : '';
    wp_nonce_field( 'team_meta_box_nonce', 'meta_box_nonce' );
?>

<p id="black-text" >
    <label for="client_name">Black Text</label><br/>
    <input type="text" name="black_text" id="black_text" value="<?php echo $black_text; ?>" style="width:100%;"/><br/>
    <small>Enter your black text here. eg: Role</small>
</p>
<p id="grey-text" >
    <label for="grey_text">Grey Text</label><br/>
    <input type="text" name="grey_text" id="grey_text" value="<?php echo $grey_text; ?>" style="width:100%;"/><br/>
    <small>Enter your grey text here. eg: Web Designer</small>
</p>
<p id="team-icon" >
    <label for="team_icon">Team Icon</label><br/>
    <input type="text" name="team_icon" id="team_icon" value="<?php echo $team_icon; ?>" style="width:100%;"/><br/>
    <small>Insert your team icon. eg: fa-camera . You can check this <a href="http://fortawesome.github.io/Font-Awesome/icons/" target="_blank" >page</a> for the icon list.</small>
</p>
<p id="gallery-port" style="display:none;">
    <label for="gallery_port">Insert Gallery shortcode here</label><br/>
    <textarea name="gallery_port" id="gallery_port" style="width:100%;" rows="5"><?php echo $gallery_sport; ?></textarea><br/>
    <small>Cut/Copy the gallery shortcode that created by wordpress in content area into here. eg. [gallery size="large" columns="1" link="file" ids="98,97,45,43,96" orderby="rand"]
    <br/>**dont forget to add <b>size="large" link="file"</b> inside the gallery shortcode! If you dont want the image linked, change into <b>link="none"</b></small>
</p>

<p id="embed-post-code" style="display:none;">
    <label for="embed_post">Insert Link Video/Audio Here</label><br/>
    <textarea name="embed_post" id="embed_post" style="width:100%;" rows="5"><?php echo $embed_link; ?></textarea><br/>
    <small>Insert the link for video/podcast here. <br />For video from youtube/vimeo just put the link without any attribute like  ?wmode=opaque.<br /> eg: http://www.youtube.com/embed/nAuo7KEQHT4 
    </small>
</p>
<?php }

function metabox_teamsc_form( $post )
{
    $values = get_post_custom( $post->ID );
	$facebook_social = isset( $values['facebook_social'] ) ? esc_attr( $values['facebook_social'][0] ) : '';
	$twitter_social = isset( $values['twitter_social'] ) ? esc_attr( $values['twitter_social'][0] ) : '';
	$google_social = isset( $values['google_social'] ) ? esc_attr( $values['google_social'][0] ) : '';
	$pinterest_social = isset( $values['pinterest_social'] ) ? esc_attr( $values['pinterest_social'][0] ) : '';
	$skype_social = isset( $values['skype_social'] ) ? esc_attr( $values['skype_social'][0] ) : '';
	$youtube_social = isset( $values['youtube_social'] ) ? esc_attr( $values['youtube_social'][0] ) : '';
	$vimeo_social = isset( $values['vimeo_social'] ) ? esc_attr( $values['vimeo_social'][0] ) : '';
	$flickr_social = isset( $values['flickr_social'] ) ? esc_attr( $values['flickr_social'][0] ) : '';
	$vk_social = isset( $values['vk_social'] ) ? esc_attr( $values['vk_social'][0] ) : '';
	$email_social = isset( $values['email_social'] ) ? esc_attr( $values['email_social'][0] ) : '';
    wp_nonce_field( 'teamsc_meta_box_nonce', 'metaasc_box_nonce' );
?>


<p id="facebook-social" >
    <label for="facebook_social">Facebook Link</label><br/>
    <input type="text" name="facebook_social" id="facebook_social" value="<?php echo $facebook_social; ?>" style="width:100%;"/><br/>
    <small>Enter your team facebook link here. Leave it blank if you dont want use it.</small>
</p>
<p id="twitter-social" >
    <label for="twitter_social">Twitter Link</label><br/>
    <input type="text" name="twitter_social" id="twitter_social" value="<?php echo $twitter_social; ?>" style="width:100%;"/><br/>
    <small>Enter your team twitter link here. Leave it blank if you dont want use it.</small>
</p>
<p id="google-social" >
    <label for="google_social">Google Plus Link</label><br/>
    <input type="text" name="google_social" id="google_social" value="<?php echo $google_social; ?>" style="width:100%;"/><br/>
    <small>Enter your team google plus link here. Leave it blank if you dont want use it.</small>
</p>
<p id="pinterest-social" >
    <label for="pinterest_social">Pinterest Link</label><br/>
    <input type="text" name="pinterest_social" id="pinterest_social" value="<?php echo $pinterest_social; ?>" style="width:100%;"/><br/>
    <small>Enter your team pinterest link here. Leave it blank if you dont want use it.</small>
</p>
<p id="skype-social" >
    <label for="skype_social">Skype Link</label><br/>
    <input type="text" name="skype_social" id="skype_social" value="<?php echo $skype_social; ?>" style="width:100%;"/><br/>
    <small>Enter your team skype link here. Leave it blank if you dont want use it.</small>
</p>
<p id="youtube-social" >
    <label for="youtube_social">Youtube Link</label><br/>
    <input type="text" name="youtube_social" id="youtube_social" value="<?php echo $youtube_social; ?>" style="width:100%;"/><br/>
    <small>Enter your team youtube link here. Leave it blank if you dont want use it.</small>
</p>
<p id="vimeo-social" >
    <label for="vimeo_social">Vimeo Link</label><br/>
    <input type="text" name="vimeo_social" id="vimeo_social" value="<?php echo $vimeo_social; ?>" style="width:100%;"/><br/>
    <small>Enter your team vimeo link here. Leave it blank if you dont want use it.</small>
</p>
<p id="flickr-social" >
    <label for="flickr_social">Flickr Link</label><br/>
    <input type="text" name="flickr_social" id="flickr_social" value="<?php echo $flickr_social; ?>" style="width:100%;"/><br/>
    <small>Enter your team flickr link here. Leave it blank if you dont want use it.</small>
</p>
<p id="vk-social" >
    <label for="vk_social">VK Link</label><br/>
    <input type="text" name="vk_social" id="vk_social" value="<?php echo $vk_social; ?>" style="width:100%;"/><br/>
    <small>Enter your team VK link here. Leave it blank if you dont want use it.</small>
</p>
<p id="email-social" >
    <label for="email_social">Email</label><br/>
    <input type="text" name="email_social" id="email_social" value="<?php echo $email_social; ?>" style="width:100%;"/><br/>
    <small>Enter your team email here. Leave it blank if you dont want use it.</small>
</p>


<?php }

add_action( 'save_post', 'team_meta_box_save' );
function team_meta_box_save( $post_id )
{
 // Bail out if we're doing an auto save
 if( defined( 'DOING_AUTOSAVE' ) && DOING_AUTOSAVE ) return;

 // If our nonce isn't there, or we can't verify it, bail out
 if( !isset( $_POST['meta_box_nonce'] ) || !wp_verify_nonce( $_POST['meta_box_nonce'], 'team_meta_box_nonce' ) ) return;

 // If our current user can't edit this post, bail out
 if( !current_user_can( 'edit_post' ) ) return;

 // Now, actually save the data
 $allowed = array(
 'a' => array(
 'href' => array(), 'title' => array()),
 'iframe' => array(
 'src' => array(),'name' => array(),'width' => array(),'height' => array(),'frameborder' => array(),'longdesc' => array(),'align' => array(),'marginwidth' => array(),'marginheight' => array(),'scrolling' => array())
 );

 // Make sure your data is set
 

 if( isset( $_POST['black_text'] ) )
 update_post_meta( $post_id, 'black_text', wp_kses( $_POST['black_text'], $allowed ) );
 if( isset( $_POST['grey_text'] ) )
 update_post_meta( $post_id, 'grey_text', wp_kses( $_POST['grey_text'], $allowed ) );
 if( isset( $_POST['embed_post'] ) )
 update_post_meta( $post_id, 'embed_post', wp_kses( $_POST['embed_post'], $allowed ) );
 if( isset( $_POST['gallery_port'] ) )
 update_post_meta( $post_id, 'gallery_port', wp_kses( $_POST['gallery_port'], $allowed ) );
 if( isset( $_POST['team_icon'] ) )
 update_post_meta( $post_id, 'team_icon', wp_kses( $_POST['team_icon'], $allowed ) );
}

add_action( 'save_post', 'teamsc_meta_box_save' );
function teamsc_meta_box_save( $post_id )
{
 // Bail out if we're doing an auto save
 if( defined( 'DOING_AUTOSAVE' ) && DOING_AUTOSAVE ) return;

 // If our nonce isn't there, or we can't verify it, bail out
 if( !isset( $_POST['metaasc_box_nonce'] ) || !wp_verify_nonce( $_POST['metaasc_box_nonce'], 'teamsc_meta_box_nonce' ) ) return;

 // If our current user can't edit this post, bail out
 if( !current_user_can( 'edit_post' ) ) return;

 // Now, actually save the data
 $allowed = array(
 'a' => array(
 'href' => array(), 'title' => array()),
 'iframe' => array(
 'src' => array(),'name' => array(),'width' => array(),'height' => array(),'frameborder' => array(),'longdesc' => array(),'align' => array(),'marginwidth' => array(),'marginheight' => array(),'scrolling' => array())
 );

 // Make sure your data is set
 if( isset( $_POST['facebook_social'] ) )
 update_post_meta( $post_id, 'facebook_social', wp_kses( $_POST['facebook_social'], $allowed ) );
 if( isset( $_POST['twitter_social'] ) )
 update_post_meta( $post_id, 'twitter_social', wp_kses( $_POST['twitter_social'], $allowed ) );
 if( isset( $_POST['google_social'] ) )
 update_post_meta( $post_id, 'google_social', wp_kses( $_POST['google_social'], $allowed ) );
 if( isset( $_POST['pinterest_social'] ) )
 update_post_meta( $post_id, 'pinterest_social', wp_kses( $_POST['pinterest_social'], $allowed ) );
 if( isset( $_POST['skype_social'] ) )
 update_post_meta( $post_id, 'skype_social', wp_kses( $_POST['skype_social'], $allowed ) );
 if( isset( $_POST['youtube_social'] ) )
 update_post_meta( $post_id, 'youtube_social', wp_kses( $_POST['youtube_social'], $allowed ) );
 if( isset( $_POST['vimeo_social'] ) )
 update_post_meta( $post_id, 'vimeo_social', wp_kses( $_POST['vimeo_social'], $allowed ) );
 if( isset( $_POST['flickr_social'] ) )
 update_post_meta( $post_id, 'flickr_social', wp_kses( $_POST['flickr_social'], $allowed ) );
 if( isset( $_POST['vk_social'] ) )
 update_post_meta( $post_id, 'vk_social', wp_kses( $_POST['vk_social'], $allowed ) );
 if( isset( $_POST['email_social'] ) )
 update_post_meta( $post_id, 'email_social', wp_kses( $_POST['email_social'], $allowed ) );
}



