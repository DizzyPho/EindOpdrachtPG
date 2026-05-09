using MultipleChoiceBL.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.DTOs
{
    public class QuizDTO
    {
        public QuizDTO(int id, string name, int questionCount, List<Topic> topics)
        {
            Id = id;
            Name = name;
            QuestionCount = questionCount;
            Topics = topics;
        }

        public int Id { get; init; }
        public string Name { get; init; }
        public int QuestionCount { get; init; }
        public List<Topic> Topics { get; init; }
    }
}
