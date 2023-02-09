using AutoMapper;
using Lms.Api.Filters;
using Lms.Core.DTOs;
using   Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Lms.Data.Migrations;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Lms.Api.Controllers
{
    //[Route("api/[controller]")] Not liked by belgian MVP E-learning teacher.    
    [ApiController]
    [Route("api/tournaments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes("application/json", "application/xml")]
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
                return NotFound(problemDetailsFactory.CreateProblemDetails(HttpContext,
                                                                         StatusCodes.Status404NotFound,
                                                                         title: "Tournaments - Do not exist",
                                                                         detail: $"The Tournaments do not exist"));
            }

            var tournaments = await _uow.TournamentRepository.GetAllAsync(includegames);
            var dto = _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
            return Ok(dto);
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
                return NotFound(problemDetailsFactory.CreateProblemDetails(HttpContext,
                                                                          StatusCodes.Status404NotFound,
                                                                          title: "Tournament does not exist",
                                                                          detail: $"The Tournament {title} does not exist"));

            var dto = _mapper.Map<TournamentDto>(tournament);

            return Ok(dto);
        }

        [HttpPost]
        [TypeFilter(typeof(TournamentExistsFilter), Arguments = new object[] { "dto" })]
        public async Task<ActionResult<TournamentDto>> CreateTournament(CreateTournamentDto dto)
        {
            //if (await _uow.TournamentRepository.GetAsync(dto.Title) != null)
            //{
            //    ModelState.AddModelError("Title", "Title exists");
            //    return BadRequest(ModelState);
            //}

            var tournament = _mapper.Map<Tournament>(dto);
            await _uow.TournamentRepository.AddAsync(tournament);
            await _uow.CompleteAsync();

            return CreatedAtAction(nameof(GetTournament), new
            {
                name = tournament.Title
            }, _mapper.Map<TournamentDto>(dto));
        }
       
        [HttpPut("{title}")]
        public async Task<ActionResult<TournamentDto>> PutTournament(string title, TournamentDto dto)
        {
            var tournament = await _uow.TournamentRepository.GetAsync(title);
            if (tournament == null)
                return NotFound(problemDetailsFactory.CreateProblemDetails(HttpContext,
                                                                          StatusCodes.Status404NotFound,
                                                                          title: "Tournament does not exist",
                                                                          detail: $"The Tournament {title} does not exist"));

            _mapper.Map(dto, tournament);
            await _uow.CompleteAsync();

            return Ok(_mapper.Map<TournamentDto>(tournament));
        }

        [HttpPatch("{title}")]
        public async Task<ActionResult<TournamentDto>> PatchTournament(string title, JsonPatchDocument<TournamentDto> patchDocument)
        {
            var tournament = await _uow.TournamentRepository.GetAsync(title, true);
            if (tournament == null)
                return NotFound(problemDetailsFactory.CreateProblemDetails(HttpContext,
                                                                          StatusCodes.Status404NotFound,
                                                                          title: "Tournament does not exist",
                                                                          detail: $"The Tournament {title} does not exist"));

            var dto = _mapper.Map<TournamentDto>(tournament);

            patchDocument.ApplyTo(dto, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _mapper.Map(dto, tournament);
            await 
                _uow.CompleteAsync();

            return Ok(_mapper.Map<TournamentDto>(tournament));
        }

        // DELETE: api/Tournaments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            if (_context.Tournament == null)
            {
                return NotFound(problemDetailsFactory.CreateProblemDetails(HttpContext,
                                                                          StatusCodes.Status404NotFound,
                                                                          title: "Tournament does not exist",
                                                                          detail: $"The Tournament with Id: {id} does not exist"));
            }
            var tournament = await _context.Tournament.FindAsync(id);
            if (tournament == null)
            {
                return NotFound(problemDetailsFactory.CreateProblemDetails(HttpContext,
                                                                          StatusCodes.Status404NotFound,
                                                                          title: "Tournament does not exist",
                                                                          detail: $"The Tournament with Id: {id} does not exist"));
            }

            _context.Tournament.Remove(tournament);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TournamentExists(int id)
        {
            return (_context.Tournament?.Any(e => e.Id == id)).GetValueOrDefault();
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
        //[HttpPut("{id}")] - SCAFFOLDED
        //public async Task<IActionResult> PutTournament(int id, Tournament tournament)
        //{
        //    if (id != tournament.Id)
        //    {
        //        return BadRequest();
        //    }
        //    _context.Entry(tournament).State = EntityState.Modified;
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TournamentExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //    return NoContent();
        //}

        // POST: api/Tournaments - SCAFFOLDED
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Tournament>> PostTournament(Tournament tournament)
        //{
        //    if (_context.Tournament == null)
        //    {
        //        return Problem("Entity set 'LmsApiContext.Tournament'  is null.");
        //    }
        //    _context.Tournament.Add(tournament);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetTournament", new
        //    {
        //        id = tournament.Id
        //    }, tournament);
        //}
    }
}
