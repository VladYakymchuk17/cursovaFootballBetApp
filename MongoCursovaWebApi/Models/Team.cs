namespace MongoCursovaWebApi.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Location Location { get; set; }
        public List<Player>? Players { get; set; }
    }
}
