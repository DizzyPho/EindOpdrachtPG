using Microsoft.Data.SqlClient;
using MultipleChoiceBL.Builders;
using MultipleChoiceBL.Domain;
using MultipleChoiceBL.DTOs;
using MultipleChoiceBL.FactoryResults;
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

        public Question GetQuestion(int questionId)
        {
            const string query = "SELECT q.question_text, a.answer_text, a.is_correct  FROM question q " +
                                 "JOIN answer a ON a.question_id = q.id " +
                                 "WHERE q.id = @id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using(SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = query;
                command.Parameters.AddWithValue("@id", questionId);
                connection.Open();

                QuestionBuilder builder = new QuestionBuilder();
                string questionText = null;
                List<Answer> answers = new List<Answer>();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (questionText == null)
                        {
                            questionText = reader.GetString(0);
                        }
                        string answerText = reader.GetString(1);
                        bool isCorrect = reader.GetBoolean(2);
                        if (Answer.TryCreate(answerText, isCorrect, out FactoryResult<Answer> result))
                        {
                            answers.Add(result.Result);
                        }
                    }
                }
                if(Question.TryCreate(questionText, answers, out FactoryResult<Question> questionResult))
                {
                    return questionResult.Result;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<QuestionDTO> GetQuestionDTOs(int topicId)
        {
            List<QuestionDTO> questions = new List<QuestionDTO>();

            const string query = "SELECT q.id, question_text from question q " +
                                 "JOIN question_topic t on t.question_id = q.id " +
                                 "WHERE t.topic_id = @topic_id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@topic_id", topicId);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        questions.Add(new QuestionDTO(reader.GetInt32(0), reader.GetString(1)));
                    }
                }
            }

            return questions;
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
        // returns id of inserted topic, or -1 if topic could not be inserted.
        public int InsertTopic(string topicName)
        {
            int id;
            const string query = "INSERT INTO topic (topic) OUTPUT inserted.id VALUES (@topic)";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@topic", topicName);
                conn.Open();
                try
                {
                    id = (int)cmd.ExecuteScalar();
                }
                catch
                {
                    id = -1;
                }
            }
            return id;
        }
    }
}
