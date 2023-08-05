///Checks for comments and rating
var addCommentBtn = document.getElementById('addCommentBtn');
var addRatingBtn = document.getElementById('addRatingBtn');
var commentTextArea = document.getElementById('commentTextArea');

/// checks for send message (mails)
var sentMsgBtn = document.getElementById('sentMsgBtn');
var SendMsgTextBox = document.getElementById('SendMsgTextBox');
var SendMsgTitleBox = document.getElementById('SendMsgTitleBox');


commentTextArea.addEventListener("keyup", ActivateAddCommentBtn);
SendMsgTextBox.addEventListener("keyup", ActivateSendMsgBtn);
SendMsgTitleBox.addEventListener("keyup", ActivateSendMsgBtn);


function ActivateAddCommentBtn() {

    if (commentTextArea.value === "") {
        addCommentBtn.disabled = true;
        $("#addCommentBtn").removeClass('btn-primary');
        $("#addCommentBtn").addClass('btn-light');
    }
    else {
        addCommentBtn.disabled = false;
        $("#addCommentBtn").removeClass('btn-light');
        $("#addCommentBtn").addClass('btn-primary');
    }
}



function ActivateSendMsgBtn() {

    if ((SendMsgTextBox.value === "") || (SendMsgTitleBox.value === "")) {
        sentMsgBtn.disabled = true;
        $("#sentMsgBtn").removeClass('btn-primary');
        $("#sentMsgBtn").addClass('btn-light');
    }
    else {
        sentMsgBtn.disabled = false;
        $("#sentMsgBtn").removeClass('btn-light');
        $("#sentMsgBtn").addClass('btn-primary');
    }
}



function ActivateRatingCommentBtn() {

    addRatingBtn.disabled = false;

    $("#addRatingBtn").removeClass('btn-light');
    $("#addRatingBtn").addClass('btn-primary');
}