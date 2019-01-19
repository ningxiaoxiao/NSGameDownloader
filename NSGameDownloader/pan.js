function getbdstoken() {
    return self.yunData.MYBDSTOKEN;
}

function getList(u) {
    $.ajax({
        url: u,
        success: function (res) {
            $("#ajaxres").text(JSON.stringify(res));
        }
    });

}

function init() {
    var holdyDiv = $('body').append('div');
    $(holdyDiv).attr('id', 'ajaxres');
   // $(holdyDiv).css('visibility', "hidden");

}