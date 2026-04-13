namespace CommunityEventCalendar;

using Spectre.Console;


public class ConsoleUI {
    DataManager dataManager = new DataManager();
    public void Show() {
        string mode;
        do {
            mode = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                    .Title("Please select mode")
                                    .AddChoices(new[] {
                                        "manager","community member", "end"
                                    }));


            if (mode == "manager") {
                string command;
                do {

                    command = AnsiConsole.Prompt(
                                        new SelectionPrompt<string>()
                                            .Title("What do you want to do?")
                                            .AddChoices(new[] {
                                                "add event","update event","delete event", "end"
                                            }));

                    if (command == "add event") {
                        //nextId++
                        var name = AnsiConsole.Prompt(new TextPrompt<string>("Enter event name:"));
                        var location = AnsiConsole.Prompt(new TextPrompt<string>("Enter event location:"));
                        var date = AnsiConsole.Prompt(new TextPrompt<string>("Enter event date:"));
                        var description = AnsiConsole.Prompt(new TextPrompt<string>("Enter event description:"));
                        var newEvent = new Event(dataManager.GetNextId(), name, description, date, location, false);
                        dataManager.AddEvent(newEvent);
                        Console.WriteLine($"Event '{name}' added!");
                    }

                    else if (command == "update event") {
                        Event? selectedEvent = null;
                        do {
                            if (dataManager.Events.Count == 0) {
                                Console.WriteLine("No events available.");
                                break;
                            }

                            var choices = new List<Event>(dataManager.Events);
                            choices.Add(new Event(-1, "end", "", "", "", false));

                            selectedEvent = AnsiConsole.Prompt(
                                new SelectionPrompt<Event>()
                                    .Title("Select an event to update")
                                    .AddChoices(choices));

                            if (selectedEvent.Name == "end") {
                                break;
                            }

                            selectedEvent.Name = AnsiConsole.Prompt(new TextPrompt<string>("Enter new name:").DefaultValue(selectedEvent.Name));
                            selectedEvent.Location = AnsiConsole.Prompt(new TextPrompt<string>("Enter new location:").DefaultValue(selectedEvent.Location));
                            selectedEvent.Date = AnsiConsole.Prompt(new TextPrompt<string>("Enter new date:").DefaultValue(selectedEvent.Date));
                            selectedEvent.Description = AnsiConsole.Prompt(new TextPrompt<string>("Enter new description:").DefaultValue(selectedEvent.Description));
                            dataManager.UpdateEvent(selectedEvent);
                            Console.WriteLine($"Event '{selectedEvent.Name}' updated!");
                        } while (selectedEvent != null && selectedEvent.Name != "end");
                    }

                    else if (command == "delete event") {

                        if (dataManager.Events.Count == 0) {
                            Console.WriteLine("No events available.");
                            break;
                        }
                        else {
                            var selectedEvent = AnsiConsole.Prompt(
                                new SelectionPrompt<Event>()
                                    .Title("Select an event to delete")
                                    .AddChoices(dataManager.Events));

                            if (AnsiConsole.Confirm($"Are you sure you want to delete '{selectedEvent.Name}'?")) {
                                dataManager.DeleteEvent(selectedEvent);
                                Console.WriteLine($"Event '{selectedEvent.Name}' deleted!");
                            } else {
                                Console.WriteLine("Deletion cancelled.");
                            }
                        }
                    }

                } while (command != "end");
            }

            else if (mode == "community member") {
                string command;
                do {

                    command = AnsiConsole.Prompt(
                                    new SelectionPrompt<string>()
                                        .Title("What do you want to do?")
                                        .AddChoices(new[] {
                                            "view events", "end" //add "register for event", "unregister for event",
                                        }));

                    if (command == "view events") {
                        if (dataManager.Events.Count == 0) {
                            Console.WriteLine("Sorry, there are no events in your area.");
                        } else {
                            var table = new Table();
                            table.AddColumn("Name");
                            table.AddColumn("Location");
                            table.AddColumn("Date");
                            table.AddColumn("Description");

                            foreach (var e in dataManager.Events) {
                                table.AddRow(e.Name, e.Location, e.Date, e.Description);
                            }

                            AnsiConsole.Write(table);
                        }
                    }

                    //else if (command == "register for event") {
                      //  Console.WriteLine("Feature not implemented yet. check back later");
                    //}

                    //else if (command == "unregister for event") {
                      //  Console.WriteLine("Feature not implemented yet. check back later");
                   // }

                } while (command != "end");
            }

        } while (mode != "end");
    }

    public static string? AskForInput(string message) {
        Console.Write(message);
        return Console.ReadLine();
    }
}
