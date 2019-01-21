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

//打通baidu api ajax
function api(u,postdata) {
     $.ajax({
        url: u,
        type:postdata?'POST':'GET',
        data:postdata?postdata:null,
        success: function (res) {
            $("#ajaxres").text(JSON.stringify(res));
        },
    });
}


//初始结果元素
function init() {
    var res = $("<p></p>").attr('id', 'ajaxres'); 
    //res.css('visibility', "hidden");   
    $('body').append(res);
}