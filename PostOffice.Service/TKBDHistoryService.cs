using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using PostOfiice.DAta.Repositories;
using System.Collections.Generic;
using System.Linq;
using System;

namespace PostOffice.Service
{
    public interface ITKBDHistoryService
    {
        TKBDHistory Add(TKBDHistory tkbd);

        void Update(TKBDHistory tkbd);

        TKBDHistory Delete(int id);

        IEnumerable<TKBDHistory> GetAll();

        IEnumerable<TKBDHistory> GetAllDistinct();

        IEnumerable<TKBDHistory> GetAll(string keyword);

        IEnumerable<TKBDHistory> Search(string keyword, int page, int pageSize, string sort, out int totalRow);

        TKBDHistory GetById(int id);
        IEnumerable<TKBDHistory> GetByAccount(string acc);
        IEnumerable<TKBDHistory> GetAllByUserName(string userName);

        void Save();
    }

    public class TKBDHistoryService : ITKBDHistoryService
    {
        private ITKBDHistoryRepository _tkbdRepository;
        private IApplicationUserRepository _userRepository;
        private IApplicationGroupRepository _groupRepository;
        private IUnitOfWork _unitOfWork;

        public TKBDHistoryService(ITKBDHistoryRepository tKBDHistoryRepository, IUnitOfWork unitOfwork, IApplicationUserRepository userRepository, IApplicationGroupRepository groupRepository)
        {
            this._tkbdRepository = tKBDHistoryRepository;
            this._unitOfWork = unitOfwork;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
        }

        public TKBDHistory Add(TKBDHistory tkbd)
        {
            return _tkbdRepository.Add(tkbd);
        }

        public TKBDHistory Delete(int id)
        {
            return _tkbdRepository.Delete(id);
        }

        public IEnumerable<TKBDHistory> GetAll()
        {
            return _tkbdRepository.GetAll();
        }

        public IEnumerable<TKBDHistory> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _tkbdRepository.GetMulti(x => x.Name.Contains(keyword));
            }
            else
            {
                return _tkbdRepository.GetAll();
            }
        }

        public IEnumerable<TKBDHistory> GetByAccount(string acc)
        {
            return _tkbdRepository.GetMulti(x => x.Account == acc).ToList();
        }

        public TKBDHistory GetById(int id)
        {
            return _tkbdRepository.GetSingleByID(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<TKBDHistory> Search(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _tkbdRepository.GetMulti(x => x.Status && x.Account.Contains(keyword));

            totalRow = query.OrderByDescending(x => x.CreatedDate).Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<TKBDHistory> GetAllDistinct()
        {
            return _tkbdRepository.GetAll().OrderBy(x=>x.TransactionDate).GroupBy(x => x.Account).Select(y => y.First()).ToList();
        }

        public void Update(TKBDHistory tkbd)
        {
            _tkbdRepository.Update(tkbd);
        }

        public IEnumerable<TKBDHistory> GetAllByUserName(string userName)
        {
            var user = _userRepository.getByUserName(userName);
            var listGroup = _groupRepository.GetListGroupByUserId(user.Id);

            bool IsManager = false;
            bool IsAdministrator = false;

            foreach (var item in listGroup)
            {
                string name = item.Name;
                if (name == "Manager")
                {
                    IsManager = true;
                }
                if (name == "Administrator")
                {
                    IsAdministrator = true;
                }
            }
            if (IsAdministrator)
            {
                return _tkbdRepository.GetAll();
            }
            else
            {
                if (IsManager)
                {
                    return _tkbdRepository.GetAllByUserName(userName);

                }
                else
                {
                    return _tkbdRepository.GetMulti(x => x.UserId == user.Id).ToList();
                }
            }
        }
    }
}