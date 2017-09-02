using OfficeOpenXml;
using OfficeOpenXml.Table;
using PostOffice.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace PostOffice.Common
{
    public static class ReportHelper
    {
        public static Task GenerateXls<T>(List<T> datasource, string filePath)
        {
            return Task.Run(() =>
            {
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                {
                    //Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(nameof(T));
                    ws.Cells["A1:H1"].Merge = true;
                    ws.Cells["A2:H2"].Merge = true;
                    ws.Cells["A1"].Value = "TỔNG CÔNG TY BƯU ĐIỆN VIỆT NAM";
                    ws.Cells["A2"].Value = "BƯU ĐIỆN TỈNH SÓC TRĂNG";
                    ws.Cells["A4"].LoadFromCollection<T>(datasource, true, TableStyles.Light1);
                    ws.Column(8).Style.Numberformat.Format = "dd/MM/yyyy";
                    ws.Cells.AutoFitColumns();
                    pck.Save();
                }
            });
        }

        public static Task StatisticXls<T>(List<T> datasource, string filePath, statisticReportViewModel vm)
        {
            return Task.Run(() =>
            {
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                {
                    //Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(nameof(T));

                    #region templateInfo
                    
                    ws.Cells["A1:E1"].Merge = true;
                    ws.Cells["A1:E1"].Value = "TỔNG CÔNG TY BƯU ĐIỆN VIỆT NAM \n BƯU ĐIỆN TỈNH SÓC TRĂNG";
                    ws.Cells["A1:E1"].Style.WrapText = true;
                    ws.Cells["A3:E3"].Merge = true;
                    ws.Cells["A3:E3"].Value = "BÁO CÁO TỔNG HỢP";
                    ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(1).Height = 35;
                    ws.Row(1).Style.Font.Bold = true;
                    ws.Row(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(3).Style.Font.Size = 13;
                    ws.Row(3).Style.Font.Bold = true;

                    // Custom fill
                    ws.Cells["C4:E4"].Merge = true;
                    ws.Cells["C4:E4"].Style.Font.Bold = true;
                    ws.Cells["C4:E4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C4:E4"].Style.Indent = 2;
                    ws.Cells["C4:E4"].Value = vm.PoName;

                    ws.Cells["C5:E5"].Merge = true;
                    ws.Cells["C5:E5"].Style.Font.Bold = true;
                    ws.Cells["C5:E5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C5:E5"].Style.Indent = 2;
                    ws.Cells["C5:E5"].Value = "Từ " + vm.fromDate + " đến " + vm.toDate;

                    ws.Cells["C6:E6"].Merge = true;
                    ws.Cells["C6:E6"].Style.Font.Bold = true;
                    ws.Cells["C6:E6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C6:E6"].Style.Indent = 2;
                    ws.Cells["C6:E6"].Value = vm.ServiceName;

                    #endregion templateInfo

                    int noRow = datasource.Count;

                    ws.Cells["A8"].LoadFromCollection<T>(datasource, true, TableStyles.Light1);
                    ws.Column(8).Style.Numberformat.Format = "dd/MM/yyyy";
                    ws.Cells.AutoFitColumns();

                    ws.Cells[noRow + 10, 3, noRow + 10, 5].Merge = true;
                    ws.Cells[noRow + 10, 3, noRow + 10, 5].Value = "Người lập báo cáo";
                    ws.Cells[noRow + 10, 3, noRow + 10, 5].Style.Font.Bold = true;
                    ws.Cells[noRow + 10, 3, noRow + 10, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[noRow + 14, 3, noRow + 14, 5].Merge = true;
                    ws.Cells[noRow + 14, 3, noRow + 14, 5].Value = vm.UserName;
                    ws.Cells[noRow + 14, 3, noRow + 14, 5].Style.Font.Bold = true;
                    ws.Cells[noRow + 14, 3, noRow + 14, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[noRow + 16, 3, noRow + 16, 5].Merge = true;
                    ws.Cells[noRow + 16, 3, noRow + 16, 5].Value = DateTime.Now;
                    ws.Cells[noRow + 16, 3, noRow + 16, 5].Style.Numberformat.Format = "dd/MM/yyyy HH:mm:ss";
                    ws.Cells[noRow + 16, 3, noRow + 16, 5].Style.Font.Italic = true;
                    ws.Cells[noRow + 16, 3, noRow + 16, 5].Style.Font.Size = 10;
                    pck.Save();
                }
            });
        }

        /*
            code: RP1
            name: Bảng kê tổng hợp thu tiền tại bưu cục
        */

        public static Task RP1<T>(List<T> datasource, string filePath, ReportTemplate vm, IEnumerable<RP1Advance> rp1)
        {
            return Task.Run(() =>
            {
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                {
                    //Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(nameof(T));

                    #region templateInfo

                    // all
                    ws.Cells["A1:Z1000"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    //header
                    ws.Cells["A1:E1"].Merge = true;
                    ws.Cells["A1:E1"].Value = "TỔNG CÔNG TY BƯU ĐIỆN VIỆT NAM \n BƯU ĐIỆN TỈNH SÓC TRĂNG";
                    ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(1).Height = 45;
                    ws.Row(1).Style.Font.Bold = true;
                    ws.Row(1).Style.Font.Size = 15;

                    //functionName
                    ws.Cells["A1:E1"].Style.WrapText = true;
                    ws.Cells["A3:E3"].Merge = true;
                    ws.Cells["A3:E3"].Formula = "upper(\"" +vm.FunctionName.ToString() +"\")";

                    ws.Row(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(3).Style.Font.Size = 13;
                    ws.Row(3).Style.Font.Bold = true;

                    // Custom fill
                    //district
                    ws.Cells["C4:E4"].Merge = true;
                    ws.Cells["C4:E4"].Style.Font.Bold = true;
                    ws.Cells["C4:E4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C4:E4"].Style.Indent = 2;
                    if (vm.District == null)
                    {
                        vm.District = "Tất cả";
                    }
                    ws.Cells["C4:E4"].Value = vm.District;

                    //unit
                    ws.Cells["C5:E5"].Merge = true;
                    ws.Cells["C5:E5"].Style.Font.Bold = true;
                    ws.Cells["C5:E5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C5:E5"].Style.Indent = 2;
                    if (vm.Unit == null)
                    {
                        vm.Unit = "Tất cả";
                    }
                    ws.Cells["C5:e5"].Value = vm.Unit;
                    
                    //time
                    ws.Cells["C6:E6"].Merge = true;
                    ws.Cells["C6:E6"].Style.Font.Bold = true;
                    ws.Cells["C6:E6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C6:E6"].Style.Indent = 2;
                    ws.Cells["C6:E6"].Value = "Từ " + vm.FromDate.ToString("dd/MM/yyyy") + " đến " + vm.ToDate.ToString("dd/MM/yyyy"); ;

                    #endregion templateInfo

                    int noRow = datasource.Count;

                    // load data
                    ws.Cells["A8"].LoadFromCollection<T>(datasource, true, TableStyles.Light1);

                    //header
                    ws.Cells["A8"].Value = "STT";
                    ws.Cells["B8"].Value = "Nhóm dịch vụ";
                    ws.Cells["C8"].Value = "Doanh thu";
                    ws.Cells["D8"].Value = "Thuế";
                    ws.Cells["E8"].Value = "Tổng cộng";
                    ws.Cells["A8:E8"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells["A8:E8"].Style.Font.Bold = true;
                    ws.Cells[8, 1, 8, 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells[8, 1, 8, 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(236, 143, 50));

                    ws.Column(8).Style.Numberformat.Format = "dd/MM/yyyy";
                    ws.Cells.AutoFitColumns();    
                                  
                    //format col 1
                    ws.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;                    
                    
                    //format col 3,4,5
                    //ws.Cells[9, 3, noRow + 10, 8].Style.Numberformat.Format = "#,##0.00";

                    //sum part 1
                    ws.Cells[noRow + 9, 2].Value = "Tổng cộng doanh thu";
                    ws.Cells[noRow + 9, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(noRow + 9).Style.Font.Bold = true;
                    ws.Cells[noRow + 9, 3].Formula = "sum(c9:c" + (noRow + 8) + ")";
                    ws.Cells[noRow + 9, 4].Formula = "sum(d9:d" + (noRow + 8) + ")";
                    ws.Cells[noRow + 9, 5].Formula = "sum(e9:e" + (noRow + 8) + ")";

                    //part 2
                    ws.Cells[noRow + 11, 2].Value = "Tiền giữ hộ";
                    ws.Cells[noRow + 11, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells[noRow + 11, 2].Style.Font.Bold = true;
                    ws.Cells[noRow + 12, 1].Value = "1";
                    ws.Cells[noRow + 13, 1].Value = "2";
                    ws.Cells[noRow + 12, 2].Value = "Phụ thu nước ngoài";
                    ws.Cells[noRow + 12, 1, noRow + 12, 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells[noRow + 12, 1, noRow + 12, 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(191, 191, 191));
                    ws.Cells[noRow + 13, 2].Value = "EMS Ngoại giao công vụ";

                    //fill data
                    int i = 12;
                    int j = 3;
                    foreach (var item in rp1)
                    {
                        ws.Cells[noRow + i, j].Value = item.Revenue;
                        ws.Cells[noRow + i, j + 1].Value = item.Tax;
                        ws.Cells[noRow + i, j + 2].Value = item.TotalMoney;
                        i++;
                    }
                    //format col 3,4,5
                    ws.Column(3).Style.Numberformat.Format = "#,##0.00";
                    ws.Column(4).Style.Numberformat.Format = "#,##0.00";
                    ws.Column(5).Style.Numberformat.Format = "#,##0.00";                    

                    //sum part 2
                    ws.Cells[noRow + 14, 2].Value = "Tổng tiền thu hộ";
                    ws.Cells[noRow + 14, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(noRow+14).Style.Font.Bold = true;
                    ws.Cells[noRow + 14, 3].Formula = "sum(c" + (noRow + 12) + ":c" + (noRow + 13) + ")";
                    ws.Cells[noRow + 14, 4].Formula = "sum(d" + (noRow + 12) + ":d" + (noRow + 13) + ")";
                    ws.Cells[noRow + 14, 5].Formula = "sum(e" + (noRow + 12) + ":e" + (noRow + 13) + ")";

                    // ------Tổng thu---------
                    ws.Cells[noRow + 15, 2].Value = "TỔNG THU";
                    ws.Cells[noRow + 15, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(noRow + 15).Style.Font.Bold = true;
                    ws.Cells[noRow + 15, 3].Formula = "C" + (noRow + 9) + "+" + "C" + (noRow + 14);
                    ws.Cells[noRow + 15, 4].Formula = "D" + (noRow + 9) + "+" + "D" + (noRow + 14);
                    ws.Cells[noRow + 15, 5].Formula = "E" + (noRow + 9) + "+" + "E" + (noRow + 14);                    

                    #region template 2

                    //info
                    ws.Cells["A4:B4"].Merge = true;
                    ws.Cells["A4:B4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(4).Style.Font.Bold = true;
                    ws.Cells["A4:B4"].Value = "Huyện: ";
                    ws.Cells["A4:B4"].Style.Indent = 1;

                    ws.Cells["A5:B5"].Merge = true;
                    ws.Cells["A5:B5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(5).Style.Font.Bold = true;
                    ws.Cells["A5:B5"].Value = "Bưu cục: ";
                    ws.Cells["A5:B5"].Style.Indent = 1;

                    ws.Cells["A6:B6"].Merge = true;
                    ws.Cells["A6:B6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(6).Style.Font.Bold = true;
                    ws.Cells["A6:B6"].Value = "Thời gian:";
                    ws.Cells["A6:B6"].Style.Indent = 1;

                    //fix width
                    ws.Column(1).Width = 6;
                    ws.Column(2).Width = 40;
                    ws.Column(3).Width = 18;
                    ws.Column(4).Width = 14;
                    ws.Column(5).Width = 20;

                    //border table
                    ws.Cells[8, 1, noRow + 15, 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    ws.Cells[8, 1, noRow + 15, 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    ws.Cells[8, 1, noRow + 15, 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    ws.Cells[8, 1, noRow + 15, 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    //signal
                    ws.Cells[noRow + 20, 1, noRow + 20, 2].Merge = true;
                    ws.Cells[noRow + 20, 1, noRow + 20, 2].Value = "Người lập bảng";
                    ws.Cells[noRow + 20, 1, noRow + 20, 2].Style.Font.Bold = true;
                    ws.Cells[noRow + 20, 1, noRow + 20, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[noRow + 21, 1, noRow + 21, 2].Merge = true;
                    ws.Cells[noRow + 21, 1, noRow + 21, 2].Value = vm.CreatedBy;
                    ws.Cells[noRow + 21, 1, noRow + 21, 2].Style.Font.Bold = true;
                    ws.Cells[noRow + 21, 1, noRow + 21, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[noRow + 20, 3, noRow + 20, 5].Merge = true;
                    ws.Cells[noRow + 20, 3, noRow + 20, 5].Value = "Người phê duyệt";
                    ws.Cells[noRow + 20, 3, noRow + 20, 5].Style.Font.Bold = true;
                    ws.Cells[noRow + 20, 3, noRow + 20, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[noRow + 22, 3, noRow + 22, 5].Merge = true;
                    ws.Cells[noRow + 22, 3, noRow + 22, 5].Value = DateTime.Now;
                    ws.Cells[noRow + 22, 3, noRow + 22, 5].Style.Numberformat.Format = "dd/MM/yyyy HH:mm:ss";
                    ws.Cells[noRow + 22, 3, noRow + 22, 5].Style.Font.Italic = true;
                    ws.Cells[noRow + 22, 3, noRow + 22, 5].Style.Font.Size = 10;
                    #endregion template 2

                    pck.Save();
                }
            });
        }

        /*
            code: 
            name: Bảng kê thu tiền tại bưu cục - tổng hợp
        */
        public static Task Export_By_Service_Group_And_Time<T1, T2, T3>(List<T1> datasource1, List<T2> datasource2, List<T3> datasource3, string filePath, ReportTemplate vm)
        {
            return Task.Run(() =>
            {
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Thống kê tổng hợp");
                    
                    #region templateInfo

                    // all
                    ws.Cells["A1:Z1000"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    //header
                    ws.Cells["A1:I1"].Merge = true;
                    ws.Cells["A1:I1"].Value = "TỔNG CÔNG TY BƯU ĐIỆN VIỆT NAM \n BƯU ĐIỆN TỈNH SÓC TRĂNG";
                    ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(1).Height = 45;
                    ws.Row(1).Style.Font.Bold = true;
                    ws.Row(1).Style.Font.Size = 15;
                    //functionName
                    ws.Cells["A1:I1"].Style.WrapText = true;
                    ws.Cells["A3:I3"].Merge = true;
                    ws.Cells["A3:I3"].Formula = "upper(\"" + vm.FunctionName.ToString() + "\")";
                    ws.Row(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(3).Style.Font.Size = 13;
                    ws.Row(3).Style.Font.Bold = true;
                   
                    // fill district
                    ws.Cells["C4:I4"].Merge = true;
                    ws.Cells["C4:I4"].Style.Font.Bold = true;
                    ws.Cells["C4:I4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C4:I4"].Style.Indent = 2;
                    if (vm.District == null)
                    {
                        vm.District = "Tất cả";
                    }
                    ws.Cells["C4:I4"].Value = vm.District;

                    // fill unit
                    ws.Cells["C5:I5"].Merge = true;
                    ws.Cells["C5:I5"].Style.Font.Bold = true;
                    ws.Cells["C5:I5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C5:I5"].Style.Indent = 2;
                    if (vm.Unit == null)
                    {
                        vm.Unit = "Tất cả";
                    }
                    ws.Cells["C5:I5"].Value = vm.Unit;

                    // fill time
                    ws.Cells["C6:I6"].Merge = true;
                    ws.Cells["C6:I6"].Style.Font.Bold = true;
                    ws.Cells["C6:I6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C6:I6"].Style.Indent = 2;
                    ws.Cells["C6:I6"].Value = "Từ " + vm.FromDate.ToString("dd/MM/yyyy") + " đến " + vm.ToDate.ToString("dd/MM/yyyy");

                    #endregion template info

                    #region count data                    
                    int noRow = datasource1.Count; //count number rows BCCP                    
                    int noRow3 = datasource2.Count; // count row of PPTT                   
                    int noRow2 = datasource3.Count;  // count row of TCBC
                    //int noRow4 = datasource4.Count; // count row of OTHER
                    #endregion count data

                    #region BCCP                    
                    if (noRow > 0)
                    {
                        //load data source 1 BCCP start A9
                        ws.Cells["A9"].LoadFromCollection<T1>(datasource1, true, TableStyles.Light1);
                        //fill STT
                        for (int i = 1; i <= noRow; i++)
                        {
                            ws.Cells["A" + (i + 9)].Value = i;
                        }

                        //format col 1
                        ws.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        //Header Label BCCP
                        ws.Cells["A8:I8"].Merge = true;
                        ws.Cells["A8:I8"].Value = "I. Nhóm Bưu Chính Chuyển Phát";
                        ws.Cells["A8:I8"].Style.Font.Bold = true;
                        ws.Row(8).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                        //header
                        ws.Row(9).Height = 30;
                        ws.Cells["A9"].Value = "STT";
                        ws.Cells["B9"].Value = "Dịch vụ";
                        ws.Cells["C9"].Value = "Số \nlượng";
                        ws.Cells["D9"].Value = "Thuế";
                        ws.Cells["E9"].Value = "Tiền mặt";
                        ws.Cells["F9"].Value = "Vat \ntiền mặt";
                        ws.Cells["G9"].Value = "Tiền nợ";
                        ws.Cells["H9"].Value = "Vat \ntiền nợ";
                        ws.Cells["I9"].Value = "DTTL";

                        ws.Cells["A9:I9"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A9:I9"].Style.Font.Bold = true;
                        ws.Cells[9, 1, 9, 9].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[9, 1, 9, 9].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(236, 143, 50));

                        ws.Cells.AutoFitColumns();
                        ws.Row(9).Style.WrapText = true;

                        ws.Cells["d10:I" + (noRow + 10)].Style.Numberformat.Format = "#,##0.00";

                        //sum group 1
                        ws.Cells[noRow + 10, 2].Value = "Tổng cộng";
                        ws.Cells[noRow + 10, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Row(noRow + 10).Style.Font.Bold = true;
                        ws.Cells[noRow + 10, 3].Formula = "sum(c10:c" + (noRow + 9) + ")";
                        ws.Cells[noRow + 10, 5].Formula = "sum(e10:e" + (noRow + 9) + ")";
                        ws.Cells[noRow + 10, 6].Formula = "sum(F10:F" + (noRow + 9) + ")";
                        ws.Cells[noRow + 10, 7].Formula = "sum(G10:G" + (noRow + 9) + ")";
                        ws.Cells[noRow + 10, 8].Formula = "sum(H10:H" + (noRow + 9) + ")";
                        ws.Cells[noRow + 10, 9].Formula = "sum(I10:I" + (noRow + 9) + ")";

                    }
                    #endregion
                    pck.Save();
                }
            });
        }
        /*
            code: RP2_1
            name: Bảng kê thu tiền tại bưu cục - chi tiết
        */

        public static Task RP2_1<T1, T2, T3>(List<T1> datasource1, List<T2> datasource2, List<T3> datasource3, string filePath, ReportTemplate vm)
        {
            return Task.Run(() =>
            {
                using (ExcelPackage pck = new ExcelPackage(new FileInfo(filePath)))
                {
                    //Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add(nameof(T1));

                    #region templateInfo

                    // all
                    ws.Cells["A1:Z1000"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    //header
                    ws.Cells["A1:I1"].Merge = true;
                    ws.Cells["A1:I1"].Value = "TỔNG CÔNG TY BƯU ĐIỆN VIỆT NAM \n BƯU ĐIỆN TỈNH SÓC TRĂNG";
                    ws.Row(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(1).Height = 45;
                    ws.Row(1).Style.Font.Bold = true;
                    ws.Row(1).Style.Font.Size = 15;

                    //functionName
                    ws.Cells["A1:I1"].Style.WrapText = true;
                    ws.Cells["A3:I3"].Merge = true;
                    ws.Cells["A3:I3"].Formula = "upper(\"" + vm.FunctionName.ToString() + "\")";

                    ws.Row(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Row(3).Style.Font.Size = 13;
                    ws.Row(3).Style.Font.Bold = true;

                    // Custom fill
                    //district
                    ws.Cells["C4:I4"].Merge = true;
                    ws.Cells["C4:I4"].Style.Font.Bold = true;
                    ws.Cells["C4:I4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C4:I4"].Style.Indent = 2;
                    if (vm.District == null)
                    {
                        vm.District = "Tất cả";
                    }
                    ws.Cells["C4:I4"].Value = vm.District;

                    //unit
                    ws.Cells["C5:I5"].Merge = true;
                    ws.Cells["C5:I5"].Style.Font.Bold = true;
                    ws.Cells["C5:I5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C5:I5"].Style.Indent = 2;
                    if (vm.Unit == null)
                    {
                        vm.Unit = "Tất cả";
                    }
                    ws.Cells["C5:I5"].Value = vm.Unit;

                    //time
                    ws.Cells["C6:I6"].Merge = true;
                    ws.Cells["C6:I6"].Style.Font.Bold = true;
                    ws.Cells["C6:I6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    ws.Cells["C6:I6"].Style.Indent = 2;
                    ws.Cells["C6:I6"].Value = "Từ " + vm.FromDate.ToString("dd/MM/yyyy") + " đến " + vm.ToDate.ToString("dd/MM/yyyy");

                    #endregion templateInfo

                    #region count data                    
                    //count number rows BCCP
                    int noRow = datasource1.Count;
                    // count row of TCBC
                    int noRow2 = datasource3.Count;
                    // count row of PPTT
                    int noRow3 = datasource2.Count;
                    #endregion count data

                    #region BCCP
                    //format number
                    if (noRow > 0)
                    {
                        //load data source 1
                        ws.Cells["A9"].LoadFromCollection<T1>(datasource1, true, TableStyles.Light1);
                        //fill STT
                        for (int i = 1; i <= noRow; i++)
                        {
                            ws.Cells["A" + (i + 9)].Value = i;
                        }

                        //format col 1
                        ws.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        //Header BCCP
                        ws.Cells["A8:I8"].Merge = true;
                        ws.Cells["A8:I8"].Value = "I. Nhóm Bưu Chính Chuyển Phát";
                        ws.Cells["A8:I8"].Style.Font.Bold = true;
                        ws.Row(8).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                        //header
                        ws.Row(9).Height = 30;
                        ws.Cells["A9"].Value = "STT";
                        ws.Cells["B9"].Value = "Dịch vụ";
                        ws.Cells["C9"].Value = "Số \nlượng";
                        ws.Cells["D9"].Value = "Thuế";
                        ws.Cells["E9"].Value = "Tiền mặt";
                        ws.Cells["F9"].Value = "Vat \ntiền mặt";
                        ws.Cells["G9"].Value = "Tiền nợ";
                        ws.Cells["H9"].Value = "Vat \ntiền nợ";
                        ws.Cells["I9"].Value = "DTTL";

                        ws.Cells["A9:I9"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A9:I9"].Style.Font.Bold = true;
                        ws.Cells[9, 1, 9, 9].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[9, 1, 9, 9].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(236, 143, 50));

                        ws.Cells.AutoFitColumns();
                        ws.Row(9).Style.WrapText = true;

                        ws.Cells["d10:I" + (noRow + 10)].Style.Numberformat.Format = "#,##0.00";                        

                        //sum group 1
                        ws.Cells[noRow + 10, 2].Value = "Tổng cộng";
                        ws.Cells[noRow + 10, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Row(noRow + 10).Style.Font.Bold = true;
                        ws.Cells[noRow + 10, 3].Formula = "sum(c10:c" + (noRow + 9) + ")";
                        ws.Cells[noRow + 10, 5].Formula = "sum(e10:e" + (noRow + 9) + ")";
                        ws.Cells[noRow + 10, 6].Formula = "sum(F10:F" + (noRow + 9) + ")";
                        ws.Cells[noRow + 10, 7].Formula = "sum(G10:G" + (noRow + 9) + ")";
                        ws.Cells[noRow + 10, 8].Formula = "sum(H10:H" + (noRow + 9) + ")";
                        ws.Cells[noRow + 10, 9].Formula = "sum(I10:I" + (noRow + 9) + ")";
                    }
                    #endregion BCCP

                    #region TCBC
                    if (noRow2 > 0)
                    {
                        // load data source 2
                        ws.Cells["A" + (noRow + 13)].LoadFromCollection<T3>(datasource3, true, TableStyles.Light1);
                        ws.Cells["A" + (noRow + 12) + ":I" + (noRow + 12)].Merge = true;
                        ws.Cells["A" + (noRow + 12) + ":I" + (noRow + 12)].Value = "II. Nhóm Tài Chính Bưu Chính";
                        ws.Cells["A" + (noRow + 12) + ":I" + (noRow + 12)].Style.Font.Bold = true;
                        ws.Row(noRow + 12).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                        //header
                        ws.Row(noRow + 13).Height = 30;
                        ws.Cells["A" + (noRow + 13)].Value = "STT";
                        ws.Cells["B" + (noRow + 13)].Value = "Dịch vụ";
                        ws.Cells["C" + (noRow + 13)].Value = "Số \nlượng";
                        ws.Cells["D" + (noRow + 13)].Value = "Thuế";
                        ws.Cells["E" + (noRow + 13)].Value = "Số tiền \nkhách nhận";
                        ws.Cells["F" + (noRow + 13)].Value = "Số tiền \nnhận của khách";
                        ws.Cells["G" + (noRow + 13)].Value = "Doanh thu \ntính lương";
                        ws.Cells["A" + (noRow + 13) + ":I" + (noRow + 13)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A" + (noRow + 13) + ":I" + (noRow + 13)].Style.Font.Bold = true;
                        ws.Cells[(noRow + 13), 1, (noRow + 13), 7].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[(noRow + 13), 1, (noRow + 13), 7].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(236, 143, 50));
                        ws.Row(noRow + 13).Style.WrapText = true;

                        // fill STT
                        for (int i = 1; i <= noRow2; i++)
                        {
                            ws.Cells["A" + (i + noRow + 13)].Value = i;
                        }

                        // sum source 2
                        ws.Cells[noRow + noRow2  +14, 2].Value = "Tổng cộng";
                        ws.Cells[noRow + noRow2  +14, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Row(noRow + noRow2  +14).Style.Font.Bold = true;
                        ws.Cells[noRow + noRow2  +14, 3].Formula = "sum(c" + (14 + noRow) + ":c" + (noRow + noRow2 + 13) + ")";
                        ws.Cells[noRow + noRow2  +14, 5].Formula = "sum(e" + (14 + noRow) + ":e" + (noRow + noRow2 + 13) + ")";
                        ws.Cells[noRow + noRow2  +14, 6].Formula = "sum(F" + (14 + noRow) + ":F" + (noRow + noRow2 + 13) + ")";
                        ws.Cells[noRow + noRow2  +14, 7].Formula = "sum(G" + (14 + noRow) + ":G" + (noRow + noRow2 + 13) + ")";
                        ws.Cells[noRow + 14, 4, noRow + noRow2 + 14, 7].Style.Numberformat.Format = "#,##0.00";
                    }

                    #endregion TCBC
                    
                    #region PPTT
                    if (noRow3 > 0)
                    {
                        // load data source 2
                        ws.Cells["A" + (noRow + noRow2 + 17)].LoadFromCollection<T2>(datasource2, true, TableStyles.Light1);
                        ws.Cells["A" + (noRow + noRow2 + 16) + ":I" + (noRow + noRow2 + 16)].Merge = true;
                        ws.Cells["A" + (noRow + noRow2 + 16) + ":I" + (noRow + noRow2 + 16)].Value = "III. Nhóm Phân Phối Truyền Thông";
                        ws.Cells["A" + (noRow + noRow2 + 16) + ":I" + (noRow + noRow2 + 16)].Style.Font.Bold = true;
                        ws.Row(noRow + noRow2 + 16).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                        //header
                        ws.Row(noRow + noRow2 + 17).Height = 30;
                        ws.Cells["A"+ (noRow + noRow2 + 17)].Value = "STT";
                        ws.Cells["B"+ (noRow + noRow2 + 17)].Value = "Dịch vụ";
                        ws.Cells["C"+ (noRow + noRow2 + 17)].Value = "Số \nlượng";
                        ws.Cells["D"+ (noRow + noRow2 + 17)].Value = "Thuế";
                        ws.Cells["E"+ (noRow + noRow2 + 17)].Value = "Tiền mặt";
                        ws.Cells["F"+ (noRow + noRow2 + 17)].Value = "Vat \ntiền mặt";
                        ws.Cells["G"+ (noRow + noRow2 + 17)].Value = "Tiền nợ";
                        ws.Cells["H"+ (noRow + noRow2 + 17)].Value = "Vat \ntiền nợ";
                        ws.Cells["I"+ (noRow + noRow2 + 17)].Value = "Doanh thu \ntính lương";
                        ws.Cells["A" + (noRow + noRow2 + 17) + ":I" + (noRow + noRow2 + 17)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Cells["A" + (noRow + noRow2 + 17) + ":I" + (noRow + noRow2 + 17)].Style.Font.Bold = true;
                        ws.Cells[(noRow + noRow2 + 17), 1, (noRow + noRow2 + 17), 9].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        ws.Cells[(noRow + noRow2 + 17), 1, (noRow + noRow2 + 17), 9].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(236, 143, 50));
                        ws.Row(noRow + noRow2 + 17).Style.WrapText = true;

                        // fill STT
                        for (int i = 1; i <= noRow3; i++)
                        {
                            ws.Cells["A" + (i + noRow + noRow2 + 17)].Value = i;
                        }

                        // sum source 2
                        ws.Cells[noRow + noRow2 + noRow3 +18, 2].Value = "Tổng cộng";
                        ws.Cells[noRow + noRow2 + noRow3 +18, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        ws.Row(noRow + noRow2 + noRow3 +18).Style.Font.Bold = true;
                        ws.Cells[noRow + noRow2 + noRow3 +18, 3].Formula = "sum(c" + (18 + noRow + noRow2) + ":c" + (noRow + noRow2 + noRow3 + 17) + ")";
                        ws.Cells[noRow + noRow2 + noRow3 +18, 5].Formula = "sum(e" + (18 + noRow + noRow2) + ":e" + (noRow + noRow2 + noRow3 + 17) + ")";
                        ws.Cells[noRow + noRow2 + noRow3 +18, 6].Formula = "sum(F" + (18 + noRow + noRow2) + ":F" + (noRow + noRow2 + noRow3 + 17) + ")";
                        ws.Cells[noRow + noRow2 + noRow3 +18, 7].Formula = "sum(G" + (18 + noRow + noRow2) + ":G" + (noRow + noRow2 + noRow3 + 17) + ")";
                        ws.Cells[noRow + noRow2 + noRow3 + 18, 8].Formula = "sum(H" + (18 + noRow + noRow2) + ":H" + (noRow + noRow2 + noRow3 + 17) + ")";
                        ws.Cells[noRow + noRow2 + noRow3 + 18, 9].Formula = "sum(I" + (18 + noRow + noRow2) + ":I" + (noRow + noRow2 + noRow3 + 17) + ")";
                        ws.Cells[noRow + noRow2 + 18, 3, noRow + noRow2 + noRow3 +18, 9].Style.Numberformat.Format = "#,##0.00";
                    }

                    #endregion PPTT

                    #region Function Info

                    //info
                    ws.Cells["A4:B4"].Merge = true;
                    ws.Cells["A4:B4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(4).Style.Font.Bold = true;
                    ws.Cells["A4:B4"].Value = "Huyện: ";
                    ws.Cells["A4:B4"].Style.Indent = 1;

                    ws.Cells["A5:B5"].Merge = true;
                    ws.Cells["A5:B5"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(5).Style.Font.Bold = true;
                    ws.Cells["A5:B5"].Value = "Bưu cục: ";
                    ws.Cells["A5:B5"].Style.Indent = 1;

                    ws.Cells["A6:B6"].Merge = true;
                    ws.Cells["A6:B6"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    ws.Row(6).Style.Font.Bold = true;
                    ws.Cells["A6:B6"].Value = "Thời gian:";
                    ws.Cells["A6:B6"].Style.Indent = 1;

                    #endregion

                    #region fix width
                    //fix width
                    ws.Column(1).Width = 5;
                    ws.Column(2).Width = 40;
                    ws.Column(3).Width = 8;
                    ws.Column(4).Width = 8;
                    ws.Column(5).Width = 20;
                    ws.Column(6).Width = 20;
                    ws.Column(7).Width = 20;
                    ws.Column(8).Width = 12;
                    ws.Column(9).Width = 20;

                    #endregion fix width
                    //border table
                    //ws.Cells[8, 1, noRow + 15, 9].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    //ws.Cells[8, 1, noRow + 15, 9].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    //ws.Cells[8, 1, noRow + 15, 9].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    //ws.Cells[8, 1, noRow + 15, 9].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    #region Signal
                    //signal
                    ws.Cells[noRow + noRow2 + 23, 1, noRow + noRow2 + 23, 2].Merge = true;
                    ws.Cells[noRow + noRow2 + 23, 1, noRow + noRow2 + 23, 2].Value = "Người lập bảng";
                    ws.Cells[noRow + noRow2 + 23, 1, noRow + noRow2 + 23, 2].Style.Font.Bold = true;
                    ws.Cells[noRow + noRow2 + 23, 1, noRow + noRow2 + 23, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[noRow + noRow2 + 26, 1, noRow + noRow2 + 26, 2].Merge = true;
                    ws.Cells[noRow + noRow2 + 26, 1, noRow + noRow2 + 26, 2].Value = vm.CreatedBy;
                    ws.Cells[noRow + noRow2 + 26, 1, noRow + noRow2 + 26, 2].Style.Font.Bold = true;
                    ws.Cells[noRow + noRow2 + 26, 1, noRow + noRow2 + 26, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[noRow + noRow2 + 23, 7, noRow + noRow2 + 23, 9].Merge = true;
                    ws.Cells[noRow + noRow2 + 23, 7, noRow + noRow2 + 23, 9].Value = "Người phê duyệt";
                    ws.Cells[noRow + noRow2 + 23, 7, noRow + noRow2 + 23, 9].Style.Font.Bold = true;
                    ws.Cells[noRow + noRow2 + 23, 7, noRow + noRow2 + 23, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[noRow + noRow2 + 27, 3, noRow + noRow2 + 27, 9].Merge = true;
                    ws.Cells[noRow + noRow2 + 27, 3, noRow + noRow2 + 27, 9].Value = DateTime.Now;
                    ws.Cells[noRow + noRow2 + 27, 3, noRow + noRow2 + 27, 9].Style.Numberformat.Format = "dd/MM/yyyy HH:mm:ss";
                    ws.Cells[noRow + noRow2 + 27, 3, noRow + noRow2 + 27, 9].Style.Font.Italic = true;
                    ws.Cells[noRow + noRow2 + 27, 3, noRow + noRow2 + 27, 9].Style.Font.Size = 10;
                    #endregion Signal

                    pck.Save();
                }
            });
        }
    }
}