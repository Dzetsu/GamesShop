namespace OrderGame.Entities;

public class OrderedGame
{
    public long id { get; set; }
    public string userName { get; set; }
    public string dateTime { get; set; }
    public string nameOfGame { get; set; }
    public string status { get; set; }
    public string idempotent_key { get; set; }
}