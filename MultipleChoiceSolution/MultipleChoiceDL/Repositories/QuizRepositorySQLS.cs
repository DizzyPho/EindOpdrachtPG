using Microsoft.Data.SqlClient;
using MultipleChoiceBL.Builders;
using MultipleChoiceBL.Domain;
using MultipleChoiceBL.DTOs;
using MultipleChoiceBL.FactoryResults;
using MultipleChoiceBL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        
        public List<QuizDTO> GetQuizDTOs()
        {
            const string query = "select quiz.id, quiz.name, topic.topic, count(question.id) count from quiz quiz " +
                                 "join quiz_questions qq on quiz.id = qq.quiz_id " +
                                 "join question question on question.id = qq.question_id " +
                                 "join question_topic q_topic on q_topic.question_id = question.id " +
                                 "join topic topic on q_topic.topic_id = topic.id " +
                                 "group by quiz.id, quiz.name, topic.topic";
            List<QuizDTO> quizDTOs = new List<QuizDTO>();

            HashSet<int> ids = new HashSet<int>();
            Dictionary <int,  string> names = new Dictionary<int, string>();
            Dictionary<int, int> questionCounts = new Dictionary<int, int>();
            Dictionary<int, List<string>> topics = new Dictionary<int, List<string>>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;
                conn.Open();

                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        ids.Add(id);
                        names.TryAdd(id, reader.GetString(1));

                        if(topics.TryGetValue(id, out var topicsList))
                        {
                            topicsList.Add(reader.GetString(2));
                        }
                        else
                        {
                            topics.Add(id, new List<string> { reader.GetString(2) });
                        }

                        if (!questionCounts.ContainsKey(id))
                        {
                            questionCounts[id] = reader.GetInt32(3);
                        }
                        else
                        {
                            questionCounts.Add(id, reader.GetInt32(3));
                        }
                    }
                }
            }
            foreach(int id in ids)
            {
                quizDTOs.Add(new QuizDTO(id, names[id], questionCounts[id], topics[id]));
            }

            return quizDTOs;
        }
        public Dictionary<int, List<int>> GetQuestionIdsByTopic()
        {
            const string query = "SELECT q.id question_id, qt.topic_id FROM question q " +
                                 "JOIN question_topic qt on qt.question_id = q.id ";
            Dictionary<int, List<int>> ids = new Dictionary<int, List<int>>();

            using(SqlConnection conn = new SqlConnection(_connectionString))
            using(SqlCommand cmd = conn.CreateCommand()) 
            {
                cmd.CommandText = query;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        int questionId = reader.GetInt32(0);
                        int topicId = reader.GetInt32(1);
                        if(ids.TryGetValue(topicId, out List<int> questionIds))
                        {
                            questionIds.Add(questionId);
                        }
                        else
                        {
                            ids[topicId] = new List<int> { questionId };
                        }
                    }
                }
            }
            return ids;
        }

        public List<Question> GetQuestions(List<int> questionIds)
        {
            string query = "select q.id, q.question_text,a.answer_text, a.is_correct from question q " +
                           "join answer a on a.question_id = q.id " +
                           "where q.id in ";

            List<Question> questions = new List<Question>();

            Dictionary<int, string> questionTexts = new Dictionary<int, string>();
            Dictionary<int, List<Answer>> questionAnswers = new Dictionary<int, List<Answer>>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {

                List<String> inClauseParameters = new List<string>();
                for (int i = 0; i < questionIds.Count; i++)
                {
                    string parameter = "@id" + i;
                    inClauseParameters.Add(parameter);
                    cmd.Parameters.AddWithValue(parameter, questionIds[i]);
                }
                cmd.CommandText = query + $"({string.Join(',',inClauseParameters)})";
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int questionId = reader.GetInt32(0);
                        if(!questionTexts.ContainsKey(questionId))
                        {
                            questionTexts.Add(questionId, reader.GetString(1));
                        }

                        Answer.TryCreate(reader.GetString(2), reader.GetBoolean(3), out FactoryResult<Answer> answerResult);
                        Answer answer = answerResult.Result;

                        if(questionAnswers.TryGetValue(questionId, out List<Answer> answerList))
                        {
                            answerList.Add(answer);
                        }
                        else
                        {
                            questionAnswers.Add(questionId, new List<Answer> { answer });
                        }
                    }
                }
            }
            
            foreach(KeyValuePair<int, List<Answer>> keyValue in questionAnswers)
            {
                string text = questionTexts[keyValue.Key];
                Question.TryCreate(text, keyValue.Value, out FactoryResult<Question> questionResult);
                questions.Add(questionResult.Result);
            }
            
            return questions;
        }

        public List<Topic> GetTopics()
        {
            List<Topic> topics = new List<Topic>();

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
                        topics.Add(new Topic(reader.GetInt32(0), reader.GetString(1)));
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

        public int InsertQuiz(Quiz quiz)
        {
            const string queryQuiz = "INSERT INTO quiz (name, seed) OUTPUT INSERTED.id VALUES (@name, @seed)";
            const string queryQuizQuestion = "INSERT INTO quiz_question (quiz_id, question_id) OUTPUT INSERTED.id VALUES (@quiz_id, @question_id)";

            int quizId = -1;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmdQuiz = conn.CreateCommand())
            using (SqlCommand cmdQuizQuestion = conn.CreateCommand())
            {
                cmdQuiz.CommandText = queryQuiz;
                cmdQuiz.Parameters.AddWithValue("@name", quiz.Name);
                cmdQuiz.Parameters.AddWithValue("@seed", quiz.Seed);

                cmdQuizQuestion.CommandText = queryQuizQuestion;
                cmdQuizQuestion.Parameters.Add(new SqlParameter("@quiz_id", SqlDbType.Int));
                cmdQuizQuestion.Parameters.Add(new SqlParameter("@question_id", SqlDbType.Int));

                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                cmdQuiz.Transaction = transaction;
                cmdQuizQuestion.Transaction = transaction;
                try
                {
                    quizId = (int)cmdQuiz.ExecuteScalar();

                    foreach (Question question in quiz.Questions)
                    {
                        cmdQuizQuestion.Parameters["quiz_id"].Value = quizId;
                        cmdQuizQuestion.Parameters["question_id"].Value = question.Id;
                        cmdQuizQuestion.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }

                return quizId;
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
