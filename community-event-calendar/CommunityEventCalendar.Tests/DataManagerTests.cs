namespace CommunityEventCalendar.Tests;
using CommunityEventCalendar;

public class DataManagerTests : IDisposable
{
    public DataManagerTests()
    {
        CleanFiles();
    }

    public void Dispose()
    {
        CleanFiles();
    }
    
    private void CleanFiles()
    {
        foreach (var file in Directory.GetFiles(".", "*.txt"))
            File.Delete(file);
    }
    // get next ID
    [Fact]
    public void GetNextID(){
        var dataManager = new DataManager();

        var first = dataManager.GetNextId();
        var second = dataManager.GetNextId();

        Assert.NotEqual(first, second);
        Assert.Equal(first + 1, second);
    }
    //Adding an event
    [Fact]
    public void AddEvent()
    {
        var dataManager = new DataManager();
        var e = new Event(dataManager.GetNextId(), "Race", "Reservoir", "Apr 1", "Fun Run", false);

        dataManager.AddEvent(e);

        Assert.Contains(e, dataManager.Events);
    }
    // Updating an event
    [Fact]
    public void UpdateEvent()
    {
        var dataManager = new DataManager();
        var e = new Event(dataManager.GetNextId(), "Race", "Reservoir", "Apr 1", "Fun Run", false);

        dataManager.AddEvent(e);

        e.Name = "Iron Man";
        dataManager.UpdateEvent(e);

        Assert.Equal("Iron Man", e.Name);
        Assert.True(e.IsUpdated);
    }
    // Deleting an Event
    [Fact]
    public void DeleteEvent()
    {
        var dataManager = new DataManager();
        var e = new Event(dataManager.GetNextId(), "Race", "Reservoir", "Apr 1", "Fun Run", false);

        dataManager.AddEvent(e);
        dataManager.DeleteEvent(e);

        Assert.DoesNotContain(e, dataManager.Events);
    }
    // Add community member
    [Fact]
    public void AddCommunityMember()
    {
        var dataManager = new DataManager();
        dataManager.AddMember("Ryan");

        Assert.Contains(dataManager.Members, member => member.Name == "Ryan");
    }
    // Delete Community Member
    [Fact]
    public void DeleteCommunityMember()
    {
        var dataManager = new DataManager();
        dataManager.AddMember("Ryan");
        var member = dataManager.Members.First(member => member.Name == "Ryan");

        dataManager.DeleteMember(member);

        Assert.DoesNotContain(member, dataManager.Members);
    }
    // Community Member Registers for Event
    [Fact]
    public void CommunityMemberRegistersForEvent()
    {
        var dataManager = new DataManager();
        dataManager.AddMember("Ryan");
        var member = dataManager.Members.First(member => member.Name == "Ryan");
        var e = new Event(dataManager.GetNextId(), "Race", "Reservoir", "Apr 1", "Fun Run", false);
        dataManager.AddEvent(e);

        dataManager.RegisterForEvent(member, e);

        Assert.Contains(e, member.RegisteredEvents);
    }
// Community Member unregisters for Event
    [Fact]
    public void CommunityMemberUnregistersForEvent()
    {
        var dataManager = new DataManager();
        dataManager.AddMember("Ryan");
        var member = dataManager.Members.First(member => member.Name == "Ryan");
        var e = new Event(dataManager.GetNextId(), "Race", "Reservoir", "Apr 1", "Fun Run", false);
        dataManager.AddEvent(e);
        dataManager.RegisterForEvent(member, e);

        dataManager.UnregisterForEvent(member, e);

        Assert.DoesNotContain(e, member.RegisteredEvents);
    }





}
