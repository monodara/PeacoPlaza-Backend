using Server.Core.src.Common;
using Server.Service.src.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Server.Service.src.ServiceAbstract.EntityServiceAbstract;

namespace Server.Controller.src.Controller;

[ApiController]
[Route("api/v1/reviews")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    // [Authorize]
    [HttpGet]
    public async Task<IEnumerable<ReviewReadDto>> GetAllReviewsAsync([FromQuery] QueryOptions options)
    {
        return await _reviewService.GetAllReviewsAsync(options);
    }

    [Authorize]
    [HttpGet("my_reviews")]
    public async Task<IEnumerable<ReviewReadDto>> GetAllReviewsByUserAsync([FromQuery] QueryOptions options)
    {
        var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userId = Guid.Parse(userClaims);
        return await _reviewService.GetAllReviewsByUserAsync(options, userId);
    }

    [HttpGet("product/{id}")]
    public async Task<IEnumerable<ReviewReadDto>> GetAllReviewsByProductIdAsync([FromQuery] QueryOptions options, [FromRoute] Guid id)
    {
        return await _reviewService.GetAllReviewsByProductIdAsync(options, id);
    }

    [HttpGet("{id}")]
    public async Task<ReviewReadDto> GetReviewByIdAsync([FromRoute] Guid id)
    {
        return await _reviewService.GetReviewByIdAsync(id);
    }
    [Authorize]
    [HttpPost]
    public async Task<ReviewReadDto> CreateReviewAsync([FromBody] ReviewCreateDto review)
    {
        var userClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userId = Guid.Parse(userClaims);
        return await _reviewService.CreateReviewAsync(userId, review);
    }

    [HttpPatch("{id}")]
    public async Task<ReviewReadDto> UpdateReviewByIdAsync([FromRoute] Guid id, [FromBody] UpdateReviewsDto updateReview)
    {
        return await _reviewService.UpdateReviewByIdAsync(id, updateReview);
    }

    [HttpDelete("{id}")]
    public async Task<bool> DeleteReviewByIdAsync([FromRoute] Guid id)
    {
        return await _reviewService.DeleteReviewByIdAsync(id);
    }
}