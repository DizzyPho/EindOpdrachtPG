using Microsoft.Data.SqlClient;
using MultipleChoiceBL.Builders;
using MultipleChoiceBL.Domain;
using MultipleChoiceBL.DTOs;
using MultipleChoiceBL.FactoryResults;
using MultipleChoiceBL.Interfaces;
using MultipleChoiceBL.Managers;
using MultipleChoiceBL.Messages;
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
        private MessageManager _messageManager;

        public QuizRepositorySQLS(string connectionString, MessageManager messageManager)
        {
            _connectionString = connectionString;
            _messageManager = messageManager;
        }

        public Question GetQuestion(int questionId)
        {
            const string query = "SELECT q.question_text, a.answer_text, a.is_correct  FROM question q " +
                                 "JOIN answer a ON a.question_id = q.id " +
                                 "WHERE q.id = @id";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
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
                if (Question.TryCreate(questionText, answers, out FactoryResult<Question> questionResult))
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

            const string query = "SELECT q.id, question_text, q.is_enabled from question q " +
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
                        questions.Add(new QuestionDTO(reader.GetInt32(0), reader.GetString(1), reader.GetBoolean(2)));
                    }
                }
            }

            return questions;
        }

        public Quiz GetQuiz(int id)
        {
            const string query = "select question.id, question.question_text, a.id, a.answer_text, a.is_correct, quiz.name, quiz.seed from quiz quiz " +
                                 "join quiz_questions qq on quiz.id = qq.quiz_id " +
                                 "join question question on question.id = qq.question_id " +
                                 "join answer a on a.question_id = question.id " +
                                 "where quiz.id = @id";

            Dictionary<int, string> questionTexts = new Dictionary<int, string>();
            Dictionary<int, List<Answer>> answers = new Dictionary<int, List<Answer>>();

            int seed = -1;
            string quizName = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (quizName == null) quizName = reader.GetString(5);
                        if (seed < 0) seed = reader.GetInt32(6);

                        int questionId = reader.GetInt32(0);
                        questionTexts.TryAdd(questionId, reader.GetString(1));

                        int answerId = reader.GetInt32(2);
                        string answerText = reader.GetString(3);
                        bool isCorrect = reader.GetBoolean(4);
                        Answer.TryCreate(answerId, answerText, isCorrect, out FactoryResult<Answer> answerResult);

                        if (answers.TryGetValue(questionId, out List<Answer> answerList))
                        {
                            answerList.Add(answerResult.Result);
                        }
                        else
                        {
                            answers.Add(questionId, [answerResult.Result]);
                        }
                    }
                }
            }

            List<Question> questions = new List<Question>();
            foreach (int questionId in answers.Keys)
            {
                Question.TryCreate(questionTexts[questionId], answers[questionId], questionId, out FactoryResult<Question> questionResult);
                questionResult.Result.ShuffleAnswers(new Random(seed));
                questions.Add(questionResult.Result);
            }
            
            Quiz.TryCreate(id, quizName, seed, questions, out FactoryResult<Quiz> quizResult);
            return quizResult.Result;

        }

        public void InsertUsersIfNotExists(IEnumerable<int> ids)
        {
            const string query = "if not exists (select id from dbo.[user] where id = @id) begin " +
                                 "insert into dbo.[user] values (@id) end";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            { 
                cmd.CommandText = query;
                cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));

                conn.Open();
                foreach(int id in ids)
                {
                    cmd.Parameters["@id"].Value = id;
                    cmd.ExecuteNonQuery();
                }

            }
        }
        public QuizDTO GetQuizDTO(int quizId)
        {
            const string queryQuizInfo = "select distinct quiz.name, topic.topic from quiz quiz " +
                                 "join quiz_questions qq on quiz.id = qq.quiz_id " +
                                 "join question question on question.id = qq.question_id " +
                                 "join question_topic q_topic on q_topic.question_id = question.id " +
                                 "join topic topic on q_topic.topic_id = topic.id " +
                                 "where quiz.id = @id";
            const string queryQuestionCounts = "select count(qq.question_id) as count from quiz quiz " +
                                               "join quiz_questions qq on quiz.id = qq.quiz_id " +
                                               "group by quiz.id " +
                                               "having quiz.id = @id";
            string name = null;
            List<string> topics = new List<string>();
            int questionCount = -1;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmdQuizInfo = conn.CreateCommand())
            using (SqlCommand cmdQuestionCount = conn.CreateCommand())
            {
                cmdQuizInfo.CommandText = queryQuizInfo;
                cmdQuizInfo.Parameters.AddWithValue("@id", quizId);
                cmdQuestionCount.CommandText = queryQuestionCounts;
                cmdQuestionCount.Parameters.AddWithValue("@id", quizId);

                conn.Open ();
                questionCount = (int)cmdQuestionCount.ExecuteScalar();
                using (SqlDataReader reader = cmdQuizInfo.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if(name == null)
                        {
                            name = reader.GetString(0);
                        }
                        topics.Add(reader.GetString(1));
                    }
                }
            }
            return new QuizDTO(quizId, name, questionCount, topics);
        }
        public List<QuizDTO> GetQuizDTOs()
        {
            const string queryTopicNames = "select distinct quiz.id, quiz.name, topic.topic from quiz quiz " +
                                 "join quiz_questions qq on quiz.id = qq.quiz_id " +
                                 "join question question on question.id = qq.question_id " +
                                 "join question_topic q_topic on q_topic.question_id = question.id " +
                                 "join topic topic on q_topic.topic_id = topic.id";
            const string queryQuestionCounts = "select quiz.id, count(qq.question_id) as count from quiz quiz " +
                                               "join quiz_questions qq on quiz.id = qq.quiz_id " +
                                               "group by quiz.id";

            List<QuizDTO> quizDTOs = new List<QuizDTO>();

            HashSet<int> ids = new HashSet<int>();
            Dictionary<int, string> names = new Dictionary<int, string>();
            Dictionary<int, int> questionCounts = new Dictionary<int, int>();
            Dictionary<int, List<string>> topics = new Dictionary<int, List<string>>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmdTopicNames = conn.CreateCommand())
            using (SqlCommand cmdQuestionCounts = conn.CreateCommand())
            {
                cmdTopicNames.CommandText = queryTopicNames;
                cmdQuestionCounts.CommandText = queryQuestionCounts;
                conn.Open();

                using (SqlDataReader reader = cmdTopicNames.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        ids.Add(id);
                        names.TryAdd(id, reader.GetString(1));

                        if (topics.TryGetValue(id, out var topicsList))
                        {
                            topicsList.Add(reader.GetString(2));
                        }
                        else
                        {
                            topics.Add(id, new List<string> { reader.GetString(2) });
                        }

                    }
                }
                using (SqlDataReader reader = cmdQuestionCounts.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        questionCounts[id] = reader.GetInt32(1);

                    }
                }
            }
            foreach (int id in ids)
            {
                quizDTOs.Add(new QuizDTO(id, names[id], questionCounts[id], topics[id]));
            }

            return quizDTOs;
        }
        public Dictionary<int, List<int>> GetQuestionIdsByTopic()
        {
            const string query = "SELECT q.id question_id, qt.topic_id FROM question q " +
                                 "JOIN question_topic qt on qt.question_id = q.id " +
                                 "WHERE q.is_enabled = 1";
            Dictionary<int, List<int>> ids = new Dictionary<int, List<int>>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int questionId = reader.GetInt32(0);
                        int topicId = reader.GetInt32(1);
                        if (ids.TryGetValue(topicId, out List<int> questionIds))
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
                cmd.CommandText = query + $"({string.Join(',', inClauseParameters)})";
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int questionId = reader.GetInt32(0);
                        if (!questionTexts.ContainsKey(questionId))
                        {
                            questionTexts.Add(questionId, reader.GetString(1));
                        }

                        Answer.TryCreate(reader.GetString(2), reader.GetBoolean(3), out FactoryResult<Answer> answerResult);
                        Answer answer = answerResult.Result;

                        if (questionAnswers.TryGetValue(questionId, out List<Answer> answerList))
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

            foreach (KeyValuePair<int, List<Answer>> keyValue in questionAnswers)
            {
                string text = questionTexts[keyValue.Key];
                Question.TryCreate(text, keyValue.Value, keyValue.Key, out FactoryResult<Question> questionResult);
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
        public List<Topic> GetNonEmptyTopics()
        {
            const string query = "SELECT distinct t.id, topic FROM topic t " +
                                  "JOIN question_topic qt on qt.topic_id = t.id";
            List<Topic> topics = new List<Topic>();

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
        //return DTOs of inserted questions
        public List<QuestionDTO> ImportQuestions(List<Question> questions, List<int> topicIds)
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
                List<QuestionDTO> questionDTOs = new List<QuestionDTO>();
                int questionId = -1;
                try
                {
                    foreach (Question question in questions)
                    {
                        cmdQuestion.Parameters["@question_text"].Value = question.QuestionText;
                        questionId = (int)cmdQuestion.ExecuteScalar();

                        cmdAnswer.Parameters["@question_id"].Value = questionId;
                        foreach (Answer answer in question.GetAnswers())
                        {
                            cmdAnswer.Parameters["@answer_text"].Value = answer.AnswerText;
                            cmdAnswer.Parameters["@is_correct"].Value = answer.IsCorrect ? 1 : 0;
                            cmdAnswer.ExecuteNonQuery();
                        }

                        foreach (int id in topicIds)
                        {
                            cmdQuestionTopic.Parameters["@question_id"].Value = questionId;
                            cmdQuestionTopic.Parameters["@topic_id"].Value = id;
                            cmdQuestionTopic.ExecuteNonQuery();
                        }
                        questionDTOs.Add(new QuestionDTO(questionId, question.QuestionText, true));
                    }
                    tran.Commit();
                    return questionDTOs;
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
            const string queryQuizQuestion = "INSERT INTO quiz_questions (quiz_id, question_id) VALUES (@quiz_id, @question_id)";

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
                        cmdQuizQuestion.Parameters["@quiz_id"].Value = quizId;
                        cmdQuizQuestion.Parameters["@question_id"].Value = question.Id;
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

        public Topic InsertTopic(string topicName)
        {
            int id;
            Topic topic = null;
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
                    topic = new Topic(id, topicName);
                }
                catch
                {
                    
                }
            }
            return topic;
        }

        public void SubmitAnswerSets(List<AnswerSetDTO> answerSets)
        {
            const string query = "INSERT INTO user_quiz (user_id, quiz_id, score, results_json) " +
                                 "VALUES (@user_id, @quiz_id, @score, @results_json)";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Parameters.Add(new SqlParameter("@user_id", SqlDbType.Int)); 
                cmd.Parameters.Add(new SqlParameter("@quiz_id", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@score", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@results_json", SqlDbType.NVarChar));

                conn.Open ();

                    foreach (AnswerSetDTO answerSet in answerSets)
                    {
                        cmd.Parameters["@user_id"].Value = answerSet.UserId;
                        cmd.Parameters["@quiz_id"].Value = answerSet.QuizId;
                        cmd.Parameters["@score"].Value = answerSet.Score;
                        cmd.Parameters["@results_json"].Value = answerSet.ToJson();
                        cmd.ExecuteNonQuery();
                    }
            }
        }

        public List<string> GetQuestionTopics(int questionId)
        {
            const string query = "select topic from topic t " +
                                 "join question_topic qt on qt.question_id = @question_id";
            List<string> topics = new List<string>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@question_id", questionId);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        topics.Add(reader.GetString(0));
                    }
                }
            }
            return topics;
        }

        public void SetQuestionEnabled(int id, bool isEnabled)
        {
            const string query = "update question set is_enabled = @is_enabled " +
                                 "where id = @id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            { 
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@is_enabled", isEnabled);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public List<ResultDTO> GetResultDTOs(int quizId)
        {
            const string query = "SELECT user_id, score FROM user_quiz WHERE quiz_id = @quiz_id";
            List<ResultDTO> results = new List<ResultDTO>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@quiz_id", quizId);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(new ResultDTO(reader.GetInt32(0), reader.GetInt32(1)));
                    }
                }
            }

            return results;
        }

        public AnswerSetDTO GetAnswerSet(int userId, int quizId)
        {
            const string query = "SELECT results_json FROM user_quiz WHERE quiz_id = @quiz_id and user_id = @user_id";
            AnswerSetDTO answerSet = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@user_id", userId);
                cmd.Parameters.AddWithValue("@quiz_id", quizId);
                conn.Open ();

                using(SqlDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        answerSet = AnswerSetDTO.FromJson(reader.GetString(0));
                    }
                }
            }
            return answerSet;
        }
    }
}
