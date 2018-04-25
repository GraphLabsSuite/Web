using GraphLabs.Dal.Ef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.DomainModel;

namespace GraphLabs.Site.Models.Survey
{
    class CategoryCreatingModel
    {
        #region Зависимости

        private readonly ICategoryRepository _categoryRepository;

        #endregion

        [Required(AllowEmptyStrings = false, ErrorMessage = "Необходимо указать название темы!")]
        [MinLength(2, ErrorMessage = "Название темы слишком короткое!")]
        [MaxLength(100, ErrorMessage = "Название темы слишком длинное!")]

        public long CategoryId { get; set; }
        public string CategoryName { get; set; }

        public bool IsValid
        {
            get
            {
                //длина темы от 1 до 100 символов
                if ((this.CategoryName.Length < 2) || (this.CategoryName.Length > 100))
                    return false;

                //все проверки пройдены
                return true;
            }
        }

        public CategoryCreatingModel(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public void Save()
        {
            _categoryRepository.SaveCategory(
            this.CategoryId,
            this.CategoryName
            );
        }

    }
}
