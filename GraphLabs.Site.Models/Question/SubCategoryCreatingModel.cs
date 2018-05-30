using GraphLabs.Dal.Ef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.DomainModel;

namespace GraphLabs.Site.Models
{
    public class SubCategoryCreatingModel
    {
        #region Зависимости

        private readonly ISurveyRepository _surveyRepository;
        private readonly ICategoryRepository _categoryRepository;

        #endregion


        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходимо указать название подтемы!")]
        public string Name { get; set; }
        public long CategoryId { get; set; }
        public long SubCategoryId { get; set; }

        public List<SelectListItem> CategoryList
        {
            get
            {
                return _categoryRepository.GetAllCategories().Select(
                    cat => new SelectListItem
                    {
                        Value = cat.Id.ToString(),
                        Text = cat.Name
                    }
                    ).ToList();
            }
        }

        public bool IsValid
        {
            get
            {
                //непустое название темы
                if (this.Name.Length == 0) return false;

                //все проверки пройдены
                return true;
            }
        }

        public SubCategoryCreatingModel(
            ISurveyRepository surveyRepository,
            ICategoryRepository categoryRepository)
        {
            _surveyRepository = surveyRepository;
            _categoryRepository = categoryRepository;
        }

        public void Save()
        {
            _surveyRepository.SaveSubCategory(
                this.CategoryId,
                this.SubCategoryId,
                this.Name);
        }
        //репозиторий если есть такой вопрос, то обновить
        //если нет, то сохранить
        //также может быть левое
    }
}