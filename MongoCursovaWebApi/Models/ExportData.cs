namespace MongoCursovaWebApi.Models
{
    public class ExportData
    {
        public string fact {  get; set; }
        public List<string> columns { get; set; }

        public string type { get; set; }
        public int limit {  get; set; }


    }
}
