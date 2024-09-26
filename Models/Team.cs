namespace ScoreboardAPI.Models
{
    public class Team
    {
        public int Id { get; set; }


        public string Name { get; set; }

        public int Score { get; set; }

        public string ?PhotoPath { get; set; }

        public string ?PhotoBase64 { get; set; }


    }
}
