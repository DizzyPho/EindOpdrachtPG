using MultipleChoiceBL.Domain;
using MultipleChoiceBL.FactoryResults;
using MultipleChoiceBL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultipleChoiceBL.Domain
{
    public class QuizGenerator
    {
        private int Seed { get; init; }
        private Random _random;
        private IQuizRepository _repository;
        private Dictionary<int, List<int>> QuestionIdsByTopic { get; init; }
        internal QuizGenerator(IQuizRepository repository)
        {
            Seed = Random.Shared.Next();
            _random = new Random(Seed);
            _repository = repository;
            QuestionIdsByTopic = _repository.GetQuestionIdsByTopic();
        }

        public Quiz GenerateQuiz(Dictionary<Topic, int> questionAmounts, string quizName)
        {
            List<int> questionIds = new List<int>();
            foreach (KeyValuePair<Topic, int> amount in questionAmounts)
            {
                int topicQuestionsAmount = 0;
                List<int> currentTopicQuestions = QuestionIdsByTopic[amount.Key.Id];
                while (topicQuestionsAmount < amount.Value)
                {
                    if (currentTopicQuestions.Count == 0)
                        break;

                    int randomQuestionId = currentTopicQuestions[_random.Next(currentTopicQuestions.Count)];

                    if (!questionIds.Contains(randomQuestionId))
                    {
                        questionIds.Add(randomQuestionId);
                        topicQuestionsAmount++;
                    }

                    currentTopicQuestions.Remove(randomQuestionId);
                }
            }

            List<Question> questions = _repository.GetQuestions(questionIds);
            Quiz.TryCreate(quizName, Seed, questions, out FactoryResult<Quiz> quizResult);

            questions.ForEach(question => question.ShuffleAnswers(_random));


            return quizResult.Result;
        }
    }
}