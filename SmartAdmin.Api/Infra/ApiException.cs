using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartAdmin.Infra
{
    public static class ApiException
    {
        public static ApiResult<T> ErrorToClient<T>(this Exception exception)
        {
            var erro = exception.InnerException != null ? exception.InnerException.Message : exception.Message;

            //erro = erro.Replace("ORA-", string.Empty).Replace("PLS-", string.Empty);

            return new ApiResult<T>() { Success = false, Message = erro };
        }
    }
}
