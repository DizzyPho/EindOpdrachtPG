using Microsoft.Data.SqlClient;
using MultipleChoiceBL.Domain;
using MultipleChoiceBL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MultipleChoiceDL.Repositories
{
    // MS SQL Server repository 
    public class QuizRepositorySQLS : IQuizRepository
    {
        private string _connectionString;

        public QuizRepositorySQLS(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ImportQuestions(List<Question> questions, int topicId)
        {
            const string queryQuestion = "INSERT INTO question (question_text, topic) OUTPUT INSERTED.id VALUES (@question_text, @topic_id)";
            const string queryAnswer = "INSERT INTO answer (answer_text, is_correct, question_id) VALUES (@answer_text, @is_correct, @question_id)";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmdQuestion = conn.CreateCommand())
            using (SqlCommand cmdAnswer = conn.CreateCommand())
            {
                cmdQuestion.CommandText = queryQuestion;
                cmdQuestion.Parameters.Add(new SqlParameter("@question_text", SqlDbType.NVarChar));
                cmdQuestion.Parameters.AddWithValue("@topic_id", topicId);

                cmdAnswer.CommandText = queryAnswer;
                cmdAnswer.Parameters.Add(new SqlParameter("@answer_text", SqlDbType.NVarChar));
                cmdAnswer.Parameters.Add(new SqlParameter("@is_correct", SqlDbType.Bit));
                cmdAnswer.Parameters.Add(new SqlParameter("@question_id", SqlDbType.Int));

                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                cmdQuestion.Transaction = tran;
                cmdAnswer.Transaction = tran;
                try
                {
                    foreach (Question question in questions)
                    {
                        cmdQuestion.Parameters["@question_text"].Value = question.QuestionText;
                        int questionId = (int)cmdQuestion.ExecuteScalar();

                        cmdAnswer.Parameters["@question_id"].Value = questionId;
                        foreach (Answer answer in question.GetAnswers())
                        {
                            cmdAnswer.Parameters["@answer_text"].Value = answer.AnswerText;
                            cmdAnswer.Parameters["@is_correct"].Value = answer.IsCorrect ? 1 : 0;
                            cmdAnswer.ExecuteNonQuery();
                        }
                    }
                    tran.Commit();
                } 
                catch (Exception ex) 
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
    }
}
