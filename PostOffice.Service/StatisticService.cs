using PostOffice.Common.ViewModels;
using PostOfiice.DAta.Repositories;
using System.Collections.Generic;
using System;
using PostOffice.Common.ViewModels.ExportModel;

namespace PostOffice.Service
{
    public interface IStatisticService
    {
        IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(string fromDate, string toDate);
        IEnumerable<UnitStatisticViewModel> GetUnitStatistic(string fromDate, string toDate);
        IEnumerable<ReportFunction1> ReportFunction1(string fromDate, string toDate);
        IEnumerable<ReportFunction1> ReportFunction1(string fromDate, string toDate, int districtId);
        IEnumerable<ReportFunction1> ReportFunction1(string fromDate, string toDate, int districtId, int unitId);
        IEnumerable<ReportFunction1> RP1(string fromDate, string toDate, int districtId, int unitId);
        IEnumerable<RP1Advance> RP1Advance();
        IEnumerable<Export_By_Service_Group_And_Time> Export_By_Service_Group_And_Time(string fromDate, string toDate, int mainGroup, int districtId, int poId, string currentUser);
        IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP> Export_By_Service_Group_And_Time_District_Po_BCCP(string fromDate, string toDate, int districtId, int poId, string currentUser);
    }

    public class StatisticService : IStatisticService
    {
        private ITransactionRepository _transactionRepository;
        private IStatisticRepository _statisticRepository;
        private IApplicationUserRepository _userRepository;
        private IPORepository _poRepository;

        public StatisticService(IPORepository poRepository, ITransactionRepository transactionRepository, IStatisticRepository statisticRepository, IApplicationUserRepository userRepository)
        {
            _transactionRepository = transactionRepository;
            _statisticRepository = statisticRepository;
            _userRepository = userRepository;
            _poRepository = poRepository;
        }

        public IEnumerable<RevenueStatisticViewModel> GetRevenueStatistic(string fromDate, string toDate)
        {
            return _transactionRepository.GetRevenueStatistic(fromDate, toDate);
        }
        public IEnumerable<UnitStatisticViewModel> GetUnitStatistic(string fromDate, string toDate)
        {
            return _statisticRepository.GetUnitStatistic(fromDate, toDate);
        }

        public IEnumerable<ReportFunction1> ReportFunction1(string fromDate, string toDate)
        {
            return _statisticRepository.ReportFunction1(fromDate, toDate);
        }
        public IEnumerable<ReportFunction1> ReportFunction1(string fromDate, string toDate, int districtId)
        {
            return _statisticRepository.ReportFunction1(fromDate, toDate, districtId);
        }
        public IEnumerable<ReportFunction1> ReportFunction1(string fromDate, string toDate, int districtId, int unitId)
        {
            return _statisticRepository.ReportFunction1(fromDate, toDate, districtId, unitId);
        }

        public IEnumerable<ReportFunction1> RP1(string fromDate, string toDate, int districtId, int unitId)
        {
            return _statisticRepository.RP1(fromDate, toDate, districtId, unitId);
        }

        public IEnumerable<RP1Advance> RP1Advance() 
        {
            return _statisticRepository.RP1Advance();
        }

        public IEnumerable<Export_By_Service_Group_And_Time> Export_By_Service_Group_And_Time(string fromDate, string toDate, int mainGroup, int districtId, int poId, string currentUser)
        {

            bool isAdmin = _userRepository.CheckRole(currentUser, "Administrator");
            bool isManager = _userRepository.CheckRole(currentUser, "Manager");

            var user = _userRepository.getByUserName(currentUser);
            string userId = null;
            if(user!=null)
            {
                userId = user.Id;
            }

            if (isAdmin)
            {
                if (districtId == 0)
                {
                    return _statisticRepository.Export_By_Service_Group_And_Time(fromDate, toDate, mainGroup);
                }
                else
                {
                    if (poId == 0)
                    {
                        return _statisticRepository.Export_By_Service_Group_And_Time_District(fromDate, toDate, mainGroup, districtId);
                    }
                    else
                    {
                        return _statisticRepository.Export_By_Service_Group_And_Time_District_Po(fromDate, toDate, mainGroup, districtId, poId);
                    }
                }
                
            }
            else
            {
                if (isManager)
                {
                    if (poId == 0)
                    {
                        return _statisticRepository.Export_By_Service_Group_And_Time_District(fromDate, toDate, mainGroup, districtId);
                    }
                    else
                    {
                        return _statisticRepository.Export_By_Service_Group_And_Time_District_Po(fromDate, toDate, mainGroup, districtId, poId);
                    }
                }
                else
                {
                    return _statisticRepository.Export_By_Service_Group_And_Time_User(fromDate, toDate, mainGroup, userId);
                }
            }
            
        }

        public IEnumerable<Export_By_Service_Group_And_Time_District_Po_BCCP> Export_By_Service_Group_And_Time_District_Po_BCCP(string fromDate, string toDate, int districtId, int poId, string currentUser)
        {
            // define role of user
            bool isAdmin = _userRepository.CheckRole(currentUser, "Administrator");
            bool isManager = _userRepository.CheckRole(currentUser, "Manager");

            //get user info
            var user = _userRepository.getByUserName(currentUser);
            string userId = null;
            if (user != null)
            {
                userId = user.Id;
            }

            if (isAdmin) //is admin
            {
                if (districtId == 0)
                {
                    return _statisticRepository.Export_By_Service_Group_And_Time_BCCP(fromDate, toDate);
                }
                else
                {
                    if (poId == 0)
                    {
                        return _statisticRepository.Export_By_Service_Group_And_Time_District_BCCP(fromDate, toDate, districtId);
                    }
                    else // po id and district id not null
                    {
                        return _statisticRepository.Export_By_Service_Group_And_Time_District_Po_BCCP(fromDate, toDate, districtId, poId);
                    }
                }

            }
            else
            {
                if (isManager) // is manager
                {
                    if (poId == 0)
                    {
                        return _statisticRepository.Export_By_Service_Group_And_Time_District_BCCP(fromDate, toDate, districtId);
                    }
                    else // po id and district id not null
                    {
                        return _statisticRepository.Export_By_Service_Group_And_Time_District_Po_BCCP(fromDate, toDate, districtId, poId);
                    }
                }
                else //is basic user
                {
                    return _statisticRepository.Export_By_Service_Group_And_Time_District_Po_User_BCCP(fromDate, toDate, districtId, poId, userId);
                }
            }
        }
    }
}