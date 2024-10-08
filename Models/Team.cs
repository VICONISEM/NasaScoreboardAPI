namespace ScoreboardAPI.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double TotalScore { get; set; }
        public double Impact { get; set; }
        public double Creativity { get; set; }
        public double Validity { get; set;}
        public double Presentation { get; set;}
        public double Relevance { get; set;}
        public string ?PhotoPath { get; set; }
       public string? PhotoUrl { get; set; } 


    }
}
