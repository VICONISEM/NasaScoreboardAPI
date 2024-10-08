using ScoreboardAPI.Models;

namespace ScoreboardAPI.Handler
{
    public interface ITeamOperation
    {
        public static abstract void Clone(Team original, Team updatedTeam);
        public static abstract void Sum(Team original);

        public static abstract void UrlPhoto(HttpRequest httpRequest, Team team);

        public static abstract void FileHandlerAddPhoto(Team team, IFormFile file);
        public static abstract void FileHandlerDeletePhoto(Team team);

    }
}
