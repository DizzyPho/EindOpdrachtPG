using Microsoft.Data.SqlClient;
using MultipleChoiceBL.Domain;
using MultipleChoiceBL.DTOs;
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

        public List<TopicDTO> GetTopics()
        {
            List<TopicDTO> topics = new List<TopicDTO>();

            const string query = "SELECT id, topic FROM topic";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            { 
                cmd.CommandText = query;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        topics.Add(new TopicDTO(reader.GetInt32(0), reader.GetString(1)));
                    }
                }
            }
            return topics;
        }

        public void ImportQuestions(List<Question> questions, List<int> topicIds)
        {
            const string queryQuestion = "INSERT INTO question (question_text) OUTPUT INSERTED.id VALUES (@question_text)";
            const string queryAnswer = "INSERT INTO answer (answer_text, is_correct, question_id) " +
                                       "VALUES (@answer_text, @is_correct, @question_id)";
            const string queryQuestionTopic = "INSERT INTO question_topic (question_id, topic_id) VALUES (@question_id, @topic_id)";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmdQuestion = conn.CreateCommand())
            using (SqlCommand cmdAnswer = conn.CreateCommand())
            using (SqlCommand cmdQuestionTopic = conn.CreateCommand())
            {
                cmdQuestion.CommandText = queryQuestion;
                cmdQuestion.Parameters.Add(new SqlParameter("@question_text", SqlDbType.NVarChar));

                cmdAnswer.CommandText = queryAnswer;
                cmdAnswer.Parameters.Add(new SqlParameter("@answer_text", SqlDbType.NVarChar));
                cmdAnswer.Parameters.Add(new SqlParameter("@is_correct", SqlDbType.Bit));
                cmdAnswer.Parameters.Add(new SqlParameter("@question_id", SqlDbType.Int));

                cmdQuestionTopic.CommandText = queryQuestionTopic;
                cmdQuestionTopic.Parameters.Add(new SqlParameter("@question_id", SqlDbType.Int));
                cmdQuestionTopic.Parameters.Add(new SqlParameter("@topic_id", SqlDbType.Int));

                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                cmdQuestion.Transaction = tran;
                cmdAnswer.Transaction = tran;
                cmdQuestionTopic.Transaction = tran;
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

                        foreach(int id in topicIds)
                        {
                            cmdQuestionTopic.Parameters["@question_id"].Value = questionId;
                            cmdQuestionTopic.Parameters["@topic_id"].Value = id;
                            cmdQuestionTopic.ExecuteNonQuery();
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
