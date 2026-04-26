namespace CommunityEventCalendar;



public class Event {
    public int id { get; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Date { get; set; }
    public string Location { get; set; }
    public bool IsUpdated { get; set; }

    public Event(int id, string name, string description, string date, string location, bool isUpdated) {
        this.id = id;
        this.Name = name;
        this.Description = description;
        this.Date = date;
        this.Location = location;
        this.IsUpdated = isUpdated;
    }

    public override string ToString() {
        return this.Name;
    }
}

public class CommunityManager{
    public string Name { get; }

    public CommunityManager(string name) {
        this.Name = name;
    }

}

public class CommunityMember{
    public string Name { get; set;}
    public List<Event> RegisteredEvents { get; set;}

    public CommunityMember(string name) {
        this.Name = name;
        this.RegisteredEvents = new List<Event>();
    }

    public override string ToString() {
        return this.Name;
    }

    public void AddRegisteredEvent(Event @event) {
        this.RegisteredEvents.Add(@event);
    }

    public void RemoveRegisteredEvent(Event @event) {
        this.RegisteredEvents.Remove(@event);
    }

}