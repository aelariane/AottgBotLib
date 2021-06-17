# AottgBotLib
Simple tiny library to ease creating "bots" for indie game AoTTG by Feng Lee<br>
<br>
Basically, this library is just a layer built on Photon Network C# libraries to simlify interations for specified game.

# Usage example
```cs
using System;
using AottgBotLib;
using AottgBotLib.Handlers;
using Photon.Realtime;

class Program
{
    public static void Main(String[] args)
    {

        var client = new BotClient("BotName"); //Creating client
        client.Region = PhotonRegion.Europe; //Set region

        //Adding Chat support
        client.RPCHandler.AddChatCallback((id, sender, content) =>
        {
            sender = sender.RemoveAll(); //Removes all color codes
            content = content.RemoveAll(); //Removes all color codes
            Console.WriteLine($"[{id}] {sender}: {content}");
        };

        await client.ConnectToMasterAsync(); //Connecting to region
        
        RoomInfo roomToGetInto = null;
        foreach (RoomInfo room in client.RoomList)
        {
            if (room.Name.Contains("roomNameYouWantToJoin"))
            {
                roomToGetInto = room;
                break;
            }
        }
        if(roomToGetInto == null)
        {
            Environment.Exit(0);
        }
        await client.JoinRoomAsync(roomToGetInto);
        //Voila. You have connected to room you want and now can read what happens there
        
        //Leaving this here to keep console open
        while(true)
        {
            string line = Console.WriteLine();
            if(line == "exit") //Typing exit to close console
            {
                Environment.Exit(0);
            }
            else
            {
                //Otherwise, what you printed will be sent to all players in room
                client.SendChatMessage(line);
            }
        }
    }
}
```

# Build
IDE with C# 8.0 support<br>
.NET Core 3.1
