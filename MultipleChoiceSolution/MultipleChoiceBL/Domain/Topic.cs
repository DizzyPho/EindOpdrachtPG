using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.Domain
{
    public record struct Topic
    {
        public Topic(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; init; }
        public string Name { get; init; }

        public override string? ToString()
        {
            return Name;
        }
    }
}
