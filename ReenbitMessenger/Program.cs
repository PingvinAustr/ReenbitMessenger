using Microsoft.EntityFrameworkCore;
using ReenbitMessenger.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>
(o => o.UseInMemoryDatabase("MyDatabase"));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
AddCustomerData(app);
app.MapRazorPages();

app.Run();
static void AddCustomerData(WebApplication app)
{
    var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetService<AppDbContext>();





    List<Chats> test = new List<Chats>();

    User user1 = new User
    {
        Name = "Tom",
        Surname = "Tomson",
        UserAvatarImage = "man.png"

    };
    User user2 = new User
    {
        Name = "Bob",
        Surname = "Bobson",
        UserAvatarImage = "man1.png"
    };

    User user3 = new User
    {
        Name = "Jango",
        Surname = "Billson",
        UserAvatarImage = "man_dark.png"
    };

    User user4 = new User
    {
        Name = "Mary",
        Surname = "Maryson",
        UserAvatarImage = "woman1.png"
    };

    User user5 = new User
    {
        Name = "Helen",
        Surname = "helenson",
        UserAvatarImage = "woman.png"
    };

    User user6 = new User
    {
        Name = "Svyatoslav",
        Surname = "Perepolochenskiy",
        UserAvatarImage = "woman1.png"
    };

    Chats chat1 = new Chats
    {
        ChatName = "MathChat",

    };
    Chats chat2 = new Chats
    {
        ChatName = "ProgrammingChat",

    };

    Chats direct_chat3 = new Chats
    {
        ChatName = "DirectChat3"
    };

    Chats direct_chat4 = new Chats
    {
        ChatName = "DirectChat4"
    };

    Chats direct_chat5 = new Chats
    {
        ChatName = "DirectChat5"
    };

    Chats direct_chat6 = new Chats
    {
        ChatName = "DirectChat6"
    };

   


    UsersChats userschats_direct1 = new UsersChats() { UserId=1,User=user1,ChatId=3,Chat=direct_chat3 }; //Tom - 3 - DM
    UsersChats userschats_direct2 = new UsersChats() { UserId=2,User=user2,ChatId=3,Chat=direct_chat3 }; //Bob - 3 - DM

    UsersChats userschats_direct3 = new UsersChats() { UserId = 2, User = user2, ChatId = 4, Chat = direct_chat4 }; //Bob - 4 - DM
    UsersChats userschats_direct4 = new UsersChats() { UserId = 5, User = user5, ChatId = 4, Chat = direct_chat4 }; //Helen - 4 - DM

    UsersChats userschats_direct5 = new UsersChats() { UserId = 3, User = user3, ChatId = 5, Chat = direct_chat5 }; //Bill - 5 - DM
    UsersChats userschats_direct6 = new UsersChats() { UserId = 5, User = user5, ChatId = 5, Chat = direct_chat5 }; //Helen - 5 - DM

    UsersChats userschats_direct7 = new UsersChats() { UserId = 3, User = user3, ChatId = 6, Chat = direct_chat6 }; //Bill - 6 - DM
    UsersChats userschats_direct8 = new UsersChats() { UserId = 1, User = user1, ChatId = 6, Chat = direct_chat6 }; //Tom - 6 - DM



    Chats group_all_chat = new Chats
    {
        ChatName = "All guys here :)"
    };
    UsersChats all_users_chat1 = new UsersChats() {UserId=1,User=user1,ChatId=7,Chat=group_all_chat }; //Tom - 7 - All guys here :) chat
    UsersChats all_users_chat2 = new UsersChats() {UserId=2,User=user2,ChatId=7,Chat=group_all_chat }; //Bob - 7 - All guys here :) chat
    UsersChats all_users_chat3 = new UsersChats() {UserId=3,User=user3,ChatId=7,Chat=group_all_chat }; //Bill - 7 - All guys here :) chat
    UsersChats all_users_chat4 = new UsersChats() {UserId=4,User=user4,ChatId=7,Chat=group_all_chat }; //Jack - 7 - All guys here :) chat
    UsersChats all_users_chat5 = new UsersChats() {UserId=5,User=user5,ChatId=7,Chat=group_all_chat }; //Helen - 7 - All guys here :) chat
    UsersChats all_users_chat6 = new UsersChats() {UserId=6,User=user6,ChatId=7,Chat=group_all_chat }; //Svyatoslav - 7 - All guys here :) chat
    

    Chats direct_chat7 = new Chats
    {
        ChatName = "DirectChat7"
    };
    UsersChats userschats_direct9 = new UsersChats() { UserId = 1, User = user1, ChatId = 8, Chat = direct_chat7 }; //Tom - 8 - DM
    UsersChats userschats_direct10 = new UsersChats() { UserId = 4, User = user4, ChatId = 8, Chat = direct_chat7 };//Jack - 8 - DM



    UsersChats usersChat1 = new UsersChats(); //Tom - 1 - MathChat (Tom and Helen DM)
    usersChat1.UserId = 1;
    usersChat1.ChatId = 1;
    usersChat1.User = user1;
    usersChat1.Chat = chat1;

    UsersChats usersChats2 = new UsersChats(); //Tom - 2 - ProgrammingChat
    usersChats2.UserId = 1;
    usersChats2.ChatId = 2;
    usersChats2.User = user1;
    usersChats2.Chat = chat2;

    UsersChats usersChats3 = new UsersChats(); //Bob - 2 - ProgrammingChat
    usersChats3.UserId = 2;
    usersChats3.ChatId = 2;
    usersChats3.User = user2;
    usersChats3.Chat = chat2;

    UsersChats usersChats4 = new UsersChats(); //Bill - 2 - ProgrammingChat
    usersChats4.UserId = 3;
    usersChats4.ChatId = 2;
    usersChats4.User = user3;
    usersChats4.Chat = chat2;

    UsersChats usersChats5 = new UsersChats(); //Jack - 2 - ProgrammingChat
    usersChats5.UserId = 4;
    usersChats5.ChatId = 2;
    usersChats5.User = user4;
    usersChats5.Chat = chat2;

    UsersChats usersChats6 = new UsersChats(); //Helen - 1 - MathChat (Tom and Helen DM)
    usersChats6.UserId = 5;
    usersChats6.ChatId = 1;
    usersChats6.User = user5;
    usersChats6.Chat = chat1;

   


    db.Users.Add(user1);
    db.Users.Add(user2);
    db.Chats.Add(chat1);
    db.Chats.Add(chat2);
    db.UsersChats.Add(usersChat1);
    db.UsersChats.Add(usersChats2);
    db.UsersChats.Add(usersChats3);
    db.UsersChats.Add(usersChats4);
    db.UsersChats.Add(usersChats5);
    db.UsersChats.Add(usersChats6);
    db.UsersChats.Add(userschats_direct1);
    db.UsersChats.Add(userschats_direct2);
    db.UsersChats.Add(userschats_direct3);
    db.UsersChats.Add(userschats_direct4);
    db.UsersChats.Add(userschats_direct5);
    db.UsersChats.Add(userschats_direct6);
    db.UsersChats.Add(userschats_direct7);
    db.UsersChats.Add(userschats_direct8);
    
    db.UsersChats.Add(all_users_chat1);
    db.UsersChats.Add(all_users_chat2);
    db.UsersChats.Add(all_users_chat3);
    db.UsersChats.Add(all_users_chat4);
    db.UsersChats.Add(all_users_chat5);
    db.UsersChats.Add(all_users_chat6);

    db.UsersChats.Add(userschats_direct9);
    db.UsersChats.Add(userschats_direct10);

   

    db.SaveChanges();

    
    Messages message1 = new Messages() { MessageText = "Hello Helen! It's Tom!", User_Id = user1.Id, Chat_Id = chat1.Id,time_sent=new DateTime(2022,8,24,19,35,0) };
    Messages message2 = new Messages() { MessageText = "Hello Tom, I am Helen, nice to meet you!", User_Id = user5.Id, Chat_Id = chat1.Id, time_sent=new DateTime(2022,8,24,19,36,10)};
    Messages message22 = new Messages() { MessageText = "How are you?", User_Id = user5.Id, Chat_Id = chat1.Id, time_sent=new DateTime(2022,8,24,19,37,10)};
    Messages message12 = new Messages() { MessageText = "I am fine, thanks", User_Id = user1.Id, Chat_Id = chat1.Id, time_sent = new DateTime(2022, 8, 24, 19, 38, 0) };
    Messages message13 = new Messages() { MessageText = " and how are you", User_Id = user1.Id, Chat_Id = chat1.Id, time_sent = new DateTime(2022, 8, 24, 19, 38, 30) };
    Messages message23 = new Messages() { MessageText = "Pretty OK, just finished my homework", User_Id = user5.Id, Chat_Id = chat1.Id, time_sent = new DateTime(2022, 8, 24, 19, 39, 10) };
    Messages message24 = new Messages() { MessageText = "What do you think about going out tomorrow?", User_Id = user5.Id, Chat_Id = chat1.Id, time_sent = new DateTime(2022, 8, 24, 19, 40, 10) };
    Messages message14 = new Messages() { MessageText = "Sure, why not!", User_Id = user1.Id, Chat_Id = chat1.Id, time_sent = new DateTime(2022, 8, 24, 19, 40, 30) };
    Messages message15 = new Messages() { MessageText = "Let's go to cinema?", User_Id = user1.Id, Chat_Id = chat1.Id, time_sent = new DateTime(2022, 8, 24, 19, 40, 40) };
    Messages message25 = new Messages() { MessageText = "Cool! I want to watch new Avatar movie", User_Id = user5.Id, Chat_Id = chat1.Id, time_sent = new DateTime(2022, 8, 24, 19, 40, 50) };
    Messages message16 = new Messages() { MessageText = "Then I will pick you up tomorrow at 18:30!", User_Id = user1.Id, Chat_Id = chat1.Id, time_sent = new DateTime(2022, 8, 24, 19, 41, 40) };
    Messages message17 = new Messages() { MessageText = "See you darling!", User_Id = user1.Id, Chat_Id = chat1.Id, time_sent = new DateTime(2022, 8, 24, 19, 41, 50) };
    Messages message26 = new Messages() { MessageText = "Okay!", User_Id = user5.Id, Chat_Id = chat1.Id, time_sent = new DateTime(2022, 8, 24, 19, 42, 50) };
    Messages message27 = new Messages() { MessageText = "Bye honey!", User_Id = user5.Id, Chat_Id = chat1.Id, time_sent = new DateTime(2022, 8, 24, 19, 43, 50) };
    
    Messages message28 = new Messages() { MessageText = "Hi!", User_Id = user1.Id, Chat_Id = chat1.Id, time_sent = new DateTime(2022, 8, 25, 19, 43, 50) };
    Messages message29 = new Messages() { MessageText = "How is your exam?", User_Id = user1.Id, Chat_Id = chat1.Id, time_sent = new DateTime(2022, 8, 25, 19, 44, 50) };
    Messages message210 = new Messages() { MessageText = "Hello Tom", User_Id = user5.Id, Chat_Id = chat1.Id, time_sent = new DateTime(2022, 8, 25, 19, 45, 50) };
    Messages message211 = new Messages() { MessageText = "I've got A+ and I am very happy!", User_Id = user5.Id, Chat_Id = chat1.Id, time_sent = new DateTime(2022, 8, 25, 19, 46, 50) };
    Messages message212 = new Messages() { MessageText = "Cool!", User_Id = user1.Id, Chat_Id = chat1.Id, time_sent = new DateTime(2022, 8, 25, 19, 47, 50) };
    Messages message213 = new Messages() { MessageText = "Thanks for your support", User_Id = user5.Id, Chat_Id = chat1.Id, time_sent = new DateTime(2022, 8, 25, 19, 48, 50) };
    Messages message214 = new Messages() { MessageText = "You are welcome", User_Id = user1.Id, Chat_Id = chat1.Id, time_sent = new DateTime(2022, 8, 25, 19, 49, 50) };
    Messages message215 = new Messages() { MessageText = "Wanna hang out tonight?", User_Id = user5.Id, Chat_Id = chat1.Id, time_sent = new DateTime(2022, 8, 25, 19, 50, 50) };
    Messages message216 = new Messages() { MessageText = "Sure!", User_Id = user1.Id, Chat_Id = chat1.Id, time_sent = new DateTime(2022, 8, 25, 19, 51, 50) };
    




    Messages message3 = new Messages() { MessageText = "Hello progers! It's Tom", User_Id = user1.Id, Chat_Id = chat2.Id, time_sent=new DateTime(2022,8,25,12,0,0) };
    Messages message4 = new Messages() { MessageText = "Nice to meet you) I am Bob", User_Id = user2.Id, Chat_Id = chat2.Id, time_sent=new DateTime(2022,8,26,19,30,0) };


    Messages message51 = new Messages() { MessageText = "Hello guys, I am Tom!", User_Id = user1.Id, Chat_Id = group_all_chat.Id, time_sent = new DateTime(2022, 8, 25, 19, 30, 0) };
    Messages message52 = new Messages() { MessageText = "Hi! I am Bob)", User_Id = user2.Id, Chat_Id = group_all_chat.Id, time_sent = new DateTime(2022, 8, 25, 19, 31, 0) };
    Messages message53 = new Messages() { MessageText = "Me Jango!", User_Id = user3.Id, Chat_Id = group_all_chat.Id, time_sent = new DateTime(2022, 8, 25, 19, 32, 0) };
    Messages message54 = new Messages() { MessageText = "My name is Mary", User_Id = user4.Id, Chat_Id = group_all_chat.Id, time_sent = new DateTime(2022, 8, 25, 19, 33, 0) };
    Messages message55 = new Messages() { MessageText = "I am Helen, hello everyone!", User_Id = user5.Id, Chat_Id = group_all_chat.Id, time_sent = new DateTime(2022, 8, 25, 19, 34, 0) };
    Messages message56 = new Messages() { MessageText = "My name is Svyatoslav, nice to meet you guys!", User_Id = user6.Id, Chat_Id = group_all_chat.Id, time_sent = new DateTime(2022, 8, 25, 19, 35, 0) };



    db.Messages.Add(message28);
    db.Messages.Add(message29);
    db.Messages.Add(message210);
    db.Messages.Add(message211);
    db.Messages.Add(message212);
    db.Messages.Add(message213);
    db.Messages.Add(message214);
    db.Messages.Add(message215);
    db.Messages.Add(message216);

    db.Messages.Add(message1);
    db.Messages.Add(message2);
    db.Messages.Add(message22);
    db.Messages.Add(message12);
    db.Messages.Add(message13);
    db.Messages.Add(message23);
    db.Messages.Add(message24);
    db.Messages.Add(message14);
    db.Messages.Add(message15);
    db.Messages.Add(message25);



    db.Messages.Add(message16);
    db.Messages.Add(message17);
    db.Messages.Add(message26);
    db.Messages.Add(message27);
    db.Messages.Add(message51);
    db.Messages.Add(message52);
    db.Messages.Add(message53);
    db.Messages.Add(message54);
    db.Messages.Add(message55);
    db.Messages.Add(message56);


    db.Messages.Add(message3);
    db.Messages.Add(message4);
    db.SaveChanges();
    /*
    foreach (Chats chat in db.Chats)
    {
        Console.WriteLine("Chat - " + chat.ChatName + " [");

        var me = db.UsersChats.Where(x => x.Chat == chat).ToList();
        foreach (var user in me)
        {
            Console.Write(user.User.Name + " ");
        }
        Console.Write("]");
    }
    */




}