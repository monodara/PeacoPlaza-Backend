using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Core.src.RepoAbstract;
using Server.Service.src.DTO;
using Server.Service.src.ServiceImplement.EntityServiceImplement;

namespace Server.Test.src.Service.Data;

public class ReviewServiceTest
{
    private ReviewService _reviewService;
    Mock<IReviewRepo> _mockReviewRepo = new Mock<IReviewRepo>();


    [Fact]
    public async void CreateReviewAsync_CreatesAReview_ReturnsTheReview()
    {
        var userId = Guid.Parse("1972b1f3-bf6b-4f99-b46e-34abaf608ae9");
        var orderedProductId = Guid.Parse("ff05a8a2-b397-4833-aa80-f8291b0a4518");
        var review = new Review(1.0, "Absolutely garbage.I dont recommend it at all", userId, orderedProductId);
        var createReviewDTO = new CreateReviewDTO(review.Rating, review.Comment, review.UserId, review.OrderedProductId, null);

        _mockReviewRepo.Setup(x => x.CreateReviewAsync(It.IsAny<Review>(), null)).ReturnsAsync(review);
        _reviewService = new ReviewService(_mockReviewRepo.Object);

        var createdReview = await _reviewService.CreateReviewAsync(createReviewDTO);

        Assert.Equal(review.Id, createdReview.Id);
    }

    [Fact]
    public async void GetAllReviewsByProductIdAsync_ReturnsAllReviews()
    {
        var productId = Guid.NewGuid();
        var options = new QueryOptions() { PageNo = 1, PageSize = 10 };

        List<Review> reviewsByProduct = new()
        {
            new Review(1.0,"Waste of money",Guid.NewGuid(),Guid.NewGuid()),
            new Review(4.9,"Great deal for the product",Guid.NewGuid(),Guid.NewGuid()),
        };

        _mockReviewRepo.Setup(x => x.GetAllReviewsByProductIdAsync(options, productId)).ReturnsAsync(reviewsByProduct);
        _reviewService = new ReviewService(_mockReviewRepo.Object);

        var results = await _reviewService.GetAllReviewsByProductIdAsync(options, productId);

        Assert.Equal(2, results.Count());
    }

    [Fact]
    public async void GetReviewByIdAsync_ThrowsException_ForInvalidId()
    {
        var review = new Review(3.0, "Okayish product", Guid.NewGuid(), Guid.NewGuid());
        _mockReviewRepo.Setup(x => x.GetReviewByIdAsync(Guid.NewGuid())).ReturnsAsync(review);

        _reviewService = new ReviewService(_mockReviewRepo.Object);
        await Assert.ThrowsAsync<InvalidDataException>(async () => await _reviewService.GetReviewByIdAsync(Guid.NewGuid()));
    }

    [Theory]
    [ClassData(typeof(ReviewServiceTestData))]
    public void UpdateReviewByIdAsync_UpdatingWithValidValues_ReturnsUpdatedReview(UpdateReviewsDTO updatedReview)
    {
        var userId = Guid.NewGuid();
        var orderdProductId = Guid.NewGuid();

        var oldReview = new Review(3.4, "Okayish product", userId, orderdProductId);
        var newReview = updatedReview.UpdateReview(oldReview);


        _mockReviewRepo.Setup(x => x.GetReviewByIdAsync(oldReview.Id)).ReturnsAsync(oldReview);

        _mockReviewRepo.Setup(x => x.UpdateReviewByIdAsync(oldReview.Id, newReview)).ReturnsAsync(newReview);

        _reviewService = new ReviewService(_mockReviewRepo.Object);

        var result = _reviewService.UpdateReviewByIdAsync(oldReview.Id, updatedReview);

        Assert.Equal(3.0, newReview.Rating);
    }
}
