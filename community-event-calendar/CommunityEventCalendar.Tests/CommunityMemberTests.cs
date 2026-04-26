namespace CommunityEventCalendar.Tests;
using CommunityEventCalendar;


public class CommunityMemberTests
{
    [Fact]
    public void CreateNewMember()
    {
        var member = new CommunityMember("Ryan");

        Assert.Equal("Ryan", member.Name);
        Assert.NotNull(member.RegisteredEvents);
        Assert.Empty(member.RegisteredEvents);
    }

    [Fact]
    public void ReturnName()
    {
        var member = new CommunityMember("Ryan");
        Assert.Equal("Ryan", member.ToString());
    
    }

    [Fact]
    public void RegisterForEvent()
    {
        var member = new CommunityMember("Ryan");
        var e = new Event(1, "Road Race", "Fun 5K Run", "Apr 1", "Reservoir", false);

        member.AddRegisteredEvent(e);

        Assert.Contains(e, member.RegisteredEvents);
    }
    [Fact]
    public void UnRegisterForEvent()
    {
        var member = new CommunityMember("Ryan");
        var e = new Event(1, "Race", "Fun Run", "Apr 1", "Reservoir", false);
        member.AddRegisteredEvent(e);

        member.RemoveRegisteredEvent(e);

        Assert.DoesNotContain(e, member.RegisteredEvents);
    }




}