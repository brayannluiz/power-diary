using PowerDiary.ChatRoom.Core.Application;
using PowerDiary.ChatRoom.Core.Repository;

// TODO: Maybe IoC eventually? 
var repository = new ChatEventRepository();
var appService = new ChatEventAppService(repository);

Console.WriteLine("Hi, this is the chat history for Power Diary. I'm actually kind of hungry, so lets get to it...\n\n");
Console.WriteLine("Press 1 for a minute by minute granularity");
Console.WriteLine("Press 2 for an hour granularity");
Console.WriteLine("Press another button exit the application");

var key = Console.ReadKey().Key;

switch (key)
{
    case ConsoleKey.NumPad1:
    case ConsoleKey.D1:
        PrintChatEventsMinuteByMinute();
        break;

    case ConsoleKey.NumPad2:
    case ConsoleKey.D2:
        PrintChatEventsHourly();
        break;

    default:
        return;
        
}

Console.WriteLine("Thanks!");
Console.ReadLine();

void PrintChatEventsMinuteByMinute()
{
    Console.WriteLine("\n");

    foreach (var item in appService.GetChatEventsMinuteByMinute())
    {
        Console.WriteLine(item);
    }
}

void PrintChatEventsHourly()
{
    foreach (var chatEventByHour in appService.GetChatEventsHourly())
    {
        Console.WriteLine("\n");
        Console.WriteLine($"{chatEventByHour.Time}:");

        foreach (var item in chatEventByHour.ChatEventDescriptions)
        {
            Console.WriteLine($"\t{item}");
        }

        Console.WriteLine("\n");
    }    
}