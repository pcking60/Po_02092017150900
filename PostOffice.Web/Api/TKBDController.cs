﻿using AutoMapper;
using OfficeOpenXml;
using PostOffice.Common;
using PostOffice.Common.ViewModels;
using PostOffice.Common.ViewModels.ExportModel;
using PostOffice.Model.Models;
using PostOffice.Service;
using PostOffice.Web.Infrastructure.Core;
using PostOffice.Web.Infrastructure.Extensions;
using PostOffice.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PostOffice.Web.Api
{
    [RoutePrefix("api/tkbd")]
    [Authorize]
    public class TKBDController : ApiControllerBase
    {
        private ITKBDService _tkbdService;
        private ITKBDHistoryService _tkbdHistoryService;
        private IApplicationUserService _applicationUserService;
        private IDistrictService _districtService;
        private IPOService _poService;
        private IApplicationUserService _userService;

        public TKBDController(IApplicationUserService userService, IDistrictService districtService, IPOService poService, IErrorService errorService, ITKBDService tkbdService, ITKBDHistoryService tkbdHistoryService, IApplicationUserService applicationUserService) : base(errorService)
        {
            this._tkbdService = tkbdService;
            this._tkbdHistoryService = tkbdHistoryService;
            _applicationUserService = applicationUserService;
            _districtService = districtService;
            _poService = poService;
            _userService = userService;
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _tkbdService.GetAll().Where(x=>x.Status==true);
                totalRow = model.Count();
                var query = model.OrderBy(x => x.Id).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<TKBDAmount>, IEnumerable<TKBDAmountViewModel>>(query);

                foreach (var item in responseData)
                {
                    item.Name = _tkbdHistoryService.GetByAccount(item.Account).FirstOrDefault().Name;
                    item.Money = _tkbdHistoryService.GetByAccount(item.Account).Where(x=>x.TransactionDate.Value.Month<=item.Month).Sum(x => x.Money);
                    var s =_tkbdHistoryService.GetByAccount(item.Account).Where(x => x.TransactionDate.Value.Month == item.Month).FirstOrDefault();
                    if (s == null)
                    {
                        item.TransactionDate = _tkbdHistoryService.GetByAccount(item.Account).OrderByDescending(x => x.TransactionDate).FirstOrDefault().TransactionDate;
                    }
                    else
                    {
                        item.TransactionDate = s.TransactionDate;
                    }
                                       
                    string userId = _tkbdHistoryService.GetByAccount(item.Account).FirstOrDefault().UserId;                    
                    item.TransactionUser = _applicationUserService.getByUserId(userId).FullName;
                }
                //ban test lai thu
                var paginationSet = new PaginationSet<TKBDAmountViewModel>//sai ne ban.
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("export")]
        [HttpGet]
        public async Task<HttpResponseMessage> Export(HttpRequestMessage request, string fromDate, string toDate, int districtId, int functionId, int poId, string userId)
        {
            #region Config Export file

            string fileName = string.Concat("TKBD_" + DateTime.Now.ToString("yyyyMMddhhmmsss") + ".xlsx");
            var folderReport = ConfigHelper.GetByKey("ReportFolder");
            string filePath = HttpContext.Current.Server.MapPath(folderReport);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = Path.Combine(filePath, fileName);

            #endregion Config Export file

            ReportTemplate vm = new ReportTemplate();


            try
            {
                #region customFill Test

                District district = new District();
                PO po = new PO();
                ApplicationUser user = new ApplicationUser();

                // Thời gian để xuất dữ liệu
                vm.FromDate = DateTime.Parse(fromDate);
                vm.ToDate = DateTime.Parse(toDate);
                vm.CreatedBy = User.Identity.Name;

                //check param đầu vào

                #region data input

                if (districtId != 0)
                {
                    district = _districtService.GetById(districtId);
                    vm.District = district.Name;
                }
                if (poId != 0)
                {
                    po = _poService.GetByID(poId);
                    vm.Unit = po.Name;
                }
                if (!string.IsNullOrEmpty(userId))
                {
                    user = _userService.getByUserId(userId);
                    vm.user = user.FullName;
                }

                vm.Service = "Tiết kiệm bưu điện";

                #endregion data input

                string currentUser = User.Identity.Name;

                switch (functionId)
                {
                    #region case 1 Thống kê tổng hợp giao dịch phát sinh

                    case 1:
                        vm.FunctionName = "Thống kê tổng hợp giao dịch phát sinh";

                        var responseTKBD = _tkbdService.Export_TKBD_By_Condition(fromDate, toDate, districtId, poId, currentUser, userId);
                        var dataSource = Mapper.Map<IEnumerable<TKBD_Export_Template>, IEnumerable<TKBD_Export_Template_ViewModel>>(responseTKBD);
                        foreach (var item in dataSource)
                        {
                            var fullName = _userService.getByUserName(item.CreatedBy).FullName;
                            if (fullName != null)
                            {
                                item.FullName = fullName;
                            }
                        }
                        await ReportHelper.TKBD_Export_General(dataSource.ToList(), fullPath, vm);

                        break;

                    #endregion case 1 Bảng kê thu tiền tại bưu cục - tổng hợp

                    #region case 2 Thống kê chi tiết giao dịch phát sinh

                    case 2:
                        vm.FunctionName = "Thống kê chi tiết giao dịch phát sinh";

                        var responseTKBD_Detail = _tkbdService.Export_TKBD_Detail_By_Condition(fromDate, toDate, districtId, poId, currentUser, userId);
                        var dataSource_Detail = Mapper.Map<IEnumerable<TKBDAmount>, IEnumerable<TKBDAmountViewModel>>(responseTKBD_Detail);
                        foreach (var item in dataSource_Detail)
                        {
                            var fullName = _userService.getByUserName(item.CreatedBy).FullName;
                            item.Name = fullName;
                        }
                        await ReportHelper.TKBD_Export_Detail(dataSource_Detail.ToList(), fullPath, vm);
                        break;

                    #endregion case 2 Bảng kê thu tiền tại bưu cục - chi tiết

                    default:
                        vm.FunctionName = "Chức năng khác";
                        break;
                }

                #endregion customFill Test

                return request.CreateErrorResponse(HttpStatusCode.OK, Path.Combine(folderReport, fileName));
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        [Route("getallhistory")]
        [HttpGet]
        public HttpResponseMessage GetAllHistory(HttpRequestMessage request, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var userName = User.Identity.Name;
                var model = _tkbdHistoryService.GetAllByUserName(userName);
                totalRow = model.Count();
                var query = model.OrderBy(x => x.Id).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<TKBDHistory>, IEnumerable<TKBDHistoryViewModel>>(query);

                foreach (var item in responseData)
                {
                    item.TransactionUser = _applicationUserService.getByUserId(item.UserId).FullName;
                }
                             
                var paginationSet = new PaginationSet<TKBDHistoryViewModel>
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("gettkbd1day")]
        [HttpGet]
        public HttpResponseMessage GetHistory1Day(HttpRequestMessage request, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var userName = User.Identity.Name;
                var model = _tkbdHistoryService.GetAllByUserName(userName);
                totalRow = model.Count();
                var query = model.OrderBy(x => x.TransactionDate).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<TKBDHistory>, IEnumerable<TKBDHistoryViewModel>>(query);

                foreach (var item in responseData)
                {
                    item.TransactionUser = _applicationUserService.getByUserId(item.UserId).FullName;
                }

                var paginationSet = new PaginationSet<TKBDHistoryViewModel>
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("gettkbd30day")]
        [HttpGet]
        public HttpResponseMessage GetHistory30Day(HttpRequestMessage request, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var userName = User.Identity.Name;
                var model = _tkbdHistoryService.GetAllByUserName30Day(userName);
                totalRow = model.Count();
                var query = model.OrderBy(x => x.TransactionDate).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<TKBDHistory>, IEnumerable<TKBDHistoryViewModel>>(query);

                foreach (var item in responseData)
                {
                    item.TransactionUser = _applicationUserService.getByUserId(item.UserId).FullName;
                }

                var paginationSet = new PaginationSet<TKBDHistoryViewModel>
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("gettkbd7day")]
        [HttpGet]
        public HttpResponseMessage GetHistory7Day(HttpRequestMessage request, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var userName = User.Identity.Name;
                var model = _tkbdHistoryService.GetAllByUserName7Day(userName);
                totalRow = model.Count();
                var query = model.OrderBy(x => x.TransactionDate).Skip(page * pageSize).Take(pageSize);

                var responseData = Mapper.Map<IEnumerable<TKBDHistory>, IEnumerable<TKBDHistoryViewModel>>(query);

                foreach (var item in responseData)
                {
                    item.TransactionUser = _applicationUserService.getByUserId(item.UserId).FullName;
                }

                var paginationSet = new PaginationSet<TKBDHistoryViewModel>
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }

        [Route("import")]
        [HttpPost]
        public async Task<HttpResponseMessage> Import()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "Định dạng không được server hỗ trợ");
            }

            var root = HttpContext.Current.Server.MapPath("~/UploadedFiles/Excels");
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            var provider = new MultipartFormDataStreamProvider(root);
            Stream reqStream = Request.Content.ReadAsStreamAsync().Result;
            MemoryStream tempStream = new MemoryStream();
            reqStream.CopyTo(tempStream);

            tempStream.Seek(0, SeekOrigin.End);
            StreamWriter writer = new StreamWriter(tempStream);
            writer.WriteLine();
            writer.Flush();
            tempStream.Position = 0;

            StreamContent streamContent = new StreamContent(tempStream);
            foreach (var header in Request.Content.Headers)
            {
                streamContent.Headers.Add(header.Key, header.Value);
            }
            try
            {
                // Read the form data.
                streamContent.LoadIntoBufferAsync().Wait();
                //This is where it bugs out
                var result = await streamContent.ReadAsMultipartAsync(provider);
                //Upload files
                int addedCount = 0;
                foreach (MultipartFileData fileData in result.FileData)
                {
                    if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                    {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Yêu cầu không đúng định dạng");
                    }
                    string fileName = fileData.Headers.ContentDisposition.FileName;
                    if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                    {
                        fileName = fileName.Trim('"');
                    }
                    if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                    {
                        fileName = Path.GetFileName(fileName);
                    }

                    var fullPath = Path.Combine(root, fileName);
                    File.Copy(fileData.LocalFileName, fullPath, true);

                    //insert to DB
                    //var 
                    List<TKBDHistory> listItem = new List<TKBDHistory>();
                    listItem = this.ReadTKBDFromExcel(fullPath);
                    if (listItem.Count > 0)
                    {
                        foreach (var product in listItem)
                        {
                            _tkbdHistoryService.Add(product);
                            addedCount++;
                        }
                        _tkbdHistoryService.Save();
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Đã nhập thành công " + addedCount + " sản phẩm thành công.");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }            
            
        }

        private List<TKBDHistory> ReadTKBDFromExcel(string fullPath)
        {
            using (var package = new ExcelPackage(new FileInfo(fullPath)))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
                List<TKBDHistory> listTKBD = new List<TKBDHistory>();
                TKBDHistoryViewModel tkbdViewModel;
                TKBDHistory tkbdHistory;

                DateTimeOffset transactionDate;
                DateTimeOffset tranDate;
                decimal money;
                decimal rate;  

                for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                {
                    tkbdViewModel = new TKBDHistoryViewModel();
                    tkbdHistory = new TKBDHistory();

                    tkbdViewModel.Name = workSheet.Cells[i, 1].Value.ToString();
                    tkbdViewModel.CustomerId = workSheet.Cells[i, 2].Value.ToString();
                    tkbdViewModel.Account = workSheet.Cells[i, 3].Value.ToString();
                    if (DateTimeOffset.TryParse(workSheet.Cells[i, 4].Value.ToString(), out transactionDate))
                    {
                        string temp = transactionDate.ToString("yyyy-MM-dd");
                        DateTimeOffset.TryParse(temp, out tranDate);
                        tkbdViewModel.TransactionDate = tranDate;

                    }
                    decimal.TryParse(workSheet.Cells[i, 5].Value.ToString().Replace(",", ""), out money);
                    tkbdViewModel.Money = money;
                    decimal.TryParse(workSheet.Cells[i, 6].Value.ToString().Replace(",", ""), out rate);
                    tkbdViewModel.Rate = rate;
                    if(_applicationUserService.getByUserName(workSheet.Cells[i, 7].Value.ToString()) != null)
                    {
                        tkbdViewModel.UserId = _applicationUserService.getByUserName(workSheet.Cells[i, 7].Value.ToString()).Id;
                    }
                    else
                    {
                        tkbdViewModel.UserId = "Người dùng không tồn tại";
                    }                    
                    tkbdViewModel.CreatedBy = User.Identity.Name;
                    tkbdViewModel.CreatedDate = DateTime.Now;           
                    tkbdViewModel.Status = true;
                    tkbdHistory.UpdateTKBDHistory(tkbdViewModel);
                    listTKBD.Add(tkbdHistory);
                }
                return listTKBD;
            }
        }

        [Route("update")]
        [HttpGet]       
        public HttpResponseMessage Update(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    int days = 0;
                    var tkbdHistories = _tkbdHistoryService.GetAllDistinct().Where(x=>x.Status==true && x.TransactionDate.Value.Month<=DateTime.Now.Month-1);
                    int c = tkbdHistories.Count();             
                    foreach (var item in tkbdHistories)
                    {
                        decimal money = _tkbdHistoryService.GetByAccount(item.Account).Where(x => x.Status == true && x.TransactionDate.Value.Month <= DateTime.Now.Month - 1).Sum(x => x.Money) ?? 0;
                        
                        if (money <= 0)
                        {
                            var oldTransaction = _tkbdHistoryService.GetByAccount(item.Account);
                            foreach (var item1 in oldTransaction)
                            {
                                item1.Status = false;
                            }
                        }
                        else
                        {
                            TimeSpan s = new TimeSpan();
                            DateTimeOffset lastDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
                            DateTimeOffset firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month-1, 1);
                            s = lastDay.Subtract(item.TransactionDate??DateTimeOffset.UtcNow);
                            days = (int)s.TotalDays;
                            if (days > 31)
                            {
                                days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month-1);
                            }                           
                           
                            //if (firstDayOfMonth > item.TransactionDate)
                            //{
                            //    s = lastDay.Subtract(firstDayOfMonth);
                            //}
                            //else
                            //{
                            //    s = lastDay.Subtract(item.TransactionDate ?? DateTimeOffset.UtcNow);
                            //}
                            
                            //days = (int)s.TotalDays;
                            TKBDAmountViewModel vm = new TKBDAmountViewModel();
                            vm.Status = true;
                            vm.Account = item.Account;
                            vm.CreatedBy = User.Identity.Name;
                            vm.UserId = item.UserId;
                            vm.Month = DateTime.Now.Month-1;
                            vm.Amount = money * item.Rate * 20 * days / 1200 / 30 ?? 0;
                            TKBDAmount tkbd = new TKBDAmount();
                            tkbd.UpdateTKBD(vm);
                            if (_tkbdService.CheckExist(vm.Account, vm.Month))
                            {
                                continue;
                            }
                            _tkbdService.Add(tkbd);
                        }
                        _tkbdService.Save();
                    }                       

                    response = request.CreateResponse(HttpStatusCode.Created, tkbdHistories.Count());
                }
                return response;
            });
        }
    }
}