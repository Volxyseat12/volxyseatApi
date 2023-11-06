using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Volxyseat.Domain.Core.Data;
using Volxyseat.Domain.Models.ClientModel;
using Volxyseat.Domain.ViewModel;
using Volxyseat.Infrastructure.Repository;

namespace Volxyseat.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ClientController(IClientRepository clientRepository, IUnitOfWork uow, IMapper mapper)
        {
            _mapper = mapper;
            _clientRepository = clientRepository;
            _uow = uow;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allClients = await _clientRepository.GetAll();

            if (allClients == null)
            {
                return NotFound("Nenhum cliente encontrado.");
            }

            return Ok(allClients);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var selectedClient = await _clientRepository.GetById(Id);
            if (selectedClient == null)
            {
                return NotFound("Esse cliente não foi encontrado");
            }

            return Ok(selectedClient);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ClientViewModel request)
        {
            if (request == null)
            {
                return BadRequest("O objeto de solicitação é nulo.");
            }


            var newClient = _mapper.Map<ClientViewModel, Client>(request);
            _clientRepository.Add(newClient);

            await _uow.SaveChangesAsync();

            return Ok(newClient);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ClientViewModel request)
        {
            var existingClient = await _clientRepository.GetById(request.Id);

            if (request.Id != existingClient.Id)
            {
                return BadRequest();
            }

            _mapper.Map(request, existingClient);

            _clientRepository.Update(existingClient);
            await _uow.SaveChangesAsync();

            return Ok(existingClient);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var clientToDelete = await _clientRepository.GetById(Id);

            if (clientToDelete == null)
            {
                return BadRequest();
            }

            _clientRepository.DeleteClient(clientToDelete);
            await _uow.SaveChangesAsync();
            return Ok(clientToDelete);
        }
    }
}
