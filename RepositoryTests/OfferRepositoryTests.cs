using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Paris2024.Data;
using Paris2024.Models;
using Paris2024.Repositories;

namespace Paris2024.Tests.RepositoryTests;

public class OfferRepositoryTests
{
    [Fact]
    public async Task AdminOfferRepository_GetOfferById_ReturnsCorrectOffer()
    {
        // Arrange
        var dbContext = await GetDbContextAsync();
        var adminOfferRepository = new AdminOfferRepository(dbContext);

       var offer = new Offer()
        {
            Offer_Code = "FTB-103",
            Offer_Sport = "FOOTBALL",
            Offer_Description = "Match finale new",
            Offer_ImagePath = string.Empty,
            Offer_UnitPrice = 100,
            Offer_EventDate = DateTime.Now,
            OfferTypeId = 1
        };
        dbContext.Offers.Add(offer);
        await dbContext.SaveChangesAsync();

        // Act
        var getOfferById = await adminOfferRepository.GetOfferById(offer.OfferId);

        // Assert
        getOfferById.Should().NotBeNull();
        getOfferById.Offer_Code.Should().Be("FTB-103");
    }




    [Fact]
    public async Task AdminOfferRepository_AddOffer_AddsOfferToDatabase()
    {
        // Arrange
        var offer = new Offer()
        {
            Offer_Code = "FTB-100",
            Offer_Sport = "FOOTBALL",
            Offer_Description = "Match finale Homme",
            Offer_ImagePath = string.Empty,
            Offer_UnitPrice = 80,
            Offer_EventDate = DateTime.Now,
            OfferTypeId = 1
        };

        var dbContext = await GetDbContextAsync();
        var adminOfferRepository = new AdminOfferRepository(dbContext);

        // Act
        await adminOfferRepository.AddOffer(offer);

        // Assert
        var addedOffer = await dbContext.Offers.FirstOrDefaultAsync(o => o.Offer_Code == "FTB-100");
        addedOffer.Should().NotBeNull();
        addedOffer.Offer_Code.Should().Be("FTB-100");
    }


    [Fact]
    public async Task AdminOfferRepository_UpdateOffer_UpdatesOfferInDatabase()
    {
        // Arrange
        var dbContext = await GetDbContextAsync();
        var adminOfferRepository = new AdminOfferRepository(dbContext);

        // ajoute offre fictive pour la mettre à jour ensuite et vérifier ainsi que le méthode UpdateOffer fonctionne
        var offer = new Offer()
        {
            Offer_Code = "FTB-102",
            Offer_Sport = "FOOTBALL",
            Offer_Description = "Match finale avant modification",
            Offer_ImagePath = string.Empty,
            Offer_UnitPrice = 80,
            Offer_EventDate = DateTime.Now,
            OfferTypeId = 1
        };
        dbContext.Offers.Add(offer);
        await dbContext.SaveChangesAsync();

        // Modifie l'offre
        offer.Offer_Description = "Match finale avec modification";

        // Act
        await adminOfferRepository.UpdateOffer(offer);

        // Assert
        var updatedOffer = await dbContext.Offers.FirstOrDefaultAsync(o => o.Offer_Code == "FTB-102");
        updatedOffer.Should().NotBeNull();
        updatedOffer.Offer_Description.Should().Be("Match finale avec modification");
    }




    [Fact]
    public async Task AdminOfferRepository_DeleteOffer_RemovesOfferFromDatabase()
    {
        // Arrange
        var offer = new Offer()
        {
            Offer_Code = "FTB-101",
            Offer_Sport = "FOOTBALL",
            Offer_Description = "Match finale femme",
            Offer_ImagePath = string.Empty,
            Offer_UnitPrice = 80,
            Offer_EventDate = DateTime.Now,
            OfferTypeId = 1
        };

        var dbContext = await GetDbContextAsync();
        var adminOfferRepository = new AdminOfferRepository(dbContext);

        // ajoute offre fictive pour la supprimer ensuite et vérifier ainsi que le méthode DeleteOffer fonctionne
        dbContext.Offers.Add(offer);
        await dbContext.SaveChangesAsync();

        // Act
        await adminOfferRepository.DeleteOffer(offer);

        // Assert
        var deletedOffer = await dbContext.Offers.FirstOrDefaultAsync(o => o.Offer_Code == "FTB-100");
        deletedOffer.Should().BeNull();
    }


    // Utilisation d'une database virtuelle pour ne pas affecter la Bdd Online
    private async Task<ApplicationDbContext> GetDbContextAsync()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var databaseContext = new ApplicationDbContext(options);
        databaseContext.Database.EnsureCreated();
        if (await databaseContext.Offers.CountAsync() < 0)
        {
            for (int i = 0; i < 10; i++)
            {
                databaseContext.Offers.Add(
                    new Offer()
                    {
                        OfferId = i,
                        Offer_Code = "FTB-0" + i,
                        Offer_Sport = "FOOTBALL",
                        Offer_Description = "Match demi-finale Homme",
                        Offer_ImagePath = string.Empty,
                        Offer_UnitPrice = 40,
                        Offer_EventDate = DateTime.Now,
                        OfferTypeId = 1
                    });
                await databaseContext.SaveChangesAsync();
            }
        }
        return databaseContext;
    }
}
