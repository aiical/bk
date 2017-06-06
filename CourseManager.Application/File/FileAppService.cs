using CourseManager;
using CourseManager.Common.Extensions;
using CourseManager.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CourseManager.File;

namespace iFuturePortal.Services.FileService
{
    public class FileAppService : CourseManagerAppServiceBase, IFileAppService
    {
        private readonly IFileRepository _fileRepository;

        public FileAppService(IFileRepository repository)
        {
            _fileRepository = repository;
        }

        public void Submit()
        {
            _fileRepository.Submit();
        }

        public Files GetFilesByCategoryIdAndType(string categoryId, string categoryType)
        {
            return _fileRepository.FirstOrDefault(o => o.RelateId == categoryId && o.CategoryType == categoryType);
        }
        public List<Files> GetFilesListByCategoryIdAndType(string categoryId, string categoryType)
        {
            return _fileRepository.GetAllList(o => o.RelateId == categoryId && o.CategoryType == categoryType);
        }
        public List<Files> GetFilesList(Expression<Func<Files, bool>> whereLamda)
        {
            return _fileRepository.GetAllList(whereLamda);
        }
        #region 增删改

        public void SetFileActive(string relateId,string categoryType, bool isActive)
        {
            var file = GetFilesListByCategoryIdAndType(relateId, categoryType);
            SetFileActive(file, isActive);
        }
        public void SetFileActive(List<Files> files, bool isActive)
        {
            if (files.IsNotEmpty())
            {
                files.ForEach(o =>
                {
                    if (o.IsActive != isActive)
                    {
                        o.IsActive = isActive;
                    }
                });
                Update(files);
            }
        }

        public Files Insert(Files model)
        {
            model = _fileRepository.Insert(model);
            Submit();
            return model;
        }

        public Files Update(Files model)
        {
            model = _fileRepository.Update(model);
            Submit();
            return model;
        }

        public void Update(List<Files> modelList)
        {
            if (modelList.IsNotEmpty())
            {
                foreach (var model in modelList)
                {
                    _fileRepository.Update(model);
                }
                Submit();
            }
        }

        public void Delete(Files model)
        {
            _fileRepository.Delete(model);
            Submit();
        }
        #endregion
    }
}
