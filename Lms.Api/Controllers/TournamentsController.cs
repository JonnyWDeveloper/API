using AutoMapper;
using Lms.Core.DTOs;
using   Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Lms.Api.Controllers
{
    //[Route("api/[controller]")] Not liked by belgian MVP E-learning teacher.    
    [ApiController]
    [Route("api/tournaments")]
    public class TournamentsController : ControllerBase
    {
        private readonly LmsApiContext _context;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ProblemDetailsFactory problemDetailsFactory;

        public TournamentsController(LmsApiContext context, IUnitOfWork uow, IMapper mapper)
        {
            _context = context;
            _uow = uow;
            _mapper = mapper;
        }

        // GET: api/Tournaments  
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournament(bool includegames)
        {
            if (_context.Tournament == null)
            {
                return NotFound();
            }

            var events = await _uow.TournamentRepository.GetAllAsync(includegames);
            var dto = _mapper.Map<IEnumerable<TournamentDto>>(events);
            return Ok(dto);

            //return await _context.Tournament.ToListAsync();
        }

        // GET: api/Tournaments/Title (unique Title, works like an id)
        [HttpGet]
        [Route("{title}")]
        public async Task<ActionResult<TournamentDto>> GetTournament(string title, bool includegames)
        {
            if (string.IsNullOrWhiteSpace(title))
                return BadRequest();

            var tournament = await _uow.TournamentRepository.GetAsync(title, includegames);

            if (tournament == null)
                return NotFound();

            var dto = _mapper.Map<TournamentDto>(tournament);

            return Ok(dto);
        }

        // GET: api/Tournaments/5       
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Tournament>> GetTournament(int id)
        //{
        //    if (_context.Tournament == null)
        //    {
        //        return NotFound();
        //    }

        //    var tournament = await _uow.TournamentRepository.GetAsync(id);

        //    if (tournament == null)
        //        return NotFound();

        //    var dto = _mapper.Map<TournamentDto>(tournament);

        //    return Ok(dto);
        //}

        // PUT: api/Tournaments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournament(int id, Tournament tournament)
        {
            if (id != tournament.Id)
            {
                return BadRequest();
            }

            _context.Entry(tournament).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TournamentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tournaments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tournament>> PostTournament(Tournament tournament)
        {
            if (_context.Tournament == null)
            {
                return Problem("Entity set 'LmsApiContext.Tournament'  is null.");
            }
            _context.Tournament.Add(tournament);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTournament", new
            {
                id = tournament.Id
            }, tournament);
        }

        //[HttpPost]
        //public async Task<ActionResult<TournamentDto>> CreateTournament(CreateTournamentDto dto)
        //{
        //    if (await _uow.TournamentRepository.GetAsync(dto.Title) != null)
        //    {
        //        ModelState.AddModelError("Title", "Title exists");
        //        return BadRequest(ModelState);
        //    }

        //    var codeEvent = _mapper.Map<Tournament>(dto);
        //    await _uow.TournamentRepository.AddAsync(Tournament);
        //    await _uow.CompleteAsync();

        //    return CreatedAtAction(nameof(Tournament), new
        //    {
        //        name = Tournament.Title
        //    }, _mapper.Map<TournamentDto>(dto));
        //}

        // DELETE: api/Tournaments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            if (_context.Tournament == null)
            {
                return NotFound();
            }
            var tournament = await _context.Tournament.FindAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }

            _context.Tournament.Remove(tournament);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TournamentExists(int id)
        {
            return (_context.Tournament?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
