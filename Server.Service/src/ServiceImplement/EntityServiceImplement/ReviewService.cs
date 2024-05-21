using Server.Core.src.Common;
using Server.Core.src.RepoAbstract;
using Server.Service.src.DTO;
using Server.Service.src.ServiceAbstract.EntityServiceAbstract;

namespace Server.Service.src.ServiceImplement.EntityServiceImplement;

public class ReviewService : IReviewService
{
    private readonly IReviewRepo _reviewrepo;

    public ReviewService(IReviewRepo reviewrepo)
    {
        _reviewrepo = reviewrepo;
    }
    public async Task<ReviewReadDto> CreateReviewAsync(Guid userId, ReviewCreateDto review)
    {
        if (review == null) throw new ArgumentNullException("Review cannot be null");
        var reviewImages = review.ReviewImages;
        var createdReview = await _reviewrepo.CreateReviewAsync(review.CreateReviews(userId), reviewImages);
        return new ReviewReadDto().ReadReviews(createdReview);
    }

    public async Task<IEnumerable<ReviewReadDto>> GetAllReviewsAsync(QueryOptions options)
    {
        var reviews = await _reviewrepo.GetAllReviewsAsync(options);
        return reviews.Select(r => new ReviewReadDto().ReadReviews(r)).ToList();
    }

    public async Task<IEnumerable<ReviewReadDto>> GetAllReviewsByProductIdAsync(QueryOptions options, Guid productId)
    {
        if (productId == Guid.Empty) throw new ArgumentException("Product Id cannot be empty");
        var reviews = await _reviewrepo.GetAllReviewsByProductIdAsync(options, productId);
        return reviews.Select(r => new ReviewReadDto().ReadReviews(r));
    }

    public async Task<IEnumerable<ReviewReadDto>> GetAllReviewsByUserAsync(QueryOptions options, Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentNullException("User Id should be a valid input");
        var results = await _reviewrepo.GetAllReviewsByUserAsync(options, userId);
        return results.Select(r => new ReviewReadDto().ReadReviews(r));
    }

    public async Task<ReviewReadDto> GetReviewByIdAsync(Guid reviewId)
    {
        if (reviewId == Guid.Empty) throw new ArgumentNullException("Review Id cannot be empty");
        var result = await _reviewrepo.GetReviewByIdAsync(reviewId);
        if (result == null) throw new InvalidDataException("The review Id provided is incorrect");
        return new ReviewReadDto().ReadReviews(result);
    }

    public async Task<ReviewReadDto> UpdateReviewByIdAsync(Guid reviewId, UpdateReviewsDto updateReviewsDto)
    {
        if (reviewId == Guid.Empty) throw new ArgumentNullException("Review Id cannot be empty");
        if (updateReviewsDto == null) throw new ArgumentNullException("Review cannot be null");

        var review = await _reviewrepo.GetReviewByIdAsync(reviewId);

        if (review == null) throw new InvalidDataException("The review Id provided is incorrect");
        var updatedReview = updateReviewsDto.UpdateReview(review);

        var result = await _reviewrepo.UpdateReviewByIdAsync(reviewId, updatedReview);
        return new ReviewReadDto().ReadReviews(result);
    }

    public Task<bool> DeleteReviewByIdAsync(Guid reviewId)
    {
        if (reviewId == Guid.Empty) throw new ArgumentNullException("Review Id cannot be empty");
        return _reviewrepo.DeleteReviewByIdAsync(reviewId);
    }
}
