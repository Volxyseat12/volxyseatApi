using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volxyseat.Api.Controllers;
using Volxyseat.Domain.Core.Data;
using Volxyseat.Domain.Models.SubscriptionModel;
using Xunit;

namespace Tests2.Controllers
{
    public class SubscriptionControllerTests
    {
        private readonly Mock<ISubscriptionRepository> _subscriptionMock;
        private readonly SubscriptionController _controller;
        private readonly Mock<IUnitOfWork> _uow;

        public SubscriptionControllerTests()
        {
            _uow = new Mock<IUnitOfWork>();
            _subscriptionMock = new Mock<ISubscriptionRepository>();
            _controller = new SubscriptionController(_subscriptionMock.Object, _uow.Object) ;
        }

        [Fact]
        public async Task GetById_WhenCalled_ReturnOkResult()
        {
            var id = Guid.NewGuid();
            var result = await _controller.GetById(id);
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetById_WhenCalledWithValidId_ReturnsOkResult()
        {
            // Arrange"
            var validId = Guid.Parse("fe36502c-a7d5-4e49-a4c0-307ca20a348e"); // Substitua pelo ID válido
            var subscription = new Subscription
            {
                Id = validId,
            };

            _subscriptionMock.Setup(repo => repo.GetById(validId))
                              .ReturnsAsync(subscription);
            // Act
            var result = await _controller.GetById(validId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

    }
}
