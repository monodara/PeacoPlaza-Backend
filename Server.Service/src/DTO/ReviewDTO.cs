using Server.Core.src.Entity;

namespace Server.Service.src.DTO;

public class ReviewReadDto
{
    public double Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public DateTime ReviewDate { get; set; } = DateTime.Now;
    public ReviewReadDto ReadReviews(Review review)
    {
        return new ReviewReadDto()
        {
            Rating = review.Rating,
            Comment = review.Comment,
            UserId = review.UserId,
        };
    }
}

public class ReviewCreateDto
{
    public double Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime ReviewDate { get; set; } = DateTime.Now;
    // public Guid UserId { get; set; }
    public Guid OrderProductId { get; set; }
    public ReviewImage[]? ReviewImages { get; set; }
    // public ReviewCreateDto(double rating, string comment, Guid userId, Guid orderdProductId, ReviewImage[] reviewsImage)
    // {
    //     Rating = rating;
    //     Comment = comment;
    //     UserId = userId;
    //     OrderProductId = orderdProductId;
    //     ReviewImages = reviewsImage;
    // }
    public Review CreateReviews(Guid userId)
    {
        return new Review(Rating, Comment, userId, OrderProductId);
    }
}

public class UpdateReviewsDto
{
    public double Rating { get; set; }
    public string Comment { get; set; } = string.Empty;

    public UpdateReviewsDto(double rating, string comment)
    {
        Rating = rating;
        Comment = comment;
    }

    public Review UpdateReview(Review oldreview)
    {
        if (oldreview.Rating != 0.0 && !String.IsNullOrEmpty(oldreview.Comment))
        {
            oldreview.Rating = Rating;
            oldreview.Comment = Comment;
        }

        return oldreview;
    }
}
