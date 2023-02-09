using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.DTOs
{
    public class TournamentDto
    {
        public string Title
        {
            get; set;
        }

        public DateTime StartDate
        {
            get; set;
        } = DateTime.Now;
        public DateTime EndDate
        {
            get; set;
        } = DateTime.Now.AddMonths(3);

        public ICollection<GameDto> Games
        {
            get; set;
        }

    }
}
