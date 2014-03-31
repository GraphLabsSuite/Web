using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.DomainModel;
using GraphLabs.DomainModel.Extensions;
using GraphLabs.DomainModel.Repositories;
using GraphLabs.DomainModel.Services;

namespace GraphLabs.Site.Logic.Labs
{
    public interface IDemoLabEngine
    {
        LabWork[] GetDemoLabs();

        LabVariant[] GetDemoLabVariantsByLabWorkId(long id);
    }
}
