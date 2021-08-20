using Itau.Common.Enums;

namespace Itau.Common.Wrapper
{
    public class Response<T>
    {
        public string RedirectTo { get; set; }
        public bool IsSuccess { get; set; }

        public ErrorEnum ErrorCode { get; set; }
        public string Message { get; set; }

        public T Result { get; set; }
    }
}