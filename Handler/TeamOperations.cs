using ScoreboardAPI.Migrations;
using ScoreboardAPI.Models;
using System.Collections;

namespace ScoreboardAPI.Handler
{
    public class TeamOperations : ITeamOperation
    {

        private readonly HttpRequest httpRequest;



        public static void Clone(Team original, Team updatedTeam)
        {

            original.Impact = updatedTeam.Impact;
            original.Creativity = updatedTeam.Creativity;
            original.Validity = updatedTeam.Validity;
            original.Relevance = updatedTeam.Relevance;
            original.Presentation = updatedTeam.Presentation;
            Sum(original);

        }

        public static void FileHandlerAddPhoto(Team newTeam, IFormFile photo)
        {
            var FileName = $"{newTeam.Name}_{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", FileName);
            using FileStream Fs = new FileStream(FilePath, FileMode.Create);
            photo.CopyTo(Fs);
            newTeam.PhotoPath = $"/Images/{FileName}";

        }

        public static void FileHandlerDeletePhoto(Team team)
        {
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", team.PhotoPath.TrimStart('/'));
            if(System.IO.File.Exists(FilePath))
            {
                System.IO.File.Delete(FilePath);
            }
        }

        public static void Sum(Team original)
        {
            original.TotalScore = original.Impact + original.Creativity + original.Validity + original.Relevance + original.Presentation;

        }

        public static void UrlPhoto(HttpRequest httpRequest , Team team)
        {

            team.PhotoUrl = $"{httpRequest.Scheme}://{httpRequest.Host}{team.PhotoPath}";
           
        }

       
    }
}
