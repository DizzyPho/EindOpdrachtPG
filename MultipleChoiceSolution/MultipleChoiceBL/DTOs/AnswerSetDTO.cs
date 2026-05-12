using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.DTOs
{
    public class AnswerSetDTO
    {
        public AnswerSetDTO(int userId, List<int> answerIds)
        {
            UserId = userId;
            AnswerIds = answerIds;
        }

        public int UserId { get; set; }
        public List<int> AnswerIds { get; set; }
    }
}
