using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.FactoryResults
{
    public class FactoryResult<T>
    {
        public FactoryResult(T result)
        {
            Result = result;
        }

        public FactoryResult(List<string> errors)
        {
            Errors = errors;
        }

        public T? Result { get; private set; }
        public List<String>? Errors { get; private set; }

    }
}
