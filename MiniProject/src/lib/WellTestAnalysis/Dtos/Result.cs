namespace WellTestAnalysis.Dtos
{
    public class Result
    {
        public Result(bool isSuccess, string message = null)
        {
            Succeeded = isSuccess;
            Message = message;
        }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
    }

    public class Result<TResult> : Result
    {
        public Result(TResult result, bool isSuccess, string message = null) : base(isSuccess, message) => Data = result;
        public Result(bool isSuccess, string message = null) : base(isSuccess, message) { }
        public TResult Data { get; }
    }
}
