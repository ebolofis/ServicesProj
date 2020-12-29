using AutoMapper;
using PmsDBModels.Protel.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace PmsDBModels
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            /*Examples START*/

            //CreateMap<buchDTO, buchDTO>();

            /*

            CreateMap<BuchDTO, BasicReservationModel>()
                .ForMember(dest => dest.ArrivalDate, orig => orig.MapFrom(src => src.globdvon))
                .ForMember(dest => dest.DepartureDate, orig => orig.MapFrom(src => src.globdbis))
                .ForMember(dest => dest.Charge, orig => orig.MapFrom(src => src.preis))
                .ForMember(dest => dest.Voucher, orig => orig.MapFrom(src => src.string1));
            */

            /*Examples END*/


        }
    }
}
