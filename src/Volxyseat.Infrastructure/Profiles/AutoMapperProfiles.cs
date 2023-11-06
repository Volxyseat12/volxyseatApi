using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volxyseat.Domain.Models.ClientModel;
using Volxyseat.Domain.ViewModel;

namespace Volxyseat.Infrastructure.Profiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Client, ClientViewModel>().ReverseMap();
        }
    }
}
