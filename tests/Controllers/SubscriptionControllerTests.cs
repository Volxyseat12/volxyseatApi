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

namespace tests2.Controllers
{
    public class SubscriptionControllerTests
    {
        private readonly Mock<ISubscriptionRepository> _subscriptionMock;
        private readonly SubscriptionController _controller;
        private readonly Mock<IUnitOfWork> _uow;
        public SubscriptionControllerTests()
        {
            _subscriptionMock = new Mock<ISubscriptionRepository>();
            _uow = new Mock<IUnitOfWork>();
            _controller = new SubscriptionController(_subscriptionMock.Object, _uow.Object);

        }

        [Fact]
        public async Task GetById_WhenCalled_ReturnOk()
        {
            var id = Guid.NewGuid();
            var result = await _controller.GetById(id);
            Assert.IsType<OkResult>(result);
        }
    }

}
