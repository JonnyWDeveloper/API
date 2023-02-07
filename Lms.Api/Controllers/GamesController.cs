﻿using System;
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

    [Route("api/tournaments/{title}/games")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly LmsApiContext _context;
        private IUnitOfWork _uow; //readonly? Not in: CodeEvents though!
        private readonly IMapper _mapper;
        private readonly ProblemDetailsFactory problemDetailsFactory;



        public GamesController(LmsApiContext context, IUnitOfWork uow, IMapper mapper)
        {
            _context = context;
            _uow = uow;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGamesForTournament(string title)
        {
            if (await _uow.TournamentRepository.GetAsync(title) is null)
            {
                return NotFound(problemDetailsFactory.CreateProblemDetails(HttpContext,
                                                                          StatusCodes.Status404NotFound,
                                                                          title: "Tournament ´Does not exist",
                                                                          detail: $"The Tournament {title} does not exist"));
            }

            var games = await _uow.TournamentRepository.GetAsync(title);
            return Ok(_mapper.Map<IEnumerable<GameDto>>(games));
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGame()
        {
          if (_context.Game == null)
          {
              return NotFound();
          }
            return await _context.Game.ToListAsync();
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
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

            return game;
        }

        // PUT: api/Games/5
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

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
          if (_context.Game == null)
          {
              return Problem("Entity set 'LmsApiContext.Game'  is null.");
          }
            _context.Game.Add(game);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGame", new { id = game.Id }, game);
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
