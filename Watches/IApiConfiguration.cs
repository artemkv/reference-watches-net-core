using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Watches
{
    public interface IApiConfiguration
    {
        int ApiPageSizeLimit { get; }
    }
}
