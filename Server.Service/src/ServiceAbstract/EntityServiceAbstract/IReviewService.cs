using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Service.src.DTO;

namespace Server.Service.src.ServiceAbstract.EntityServiceAbstract;

public interface IReviewService
{
    public Task<IEnumerable<ReviewReadDto>> GetAllReviewsAsync(QueryOptions options);
    public Task<IEnumerable<ReviewReadDto>> GetAllReviewsByUserAsync(QueryOptions options, Guid userId);
    public Task<IEnumerable<ReviewReadDto>> GetAllReviewsByProductIdAsync(QueryOptions options, Guid productId);
    public Task<ReviewReadDto> GetReviewByIdAsync(Guid reviewId);
    public Task<ReviewReadDto> CreateReviewAsync(Guid userId, ReviewCreateDto review);
    public Task<ReviewReadDto> UpdateReviewByIdAsync(Guid reviewId, UpdateReviewsDTO updateReviewsDTO);
    public Task<bool> DeleteReviewByIdAsync(Guid reviewId);
}
