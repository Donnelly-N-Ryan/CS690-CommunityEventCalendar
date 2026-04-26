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
                                                "add event","update event","delete event", "manage members", "end"
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

                    else if (command == "manage members") {
                        var memberCommand = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("What do you want to do?")
                                .AddChoices(new[] {
                                    "add member", "delete member", "end"
                                }));

                        if (memberCommand == "add member") {
                            var name = AnsiConsole.Prompt(new TextPrompt<string>("Enter member name:"));
                            dataManager.AddMember(name);
                            Console.WriteLine($"Member '{name}' added!");
                        }

                        else if (memberCommand == "delete member") {
                            if (dataManager.Members.Count == 0) {
                                Console.WriteLine("No members available.");
                            } else {
                                var selectedMember = AnsiConsole.Prompt(
                                    new SelectionPrompt<CommunityMember>()
                                        .Title("Select a member to delete")
                                        .AddChoices(dataManager.Members));

                                if (AnsiConsole.Confirm($"Are you sure you want to delete '{selectedMember.Name}'?")) {
                                    dataManager.DeleteMember(selectedMember);
                                    Console.WriteLine($"Member '{selectedMember.Name}' deleted!");
                                } else {
                                    Console.WriteLine("Deletion cancelled.");
                                }
                            }
                        }
                    }

                } while (command != "end");
            }

            else if (mode == "community member") {
                if (dataManager.Members.Count == 0) {
                    Console.WriteLine("No community members exist. Ask a manager to create your account.");
                    continue;
                }

                var selectedMember = AnsiConsole.Prompt(
                    new SelectionPrompt<CommunityMember>()
                        .Title("Who are you?")
                        .AddChoices(dataManager.Members));
               
                string command;
                do {

                    command = AnsiConsole.Prompt(
                                    new SelectionPrompt<string>()
                                        .Title("What do you want to do?")
                                        .AddChoices(new[] {
                                            "view events","register for event","unregister for event", "end"
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

                    else if (command == "register for event") {
                        if (dataManager.Events.Count == 0) {
                            Console.WriteLine("No events available.");
                            continue;
                        }

                        var selectedEvent = AnsiConsole.Prompt(
                            new SelectionPrompt<Event>()
                                .Title("Select an event to register for")
                                .AddChoices(dataManager.Events));

                        if (selectedMember.RegisteredEvents.Contains(selectedEvent)) {
                            Console.WriteLine($"You are already registered for '{selectedEvent.Name}'.");
                        } else {
                            dataManager.RegisterForEvent(selectedMember, selectedEvent);
                            Console.WriteLine($"You are now registered for '{selectedEvent.Name}'!");
                        }
                    }

                    else if (command == "unregister for event") {
                        if (selectedMember.RegisteredEvents.Count == 0) {
                            Console.WriteLine("You are not registered for any events.");
                            continue;
                        }

                        var selectedEvent = AnsiConsole.Prompt(
                            new SelectionPrompt<Event>()
                                .Title("Select an event to unregister from")
                                .AddChoices(selectedMember.RegisteredEvents));

                            dataManager.UnregisterForEvent(selectedMember, selectedEvent);
                            Console.WriteLine($"You have been unregistered from '{selectedEvent.Name}'.");}

                } while (command != "end");
            }

        } while (mode != "end");
    }

    public static string? AskForInput(string message) {
        Console.Write(message);
        return Console.ReadLine();
    }
}
