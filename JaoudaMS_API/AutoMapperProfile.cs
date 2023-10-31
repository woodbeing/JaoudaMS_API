using AutoMapper;
using JaoudaMS_API.DTOs;
using JaoudaMS_API.Models;

namespace JaoudaMS_API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<EmployeeDto, Employee>();
            CreateMap<Employee, EmployeeDto>();
            CreateMap<BoxDto, Box>();
            CreateMap<Box, BoxDto>();
        }
    }
}
