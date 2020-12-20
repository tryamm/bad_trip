using AutoMapper;
using BusinessLayer.DTO;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Comon
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Drug, DrugDTO>().ReverseMap();
            CreateMap<Patient, PatientDTO>().ReverseMap();
            //CreateMap<List<Drug>, List<DrugDTO>>().ReverseMap();
        }
    }
    //public class MappingService
    //{

    //    private readonly IMapper _mapper;

    //    public MappingService(IMapper mapper)
    //    {
    //        _mapper = mapper;
    //        var config = new MapperConfiguration(cfg =>
    //        {
    //            cfg.CreateMap<Drug, DrugDTO>().ReverseMap();
    //            cfg.CreateMap<List<Drug>, List<DrugDTO>>().ReverseMap();
    //        });
    //        _mapper = config.CreateMapper();
    //    }

    //    public T Map<T>(object source)
    //    {
    //        return _mapper.Map<T>(source);
    //    }

    //}
}
