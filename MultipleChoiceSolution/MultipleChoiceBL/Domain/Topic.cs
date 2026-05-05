using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.Domain
{
    public record struct Topic
    {
        public string Name { get; init; }
        public List<String> Questions { get; init; }

        public Topic(string name, List<String> questions)
        {
            Name = name;
            Questions = questions;
        }
    }
}
