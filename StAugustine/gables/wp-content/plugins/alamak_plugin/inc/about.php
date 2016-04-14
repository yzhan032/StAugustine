<?php
// Registers the new post type 

function wpt_about_posttype() {
	register_post_type( 'about-us',
		array(
			'labels' => array(
				'name' => __( 'About Us' ),
				'singular_name' => __( 'About Us' ),
				'add_new' => __( 'Add New About Us' ),
				'add_new_item' => __( 'Add New About Us' ),
				'edit_item' => __( 'Edit About Us' ),
				'new_item' => __( 'Add New About Us' ),
				'view_item' => __( 'View About Us' ),
				'search_items' => __( 'Search About Us' ),
				'not_found' => __( 'No about us found' ),
				'not_found_in_trash' => __( 'No about us found in trash' )
			),
			'public' => true,
			'supports' => array( 'title', 'editor', 'thumbnail', 'comments', 'post-formats' , 'excerpt'),
			'capability_type' => 'post',
			'rewrite' => array("slug" => "about-us"), // Permalinks format
			'menu_position' => 5,
			'register_meta_box_cb' => 'abt_metabox_add',
			'exclude_from_search' => true 
		)
	);

}

add_action( 'init', 'wpt_about_posttype' );



//metabox-custom script

function abt_metabox() {
    wp_enqueue_script('custom-meta-boxes', plugins_url( '/js/custom-meta-boxes.js' , __FILE__ ) , array('jquery'),'', true);
}    

add_action('admin_enqueue_scripts', 'abt_metabox');

//add Post Details metabox
add_action( 'add_meta_boxes', 'abt_metabox_add' );
function abt_metabox_add()
{
    add_meta_box( 'about-details', 'Post Details', 'metabox_about_form', 'about-us', 'normal', 'high' );
}

function metabox_about_form( $post )
{
    $values = get_post_custom( $post->ID );
	$tab_title = isset( $values['tab_title'] ) ? esc_attr( $values['tab_title'][0] ) : '';
	$black_text = isset( $values['black_text'] ) ? esc_attr( $values['black_text'][0] ) : '';
	$grey_text = isset( $values['grey_text'] ) ? esc_attr( $values['grey_text'][0] ) : '';
	$gallery_sport = isset( $values['gallery_port'] ) ? esc_attr( $values['gallery_port'][0] ) : '';
	$embed_link = isset( $values['embed_post'] ) ? esc_attr( $values['embed_post'][0] ) : '';
    wp_nonce_field( 'about_meta_box_nonce', 'meta_box_nonce' );
?>
<p id="tab-title-name" >
    <label for="tab_title">Tab Title</label><br/>
    <input type="text" name="tab_title" id="tab_title" value="<?php echo $tab_title; ?>" style="width:100%;"/><br/>
    <small>Enter Title for "about us" tab here</small>
</p>
<p id="black-text" >
    <label for="client_name">Black Text</label><br/>
    <input type="text" name="black_text" id="black_text" value="<?php echo $black_text; ?>" style="width:100%;"/><br/>
    <small>Enter your black text here. eg: 0.1</small>
</p>
<p id="grey-text" >
    <label for="grey_text">Grey Text</label><br/>
    <input type="text" name="grey_text" id="grey_text" value="<?php echo $grey_text; ?>" style="width:100%;"/><br/>
    <small>Enter your grey text here. eg: History</small>
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

add_action( 'save_post', 'about_meta_box_save' );
function about_meta_box_save( $post_id )
{
 // Bail out if we're doing an auto save
 if( defined( 'DOING_AUTOSAVE' ) && DOING_AUTOSAVE ) return;

 // If our nonce isn't there, or we can't verify it, bail out
 if( !isset( $_POST['meta_box_nonce'] ) || !wp_verify_nonce( $_POST['meta_box_nonce'], 'about_meta_box_nonce' ) ) return;

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
 
  if( isset( $_POST['tab_title'] ) )
 update_post_meta( $post_id, 'tab_title', wp_kses( $_POST['tab_title'], $allowed ) );
 if( isset( $_POST['black_text'] ) )
 update_post_meta( $post_id, 'black_text', wp_kses( $_POST['black_text'], $allowed ) );
 if( isset( $_POST['grey_text'] ) )
 update_post_meta( $post_id, 'grey_text', wp_kses( $_POST['grey_text'], $allowed ) );
 if( isset( $_POST['embed_post'] ) )
 update_post_meta( $post_id, 'embed_post', wp_kses( $_POST['embed_post'], $allowed ) );
 if( isset( $_POST['gallery_port'] ) )
 update_post_meta( $post_id, 'gallery_port', wp_kses( $_POST['gallery_port'], $allowed ) );
}