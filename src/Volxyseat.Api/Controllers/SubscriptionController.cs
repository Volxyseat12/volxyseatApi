using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Volxyseat.Domain.Core.Data;
using Volxyseat.Domain.Models.ClientModel;
using Volxyseat.Domain.Models.SubscriptionModel;
using Volxyseat.Domain.ViewModel;
using Volxyseat.Infrastructure.Repository;

namespace Volxyseat.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUnitOfWork _uow;

        public SubscriptionController(ISubscriptionRepository subscriptionRepository, IUnitOfWork uow)
        {
            _subscriptionRepository = subscriptionRepository;
            _uow = uow;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allSubscriptions = await _subscriptionRepository.GetAll();

            if (allSubscriptions == null)
            {
                return NotFound("Planos não encontrados");
            }

            var activeSubscriptions = allSubscriptions.Where(subscription => subscription.IsActive == true).ToList();

            if (activeSubscriptions.Count == 0)
            {
                return NotFound("Nenhum plano ativo encontrado");
            }

            return Ok(allSubscriptions);
        }

 

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var subscriptionClient = await _subscriptionRepository.GetById(Id);
            if (subscriptionClient == null)
            {
                return NotFound("Esse plano não foi encontrado");
            }

            return Ok(subscriptionClient);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] SubscriptionViewModel request)
        {
            if (request == null)
            {
                return BadRequest("O objeto de solicitação é nulo.");
            }

            var newClient = new Subscription
            {
                Id = Guid.NewGuid(),
                Type = request.Type,
                Description = request.Description,
                IsActive = request.IsActive,
                Price = request.Price
            };
            _subscriptionRepository.Add(newClient);

            await _uow.SaveChangesAsync();

            return Ok(newClient);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] SubscriptionViewModel request)
        {
            var existingSubscription = await _subscriptionRepository.GetById(request.Id);

            if (request.Id != existingSubscription.Id)
            {
                return BadRequest();
            }

            existingSubscription.Type = request.Type;
            existingSubscription.Price = request.Price;
            existingSubscription.Description = request.Description;
            
            _subscriptionRepository.Update(existingSubscription);
            await _uow.SaveChangesAsync();

            return Ok(existingSubscription);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> switchSubscription(Guid Id)
        {
            var existingSubscription = await _subscriptionRepository.GetById(Id);
            if(Id != existingSubscription.Id)
            {
                return BadRequest();
            }

            if (!existingSubscription.IsActive)
            {
                existingSubscription.IsActive = true;
            }
            else
            {
                existingSubscription.IsActive = false;
            }
            _subscriptionRepository.SwitchSubscription(existingSubscription);
            await _uow.SaveChangesAsync();

            return Ok(existingSubscription);
        }


    }
}
