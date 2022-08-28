﻿var opened_chat_id = 0;
var editing_mode_on = false;
var editing_message_id = 0;
var reply_message_id = 0;
var num_pages = "";
var msg_per_page = 5;
var displayed_num_of_msg = -1;
var current_page;

const interval = setInterval(function () {
    // method to be executed;
    if (opened_chat_id != 0) {
        console.log("interval_current_page" + " " + current_page);
        console.log("interval_opened_chat" + " " + opened_chat_id);
        OpenChat(opened_chat_id, current_page);
        
    }
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

async function OpenChat(id, page_to_open) {
    //alert("OpenChat:" + page_to_open);
    if (page_to_open == null) page_to_open = -1;
    console.log("OpenChat,page_to_open:"+page_to_open);
  
    current_page = page_to_open;

     

   
    await NumOfMessagesInChat(id);

    
    //alert("[" + num_pages + "]");
    let num_pagination = Math.ceil(parseInt(num_pages) / msg_per_page);
    if (num_pagination == 0) num_pagination = 1;
    
    
    opened_chat_id = id;
    var url = "?handler=OpenChat";
    return $.ajax({
        type: "POST",
        url: url,
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        data: {
            chat_id: id,
            page_to_open: page_to_open
        },
        //contentType: "application/json;",
        dataType: "json",
        success: function (response) {
            console.log("Successfully retrieved messages from chat via ajax:");
            console.log(response);
            if (current_page == -1) current_page = num_pagination;
            //alert("current_page_after-1_request" + current_page);
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
    displayed_num_of_msg = response.length;
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
        let test = "OpenChat(" + opened_chat_id + "," + (i) + ")";
        pagination_div += "<span onclick='"+test+"'>" + i + "</span>"
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
                if (displayed_num_of_msg == msg_per_page) {
                    //alert("SendMessage_A" + displayed_num_of_msg + "___" + msg_per_page);
                    OpenChat(opened_chat_id, current_page + 1);
                }
                else {
                    //alert("SendMessage_B" + displayed_num_of_msg + "___" + msg_per_page);
                    OpenChat(opened_chat_id, current_page);
                }
               

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
                OpenChat(opened_chat_id, current_page);

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
                if (displayed_num_of_msg == msg_per_page) {
                    //alert("A" + displayed_num_of_msg);
                    OpenChat(opened_chat_id, current_page + 1);
                }
                else {
                    //alert("B" + displayed_num_of_msg);
                    OpenChat(opened_chat_id, current_page);
                }

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
            if (displayed_num_of_msg == 1) { OpenChat(opened_chat_id, current_page - 1); }
            else { OpenChat(opened_chat_id, current_page); }

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

var burger_opened = false;
function BurgerCheck() {
    let hide_show1 = document.querySelectorAll(".chat-item-right");
    let hide_show2 = document.querySelectorAll(".chat-item-left");

    if (burger_opened) {
        hide_show1.forEach(item => {
            item.classList.add("hidden");
        });
        hide_show2.forEach(item => {
            item.style.width = "auto";
        });

        document.getElementById("left_part").style.width = "15vw";
        //document.getElementById("right_part").style.width = "85vw";

        burger_opened = false;
    }
    else {
        hide_show1.forEach(item => {
            item.classList.remove("hidden");
        });
        hide_show2.forEach(item => {
            item.style.width = "calc(25% - 2px);";
        });
        document.getElementById("left_part").style.width = "45vw";
        //document.getElementById("right_part").style.width = "55vw";
        burger_opened = true;
    }
}