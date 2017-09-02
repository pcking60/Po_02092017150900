using AutoMapper;
using PostOffice.Common.ViewModels;
using PostOffice.Model.Models;
using PostOffice.Web.Models;

namespace PostOffice.Web.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<ApplicationGroup, ApplicationGroupViewModel>();
            Mapper.CreateMap<ApplicationRole, ApplicationRoleViewModel>();
            Mapper.CreateMap<ApplicationUser, ApplicationUserViewModel>();
            Mapper.CreateMap<District, DistrictViewModel>();
            Mapper.CreateMap<PO, POViewModel>();
            Mapper.CreateMap<ServiceGroup, ServiceGroupViewModel>();
            Mapper.CreateMap<PostOffice.Model.Models.Service, ServiceViewModel>();
            Mapper.CreateMap<MainServiceGroup, MainServiceGroupViewModel>();
            Mapper.CreateMap<PropertyService, PropertyServiceViewModel>();
            Mapper.CreateMap<Transaction, TransactionViewModel>();
            Mapper.CreateMap<TransactionDetail, TransactionDetailViewModel>();
            Mapper.CreateMap<Model.Models.Service, ReportServiceViewModel>();
            Mapper.CreateMap<ServiceViewModel, ReportServiceViewModel>();
            Mapper.CreateMap<TKBDAmount, TKBDAmountViewModel>();
            Mapper.CreateMap<TKBDHistory, TKBDHistoryViewModel>();
            Mapper.CreateMap<TransactionViewModel, RP2_1>();
            Mapper.CreateMap<TransactionViewModel, MainGroup1>();
            Mapper.CreateMap<TransactionViewModel, MainGroup3>();
        }
    }
}