using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Service.src.DTO;

namespace Server.Service.src.ServiceAbstract.EntityServiceAbstract;

public interface IReviewService
{
    public Task<IEnumerable<ReadReviewDTO>> GetAllReviewsAsync(QueryOptions options);
    public Task<IEnumerable<ReadReviewDTO>> GetAllReviewsByUserAsync(QueryOptions options, Guid userId);
    public Task<IEnumerable<ReadReviewDTO>> GetAllReviewsByProductIdAsync(QueryOptions options, Guid productId);
    public Task<ReadReviewDTO> GetReviewByIdAsync(Guid reviewId);
    public Task<ReadReviewDTO> CreateReviewAsync(Guid userId, CreateReviewDTO review);
    public Task<ReadReviewDTO> UpdateReviewByIdAsync(Guid reviewId, UpdateReviewsDTO updateReviewsDTO);
    public Task<bool> DeleteReviewByIdAsync(Guid reviewId);
}
