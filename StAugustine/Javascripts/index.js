jQuery("#txtSearchCritera").keyup(function (e) {

    var code = e.which; // recommended to use e.which, it's normalized across browsers
    if (code == 13) e.preventDefault();
    if (code == 186 || code == 187 || code == 188 || code == 189 || code == 191 || code == 192 || code == 220 || code == 222) {
        var backspace = $('#searchCritera').val();

        alert("Invalid Character.");
        $('#searchCritera').val(backspace.substring(0, backspace.length - 1));
        return;
    }
    if (code == 13) {
        searchClick();
    } // missing closing if brace

});

function searchClick() {
    if (jQuery("#txtSearchCritera").val() != "") {
        window.open('StAuDC.htm?id=usach&critera=' + jQuery("#txtSearchCritera").val());
    }
    else
        {
        var url = 'StAuDC.htm?id=usach';
        window.open(url);
    }
}