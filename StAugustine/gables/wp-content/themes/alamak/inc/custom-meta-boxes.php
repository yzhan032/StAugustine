<?php


//metabox-custom script

function rdn_metabox_post() {
	wp_enqueue_script('custom-metabox', get_template_directory_uri() . '/inc/js/custom-meta-boxes.js', array('jquery'),'', true);	
}    
add_action('admin_enqueue_scripts', 'rdn_metabox_post');




//add Post Details metabox
add_action( 'add_meta_boxes', 'cd_meta_box_add' );
function cd_meta_box_add()
{
    add_meta_box( 'post-details', 'Post Details', 'cd_meta_box_cb', 'post', 'normal', 'high' );
}

function cd_meta_box_cb( $post )
{
    $values = get_post_custom( $post->ID );
    $link_url = isset( $values['link_post_url'] ) ? esc_attr( $values['link_post_url'][0] ) : '';
    $quote_nauthor = isset( $values['quote_author'] ) ? esc_attr( $values['quote_author'][0] ) : '';
    $embed_link = isset( $values['embed_post'] ) ? esc_attr( $values['embed_post'][0] ) : '';
    $gallery_scode = isset( $values['gallery_post'] ) ? esc_attr( $values['gallery_post'][0] ) : '';
    wp_nonce_field( 'my_meta_box_nonce', 'meta_box_nonce' );
?>
<p id="link-post-url" style="display:none;">
    <label for="link_post_url">Link URL</label><br/>
    <input type="text" name="link_post_url" id="link_post_url" value="<?php echo $link_url; ?>" style="width:100%;"/><br/>
    <small>Enter the URL to be used for this Link post.</small>
</p>

<p id="quote-code" style="display:none;">
    <label for="quote_author">Author Quote</label><br/>
    <input type="text" name="quote_author" id="quote_author" value="<?php echo $quote_nauthor; ?>" style="width:100%;"/><br/>
    <small>Enter the author for the quote.</small>
</p>
<p id="gallery-code" style="display:none;">
    <label for="gallery_post">Insert Gallery shortcode here</label><br/>
    <textarea name="gallery_post" id="gallery_post" style="width:100%;" rows="5"><?php echo $gallery_scode; ?></textarea><br/>
    <small>Cut/Copy the gallery shortcode that created by wordpress in content area into here. eg. [gallery size="large" columns="1" link="file" ids="98,97,45,43,96" orderby="rand"]
    <br/>**dont forget to add <b>size="large" link="file"</b> inside the gallery shortcode!</small>
</p>

<p id="embed-post-code" style="display:none;">
    <label for="embed_post">Insert Link Video/Audio Here</label><br/>
    <textarea name="embed_post" id="embed_post" style="width:100%;" rows="5"><?php echo $embed_link; ?></textarea><br/>
    <small>Insert the link for video/podcast here.<br />
    For video from youtube/vimeo just put the link without any attribute like ?wmode=opaque.<br />
	eg: http://www.youtube.com/embed/nAuo7KEQHT4 </small>
</p>

<?php }

add_action( 'save_post', 'post_metabox_save' );
function post_metabox_save( $post_id )
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
 if( isset( $_POST['link_post_url'] ) )
 update_post_meta( $post_id, 'link_post_url', wp_kses( $_POST['link_post_url'], $allowed ) );
 if( isset( $_POST['quote_author'] ) )
 update_post_meta( $post_id, 'quote_author', wp_kses( $_POST['quote_author'], $allowed ) );
 if( isset( $_POST['embed_post'] ) )
 update_post_meta( $post_id, 'embed_post', wp_kses( $_POST['embed_post'], $allowed ) );
 if( isset( $_POST['gallery_post'] ) )
 update_post_meta( $post_id, 'gallery_post', wp_kses( $_POST['gallery_post'], $allowed ) );
}

