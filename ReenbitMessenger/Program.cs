using Microsoft.EntityFrameworkCore;
using ReenbitMessenger.DataAccess;


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
        UserAvatarImage = "profile.png"
    };

    User user3 = new User
    {
        Name = "Bill",
        Surname = "Billson",
        UserAvatarImage = "user.png"
    };

    User user4 = new User
    {
        Name = "Jack",
        Surname = "Jackson",
        UserAvatarImage = "userMan.png"
    };

    User user5 = new User
    {
        Name = "Helen",
        Surname = "helenson",
        UserAvatarImage = "woman.png"
    };



    Chats chat1 = new Chats
    {
        ChatName = "MathChat",

    };
    Chats chat2 = new Chats
    {
        ChatName = "ProgrammingChat",

    };

    UsersChats usersChat1 = new UsersChats();
    usersChat1.UserId = 1;
    usersChat1.ChatId = 1;
    usersChat1.User = user1;
    usersChat1.Chat = chat1;

    UsersChats usersChats2 = new UsersChats();
    usersChats2.UserId = 1;
    usersChats2.ChatId = 2;
    usersChats2.User = user1;
    usersChats2.Chat = chat2;

    UsersChats usersChats3 = new UsersChats();
    usersChats3.UserId = 2;
    usersChats3.ChatId = 2;
    usersChats3.User = user2;
    usersChats3.Chat = chat2;

    UsersChats usersChats4 = new UsersChats();
    usersChats4.UserId = 3;
    usersChats4.ChatId = 2;
    usersChats4.User = user3;
    usersChats4.Chat = chat2;

    UsersChats usersChats5 = new UsersChats();
    usersChats5.UserId = 4;
    usersChats5.ChatId = 2;
    usersChats5.User = user4;
    usersChats5.Chat = chat2;

    UsersChats usersChats6 = new UsersChats();
    usersChats6.UserId = 5;
    usersChats6.ChatId = 1;
    usersChats6.User = user5;
    usersChats6.Chat = chat1;

    Messages message1 = new Messages() { MessageText = "Message from Tom to Math Chat", User = user1, Chat = chat1 };
    Messages message2 = new Messages() { MessageText = "Message from Helen to Math Chat", User = user5, Chat = chat1 };


    Messages message3 = new Messages() { MessageText = "Message from Tom to Progr Chat", User = user1, Chat = chat2 };
    Messages message4 = new Messages() { MessageText = "Message from Bob to Math Chat", User = user2, Chat = chat2 };


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

    db.Messages.Add(message1);
    db.Messages.Add(message2);
    db.Messages.Add(message3);
    db.Messages.Add(message4);

    db.SaveChanges();

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



}