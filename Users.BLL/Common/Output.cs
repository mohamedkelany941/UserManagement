using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.BLL.Common
{
    public class Output<T>
    {
        public List<ErrorOutput> ErrorsList { get; set; }
        public string[] Errors { get; set; }
        public ErrorOutput FirstError
        { get { return !IsValid ? ErrorsList?.FirstOrDefault() : new ErrorOutput(); } }
        public string FirstErrorMessage
        { get { return !IsValid ? ErrorsList?.FirstOrDefault()?.Message : ""; } }

        public bool IsValid
        { get { return ErrorsList == null || ErrorsList.Count <= 0; } }
        public T Value { get; set; }

        public Output(T value)
        {
            Value = value;
        }

        public Output()
        {
        }

        public void AddError(string code, string message)
        {
            if (ErrorsList == null)
                ErrorsList = new List<ErrorOutput>();
            ErrorsList.Add(new ErrorOutput { Code = code, Message = message });
        }
    }

    public class ErrorOutput
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
