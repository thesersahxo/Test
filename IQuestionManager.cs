using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Health.Common.Question
{
    public interface IQuestionManager
    {
        IQueryable<Question> GetAllQuestions();
        Task<List<Question>> GetAllQuestionsAsync();
        Task<Question> GetQuestionByIdAsync(int id);
        int InsertOrUpdateQuestion(Question question);

        // --- Answer Methods ---
        Task<Answer> GetAnswerByIdAsync(int id);
        Task<List<Answer>> GetAllAnswersAsync();
        IQueryable<Answer> GetAllAnswers();
        Task<List<Answer>> GetAnswersByQuestionIdAsync(int questionId);
        int InsertOrUpdateAnswer(Answer answer);
        Task DeleteAnswerAsync(Answer answer);

        // --- QuestionRelation Methods ---
        Task<List<QuestionRelation>> GetAllRelationsAsync();
        IQueryable<QuestionRelation> GetAllQuestionRelations();
        Task<List<QuestionRelation>> GetRelationsByParentQuestionIdAsync(int parentId);
        Task<List<QuestionRelation>> GetRelationsByChildQuestionIdAsync(int childId);
        int InsertOrUpdateRelation(QuestionRelation relation);

        // --- QuestionGroup Methods ---
        Task<List<QuestionGroup>> GetAllQuestionGroupsAsync();
        IQueryable<QuestionGroup> GetAllQuestionGroups();
        Task<QuestionGroup> GetQuestionGroupByIdAsync(int id);
        Task<QuestionGroup> GetByGroupNameAsync(string groupName);
        int InsertOrUpdateQuestionGroup(QuestionGroup group);
    }
}
