using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using P4P.Data;
using P4P.Models;
using P4P.Repositories;

namespace P4P.IntegrationTests.Data;

public class LocationRepositoryTests
{
    private DbContextOptions<P4PContext> _options = null!;

    [SetUp]
    public async Task SetUp()
    {
        _options = new DbContextOptionsBuilder<P4PContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString(), b => b.EnableNullChecks(false))
            .Options;

        await using var context = new P4PContext(_options);
        await DbInitializer.SeedP4PContext(context);
    }

    [Test]
    public async Task LocationRepositoryGetAsyncTest_RetrieveAllLocations_AmountOfSeededLocationsRetrieved()
    {
        var expectedIds = DbInitializer.Locations.Value.Select(x => x.Id).ToArray();
        await using var context = new P4PContext(_options);
        var dbRepository = new GenericRepository<Location>(context);

        var response = await dbRepository.GetAsync();

        Assert.That(response.Count, Is.EqualTo(expectedIds.Length));
    }

    [Test]
    public async Task LocationRepositoryGetFirstAsyncTest_GetFirstInstanceOfLocation_LocationWithProvidedX()
    {
        var expectedLocation = DbInitializer.Locations.Value.Skip(1).Take(1).Single();
        await using var context = new P4PContext(_options);
        var dbRepository = new GenericRepository<Location>(context);

        var response = await dbRepository.GetFirstAsync(x => x.X == expectedLocation.X);

        Assert.That(response.Name, Is.EqualTo(expectedLocation.Name));
    }

    [Test]
    public async Task LocationRepositoryGetById_RetrieveRytisById_UserRytis()
    {
        var expectedJson = DbInitializer.Users.Value.Skip(1).Take(1).Single().ToJson();
        await using var context = new P4PContext(_options);
        var dbRepository = new GenericRepository<User>(context);

        var response = await dbRepository.GetByIdAsync(2);

        Assert.That(expectedJson, Is.EqualTo(response.ToJson()));
    }

    [Test]
    public async Task LocationRepositoryInsertAsync_RepeatingPrimaryKey_ThrowsArgumentException()
    {
        var location = DbInitializer.Locations.Value.Take(1).Single();
        await using var context = new P4PContext(_options);
        var dbRepository = new GenericRepository<Location>(context);

        Assert.ThrowsAsync<ArgumentException>(() => dbRepository.InsertAsync(location));
    }

    [Test]
    public async Task LocationRepositoryDeleteAsync_DeleteNewInstance_ThrowsDbUpdateConcurrencyException()
    {
        var newLocationInstance = new Location();
        await using var context = new P4PContext(_options);
        var dbRepository = new GenericRepository<Location>(context);

        Assert.ThrowsAsync<DbUpdateConcurrencyException> (() => dbRepository.DeleteAsync(newLocationInstance));
    }

    [Test]
    public async Task LocationRepositoryDeleteAsync_InstanceExists_NoExceptionAndEntityDoesNotExist()
    {
        var deleteLocation = DbInitializer.Locations.Value.Take(1).Single();
        await using var context = new P4PContext(_options);
        var dbRepository = new GenericRepository<Location>(context);

        Assert.DoesNotThrowAsync(() => dbRepository.DeleteAsync(deleteLocation));
        Assert.That((await dbRepository.GetAsync()).Select(x => x.Id), Does.Not.Contain(deleteLocation.Id));
    }

    [Test]
    public async Task LocationRepositoryUpdateAsync_UpdateNameForKebabine_NoExceptionAndEntityUpdated()
    {
        var location = DbInitializer.Locations.Value.Skip(2).Take(1).Single();
        await using var context = new P4PContext(_options);
        var dbRepository = new GenericRepository<Location>(context);

        location.Name = "GeraKebabine";

        Assert.DoesNotThrowAsync(() => dbRepository.UpdateAsync(location));
        Assert.That((await dbRepository.GetByIdAsync(location.Id)).Name, Is.EqualTo("GeraKebabine"));
    }
}
