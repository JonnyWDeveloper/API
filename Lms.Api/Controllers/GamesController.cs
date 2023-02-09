using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.Core.Entities;
using Lms.Data.Data;
using Lms.Core.Repositories;
using Lms.Data.Repositories.Lms.Core.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Lms.Core.DTOs;

namespace Lms.Api.Controllers
{
    [ApiController]
    [Route("api/tournaments/{title}/games")]    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes("application/json", "application/xml")]
    public class GamesController : ControllerBase
    {
        private readonly LmsApiContext _context;
        private readonly IUnitOfWork _uow; 
        private readonly IMapper _mapper;
        private readonly ProblemDetailsFactory problemDetailsFactory;


        /// <summary>
        /// An API Controller handling the tasks resulting in showing the games
        ///for the appropriate DTO.
        /// </summary>
        /// <param name="context">The database context for the two DbSets.</param>
        /// <param name="uow">The interface IUnitOfWork calling the two repositories´methods.</param>
        /// <param name="mapper">The AutoMapper interface: executes a mapping from the source object<br></br> 
        /// to a new destination object with supplied mapping options.</param>
        public GamesController(LmsApiContext context, IUnitOfWork uow, IMapper mapper)
        {
            _context = context;
            _uow = uow;
            _mapper = mapper;

        }
        /// <summary>
        /// The Get method [HttpGet] for all the games belonging to a single tournament.
        /// </summary>
        /// <param name="title">The tournament title.</param>
        /// <returns>A single tournament by title and its games.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGamesForTournament(string title)
        {
            if (await _uow.TournamentRepository.GetAsync(title) is null) //Get Single Tournament
            {
                return NotFound(problemDetailsFactory.CreateProblemDetails(HttpContext,
                                                                          StatusCodes.Status404NotFound,
                                                                          title: "Tournament does not exist",
                                                                          detail: $"The Tournament {title} does not exist"));
            }

            var games = await _uow.GameRepository.GetAllAsync(title); //Get Games
            return Ok(_mapper.Map<IEnumerable<GameDto>>(games));
        }

        // GET: /games/id
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGame(string title, int id)
        {
            if (await _uow.TournamentRepository.GetAsync(title) is null)
            {
                return NotFound(problemDetailsFactory.CreateProblemDetails(HttpContext,
                                                                          StatusCodes.Status404NotFound,
                                                                          title: "Tournament ´Does not exist",
                                                                          detail: $"The Tournament {title} does not exist"));
            }

            var game = await _uow.GameRepository.GetAsync(title, id);

            if (game == null)
                return NotFound();

            return Ok(_mapper.Map<GameDto>(game));
        }

        // GET: /games/5
        //[HttpGet]
        //[Route("{id}")]
        //public async Task<ActionResult<Game>> GetGame(int id)
        //{
        //  if (_context.Game == null)
        //  {
        //      return NotFound();
        //  }
        //    var game = await _context.Game.FindAsync(id);

        //    if (game == null)
        //    {
        //        return NotFound();
        //    }

        //    return game;
        //}

        // PUT: /games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]        
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            _context.Entry(game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
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

        // POST: /Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GameDto>> CreateLecture(string title, CreateGameDto dto)
        {
            var tournament = await _uow.TournamentRepository.GetAsync(title);

            if (tournament is null)
            {
                return NotFound(problemDetailsFactory.CreateProblemDetails(HttpContext,
                                                                          StatusCodes.Status404NotFound,
                                                                          title: "Tournament does not exist",
                                                                          detail: $"The Tournament {title} doesent exist"));
            }

            var game = _mapper.Map<Game>(dto);
            game.Tournament = tournament;
            await _uow.GameRepository.AddAsync(game);
            await _uow.CompleteAsync();

            var created = _mapper.Map<GameDto>(game);

            return CreatedAtAction(nameof(GetGame), new
            {
                title = tournament.Title,
                id = created.Id
            }, created);



        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            if (_context.Game == null)
            {
                return NotFound();
            }
            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _context.Game.Remove(game);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GameExists(int id)
        {
            return (_context.Game?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
