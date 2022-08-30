//Global variables for corresponding functions
var opened_chat_id = 0;
var editing_mode_on = false;
var editing_message_id = 0;
var reply_message_id = 0;
var num_pages = "";
var msg_per_page = 20;
var displayed_num_of_msg = -1;
var current_page;
var IsTimerPaused = false;
var avoid500 = false;


//Onload function (it is used for opening of new DM chat after user chooses this option in the popup)
window.onload = function () {
    if (String(getCookie("ChatToInstantOpen")) != "null") {
        avoid500 = true;
        OpenChat(getCookie("ChatToInstantOpen"));
    }
    //console.log(avoid500);
}


//Function that refreshes the chat every 5s (to load new messages from opponents)
const interval = setInterval(function () {
  
    if (!IsTimerPaused) {
        if (opened_chat_id != 0) {           
            OpenChat(opened_chat_id, current_page);

        }
        //console.log("timer tick");
    }
}, 5000);


//Function that returns cookie value by name
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


//AJAX  function that returns num of pages on chat
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
            
            num_pages = String(response);
           
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
  
}



//AJAX function that opens chat by id and needed pagination page
async function OpenChat(id, page_to_open) {
    if (page_to_open == null) page_to_open = -1;
    //console.log("OpenChat,page_to_open:"+page_to_open); 
    current_page = page_to_open; 
    await NumOfMessagesInChat(id);

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
            //console.log("Successfully retrieved messages from chat via ajax:");
            //console.log(response);
            if (current_page == -1) current_page = num_pagination;
          
            if (!avoid500) LoadChat(response, num_pagination);
            else {
                LoadChat(response, num_pagination);
                avoid500 = true;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
    

}


//AJAX function that loads chat messages
async function LoadChat(response,num_pagination) {

    document.getElementById("write-tools-block").style.display = "flex";
    let chat_name = "";
    let num_of_users = 0;


    
    let chat_window = document.getElementById("chat_window");
    let num_of_messages = Object.keys(response).length;

   //Clear current chat window
    chat_window.innerHTML = "";
    displayed_num_of_msg = response.length;
    for (let msg_object in response) {
        chat_name = response[msg_object]["chats"]["chatName"];
        let message_id = response[msg_object]["id"];
        let message_author_id = response[msg_object]["user"]["id"];
        let message_text = response[msg_object]["messageText"];
        let message_author_name = response[msg_object]["user"]["name"];
        let message_author_surname = response[msg_object]["user"]["surname"];

        
        let message_reply_to = null;
        let time_sent = response[msg_object]["time_sent"];
        time_sent = String(time_sent).replace("T", " ");
        time_sent = String(time_sent).substr(0, time_sent.length - 3);
  
        if (time_sent.length > 16) time_sent = String(time_sent).substr(0, 16);

        //console.log(time_sent);
        let author_img = response[msg_object]["user"]["userAvatarImage"];
        if (response[msg_object]["reply"] != null) message_reply_to = response[msg_object]["reply"]["messageText"];
        else message_reply_to = null;
       

        let reply_string = "";
        if (message_reply_to != null) {
            reply_string = "RE: <div style='display:inline-block; color: gray'>" + message_reply_to+"</div>";
        }

  
        if (parseInt(getCookie("Current_user_id")) == parseInt(message_author_id)) {     //"<div class='my_spans'> <span onclick='EditingMode(" + message_id + ")'> Edit</span > <span>DeleteMe</span> <span onclick='DeleteMessage(" + message_id + ")'" + " > DeleteAll</span ></div>"
            let img = "<div class='small_img_container'><img class='small_img' src='../user_avatars/" + author_img + "'/></div>";
            let myspans = "<div class='my_spans'> <span onclick='EditingMode(" + message_id + ")'> Edit</span >  <span onclick='DeleteMessage(" + message_id + ")'" + " > Delete</span ></div>";

            chat_window.innerHTML += "<div id='" + message_id + "' class='right-part-chat-window-row right-part-chat-window-row-reverse'><div class='msg_wrap'><div id='message_body" + message_id+"' class='message_body' > " + "<div class='msg_text' id='message_" + message_id + "'>" + message_text + "</div>" + reply_string + "</div>" + myspans + "<div class='time-sent'>" + time_sent + "</div></div> " + img + "</div>";
        }
        else {
            let func = "OpenAuthorPopup(" + parseInt(message_author_id) + ",'" + author_img + "','" + message_author_name + "','" + message_author_surname+"')";
            let img = "<div class='small_img_container'><img onclick="+func+" class='small_img' src='../user_avatars/" + author_img + "'/></div>";
            chat_window.innerHTML += "<div id='" + message_id + "' class='right-part-chat-window-row' >" + img + "<div class='msg_wrap'> <div id='message_body" + message_id+"' class='message_body message_body_reverse'>  <div id='message_" + message_id + "' class='message_text'>" + message_text + "</div>" + reply_string + "</div>" + "<div class='reply'><span onclick='ReplyToMessage(" + message_id + ")'>Reply</span></div>" + "<div class='time-sent time-sent-reverse'>" + time_sent + "</div></div></div> ";
        }
        }

    if (num_pagination > 1) {
        let pagination_div = "<div style='color:gray'>Pages:</div>";
        for (i = 1; i <= num_pagination; i++) {
            let test = "OpenChat(" + opened_chat_id + "," + (i) + ")";
            pagination_div += "<span class='pagination_span' onclick='" + test + "'>" + i + "</span>"
        }


        let father_pagination = document.createElement("div");
        father_pagination.innerHTML = pagination_div;
        father_pagination.classList.add("pagination_div");
        document.getElementById("chat_window").appendChild(father_pagination);
    }
   

  

    

    
    await GetNumOfChatUsers();
    document.cookie = 'ChatToInstantOpen=' + null + '; path=/';
    if (avoid500) avoid500 = false;

}


//Function that opens popup that allows to create new DM room
function OpenAuthorPopup(author_id, author_img,author_name,author_surname) {    
    let popup = document.createElement("div");
    popup.classList.add("author_dm_popup");
    document.getElementById("chat_window").appendChild(popup);
    IsTimerPaused = true;

    let cross = document.createElement('div');
    cross.classList.add("close");
    cross.addEventListener("click", function () {
        popup.remove();
        IsTimerPaused = false;
    });
    popup.appendChild(cross);

    let father_block = document.createElement("div");
    father_block.classList.add("popup_father");
    popup.appendChild(father_block);


    let name_row = document.createElement("div");
    name_row.classList.add("name_row");
    father_block.appendChild(name_row);

    let avatar = document.createElement("img");
    avatar.classList.add("popup_image");
    avatar.src = "../user_avatars/" + author_img;
    name_row.appendChild(avatar);

    let flex_parent = document.createElement("div");
    flex_parent.classList.add("flex_parent");
    name_row.appendChild(flex_parent);


    author_name = " " + author_name;
    author_surname = " " + author_surname;
    let name_block = document.createElement("div");
    name_block.classList.add("popup_text");
    name_block.innerHTML = "<div class='popup_span'> Name: </div>" +author_name;
    flex_parent.appendChild(name_block);

    let surname_block = document.createElement("div");
    surname_block.classList.add("popup_text");
    surname_block.innerHTML = "<div class='popup_span'> Surname: </div>" + author_surname;
    flex_parent.appendChild(surname_block);

  
    let button_block = document.createElement("div");
    button_block.classList.add("popup_button");
    button_block.innerHTML = "Send DM";
    button_block.addEventListener("click", function () { SendDM(author_id) });
    name_row.appendChild(button_block);
    

    
}

//AJAX function that allows to send DM (create new DM chat)
function SendDM(opponent_id) {
  

    var url = "?handler=SendDM";
    $.ajax({
        type: "POST",
        url: url,
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        data: {
            opponent_id: opponent_id
        },
        //contentType: "application/json;",
        dataType: "text",
        success: function (response) {
            let num = "";
            
            for (let item in response) {
                num += response[item];
            }
            document.cookie = 'ChatToInstantOpen='+num+'; path=/';
           
           GetRequest(num);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });

}

//AJAX Get request to update page
function GetRequest(chat_to_open) {
    var url = "";
    $.ajax({
        type: "GET",
        url: url,
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        data: {
            id: getCookie("Current_user_id")
        },
        //contentType: "application/json;",
        dataType: "text",
        success: function (response) {
            
            document.location.reload();
            
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
}

//AJAX function to get number of chat users
function GetNumOfChatUsers() {
    var url = "?handler=GetNumOfChatUsers";
    $.ajax({
        type: "POST",
        url: url,
        headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        data: {
            chat_id: opened_chat_id
        },
        //contentType: "application/json;",
        dataType: "json",
        success: function (response) {
           
            let info = String(response);

            let chat_name_block = document.getElementById("chat_name_image");            
            chat_name_block.innerHTML = "";


            let chatNameBlock = document.createElement("div");
            chatNameBlock.innerHTML = response[2];
            chatNameBlock.classList.add("chat_name_text");

            let chatImgBlock = document.createElement("img");
            chatImgBlock.src = "../user_avatars/" + response[1];
            chatImgBlock.classList.add("profile_image");

            chat_name_block.appendChild(chatImgBlock);
            chat_name_block.appendChild(chatNameBlock);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
}

//Function that turns on layout editing mode
function EditingMode(msg_id) {

    if (reply_message_id == 0) {
        editing_mode_on = true;
        editing_message_id = msg_id;
        document.getElementById("input_message").value = document.getElementById("message_" + msg_id).innerHTML;
        document.getElementById("write-tools-block").style.border = "2px solid red";
        document.getElementById("send_button").innerHTML = "Edit";
    }
}


// Adding option to send messages with "Enter" button
var input = document.getElementById("input_message");

// Execute a function when the user presses a key on the keyboard
input.addEventListener("keypress", function (event) {
    // If the user presses the "Enter" key on the keyboard
    if (event.key === "Enter") {
        // Cancel the default action, if needed
        event.preventDefault();
        // Trigger the button element with a click
        document.getElementById("send_button").click();
    }
});




//AJAX function to send messages. It has 3 modes - regular, reply mode, editing mode. 
function SendMessage(msg_id,reply_mode) {
    message_text = document.getElementById("input_message").value;
    if (message_text != "") {
        document.getElementById("input_message").placeholder = "Start typing...";

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
                    //console.log("Successfully sent message via ajax:");
                    //console.log(response);
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
                    
                    //console.log("Successfully EDITED message via ajax:");
                    //console.log(response);
                    document.getElementById("input_message").value = "";
                    editing_message_id = null;
                    editing_mode_on = false;

                    document.getElementById("write-tools-block").style.border = "";
                    document.getElementById("send_button").innerHTML = "Send";

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
                   
                    //console.log("Successfully REPLIED TO message via ajax:");
                    //console.log(response);
                    reply_message_id = 0;


                    document.getElementById("rep_block").remove();
                    document.getElementById("send_button").innerHTML = "Send";
                    document.querySelectorAll("right-part-write-tools").style = "justify-content:center";

                    document.getElementById("input_message").value = "";
                    if (displayed_num_of_msg == msg_per_page) {
                        //alert("A" + displayed_num_of_msg);
                        OpenChat(opened_chat_id, current_page + 1);
                    }
                    else {
                        //alert("B" + displayed_num_of_msg);
                        OpenChat(opened_chat_id, current_page);
                    }
                    IsTimerPaused = false;
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });
        }
    }
    else {
        document.getElementById("input_message").placeholder = "Please enter something!";
    }
   
}

//AJAX function to delete messages
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
            //console.log("Successfully delete message for ALL via ajax:");
            //console.log(response);
            if (displayed_num_of_msg == 1) { OpenChat(opened_chat_id, current_page - 1); }
            else { OpenChat(opened_chat_id, current_page); }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
}

//Function to create reply to message layout
function ReplyToMessage(message_id) {

    if (editing_mode_on == false) {
        //alert("Please enter reply text to input field and press the button!");
        let block = document.getElementById("right-part-write-tools");
        let rep_block = document.createElement("div");
        rep_block.classList.add('reply_notification');
        rep_block.id = "rep_block";
        let string = document.getElementById("message_" + message_id).innerHTML;
        if (string.length >= 75) string = string.substr(0, 75)+"...";
        rep_block.innerHTML = "<div>Reply to message: <span class='reply_msg'>" + string + "</span></div>";

        document.getElementById("message_body" + message_id).classList.add("reply_anim");
        IsTimerPaused = true;

        document.getElementById("send_button").innerHTML = "Reply";

        document.querySelectorAll("right-part-write-tools").style = "justify-content:flex-start";

        block.insertBefore(rep_block, document.getElementById("write-tools-block"));

        reply_message_id = message_id;

        let cancel = document.createElement("div");
        cancel.classList.add("cancel_cross");
        cancel.innerHTML = "&#10006";
        cancel.addEventListener("click", function () {
            rep_block.remove();
            document.getElementById("send_button").innerHTML = "Send";
            reply_message_id = 0;
            document.querySelectorAll("right-part-write-tools").style = "justify-content:center";
            document.getElementById("message_body" + message_id).classList.remove("reply_anim");
            IsTimerPaused = false;

        });
        rep_block.appendChild(cancel);


    }
    
}


//Function to open/close burger menu
var burger_opened = true;
function BurgerCheck() {

    let elems = document.querySelectorAll(".burger_line");
    elems.forEach(item => {
        item.classList.remove("cross");
    })
 

    let hide_show1 = document.querySelectorAll(".chat-item-right");
    let hide_show2 = document.querySelectorAll(".chat-item-left");

    if (burger_opened) {
        hide_show1.forEach(item => {
            item.classList.add("hidden");
        });
        hide_show2.forEach(item => {
            item.style.width = "auto";
        });

  
        document.getElementById("left_part").classList.add("width15");
   

        burger_opened = false;
    }
    else {

        let elems = document.querySelectorAll(".burger_line");
        elems.forEach(item => {
            item.classList.add("cross");
        })
        


        hide_show1.forEach(item => {
            item.classList.remove("hidden");
        });
        hide_show2.forEach(item => {
            item.style.width = "calc(25% - 2px);";
        });
    
        document.getElementById("left_part").classList.remove("width15");
        
        burger_opened = true;
    }
}