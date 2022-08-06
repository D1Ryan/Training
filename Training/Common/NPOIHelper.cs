//using NPOI.HSSF.UserModel;
//using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System;
using System.Data;
using System.IO;
using System.Xml;

namespace SAPI.Common
{
    public class Char
    {
        /// <summary>
        /// 生成表列序
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static string Chr(int i)
        {
            //列头1~26为A,后续为AA~AZ,BA~BZ           
            string ch;
            if (i < 27)
            {
                ch = ((char)(64 + i)).ToString();
            }
            else
            {
                ch = ((char)(64 + (i / 26))).ToString() + ((char)(64 + (i % 26))).ToString();
            }
            return ch;
        }
    }

    public class NPOIHelper
    {
        /// <summary>
        /// 生成Excel文件
        /// </summary>
        /// <param name="tableHtml"></param>
        /// <param name="sheetname"></param>
        /// <returns></returns>
        public static byte[] GenerateXlsxBytes(string tableHtml, string sheetname)
        {
            tableHtml = tableHtml.Replace("&", "&amp;");
            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + tableHtml;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode table = doc.SelectSingleNode("/table");
            int colspan = 1;
            int rowspan = 1;
            int rowNum;
            int columnNum;
            rowNum = 1;
            columnNum = 1;
            var workBook = new XSSFWorkbook();//20190309
            var ws = workBook.CreateSheet(sheetname);
            string mapKey = string.Empty;
            string mergKey = string.Empty;
            int rowCount = table.ChildNodes.Count;
            int colCount = FetchColCount(table.ChildNodes);
            InitSheet(ws, rowCount, colCount);
            bool[,] map = new bool[rowCount + 1, colCount + 1];
            foreach (XmlNode row in table.ChildNodes)
            {
                columnNum = 1;
                foreach (XmlNode column in row.ChildNodes)
                {
                    if (column.Attributes["rowspan"] != null)
                    {
                        rowspan = Convert.ToInt32(column.Attributes["rowspan"].Value);
                    }
                    else
                    {
                        rowspan = 1;
                    }
                    if (column.Attributes["colspan"] != null)
                    {
                        colspan = Convert.ToInt32(column.Attributes["colspan"].Value);
                    }
                    else
                    {
                        colspan = 1;
                    }
                    while (map[rowNum, columnNum])
                    {
                        columnNum++;
                    }
                    if (rowspan == 1 && colspan == 1)
                    {
                        SetCellValue(ws, string.Format("{0}{1}", Char.Chr(columnNum), rowNum), column.InnerText);
                        map[rowNum, columnNum] = true;
                    }
                    else
                    {
                        SetCellValue(ws, string.Format("{0}{1}", Char.Chr(columnNum), rowNum), column.InnerText);
                        mergKey = string.Format("{0}{1}:{2}{3}", Char.Chr(columnNum), rowNum, Char.Chr(columnNum + colspan - 1), rowNum + rowspan - 1);
                        MergCells(ws, mergKey);

                        for (int m = 0; m < rowspan; m++)
                        {
                            for (int n = 0; n < colspan; n++)
                            {
                                map[rowNum + m, columnNum + n] = true;
                            }
                        }
                    }
                    columnNum++;
                }
                rowNum++;
            }
            MemoryStream stream = new MemoryStream();
            workBook.Write(stream);
            return stream.ToArray();
        }

        /// <summary>
        /// 列循环
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        static int FetchColCount(XmlNodeList nodes)
        {
            int colCount = 0;
            foreach (XmlNode row in nodes)
            {
                if (colCount < row.ChildNodes.Count)
                {
                    colCount = row.ChildNodes.Count;
                }
            }
            return colCount;
        }

        /// <summary>
        /// 表格初始化
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowCount"></param>
        /// <param name="colCount"></param>
        static void InitSheet(ISheet sheet, int rowCount, int colCount)
        {
            for (int i = 0; i < rowCount; i++)
            {
                IRow row = sheet.CreateRow(i);
                for (int j = 0; j < colCount; j++)
                {
                    row.CreateCell(j);
                }
            }
        }

        /// <summary>
        /// 生成单元格
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="cellReferenceText"></param>
        /// <param name="value"></param>
        static void SetCellValue(ISheet sheet, string cellReferenceText, string value)
        {
            CellReference cr = new CellReference(cellReferenceText);
            IRow row = sheet.GetRow(cr.Row);
            ICell cell = row.GetCell(cr.Col);
            cell.SetCellValue(value);
        }

        /// <summary>
        /// 合并表格
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="mergeKey"></param>
        static void MergCells(ISheet sheet, string mergeKey)
        {
            string[] cellReferences = mergeKey.Split(':');
            CellReference first = new CellReference(cellReferences[0]);
            CellReference last = new CellReference(cellReferences[1]);
            CellRangeAddress region = new CellRangeAddress(first.Row, last.Row, first.Col, last.Col);
            sheet.AddMergedRegion(region);
        }

        /// <summary>
        /// 读取excel到datatable 
        /// </summary>      
        /// <param name="strFileName">excel文档路径</param>      
        /// <returns></returns>      
        public static DataTable ExcelToDataTable(string strFileName)
        {
            /// 默认第一行为表头，导入第一个工作表  
            DataTable dt = new DataTable();
            FileStream file = null;
            IWorkbook Workbook = null;
            try
            {
                using (file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))//C#文件流读取文件
                {
                    if (strFileName.IndexOf(".xlsx") > 0)
                        //把xlsx文件中的数据写入Workbook中
                        Workbook = new XSSFWorkbook(file);

                    else if (strFileName.IndexOf(".xls") > 0)
                        //把xls文件中的数据写入Workbook中
                        Workbook = new HSSFWorkbook(file);

                    if (Workbook != null)
                    {
                        ISheet sheet = Workbook.GetSheetAt(0);//读取第一个sheet
                        System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                        //得到Excel工作表的行 
                        IRow headerRow = sheet.GetRow(0);
                        //得到Excel工作表的总列数  
                        int cellCount = headerRow.LastCellNum;

                        for (int j = 0; j < cellCount; j++)
                        {
                            //得到Excel工作表指定行的单元格  
                            ICell cell = headerRow.GetCell(j);
                            dt.Columns.Add(cell.ToString());
                        }

                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            DataRow dataRow = dt.NewRow();

                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                if (row.GetCell(j) != null)
                                    dataRow[j] = row.GetCell(j).ToString();
                            }
                            dt.Rows.Add(dataRow);
                        }
                    }
                    return dt;
                }
            }

            catch (Exception)
            {
                if (file != null)
                {
                    file.Close();//关闭当前流并释放资源
                }
                return null;
            }
        }
    }
}