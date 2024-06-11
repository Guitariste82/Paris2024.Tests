using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Paris2024.Controllers;
using Paris2024.Interfaces;
using Paris2024.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paris2024.Tests.Controler
{
    /// <summary>
    /// Test Offer Controller
    /// </summary>
    public class OfferControllerTests
    {
        private OfferController _offerController;
        private readonly ILogger<HomeController> _logger;
        private readonly IOfferRepository _offerRepository;

        public OfferControllerTests()
        {
            // Dependancy
            _logger = A.Fake<ILogger<HomeController>>();
            _offerRepository = A.Fake<IOfferRepository>();

            //SUT
            _offerController = new OfferController(_logger, _offerRepository);
        }

        [Fact]
        public void OfferController_Index_ReturnsSuccess()
        {
            // Arrange
            var offers = A.Fake<IEnumerable<Offer>>();
            var offerTypes = A.Fake<IEnumerable<OfferType>>();
            int offerTypeId = 1;
            string searchString = "Football";

            A.CallTo(() => _offerRepository.GetOffers(offerTypeId, searchString)).Returns(offers);

            // Act
            var result = _offerController.Index();
            // Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]
        public void OfferController_Details_ReturnsSuccess()
        {
            // Arrange
            var Offerid = 1;
            var offers = A.Fake<Offer>();
           
            A.CallTo(() => _offerRepository.GetOfferById(Offerid)).Returns(offers);

            // Act
            var result = _offerController.Details(Offerid);
            // Assert
            result.Should().BeOfType<Task<IActionResult>>();
        }
    }
}

