using PostOffice.Model.Models;
using PostOfiice.DAta.Infrastructure;
using PostOfiice.DAta.Repositories;
using System.Collections.Generic;
using System.Linq;
using System;

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

        TKBDAmount GetById(int id);

        bool CheckExist(string account, int month);

        void Save();
    }

    public class TKBDService : ITKBDService
    {
        private ITKBDRepository _tKBDRepository;
        private IUnitOfWork _unitOfWork;

        public TKBDService(ITKBDRepository tKBDRepository, IUnitOfWork unitOfWork)
        {
            this._tKBDRepository = tKBDRepository;
            this._unitOfWork = unitOfWork;
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