namespace MongoCursovaWebApi.Models
{
    public class TeamStat
    {
        public string team_name {  get; set; }
        public int goals { get; set; }
        public int assists { get; set; }
        public int fouls { get; set; }
        public int substitutions {  get; set; }
    }
}
