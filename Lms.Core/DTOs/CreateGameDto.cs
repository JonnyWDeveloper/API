using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.DTOs
{
    public class CreateGameDto
    {
        public string Title
        {
            get; set;
        }
        public DateTime StartDate
        {
            get; set;
        } = DateTime.Now;
    }
}
