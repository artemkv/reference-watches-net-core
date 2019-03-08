using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Watches.Exceptions;

namespace Watches.Controllers.Helpers
{
    public static class PagingValidationHelper
    {
        public static void ValidatePageNumber(int pageNumber)
        {
            if (pageNumber < 0)
            {
                throw new BadRequestException(
                    $"Wrong value for page number: {pageNumber}. Page number is expected to be greater than 0.", "pageNumber");
            }
        }

        public static void ValidatePageSize(int pageSize, int pageSizeLimit)
        {
            if (pageSize < 1 || pageSize > pageSizeLimit)
            {
                throw new BadRequestException(
                    $"Wrong value for page size: {pageSize}. Page number is expected to be in 1-{pageSizeLimit} range.", "pageSize");
            }
        }
    }
}
