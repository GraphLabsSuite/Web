using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLabs.Site.Models.Preview
{
    public interface ITaskVariantPreviewModelLoader
    {
        TaskVariantPreviewModel Load(int taskId, int labWorkId);
    }
}
