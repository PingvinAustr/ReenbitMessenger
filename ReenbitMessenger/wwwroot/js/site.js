
function getCookie(name) {
    // Split cookie string and get all individual name=value pairs in an array
    var cookieArr = document.cookie.split(";");

    // Loop through the array elements
    for (var i = 0; i < cookieArr.length; i++) {
        var cookiePair = cookieArr[i].split("=");

        /* Removing whitespace at the beginning of the cookie name
        and compare it with the given string */
        if (name == cookiePair[0].trim()) {
            // Decode the cookie value and return
            return decodeURIComponent(cookiePair[1]);
        }
    }

    // Return null if not found
    return null;
}

function OpenChat(id) {
    var url = "?handler=OpenChat";
    return $.ajax({
        type: "POST",
        url: url,
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        data: {
            chat_id: id          
        },
        //contentType: "application/json;",
        dataType: "json",
        success: function (response) {
            console.log("Successfully retrieved messages from chat via ajax:");
            console.log(response);
            LoadChat(response);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });

}


function LoadChat(response) {
    
    let chat_window = document.getElementById("chat_window");
    let num_of_messages = Object.keys(response).length;

   //Clear current chat window
    chat_window.innerHTML = "";

    for (let msg_object in response) {
        let message_id = response[msg_object]["id"];
        let message_author_id = response[msg_object]["user"]["id"];
        let message_text = response[msg_object]["messageText"];
        let message_author_name = response[msg_object]["user"]["name"];
        let message_author_surname = response[msg_object]["user"]["surname"];

        //alert(parseInt(getCookie("Current_user_id")) == parseInt(message_author_id));
        //alert("[" + getCookie("Current_user_id") + "]" + "[" + message_author_id + "]");
     
        if (parseInt(getCookie("Current_user_id")) == parseInt(message_author_id)) {
            chat_window.innerHTML += "<div class='right-part-chat-window-row right-part-chat-window-row-reverse'>" + message_text + "</div>";
        }
        else
            chat_window.innerHTML += "<div class='right-part-chat-window-row'>" +message_text + "</div>";
    }
}