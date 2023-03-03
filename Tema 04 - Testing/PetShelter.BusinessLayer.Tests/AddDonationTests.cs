using Moq;
using PetShelter.BusinessLayer.Validators;
using PetShelter.DataAccessLayer.Models;
using PetShelter.DataAccessLayer.Repository;

namespace PetShelter.BusinessLayer.Tests;

public class AddDonationTests
{
    [Fact]
    public async Task GivenValidRequest_WhenAddDonation_DonationIsAdded()
    {
        var mockDonationRepository = new Mock<IDonationRepository>();
        var donationServiceSut = new DonationService(mockDonationRepository.Object, new AddDonationRequestValidator());

        var request = new AddDonationRequest
        {
            Amount = 10
        };
        await donationServiceSut.AddDonation(request);

        mockDonationRepository.Verify(x => x.Add(It.Is<Donation>(d => d.Amount == request.Amount)), Times.Once);
    }

    [Fact]
    public async Task GivenRequestWithMissingAmount_WhenAddDonation_DonationIsNotAdded()
    {
        var mockDonationRepository = new Mock<IDonationRepository>();
        var donationServiceSut = new DonationService(mockDonationRepository.Object, new AddDonationRequestValidator());

        var request = new AddDonationRequest();
        await Assert.ThrowsAsync<ArgumentException>(() => donationServiceSut.AddDonation(request));

        mockDonationRepository.Verify(x => x.Add(It.IsAny<Donation>()), Times.Never);
    }

    [Fact]
    public async Task GivenRequestWithInvalidDonor_WhenAddDonation_DonationIsNotAdded()
    {
        var mockDonationRepository = new Mock<IDonationRepository>();
        var donationServiceSut = new DonationService(mockDonationRepository.Object, new AddDonationRequestValidator());

        var request = new AddDonationRequest
        {
            Amount = 50,
            Donor = new Person
            {
                Name = "A",
                IdNumber = "1234567890123",
                DateOfBirth = new DateTime(2005, 3, 3)
            }
        };

        await Assert.ThrowsAsync<ArgumentException>(() => donationServiceSut.AddDonation(request));

        mockDonationRepository.Verify(x => x.Add(It.IsAny<Donation>()), Times.Never);
    }

    [Fact]
    public async Task GivenRequestWithValidDonor_WhenAddDonation_DonationIsAdded()
    {
        var mockDonationRepository = new Mock<IDonationRepository>();
        var donationServiceSut = new DonationService(mockDonationRepository.Object, new AddDonationRequestValidator());

        var request = new AddDonationRequest
        {
            Amount = 50,
            Donor = new Person
            {
                Name = "John Doe",
                IdNumber = "1234567890123",
                DateOfBirth = new DateTime(1980, 3, 3)
            }
        };

        await donationServiceSut.AddDonation(request);

        mockDonationRepository.Verify(x => x.Add(It.Is<Donation>(d => d.Amount == request.Amount && d.Donor.Name == request.Donor.Name)), Times.Once);
    }
}
