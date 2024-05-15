using AutoMapper;
using Moq;
using Server.Core.src.Common;
using Server.Core.src.Entity;
using Server.Core.src.RepoAbstract;
using Server.Core.src.ValueObject;
using Server.Infrastructure.src.Database;
using Server.Service.src.DTO;
using Server.Service.src.ServiceImplement.EntityServiceImplement;
using Server.Service.src.Shared;

namespace Server.Test.src;

public class CategoryServiceTest
{
    private static IMapper _mapper;
    private readonly CategoryService _categoryService;
    private readonly Mock<ICategoryRepo> _mockCategoryRepo = new Mock<ICategoryRepo>();
    private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();

    public CategoryServiceTest()
    {
        if (_mapper == null)
        {
            var mappingConfig = new MapperConfiguration(map =>
            {
                map.AddProfile(new MapperProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
        }
        _categoryService = new CategoryService(_mockCategoryRepo.Object, _mapper);
    }

    [Fact]
    public async Task GetAllCategoriesAsync_ShouldReturnAllCategories()
    {
        // Arrange
        var categories = SeedingData.GetCategories();
        var options = new QueryOptions { PageNo = 0, PageSize = 6, sortType = SortType.byTitle, sortOrder = SortOrder.asc };
        _mockCategoryRepo.Setup(x => x.GetAllAsync(options)).ReturnsAsync(categories);
        _mockMapper.Setup(m => m.Map<IEnumerable<CategoryReadDTO>>(categories))
            .Returns(categories.Select(c => new CategoryReadDTO { Name = c.Name, Image = c.Image }));

        // Act
        var result = await _categoryService.GetAllCategoriesAsync(options);

        // Assert
        Assert.Equal(6, result.Count());
    }

    [Fact]
    public async void GetAllCategoriesAsync_ShouldInvokeRepoMethod()
    {
        var options = new QueryOptions { PageNo = 0, PageSize = 6, sortType = SortType.byTitle, sortOrder = SortOrder.asc };
        await _categoryService.GetAllCategoriesAsync(options);
        _mockCategoryRepo.Verify(repo => repo.GetAllAsync(options), Times.Once);
    }

    [Fact]
    public async void GetCategoryById_ShouldInvokeRepoMethod()
    {
        Category category =  new Category("", "");
        _mockCategoryRepo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(category);

        await _categoryService.GetCategoryById(It.IsAny<Guid>());

        _mockCategoryRepo.Verify(repo => repo.GetOneByIdAsync(It.IsAny<Guid>()), Times.Once);
    }
}