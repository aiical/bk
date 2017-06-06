using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using CourseManager.Common.Extensions;
using System.Data;
using System.Collections;
using System.Xml.Linq;

namespace CourseManager.Utils.Import
{
    /// <summary>
    /// NPIO插件处理Excel相关操作
    /// </summary>
    public class NPIOExcelHelper
    {
        #region 列表头
        /// <summary>
        /// Excel 表格中 列标头 与 列索引 的对应转换
        /// </summary>
        public enum EnumExcelColumn
        {
            A = 0, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
            AA, AB, AC, AD, AE, AF, AG, AH, AI, AJ, AK, AL, AM, AN, AO, AP, AQ, AR, AS, AT, AU, AV, AW, AX, AY, AZ,
            BA, BB, BC, BD, BE, BF, BG, BH, BI, BJ, BK, BL, BM, BN, BO, BP, BQ, BR, BS, BT, BU, BV, BW, BX, BY, BZ,
            CA, CB, CC, CD, CE, CF, CG, CH, CI, CJ, CK, CL, CM, CN, CO, CP, CQ, CR, CS, CT, CU, CV, CW, CX, CY, CZ,
            DA, DB, DC, DD, DE, DF, DG, DH, DI, DJ, DK, DL, DM, DN, DO, DP, DQ, DR, DS, DT, DU, DV, DW, DX, DY, DZ,
            EA, EB, EC, ED, EE, EF, EG, EH, EI, EJ, EK, EL, EM, EN, EO, EP, EQ, ER, ES, ET, EU, EV, EW, EX, EY, EZ,
            FA, FB, FC, FD, FE, FF, FG, FH, FI, FJ, FK, FL, FM, FN, FO, FP, FQ, FR, FS, FT, FU, FV, FW, FX, FY, FZ,
            GA, GB, GC, GD, GE, GF, GG, GH, GI, GJ, GK, GL, GM, GN, GO, GP, GQ, GR, GS, GT, GU, GV, GW, GX, GY, GZ,
            HA, HB, HC, HD, HE, HF, HG, HH, HI, HJ, HK, HL, HM, HN, HO, HP, HQ, HR, HS, HT, HU, HV, HW, HX, HY, HZ,
            IA, IB, IC, ID, IE, IF, IG, IH, II, IJ, IK, IL, IM, IN, IO, IP, IQ, IR, IS, IT, IU, IV
        }
        #endregion

        #region 变量

        protected String m_MappingFile;     //映射配置文件路径
        protected String m_ExcelSheetName;    //Excel中要导入数据的表名
        protected String m_SqlTableName;    //要导入的Sql表名，也可为其它类型的，如Oracle
        protected ArrayList[] m_ColumnMapping;   //列映射配置列表，包括3部分 0--Sql列名，1--Excel列索引，2-- 如当前Excel行为空，是否赋值为上一行的值
        private bool isLoadMapping;
        /// <summary>
        /// 指定要解析的Isheet
        /// </summary>
        private ISheet m_ISheet = null;

        #endregion

        #region 构造函数

        /// <summary>
        /// 无参构造
        /// </summary>
        public NPIOExcelHelper()
        {
            m_MappingFile = "";
            m_ExcelSheetName = "";
            isLoadMapping = false;
            m_ColumnMapping = new ArrayList[3];
            m_ColumnMapping[0] = new ArrayList();
            m_ColumnMapping[1] = new ArrayList();
            m_ColumnMapping[2] = new ArrayList();
        }
        /// <summary>
        /// 构造函数重载
        /// </summary>
        /// <param name="mappingFilePath">映射配置文件路径</param>
        /// <param name="isheet">指定要解析的ISheet</param>
        /// <param name="excelSheetName">Excel中要导入数据的表名</param>
        public NPIOExcelHelper(String mappingFilePath, ISheet isheet = null, String excelSheetName = "")
        {
            m_ISheet = isheet;
            m_MappingFile = mappingFilePath;
            m_ExcelSheetName = excelSheetName;
            isLoadMapping = false;
            m_ColumnMapping = new ArrayList[3];

            m_ColumnMapping[0] = new ArrayList();
            m_ColumnMapping[1] = new ArrayList();
            m_ColumnMapping[2] = new ArrayList();
        }

        #endregion

        #region 属性

        /// <summary>
        /// 读取或设置 映射配置文件路径
        /// </summary>
        public String MappingFilePath
        {
            get
            {
                return m_MappingFile;
            }
            set
            {
                m_MappingFile = value;
                isLoadMapping = false;
            }
        }
        /// <summary>
        /// 读取或设置 Excel中要导入数据的表名
        /// </summary>
        public String ExcelSheetName
        {
            get
            {
                return m_ExcelSheetName;
            }
            set
            {
                m_ExcelSheetName = value;
                isLoadMapping = false;
            }
        }

        #endregion
        #region 受保护的虚函数，子类须重写

        /// <summary>
        /// 在导入前对Excel行数据进行处理
        /// </summary>
        /// <param name="drExcelRow">正在读取的当前Excel行</param>
        /// <returns>true -- 继续处理，false -- 跳过当前行
        /// </returns>
        protected virtual bool ImportingBefore(ref DataRow drExcelRow)
        {
            return true;
        }
        /// <summary>
        /// 在数据转存后对当前行进行处理
        /// </summary>
        /// <param name="drSqlRow">已经转存数据的当前Sql行</param>
        /// <returns>true -- 继续处理，false -- 跳过当前行
        /// </returns>
        protected virtual bool ImportingAfter(ref DataRow drSqlRow)
        {
            return true;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 加载配置文件，取得表和列的映射
        /// </summary>
        /// <returns></returns>
        private bool LoadMapping(ref DataTable dtTarget)
        {
            try
            {
                //清除已过时的配置
                m_ColumnMapping[0].Clear();
                m_ColumnMapping[1].Clear();
                m_ColumnMapping[2].Clear();

                if (m_MappingFile == null || m_MappingFile == "")
                {
                    throw new Exception("找不到配置文件");
                }

                //读入配置文件
                XDocument xd = XDocument.Load(m_MappingFile);
                if (xd.Root == null)
                {
                    throw new Exception("读取配置文件失败");
                }
                if (string.IsNullOrEmpty(m_ExcelSheetName))
                {
                    var tm = xd.Root.Elements("TableMapping").FirstOrDefault();
                    if (tm != null)
                    {
                        m_ExcelSheetName = tm.Attribute("excelSheet") != null ? tm.Attribute("excelSheet").Value : "";
                    }
                    if (string.IsNullOrEmpty(m_ExcelSheetName))
                        throw new Exception("该Sheet不存在");
                }
                var tMap = (from a in xd.Root.Elements("TableMapping")
                            where a.Attribute("excelSheet").Value.Equals(m_ExcelSheetName)
                            select a);
                if (tMap == null || tMap.Count() != 1)
                {
                    throw new Exception("该Sheet不存在或多次配置");
                }
                var cMapping = from b in tMap.Elements("ColumnMapping")
                               select b;

                bool createDt = dtTarget.Columns.Count == 0;
                foreach (XElement c in cMapping)
                {
                    m_ColumnMapping[0].Add(c.Element("sqlCol").Value);
                    m_ColumnMapping[1].Add((int)Enum.Parse(typeof(EnumExcelColumn), c.Element("excelCol").Value, true));
                    m_ColumnMapping[2].Add(c.Element("inherit").Value);
                    if (createDt)
                    {
                        dtTarget.Columns.Add(new DataColumn(c.Element("sqlCol").Value));
                    }
                }

                //设置为已加载配置
                isLoadMapping = true;

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        /// <summary>
        /// 导入Excel文件到dataset
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private DataSet ImportExcelDateToDataSet(string fileName, string sheetName = null)
        {
            DataSet dsDataSet = new DataSet();
            if (m_ISheet != null)
            {
                DataTable dt = GetSheetData(m_ISheet);
                if (dt != null)
                {
                    dsDataSet.Tables.Add(dt);
                }
            }
            else
            {
                XSSFWorkbook xssfworkbook = null;
                using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    xssfworkbook = new XSSFWorkbook(file);
                }
                int sheetCount = xssfworkbook.NumberOfSheets;
                if (sheetCount == 0) return null;
                for (int i = 0; i < sheetCount; i++)
                {
                    ISheet sheet = xssfworkbook.GetSheetAt(i);
                    if (!string.IsNullOrEmpty(sheetName) && !sheet.SheetName.Equals(sheetName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        continue;
                    }
                    DataTable dt = GetSheetData(sheet);
                    if (dt != null)
                    {
                        dsDataSet.Tables.Add(dt);
                    }
                }
            }
            return dsDataSet;
        }

        private DataTable GetSheetData(ISheet sheet)
        {
            //表头
            var rowHeader = sheet.GetRow(0);
            if (rowHeader == null || rowHeader.Cells == null || rowHeader.Cells.Count == 0) return null;
            DataTable dt = new DataTable(m_ExcelSheetName.IsNullOrEmpty() ? sheet.SheetName : m_ExcelSheetName);
            for (int j = 0; j < rowHeader.LastCellNum; j++)
            {
                dt.Columns.Add(rowHeader.Cells[j].StringCellValue);
            }
            var rows = sheet.GetRowEnumerator();
            rows.MoveNext();
            while (rows.MoveNext())
            {
                IRow row = (XSSFRow)rows.Current;

                DataRow dr = dt.NewRow();
                for (int j = 0; j < row.LastCellNum; j++)
                {
                    if (j >= dt.Columns.Count)
                    {
                        continue;
                    }

                    ICell cell = row.GetCell(j);
                    if (cell == null || string.IsNullOrEmpty(cell.ToString()))
                    {
                        dr[j] = "";
                    }
                    else
                    {
                        dr[j] = cell.ToString();
                    }
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// 导入Excel
        /// </summary>
        /// <param name="excelFilePath">Excel物理路径</param>
        /// <param name="dtTarget">返回DataTable</param>
        /// <param name="errorFun">回调函数</param>
        /// <returns></returns>
        public bool Import(string excelFilePath, ref DataTable dtTarget, Action<Exception> errorFun = null)
        {
            try
            {
                if (!isLoadMapping)
                {
                    if (!LoadMapping(ref dtTarget))
                    {
                        return false;
                    }
                }

                DataSet ds = ImportExcelDateToDataSet(excelFilePath, m_ExcelSheetName);
                if (ds.Tables[m_ExcelSheetName].Columns.Count != dtTarget.Columns.Count)
                {
                    errorFun(new Exception("上传模版有误"));
                    return false;
                }
                #region 数据映射
                for (int i = 0; i < ds.Tables[m_ExcelSheetName].Rows.Count; i++)
                {
                    DataRow tempRow = ds.Tables[m_ExcelSheetName].Rows[0];
                    DataRow excelRow = ds.Tables[m_ExcelSheetName].Rows[i];
                    //调用导入前数据处理函数，并根据返回值确定下一步处理
                    if (!ImportingBefore(ref excelRow))
                    {
                        continue;
                    }
                    DataRow sqlNewRow = dtTarget.NewRow();
                    int nnn = excelRow.ItemArray.Length < m_ColumnMapping[0].Count ? excelRow.ItemArray.Length : m_ColumnMapping[0].Count;
                    for (int j = 0; j < nnn; j++)
                    {
                        String sqlColName = m_ColumnMapping[0][j].ToString();
                        int excelColindex = (int)m_ColumnMapping[1][j];
                        bool inherit = Convert.ToBoolean(m_ColumnMapping[2][j]);
                        if (excelRow[excelColindex] == null) { break; }
                        //如果当前行当前列为空
                        if (Convert.IsDBNull(excelRow[excelColindex]))
                        {
                            //如果允许以临时值填充
                            if (inherit)
                            {
                                sqlNewRow[sqlColName] = tempRow[excelColindex];
                            }
                        }
                        else
                        {
                            //填充数据，更新缓存行数据
                            sqlNewRow[sqlColName] = excelRow[excelColindex];
                            tempRow[excelColindex] = excelRow[excelColindex];
                        }
                    }
                    //调用导入后数据处理，并根据返回值决定下一步处理
                    if (ImportingAfter(ref sqlNewRow))
                    {
                        dtTarget.Rows.Add(sqlNewRow);
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                if (errorFun != null)
                {
                    errorFun(ex);
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dt">DataTable：要导出的数据源</param>
        /// <param name="excelFilePath">string：文件的物理路径</param>
        /// <param name="isCustomHead">bool：是否自定义表头</param>
        /// <param name="tbColumnNames">List:表头</param>
        /// <param name="sheetName">string：sheet表名</param>
        /// <returns></returns>
        public static bool Output(DataTable dt, string excelFilePath, bool isCustomHead = false, List<string> tbColumnNames = null, string sheetName = "", Action<Exception> errorFun = null, int sheetIndex = 0)
        {
            if (!System.IO.File.Exists(excelFilePath))
            {
                throw new Exception("Excel 文件不存在");
            }
            if (null == dt && dt.Rows.Count == 0)
            {
                return false;
            }
            try
            {
                //1.0 创建工作薄 和 工作表对象
                NPOI.HSSF.UserModel.HSSFWorkbook book;
                using (FileStream Readfile = new FileStream(excelFilePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    book = new NPOI.HSSF.UserModel.HSSFWorkbook(Readfile);
                }
                sheetName = string.IsNullOrEmpty(sheetName) ? dt.TableName : sheetName;
                NPOI.SS.UserModel.ISheet sheet1 = book.GetSheetAt(sheetIndex); //book.CreateSheet(string.IsNullOrEmpty(sheetName) ? dt.TableName : sheetName); //添加一个sheet表
                if (null == sheet1)
                {
                    sheet1 = book.CreateSheet(sheetName);
                }
                else if (sheet1.SheetName != sheetName)
                {
                    book.SetSheetName(sheetIndex, sheetName);
                }
                //2.0给sheet1添加第一行的头部标题
                if (!isCustomHead || tbColumnNames == null || tbColumnNames.Count != dt.Columns.Count)
                {
                    tbColumnNames = new List<string>();
                    foreach (DataColumn item in dt.Columns)
                    {
                        tbColumnNames.Add(item.ColumnName);
                    }
                }
                NPOI.SS.UserModel.IRow rowHead = sheet1.CreateRow(0);//创建标题行
                for (int i = 0; i < tbColumnNames.Count; i++)
                {
                    rowHead.CreateCell(i).SetCellValue(tbColumnNames[i]);
                }

                //3.0 填充表格数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    NPOI.SS.UserModel.IRow rowTemp = sheet1.CreateRow(i + 1);//创建数据行
                    for (int j = 0; j < dt.Columns.Count; j++)//填充行数据
                    {
                        rowTemp.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                    }
                }
                //4.0 写入文件
                using (FileStream wfile = new FileStream(excelFilePath, FileMode.Create))
                {
                    book.Write(wfile);
                }
            }
            catch (Exception ex)
            {
                if (errorFun != null)
                {
                    errorFun(ex);
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="ds">要导出的数据源</param>
        /// <param name="excelFilePath">文件的物理路径</param>
        /// <param name="errorFun"></param>
        /// <returns></returns>
        public static bool Output(DataSet ds, string excelFilePath, Action<Exception> errorFun = null)
        {

            if (!System.IO.File.Exists(excelFilePath))
            {
                throw new Exception("Excel 文件不存在");
            }
            if (null == ds || ds.Tables.Count == 0)
            {
                return false;
            }
            try
            {
                //1.0 创建工作薄 和 工作表对象
                NPOI.HSSF.UserModel.HSSFWorkbook book;
                using (FileStream Readfile = new FileStream(excelFilePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    book = new NPOI.HSSF.UserModel.HSSFWorkbook(Readfile);
                }
                int sheetIndex = 0;
                foreach (DataTable dt in ds.Tables)
                {
                    string sheetName = dt.TableName ?? "sheet" + sheetIndex;
                    NPOI.SS.UserModel.ISheet sheet1 = book.GetSheetAt(sheetIndex); //book.CreateSheet(string.IsNullOrEmpty(sheetName) ? dt.TableName : sheetName); //添加一个sheet表
                    if (null == sheet1)
                    {
                        sheet1 = book.CreateSheet(sheetName);
                    }
                    else if (sheet1.SheetName != sheetName)
                    {
                        book.SetSheetName(sheetIndex, sheetName);
                    }
                    int beginRow = 1;
                    //2.0给sheet1添加第一行的头部标题
                    NPOI.SS.UserModel.IRow rowHead = sheet1.CreateRow(0);//创建标题行
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        rowHead.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                    }
                    //3.0 填充表格数据
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        NPOI.SS.UserModel.IRow rowTemp = sheet1.CreateRow(i + beginRow);//创建数据行
                        for (int j = 0; j < dt.Columns.Count; j++)//填充行数据
                        {
                            rowTemp.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                        }
                    }
                    sheetIndex++;
                }
                //4.0 写入文件
                using (FileStream wfile = new FileStream(excelFilePath, FileMode.Create))
                {
                    book.Write(wfile);
                }
                return true;
            }
            catch (Exception ex)
            {
                if (errorFun != null)
                {
                    errorFun(ex);
                }
                return false;
            }
        }

    }
}
