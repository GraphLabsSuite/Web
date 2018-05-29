using GraphLabs.Dal.Ef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.Site.Models
{
    public class CategoryCreatingModel
    {
        #region Зависимости

        private readonly ISurveyRepository _surveyRepository;
        private readonly ICategoryRepository _categoryRepository;

        #endregion


        [Required(AllowEmptyStrings = false, ErrorMessage = "Введите название темы!")]
        [MinLength(2, ErrorMessage = "Название темы слишком короткое!")]
        [MaxLength(100, ErrorMessage = "Название темы слишком длинное!")]
        public string Name { get; set; }

        public long Id { get; set; }
        
        /*public List<SelectListItem> CategoryList
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
        }*/
        public bool IsValid
        {
            get
            {
                //длина названия темы от 2 до 100 символов
                if ((this.Name.Length < 2) || (this.Name.Length > 100))
                    return false;

                //все проверки пройдены
                return true;
            }
        }

        public CategoryCreatingModel(
            ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public void Save()
        {
            /*_categoryRepository.SaveCategory(
                this.Id,
                this.Name
            );*/
        }
        //репозиторий если есть такой вопрос, то обновить
        //если нет, то сохранить
        //также может быть левое
    }
}