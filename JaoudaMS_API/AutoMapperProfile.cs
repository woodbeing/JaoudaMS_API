using AutoMapper;
using JaoudaMS_API.DTOs;
using JaoudaMS_API.Models;

namespace JaoudaMS_API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Box
            CreateMap<BoxDto, Box>();
            CreateMap<Box, BoxDto>();
            
            //Employee
            CreateMap<EmployeeDto, Employee>();
            CreateMap<Employee, EmployeeDto>();
            
            //InBox
            CreateMap<InBoxDto, InBox>();
            CreateMap<InBox, InBoxDto>();

            //Payment 
            CreateMap<PaymentDto, Payment>();
            CreateMap<Payment, PaymentDto>();

            //Product
            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>();

            //Purchase
            CreateMap<PurchaseDto, Purchase>();
            CreateMap<Purchase, PurchaseDto>();

            //PurchaseInfo
            CreateMap<PurchaseInfoDto, PurchaseInfo>();
            CreateMap<PurchaseInfo, PurchaseInfoDto>();

            //Supplier
            CreateMap<SupplierDto, Supplier>();
            CreateMap<Supplier, SupplierDto>();

            //Trip
            CreateMap<TripDto, Trip>();
            CreateMap<Trip, TripDto>();

            //TripInfo
            CreateMap<TripInfoDto, TripInfo>();
            CreateMap<TripInfo, TripInfoDto>();

            //TripWaste
            CreateMap<TripWasteDto, TripWaste>();
            CreateMap<TripWaste, TripWasteDto>();

            //TripCharge
            CreateMap<TripChargeDto, TripCharge>();
            CreateMap<TripCharge, TripChargeDto>();

            //Truck
            CreateMap<TruckDto, Truck>();
            CreateMap<Truck, TruckDto>();

            //Waste
            CreateMap<WasteDto, Waste>();
            CreateMap<Waste, WasteDto>();
        }
    }
}
