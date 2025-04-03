using BookingSystem.DataAccessLayer;
using BookingSystem.DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Tests.BookingSystem.DataAccessLayer.Tests;

public class AppDbContextTests
{
    private AppDbContext _context;
    public AppDbContextTests()
    {
        _context = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("BookingSystemTests").Options);
    }

    [Fact]
    public async Task SaveChangesBooleanAsync_AddingNewMembers_ReturnsTrue()
    {
        //Arrange
        var memeber = new Member()
        {
            FirstName = "Sophie",
            LastName = "Davis",
            BookingCount = 1,
            CreatedDate = DateTime.Now,
            IsActive = true
        };

        //Act
        await _context.Members.AddAsync(memeber);
        var result = await _context.SaveChangesBooleanAsync(CancellationToken.None);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task SaveChangesBooleanAsync_AddingNewInventory_ReturnsTrue()
    {
        //Arrange
        var inventory = new Inventory()
        {
            Title = "Bali",
            Description = "Suspendisse congue erat ac ex venenatis mattis. Sed finibus sodales nunc, nec maximus",
            RemainingCount = 5,
            ExpirationDate = DateTime.Now,
            IsActive = true
        };

        //Act
        await _context.Inventory.AddAsync(inventory);
        var result = await _context.SaveChangesBooleanAsync(CancellationToken.None);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task SaveChangesBooleanAsync_AddingNewBookingDetails_ReturnsTrue()
    {
        //Arrange
        var bookingDetails = new BookingDetails()
        {
            Inventory = new Inventory()
            {
                Title = "Bali",
                Description = "Suspendisse congue erat ac ex venenatis mattis. Sed finibus sodales nunc, nec maximus",
                RemainingCount = 5,
                ExpirationDate = DateTime.Now,
                IsActive = true
            },
            Member = new Member()
            {
                FirstName = "Sophie",
                LastName = "Davis",
                BookingCount = 1,
                CreatedDate = DateTime.Now,
                IsActive = true
            },
            BookingTime = DateTime.Now,
            IsActive = true
        };

        //Act
        await _context.BookingDetails.AddAsync(bookingDetails);
        var result = await _context.SaveChangesBooleanAsync(CancellationToken.None);

        //Assert
        Assert.True(result);
    }


    [Fact]
    public async Task SaveChangesBooleanAsync_CallWithoutModificationToContext_ReturnsFalse()
    {
        //Arrange
        CancellationToken cancellationToken = CancellationToken.None;

        //Act
        var result = await _context.SaveChangesBooleanAsync(cancellationToken);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void OnModelCreating_ShouldLoadConfigurationsFromAssembly()
    {
        //Arrange
        
        //Act
        var entity = _context.Model.FindEntityType(typeof(Member));

        //Assert
        Assert.NotNull(entity);
        Assert.True(entity.GetTableName() != null);        
    }
}
