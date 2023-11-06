using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volxyseat.Domain.Core.Data;

namespace Volxyseat.Domain.Models.ClientModel
{
    public interface IClientRepository : IRepository<Client, Guid>
    {
        void DeleteClient(Client client);
    }
}
