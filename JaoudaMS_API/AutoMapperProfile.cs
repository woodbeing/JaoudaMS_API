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

            //Payment 
            CreateMap<PaymentDto, Payment>();
            CreateMap<Payment, PaymentDto>();

            //Product
            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>();

            //Purchase
            CreateMap<PurchaseDto, Purchase>();
            CreateMap<Purchase, PurchaseDto>();

            //PurchaseProduct
            CreateMap<PurchaseProductDto, PurchaseProduct>();
            CreateMap<PurchaseProduct, PurchaseProductDto>();

            //PurchaseBox
            CreateMap<PurchaseBoxDto, PurchaseBox>();
            CreateMap<PurchaseBox, PurchaseBoxDto>();

            //PurchaseWaste
            CreateMap<PurchaseWasteDto, PurchaseWaste>();
            CreateMap<PurchaseWaste, PurchaseWasteDto>();

            //Supplier
            CreateMap<SupplierDto, Supplier>();
            CreateMap<Supplier, SupplierDto>();

            //Trip
            CreateMap<TripDto, Trip>();
            CreateMap<Trip, TripDto>();

            //TripBox
            CreateMap<TripBoxDto, TripBox>();
            CreateMap<TripBox, TripBoxDto>();

            //TripCharge
            CreateMap<TripChargeDto, TripCharge>();
            CreateMap<TripCharge, TripChargeDto>();

            //TripProduct
            CreateMap<TripProductDto, TripProduct>();
            CreateMap<TripProduct, TripProductDto>();

            //TripWaste
            CreateMap<TripWasteDto, TripWaste>();
            CreateMap<TripWaste, TripWasteDto>();

            //Truck
            CreateMap<TruckDto, Truck>();
            CreateMap<Truck, TruckDto>();

            //Waste
            CreateMap<WasteDto, Waste>();
            CreateMap<Waste, WasteDto>();
        }
    }
}
