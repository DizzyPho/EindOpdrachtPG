using MultipleChoiceBL.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.Messages
{
    public class NewTopicMessage
    {
        public NewTopicMessage(Topic topic)
        {
            Topic = topic;
        }

        public Topic Topic { get; init; } 
    }
}
