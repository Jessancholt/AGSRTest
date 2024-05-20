using FluentAssertions;
using Moq;
using Test.Core.Exceptions;
using Test.UnitTests.Infrastructure;
using Xunit;

namespace Test.UnitTests.Services
{
    [Trait(Constants.Category, Constants.UnitTest)]
    public class PatientsServiceTests
    {
        [Fact]
        public async Task GetAllReturnsResultTest()
        {
            // Arrange
            var verifier = new PatientsServiceVerifierBuilder()
                .SetPatientsServiceGetAllResult()
                .SetupPatientsServiceCacheWrapperGetList()
                .Build();

            // Act
            var result = await verifier.Service.GetAsync(It.IsAny<CancellationToken>());

            // Assert
            result.Should().BeEquivalentTo(verifier.GetAllPatients);

            verifier.VerifyPostsControllerCacheWrapperGetList();
        }

        [Fact]
        public async Task GetByIdReturnsResultTest()
        {
            // Arrange
            var verifier = new PatientsServiceVerifierBuilder()
                .SetPatientsServiceGetByIdResult()
                .SetupPatientsServiceCacheWrapperGet()
                .Build();

            // Act
            var result = await verifier.Service.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>());

            // Assert
            result.Should().Be(verifier.GetPatient);

            verifier.VerifyPostsControllerCacheWrapperGet();
        }

        [Fact]
        public async Task GetByIdReturnsNotFoundTest()
        {
            // Arrange
            var verifier = new PatientsServiceVerifierBuilder()
                .SetupPatientsServiceCacheWrapperGet()
                .Build();

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundCoreException>(() => verifier.Service.GetAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));

            verifier.VerifyPostsControllerCacheWrapperGet();
        }
    }
}
