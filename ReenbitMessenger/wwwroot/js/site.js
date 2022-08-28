var opened_chat_id = 0;
var editing_mode_on = false;
var editing_message_id = 0;
var reply_message_id = 0;
var num_pages = "";

const interval = setInterval(function () {
    // method to be executed;
    if (opened_chat_id!=0) OpenChat(opened_chat_id);
    console.log("timer tick");
}, 5000);



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

function NumOfMessagesInChat(id) {
   
    var url = "?handler=PaginationFunc";
    return $.ajax({
        type: "POST",
        url: url,
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        data: {
            chat_id: id
        },
        //contentType: "application/json;",
        dataType: "text",
        success: function (response) {
            console.log("Successfully retrieved NUM of messages from chat via ajax:");
            num_pages = String(response);
            //console.log("[" + num + "]");
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
  
}

async function OpenChat(id,page_to_open) {


    

    await NumOfMessagesInChat(id);

    
    //alert("[" + num_pages + "]");
    let num_pagination = Math.ceil(parseInt(num_pages) / 5);
    if (num_pagination == 0) num_pagination = 1;
    
    
    opened_chat_id = id;
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
            LoadChat(response, num_pagination);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
    

}


function LoadChat(response,num_pagination) {

    //alert("!" + num_pagination);


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
        let message_reply_to = null;
        if (response[msg_object]["reply"] != null) message_reply_to = response[msg_object]["reply"]["messageText"];
        else message_reply_to = null;
       

        let reply_string = "";
        if (message_reply_to != null) {
            reply_string = "RE: " + message_reply_to;
        }

        //alert(parseInt(getCookie("Current_user_id")) == parseInt(message_author_id));
        //alert("[" + getCookie("Current_user_id") + "]" + "[" + message_author_id + "]");
     
        if (parseInt(getCookie("Current_user_id")) == parseInt(message_author_id)) {
            chat_window.innerHTML += "<div id='" + message_id + "' class='right-part-chat-window-row right-part-chat-window-row-reverse'><span id='message_" + message_id + "'> " + message_text + "</span ><span onclick='EditingMode(" + message_id + ")'> Edit</span > <span>DeleteMe</span> <span onclick='DeleteMessage(" + message_id + ")'" + " > DeleteAll</span >" + reply_string+"</div > ";
        }
        else
            chat_window.innerHTML += "<div id='" + message_id + "' class='right-part-chat-window-row' > " + message_text + "<span onclick='ReplyToMessage(" + message_id + ")'>Reply</span>" + reply_string+"</div > ";
    }

    let pagination_div = "";
    for (i = 1; i <= num_pagination; i++) {
        pagination_div+="<span>"+i+"</span>"
    }

    let father_pagination = document.createElement("div");
    father_pagination.innerHTML = pagination_div;
    father_pagination.classList.add("pagination_div");
    
   

    //let html_pagination_div = $(pagination_div);
    document.getElementById("chat_window").appendChild(father_pagination);
    //alert(pagination_div);

}

function EditingMode(msg_id) {
    editing_mode_on = true;
    editing_message_id = msg_id;
    document.getElementById("input_message").value = document.getElementById("message_" + msg_id).innerHTML;
    
}

function SendMessage(msg_id,reply_mode) {
    message_text = document.getElementById("input_message").value;
    if (editing_mode_on == false && reply_message_id == 0) {

        var url = "?handler=SendMessage";
        return $.ajax({
            type: "POST",
            url: url,
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                msg: message_text,
                author_id: getCookie("Current_user_id"),
                chat_id: opened_chat_id
            },
            //contentType: "application/json;",
            dataType: "json",
            success: function (response) {
                console.log("Successfully sent message via ajax:");
                console.log(response);
                document.getElementById("input_message").value = "";
                OpenChat(opened_chat_id);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    }
    else if (editing_mode_on == true && reply_message_id == 0) {
        var url = "?handler=EditMessage";
        return $.ajax({
            type: "POST",
            url: url,
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                msg_id: editing_message_id,
                msg_new_text: message_text
            },
            //contentType: "application/json;",
            dataType: "json",
            success: function (response) {
                console.log("Successfully EDITED message via ajax:");
                console.log(response);
                document.getElementById("input_message").value = "";
                editing_message_id = null;
                editing_mode_on = false;
                OpenChat(opened_chat_id);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    }
    else if (editing_mode_on == false && reply_message_id != 0) {
        var url = "?handler=ReplyToMessage";
        return $.ajax({
            type: "POST",
            url: url,
            headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
            data: {
                msg_id: reply_message_id,
                msg_text: message_text,
                author_id: getCookie("Current_user_id")
            },
            //contentType: "application/json;",
            dataType: "json",
            success: function (response) {
                console.log("Successfully REPLIED TO message via ajax:");
                console.log(response);
                reply_message_id = 0;
                document.getElementById("input_message").value = "";
                OpenChat(opened_chat_id);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    }
   
}


function DeleteMessage(message_id) {
    var url = "?handler=DeleteMessage";
    return $.ajax({
        type: "POST",
        url: url,
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        data: {
            msg_id: message_id,           
        },
        //contentType: "application/json;",
        dataType: "json",
        success: function (response) {
            console.log("Successfully delete message for ALL via ajax:");
            console.log(response);
            OpenChat(opened_chat_id);

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
}

function ReplyToMessage(message_id) {
    alert("Please enter reply text to input field and press the button!");
    reply_message_id = message_id;
}