<?php
// Registers the new post type and taxonomy

function wpt_pricing_table_posttype() {
	register_post_type( 'pricing-table',
		array(
			'labels' => array(
				'name' => __( 'Pricing Tables' ),
				'singular_name' => __( 'Pricing Table' ),
				'add_new' => __( 'Add New Pricing Table' ),
				'add_new_item' => __( 'Add New Pricing Table' ),
				'edit_item' => __( 'Edit Pricing Table' ),
				'new_item' => __( 'Add New Pricing Table' ),
				'view_item' => __( 'View Pricing Table' ),
				'search_items' => __( 'Search Pricing Tables' ),
				'not_found' => __( 'No Pricing Table found' ),
				'not_found_in_trash' => __( 'No Pricing Table found in trash' )
			),
			'public' => true,
			'supports' => array( 'title', 'editor', 'thumbnail', 'comments', 'post-formats' , 'excerpt'),
			'capability_type' => 'post',
			'rewrite' => array("slug" => "pricing-table"), // Permalinks format
			'menu_position' => 5,
			'register_meta_box_cb' => 'pricing_table_metabox_add',
			'exclude_from_search' => true 
		)
	);

}

add_action( 'init', 'wpt_pricing_table_posttype' );




//add Post Details metabox
add_action( 'add_meta_boxes', 'pricing_table_metabox_add' );
function pricing_table_metabox_add()
{
    add_meta_box( 'pricing-table-details', 'Pricing Table Details', 'metabox_pricing_table_form', 'pricing-table', 'normal', 'high' );
}

function metabox_pricing_table_form( $post )
{
    $values = get_post_custom( $post->ID );
	$pricing_icon = isset( $values['pricing_icon'] ) ? esc_attr( $values['pricing_icon'][0] ) : '';
	$pricing_price = isset( $values['pricing_price'] ) ? esc_attr( $values['pricing_price'][0] ) : '';
	$pricing_button = isset( $values['pricing_button'] ) ? esc_attr( $values['pricing_button'][0] ) : '';
	$pricing_link = isset( $values['pricing_link'] ) ? esc_attr( $values['pricing_link'][0] ) : '';
    wp_nonce_field( 'my_meta_box_nonce', 'meta_box_nonce' );
?>

<p id="pricing-icon" >
    <label for="pricing_icon">Input Your Pricing Table Icon Here</label><br/>
    <input type="text" name="pricing_icon" id="pricing_icon" value="<?php echo $pricing_icon; ?>" style="width:100%;"/><br/>
    <small>Enter the icon class here. eg: fa-envelope . You can check this <a href="http://fortawesome.github.io/Font-Awesome/icons/" target="_blank">page</a> for icon list.</small>
</p>
<p id="pricing-price" >
    <label for="pricing_price">Input Your Price Here</label><br/>
    <input type="text" name="pricing_price" id="pricing_price" value="<?php echo $pricing_price; ?>" style="width:100%;"/><br/>
    <small>Enter the price here. eg: $30/month</small>
</p>
<p id="pricing-button-text" >
    <label for="pricing_button">Input Your Button Text Here</label><br/>
    <input type="text" name="pricing_button" id="pricing_button" value="<?php echo $pricing_button; ?>" style="width:100%;"/><br/>
    <small>Enter the pricing button text here. eg: Order Now</small>
</p>
<p id="pricing-button-link" >
    <label for="pricing_link">Input Your Button Link Here</label><br/>
    <input type="text" name="pricing_link" id="pricing_link" value="<?php echo $pricing_link; ?>" style="width:100%;"/><br/>
    <small>Enter the pricing button link here.</small>
</p>

<?php }

add_action( 'save_post', 'pricing_table_meta_box_save' );
function pricing_table_meta_box_save( $post_id )
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
 if( isset( $_POST['pricing_icon'] ) )
 update_post_meta( $post_id, 'pricing_icon', wp_kses( $_POST['pricing_icon'], $allowed ) );
 if( isset( $_POST['pricing_price'] ) )
 update_post_meta( $post_id, 'pricing_price', wp_kses( $_POST['pricing_price'], $allowed ) );
 if( isset( $_POST['pricing_button'] ) )
 update_post_meta( $post_id, 'pricing_button', wp_kses( $_POST['pricing_button'], $allowed ) );
 if( isset( $_POST['pricing_link'] ) )
 update_post_meta( $post_id, 'pricing_link', wp_kses( $_POST['pricing_link'], $allowed ) );
}


//Pricing Table Type metabox
add_action( 'add_meta_boxes', 'pricing_meta_box' );

function pricing_meta_box($post){
    add_meta_box('pricing_meta_box', 'Pricing Table Type', 'pricing_table_type_meta_box', 'pricing-table', 'normal' , 'high');
}

add_action('save_post', 'pricing_type_save_metabox');

function pricing_type_save_metabox(){ 
    global $post;
    if(isset($_POST["pricing_table_type"])){
         //UPDATE: 
        $pricing_tables = $_POST['pricing_table_type'];
        //END OF UPDATE

        update_post_meta($post->ID, 'pricing_table_type_meta_box', $pricing_tables);
        //print_r($_POST);
    }
}

function pricing_table_type_meta_box($post){
    $pricing_tables = get_post_meta($post->ID, 'pricing_table_type_meta_box', true); //true ensures you get just one value instead of an array
    ?>   
    <label>Choose the size of the element :  </label>

    <select name="pricing_table_type" id="pricing_table_type">
      <option value="light" <?php selected( $pricing_tables, 'light' ); ?>>Default (Light) Pricing Table</option>
      <option value="dark" <?php selected( $pricing_tables, 'dark' ); ?>>Special(Dark) Pricing Table</option>
    </select>
    <?php
}