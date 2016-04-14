<?php
// Registers the new post type and taxonomy

function wpt_portfolio_posttype() {
	register_post_type( 'portfolio',
		array(
			'labels' => array(
				'name' => __( 'Portfolio' ),
				'singular_name' => __( 'Portfolio' ),
				'add_new' => __( 'Add New Portfolio' ),
				'add_new_item' => __( 'Add New Portfolio' ),
				'edit_item' => __( 'Edit Portfolio' ),
				'new_item' => __( 'Add New Portfolio' ),
				'view_item' => __( 'View Portfolio' ),
				'search_items' => __( 'Search Portfolio' ),
				'not_found' => __( 'No portfolio found' ),
				'not_found_in_trash' => __( 'No portfolio found in trash' )
			),
			'public' => true,
			'supports' => array( 'title', 'editor', 'thumbnail', 'comments', 'post-formats' , 'excerpt'),
			'capability_type' => 'post',
			'rewrite' => array("slug" => "portfolio"), // Permalinks format
			'menu_position' => 5,
			'has_archive' => true, //added archive
			'register_meta_box_cb' => 'sgn_metabox_add',
			'exclude_from_search' => true 
		)
	);

}

add_action( 'init', 'wpt_portfolio_posttype' );

function my_taxonomies_portfolio() {
	$labels = array(
		'name'              => _x( 'Portfolio Categories', 'taxonomy general name' ),
		'singular_name'     => _x( 'Portfolio Category', 'taxonomy singular name' ),
		'search_items'      => __( 'Search Portfolio Categories' ),
		'all_items'         => __( 'All Portfolio Categories' ),
		'parent_item'       => __( 'Parent Portfolio Category' ),
		'parent_item_colon' => __( 'Parent Portfolio Category:' ),
		'edit_item'         => __( 'Edit Portfolio Category' ), 
		'update_item'       => __( 'Update Portfolio Category' ),
		'add_new_item'      => __( 'Add New Portfolio Category' ),
		'new_item_name'     => __( 'New Portfolio Category' ),
		'menu_name'         => __( 'Portfolio Categories' ),
	);
	$args = array(
		'labels' => $labels,
		'hierarchical' => true,
	);
	register_taxonomy( 'portfolio_category', 'portfolio', $args );
}
add_action( 'init', 'my_taxonomies_portfolio', 0 );

//metabox-custom script

function rdn_metabox() {
    wp_enqueue_script('custom-meta-boxes', plugins_url( '/js/custom-meta-boxes.js' , __FILE__ ) , array('jquery'),'', true);
}    

add_action('admin_enqueue_scripts', 'rdn_metabox');

//add Post Details metabox
add_action( 'add_meta_boxes', 'sgn_metabox_add' );
function sgn_metabox_add()
{
    add_meta_box( 'portfolio-details', 'Portfolio Details', 'metabox_portfolio_form', 'portfolio', 'normal', 'high' );
}

function metabox_portfolio_form( $post )
{
    $values = get_post_custom( $post->ID );
	$clientname = isset( $values['client_name'] ) ? esc_attr( $values['client_name'][0] ) : '';
	$btn_name = isset( $values['btn_name'] ) ? esc_attr( $values['btn_name'][0] ) : '';
	$btn_link = isset( $values['btn_link'] ) ? esc_attr( $values['btn_link'][0] ) : '';
    $gallery_sport = isset( $values['gallery_port'] ) ? esc_attr( $values['gallery_port'][0] ) : '';
	$embed_link = isset( $values['embed_post'] ) ? esc_attr( $values['embed_post'][0] ) : '';
    wp_nonce_field( 'my_meta_box_nonce', 'meta_box_nonce' );
?>
<p id="client-name" >
    <label for="client_name">Client Name</label><br/>
    <input type="text" name="client_name" id="client_name" value="<?php echo $clientname; ?>" style="width:100%;"/><br/>
    <small>Enter Client Name here</small>
</p>
<p id="btn-name" >
    <label for="btn_name">Button Text</label><br/>
    <input type="text" name="btn_name" id="btn_name" value="<?php echo $btn_name; ?>" style="width:100%;"/><br/>
    <small>Enter Text for the button at the bottom. Leave this blank if you dont want the button.</small>
</p>
<p id="btn-link" >
    <label for="btn_name">Button Link</label><br/>
    <input type="text" name="btn_link" id="btn_link" value="<?php echo $btn_link; ?>" style="width:100%;"/><br/>
    <small>Enter Link for the button at the bottom. Leave this blank if you dont want the button.</small>
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
    <small>Insert the link for video/podcast here. <br />For video from youtube/vimeo just put the link without any attribute like  ?wmode=opaque.<br /> eg: http://www.youtube.com/embed/nAuo7KEQHT4 </small>
</p>

<?php }

add_action( 'save_post', 'cd_meta_box_save' );
function cd_meta_box_save( $post_id )
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
  if( isset( $_POST['client_name'] ) )
 update_post_meta( $post_id, 'client_name', wp_kses( $_POST['client_name'], $allowed ) );
 if( isset( $_POST['embed_post'] ) )
 update_post_meta( $post_id, 'embed_post', wp_kses( $_POST['embed_post'], $allowed ) );
 if( isset( $_POST['gallery_port'] ) )
 update_post_meta( $post_id, 'gallery_port', wp_kses( $_POST['gallery_port'], $allowed ) );
 if( isset( $_POST['btn_link'] ) )
 update_post_meta( $post_id, 'btn_link', wp_kses( $_POST['btn_link'], $allowed ) );
 if( isset( $_POST['btn_name'] ) )
 update_post_meta( $post_id, 'btn_name', wp_kses( $_POST['btn_name'], $allowed ) );
}