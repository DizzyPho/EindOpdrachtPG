using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.DTOs
{
    public class ResultDTO
    {
        public ResultDTO(int userId, int score)
        {
            UserId = userId;
            Score = score;
        }

        public int UserId { get; init; }
        public int Score { get; init; }
    }
}
