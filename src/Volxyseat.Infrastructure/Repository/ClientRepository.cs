using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volxyseat.Domain.Core.Data;
using Volxyseat.Domain.Models.ClientModel;
using Volxyseat.Infrastructure.Data;

namespace Volxyseat.Infrastructure.Repository
{
    public class ClientRepository : RepositoryBase<Client, Guid> , IClientRepository
    {
        public ClientRepository(ApplicationDataContext applicationDataContext) : base(applicationDataContext)
        {
            
        }

        public void DeleteClient(Client client)
        {
            _entity.Remove(client);
        }
    }
}
