namespace CommunityEventCalendar;


public class DataManager {
    private int nextId;
    public List<Event> Events { get; }
    public List<CommunityMember> Members { get ; } 
    
    public DataManager() {

        Events = new List<Event>();
        Members = new List<CommunityMember>();


        LoadData();
        nextId = Events.Any() ? Events.Max(e => e.id) + 1 : 1;
    }

    public int GetNextId() {
        return nextId++;
    }

    public void AddEvent(Event e) {
        Events.Add(e);
        SaveEvents();
    }

    public void UpdateEvent(Event e) {
        e.IsUpdated = true;
        SaveEvents();
    }

    public void DeleteEvent(Event e) {
        Events.Remove(e);
        foreach (var member in Members) {
            member.RegisteredEvents.Remove(e);
            SaveMemberRegistrations(member);
        }
        SaveEvents();
    }

    public void SaveEvents() {
        var lines = Events.Select(e => $"{e.id}|{e.Name}|{e.Description}|{e.Date}|{e.Location}|{e.IsUpdated}").ToArray();
        File.WriteAllLines("events.txt", lines);
    }

    public void RegisterForEvent(CommunityMember m,Event e) {
        m.RegisteredEvents.Add(e);
        SaveMemberRegistrations(m);
    }

    public void UnregisterForEvent(CommunityMember m,Event e) {
        m.RegisteredEvents.Remove(e);
        SaveMemberRegistrations(m);
    }

    public void AddMember(string name) {
        Members.Add(new CommunityMember(name));
        SaveMembers();
    }

    public void DeleteMember(CommunityMember m) {
        Members.Remove(m);
        if (File.Exists($"{m.Name}.txt")) {
            File.Delete($"{m.Name}.txt");
        }
        SaveMembers();
    }

    public void SaveMembers() {
        var memberNames = Members.Select(m => m.Name).ToArray();
        File.WriteAllLines("members.txt", memberNames);
    }

    public void SaveMemberRegistrations(CommunityMember m) {
        var eventIds = m.RegisteredEvents.Select(e => e.id.ToString()).ToArray();
        File.WriteAllLines($"{m.Name}.txt", eventIds);
    }
    
    public void LoadData() {
        if (File.Exists("events.txt")) {
            var eventLines = File.ReadAllLines("events.txt");
            foreach (var line in eventLines) {
                var parts = line.Split('|');
                Events.Add(new Event(int.Parse(parts[0]), parts[1], parts[2], parts[3], parts[4], bool.Parse(parts[5])));
            }
        }

        if (File.Exists("members.txt")) {
            var memberNames = File.ReadAllLines("members.txt");
            foreach (var memberName in memberNames) {
                Members.Add(new CommunityMember(memberName));
            }
        }

        foreach (var member in Members) {
            if (File.Exists($"{member.Name}.txt")) {
                var eventIds = File.ReadAllLines($"{member.Name}.txt");
                foreach (var id in eventIds) {
                    var matchedEvent = Events.FirstOrDefault(e => e.id == int.Parse(id));
                    if (matchedEvent != null) {
                        member.RegisteredEvents.Add(matchedEvent);
                    }
                }
            }
        }
    }
}