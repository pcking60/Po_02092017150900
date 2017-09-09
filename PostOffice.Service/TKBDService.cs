﻿using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using PostOfiice.DAta.Repositories;
using System.Collections.Generic;
using System.Linq;
using System;
using PostOffice.Common.ViewModels.ExportModel;

namespace PostOffice.Service
{
    public interface ITKBDService
    {
        TKBDAmount Add(TKBDAmount tkbd);

        void Update(TKBDAmount tkbd);

        TKBDAmount Delete(int id);

        IEnumerable<TKBDAmount> GetAll();
        IEnumerable<TKBDAmount> GetAllByMoney();

        IEnumerable<TKBDAmount> GetAllDistinct();

        IEnumerable<TKBDAmount> GetAll(string keyword);

        IEnumerable<TKBDAmount> Search(string keyword, int page, int pageSize, string sort, out int totalRow);
        IEnumerable<TKBD_Export_Template> Export_TKBD_By_Condition(string fromDate, string toDate, int districtId, int poId, string currentUser, string userSelected);

        TKBDAmount GetById(int id);

        bool CheckExist(string account, int month);

        void Save();
    }

    public class TKBDService : ITKBDService
    {
        private ITKBDRepository _tKBDRepository;
        private IUnitOfWork _unitOfWork;
        private IApplicationUserRepository _userRepository;

        public TKBDService(ITKBDRepository tKBDRepository, IUnitOfWork unitOfWork, IApplicationUserRepository userRepository)
        {
            this._tKBDRepository = tKBDRepository;
            this._unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public TKBDAmount Add(TKBDAmount tkbd)
        {
            return _tKBDRepository.Add(tkbd);
        }

        public bool CheckExist(string account, int month)
        {
            return _tKBDRepository.GetMulti(x => x.Account == account && x.Month == month).FirstOrDefault() != null ? true : false;
        }

        public TKBDAmount Delete(int id)
        {
            return _tKBDRepository.Delete(id);
        }

        public IEnumerable<TKBD_Export_Template> Export_TKBD_By_Condition(string fromDate, string toDate, int districtId, int poId, string currentUser, string userSelected)
        {
            // define role of user
            bool isAdmin = _userRepository.CheckRole(currentUser, "Administrator");
            bool isManager = _userRepository.CheckRole(currentUser, "Manager");

            //get user info
            var user = _userRepository.getByUserId(userSelected);
            string userId = null;
            if (user != null)
            {
                userId = user.Id;
            }

            if (isAdmin) //is admin
            {
                if (districtId == 0)
                {
                    return _tKBDRepository.Export_By_Time(fromDate, toDate);
                }
                else
                {
                    if (poId == 0)
                    {
                        return _tKBDRepository.Export_By_Time_District(fromDate, toDate, districtId);
                    }
                    else // po id and district id not null
                    {
                        if (user == null) //po && district are not null && user null
                        {
                            return _tKBDRepository.Export_By_Time_District_Po(fromDate, toDate, districtId, poId);
                        }
                        else // po && district && user are not null
                        {
                            return _tKBDRepository.Export_By_Time_District_Po_User(fromDate, toDate, districtId, poId, userSelected);
                        }
                    }
                }

            }
            else
            {
                if (isManager) // is manager
                {
                    if (poId == 0)
                    {
                        return _tKBDRepository.Export_By_Time_District(fromDate, toDate, districtId);
                    }
                    else // po id and district id not null
                    {
                        if (userId == null) //po && district are not null && user null
                        {
                            return _tKBDRepository.Export_By_Time_District_Po(fromDate, toDate, districtId, poId);
                        }
                        else // po && district && user are not null
                        {
                            return _tKBDRepository.Export_By_Time_District_Po_User(fromDate, toDate, districtId, poId, userSelected);
                        }
                    }
                }
                else //is basic user
                {
                    return _tKBDRepository.Export_By_Time_District_Po_User(fromDate, toDate, districtId, poId, currentUser);
                }
            }


        }

        public IEnumerable<TKBDAmount> GetAll()
        {
            return _tKBDRepository.GetAll();
        }

        public IEnumerable<TKBDAmount> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _tKBDRepository.GetMulti(x => x.Account.Contains(keyword));
            }
            else
            {
                return _tKBDRepository.GetAll();
            }
        }

        public IEnumerable<TKBDAmount> GetAllByMoney()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TKBDAmount> GetAllDistinct()
        {
            return _tKBDRepository.GetAll().GroupBy(x => x.Account).Select(y => y.First()).ToList();
        }

        public TKBDAmount GetById(int id)
        {
            return _tKBDRepository.GetSingleByID(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<TKBDAmount> Search(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _tKBDRepository.GetMulti(x => x.Status && x.Account.Contains(keyword));

            totalRow = query.OrderByDescending(x => x.CreatedDate).Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public void Update(TKBDAmount tkbd)
        {
            _tKBDRepository.Update(tkbd);
        }
    }
}