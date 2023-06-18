namespace DiscordBot.Database.Entities;

public class Entity
{
    public Entity(int id, string value)
    {
        Id = id;
        Value = value;
    }
    
    public int Id { get; set; }
    
    public string Value { get; set; }
}