namespace CommunityEventCalendar.Tests;
using CommunityEventCalendar;


public class EventTests{

    [Fact]
    public void Fields()
    {
        var e = new Event(7, "Race", "5 K", "Apr 1",  "Reservoir",false);

        Assert.Equal(7, e.id);
        Assert.Equal("Race", e.Name);        
        Assert.Equal("Reservoir", e.Location);
        Assert.Equal("5 K", e.Description);
        Assert.Equal("Apr 1", e.Date);
        Assert.False(e.IsUpdated);
    }
    [Fact]
    public void ToStringTest()
    {
        var e = new Event(1, "Concert", "Bluegrass", "Apr 1", "Town Square", false);

        Assert.Equal("Concert", e.ToString());
    }
    [Fact]
    public void EditFieldTest()
    {
        var e = new Event(10, "Race", "5 K", "Apr 1",  "Reservoir",false);

        e.Name = "Ironman";
        e.IsUpdated =true;


        Assert.Equal("Ironman", e.Name);
        Assert.True(e.IsUpdated);        
    }

}