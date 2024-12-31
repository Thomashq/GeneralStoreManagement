using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }

        public ApiResponse(T data, bool success, string message = null, List<string> errors = null)
        {
            Data = data;
            Success = success;
            Message = message ?? (success ? "Operation succeeded." : "Operation failed.");
            Errors = errors ?? new List<string>();
        }
    }
}
