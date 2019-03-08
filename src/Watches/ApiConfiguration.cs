using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Watches
{
    public class ApiConfiguration : IApiConfiguration
    {
        IConfiguration _config;

        public ApiConfiguration(IConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            _config = config;
        }

        public int ApiPageSizeLimit
        {
            get
            {
                return _config.GetValue("ApiLimits:PageSizeLimit", 100);
            }
        }

        public int ApiDefaultPageSize
        {
            get
            {
                return _config.GetValue("ApiDefaults:PageSize", 20);
            }
        }
    }
}
