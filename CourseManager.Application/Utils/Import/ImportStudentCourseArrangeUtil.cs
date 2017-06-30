using Abp.Dependency;
using Abp.Logging;
using CourseManager.Common;
using CourseManager.Common.Config;
using CourseManager.Core.EntitiesFromCustom;
using CourseManager.CourseArrange;
using CourseManager.CourseArrange.Dto;
using CourseManager.Students;
using CourseManager.Utils.Import.Models;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace CourseManager.Utils.Import
{
    /// <summary>
    /// 学生课程导入工具
    /// </summary>
    public class ImportStudentCourseArrangeUtil : ITransientDependency
    {
        private readonly IStudentAppService _studentAppService;
        private readonly IStudentCourseArrangeAppService _studentCourseArrangeAppService;
        /// <summary>
        /// 构造实例
        /// </summary>
        /// <param name="studentAppService"></param>
        /// <param name="studentCourseArrangeAppService"></param>
        public ImportStudentCourseArrangeUtil(
            IStudentAppService studentAppService
            , IStudentCourseArrangeAppService studentCourseArrangeAppService)
        {
            this._studentAppService = studentAppService;
            this._studentCourseArrangeAppService = studentCourseArrangeAppService;
        }


        /// <summary>
        /// 导入学生课程到数据库
        /// </summary>
        /// <param name="fileFullPath">相对路径和绝对路径都可以</param>
        /// <param name="isheet">指定要解析的ISheet</param>
        /// <returns></returns>
        public Tuple<bool, List<CreateStudentCourseArrangeInput>> SaveStudentCourseArrangeData(string fileFullPath, ISheet isheet = null)
        {

            List<CreateStudentCourseArrangeInput> resultData = new List<CreateStudentCourseArrangeInput>();
            Tuple<bool, List<CreateStudentCourseArrangeInput>> result = Tuple.Create(true, resultData);

            NPIOExcelHelper npioExcel = new NPIOExcelHelper();
            var xmlPath = WebHelper.GetMapPath(new AppConfig.WebSettings().StudentCourseArrangeImportConfig);
            fileFullPath = WebHelper.GetMapPath(fileFullPath);
            FileInfo fi = new FileInfo(fileFullPath);
            if (fi.Exists)
            {
                DataTable dt = new DataTable();
                NPIOExcelHelper npoihelp = new NPIOExcelHelper(xmlPath, isheet);
                bool isOK = npoihelp.Import(fileFullPath, ref dt, (ex) =>
                {
                    LogHelper.Logger.Error("导入学生课程出错：", ex);
                });
                if (isOK && dt != null && dt.Rows.Count > 0)
                {
                    #region 特殊处理的字段
                    Dictionary<string, Action<string, ImportStudentCourseArrangeModel>> customAction = new Dictionary<string, Action<string, ImportStudentCourseArrangeModel>>();
                    // customAction.Add("field", SetField);//字段名，对应处理方法

                    #endregion
                    #region 忽略的值

                    List<string> ignoreVals = new List<string> { "N/A" };

                    #endregion
                    List<ImportStudentCourseArrangeModel> excelData = new List<ImportStudentCourseArrangeModel>();
                    ObjectToListModel<ImportStudentCourseArrangeModel> dtToList = new ObjectToListModel<ImportStudentCourseArrangeModel>();
                    excelData = dtToList.DataTableConvertToModel(dt, customAction, ignoreVals);
                    excelData.Where(w => string.IsNullOrWhiteSpace(w.StudentName) == false).ToList().ForEach(o =>
                        {

                            //如果该学生 在这个时间已经排课了 就不插入
                            //if (_studentCourseArrangeAppService.GetArranage(new StudentCourseArrangeInput { StudentId = o.StudentName}) != null)
                            //{
                            //    return;
                            //}
                            resultData.Add(new CreateStudentCourseArrangeInput
                            {
                                StudentId = o.StudentId,
                                ClassType = o.ClassType,
                                CourseAddressType = o.CourseAddressType,
                                CourseType = o.CourseType,
                                BeginTime = o.BeginTime,
                                EndTime = o.EndTime,
                                Address = o.Address,
                                Remark = o.Remark,
                                CreationTime = DateTime.Now,
                                CreatorUserId = 1,
                                Id = IdentityCreator.NextIdentity,
                                TenantId = 1
                            });


                        });
                    var validateResult = ValidateData(resultData);
                    if (validateResult.isSuccess)
                    {
                        _studentCourseArrangeAppService.BatchInsert(resultData);
                    }
                    result = Tuple.Create(validateResult.isSuccess, resultData);
                }
                else
                {
                    if (isOK == false)
                    {
                        result = Tuple.Create(false, resultData);
                    }
                }
            }
            return result;

        }


        #region 特殊字段处理


        #endregion


        /// <summary>
        /// 验证数据的正确性
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ResultData ValidateData(List<CreateStudentCourseArrangeInput> data)
        {
            ResultData result = new ResultData();

            return result;

        }
    }
}
