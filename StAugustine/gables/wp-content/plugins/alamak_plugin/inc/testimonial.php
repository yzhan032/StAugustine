<?php
// Registers the new post type and taxonomy

function wpt_testimonial_posttype() {
	register_post_type( 'testimonial',
		array(
			'labels' => array(
				'name' => __( 'Testimonial' ),
				'singular_name' => __( 'Testimonial' ),
				'add_new' => __( 'Add New Testimonial' ),
				'add_new_item' => __( 'Add New Testimonial' ),
				'edit_item' => __( 'Edit Testimonial' ),
				'new_item' => __( 'Add New Testimonial' ),
				'view_item' => __( 'View Testimonial' ),
				'search_items' => __( 'Search Testimonial' ),
				'not_found' => __( 'No testimonial found' ),
				'not_found_in_trash' => __( 'No testimonial found in trash' )
			),
			'public' => true,
			'supports' => array( 'title', 'editor', 'thumbnail', 'comments', 'post-formats' , 'excerpt'),
			'capability_type' => 'post',
			'rewrite' => array("slug" => "testimonial"), // Permalinks format
			'menu_position' => 5,
			'register_meta_box_cb' => 'testimonial_metabox_add',
			'exclude_from_search' => true 
		)
	);

}

add_action( 'init', 'wpt_testimonial_posttype' );




//add Post Details metabox
add_action( 'add_meta_boxes', 'testimonial_metabox_add' );
function testimonial_metabox_add()
{
    add_meta_box( 'testimonial-details', 'Testimonial Details', 'metabox_testimonial_form', 'testimonial', 'normal', 'high' );
}

function metabox_testimonial_form( $post )
{
    $values = get_post_custom( $post->ID );
	$client_position = isset( $values['client_position'] ) ? esc_attr( $values['client_position'][0] ) : '';
    wp_nonce_field( 'my_meta_box_nonce', 'meta_box_nonce' );
?>

<p id="client-position" >
    <label for="client_position">Client Company/Position</label><br/>
    <input type="text" name="client_position" id="client_position" value="<?php echo $client_position; ?>" style="width:100%;"/><br/>
    <small>Enter Client Company/Position here</small>
</p>

<?php }

add_action( 'save_post', 'testimonial_meta_box_save' );
function testimonial_meta_box_save( $post_id )
{
 // Bail out if we're doing an auto save
 if( defined( 'DOING_AUTOSAVE' ) && DOING_AUTOSAVE ) return;

 // If our nonce isn't there, or we can't verify it, bail out
 if( !isset( $_POST['meta_box_nonce'] ) || !wp_verify_nonce( $_POST['meta_box_nonce'], 'my_meta_box_nonce' ) ) return;

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
 if( isset( $_POST['client_position'] ) )
 update_post_meta( $post_id, 'client_position', wp_kses( $_POST['client_position'], $allowed ) );
}