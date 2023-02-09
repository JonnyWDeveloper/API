using Lms.Core.DTOs;
using Lms.Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Lms.Api.Filters
{
    public class TournamentExistsFilter : ActionFilterAttribute
    {
        private readonly string actionArg;
        private readonly IUnitOfWork uow;
        private readonly ProblemDetailsFactory problemDetailsFactory;

        public TournamentExistsFilter(string actionArg, IUnitOfWork uow, ProblemDetailsFactory problemDetailsFactory)
        {
            this.actionArg = actionArg;
            this.uow = uow;
            this.problemDetailsFactory = problemDetailsFactory;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue(actionArg, out object? dto))
            {
                if (dto is CreateTournamentDto createTournamentDto)
                {
                    var tournament = await uow.TournamentRepository.GetAsync(createTournamentDto.Title);
                    if (tournament is not null)
                    {
                        context.Result = new BadRequestObjectResult(problemDetailsFactory.CreateProblemDetails(context.HttpContext,
                                                                         StatusCodes.Status400BadRequest,
                                                                         title: "Tournament already exists",
                                                                         detail: $"The tournament {createTournamentDto.Title}  exists"));
                    }
                }
            }

            await base.OnActionExecutionAsync(context, next);
        }
    }
}
