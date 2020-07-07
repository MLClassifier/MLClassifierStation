using FL.Entities.Attributes;
using FL.Entities.Implicants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FL.Services
{
    public interface IOpenModelService
    {
        IEnumerable<IAttribute> Attributes { get; set; }
        IEnumerable<IImplicant> Examples { get; set; }
    }
}
