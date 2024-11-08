namespace MongoCursovaWebApi.Models
{
    public class AddBet
    {
        public int GameId { get; set; }
        public string Name { get; set; }
        public double Coefficient { get; set; }
        public int NumberOfBets { get; set; }
        public int Income { get; set; }
    }
}
