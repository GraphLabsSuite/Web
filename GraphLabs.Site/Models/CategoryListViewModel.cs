using GraphLabs.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GraphLabs.Site.Models
{
	public class CategoryViewModel
	{
		public long Id { get; set; }
		public string Name { get; set; }
	}

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