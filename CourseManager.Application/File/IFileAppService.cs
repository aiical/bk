using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CourseManager.File
{
    public interface IFileAppService : IApplicationService
    {
        Files GetFilesByCategoryIdAndType(string categoryId, string categoryType);
        List<Files> GetFilesListByCategoryIdAndType(string categoryId, string categoryType);
        List<Files> GetFilesList(Expression<Func<Files, bool>> whereLamda);
        /// <summary>
        /// 提交修改，保存到数据库
        /// </summary>
        void Submit();

        #region 增删改

        Files Insert(Files model);
        Files Update(Files model);
        void Update(List<Files> modelList);
        void Delete(Files model);
        /// <summary>
        /// 修改文件的Active状态值（备用，暂时不调用）
        /// </summary>
        /// <param name="relateId"></param>
        /// <param name="categoryType"></param>
        /// <param name="isActive"></param>
        void SetFileActive(string relateId, string categoryType, bool isActive);
        /// <summary>
        /// 修改文件的Active状态值（备用，暂时不调用）
        /// </summary>
        /// <param name="files"></param>
        /// <param name="isActive"></param>
        void SetFileActive(List<Files> files, bool isActive);

        #endregion
    }
}
