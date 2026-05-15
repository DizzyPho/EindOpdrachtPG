using MultipleChoiceBL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.Messages
{
    public class QuestionsImportedMessage
    {
        public QuestionsImportedMessage(List<int> topicIds, List<QuestionDTO> questions)
        {
            TopicIds = topicIds;
            Questions = questions;
        }

        public List<int> TopicIds { get; init; }
        public List<QuestionDTO> Questions { get; init; }
    }
}
