using Gvn.GvnAI.Dictionary.Application.Features.Quiz.Commands.CompleteQuiz;
using Gvn.GvnAI.Dictionary.Application.Features.Quiz.Commands.StartQuiz;
using Gvn.GvnAI.Dictionary.Application.Features.Quiz.Commands.SubmitAnswer;
using Gvn.GvnAI.Dictionary.Application.Features.Quiz.Queries.GetLeaderboard;
using Gvn.GvnAI.Dictionary.Application.Features.Quiz.Queries.GetNextQuestion;
using Gvn.GvnAI.Dictionary.Application.Features.Quiz.Queries.GetQuizResult;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gvn.GvnAI.Dictionary.API.Controllers;

[Route("api/[controller]")]
public class QuizController(IMediator mediator) : DictionaryControllerBase
{
    [Authorize]
    [HttpPost("start")]
    public async Task<IActionResult> Start()
    {
        var userId = GetUserId();
        var result = await mediator.Send(new StartQuizCommand(userId));
        return HandleResult(result);
    }

    [Authorize]
    [HttpGet("{sessionId:guid}/next")]
    public async Task<IActionResult> NextQuestion(Guid sessionId)
    {
        var result = await mediator.Send(new GetNextQuestionQuery(sessionId));
        return HandleResult(result);
    }

    [Authorize]
    [HttpPost("{sessionId:guid}/answer")]
    public async Task<IActionResult> SubmitAnswer(Guid sessionId, [FromBody] SubmitAnswerRequest request)
    {
        var result = await mediator.Send(new SubmitAnswerCommand(
            sessionId, request.WordId, request.SelectedOptionId, request.CorrectOptionId, request.ResponseTimeMs));
        return HandleResult(result);
    }

    [Authorize]
    [HttpPost("{sessionId:guid}/complete")]
    public async Task<IActionResult> Complete(Guid sessionId)
    {
        var result = await mediator.Send(new CompleteQuizCommand(sessionId));
        return HandleResult(result);
    }

    [Authorize]
    [HttpGet("{sessionId:guid}/result")]
    public async Task<IActionResult> GetResult(Guid sessionId)
    {
        var result = await mediator.Send(new GetQuizResultQuery(sessionId));
        return HandleResult(result);
    }

    [HttpGet("leaderboard")]
    public async Task<IActionResult> Leaderboard([FromQuery] int top = 10)
    {
        var result = await mediator.Send(new GetLeaderboardQuery(top));
        return HandleResult(result);
    }

}

public record SubmitAnswerRequest(Guid WordId, Guid? SelectedOptionId, Guid CorrectOptionId, int ResponseTimeMs);
