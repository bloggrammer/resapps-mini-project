using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WellTestAnalysis.Dtos
{
    public static class ResultStatus<TResult>
    {
        public static Result<TResult> Pass(TResult response, string message = null) => new(response, true, message);
        public static Result<TResult> Pass(string message = null) => new(true, message);
        public static Result<TResult> Fail(string error) => new(false, error);
    }
}
