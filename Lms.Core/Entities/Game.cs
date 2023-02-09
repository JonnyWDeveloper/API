using System.ComponentModel.DataAnnotations;

namespace Lms.Core.Entities
{
    public class Game
    {
        public int Id
        {
            get; set;
        }
        [Required]
        [MaxLength(30)]
        public string Title
        {
            get; set;
        } = string.Empty;

        public DateTime Time
        {
            get; set;
        }
        public int TournamentId
        {
            get; set;
        }
        public Tournament Tournament
        {
            get; set;
        }   
    }
}