using GraphLabs.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using GraphLabs.DomainModel.Repositories;

namespace GraphLabs.Site.Models
{
    public class CategoryListViewModel
	{
		public CategoryViewModel[] Items { get; private set; }

		public CategoryListViewModel(Category[] categories)
		{
			Items = categories.Select(c => new CategoryViewModel
			{
				Id = c.Id,
				Name = c.Name
			})
			.ToArray();
		}
	}
}