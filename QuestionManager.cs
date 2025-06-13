using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using Health.Policies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health.Common.Question
{
    public class QuestionManager : ITransientDependency, IQuestionManager
    {
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<Answer> _answerRepository;
        private readonly IRepository<QuestionRelation> _questionRelationRepository;
        private readonly IRepository<QuestionGroup> _questionGroupRepository;

        public QuestionManager(
            IRepository<Question> questionRepository,
            IRepository<Answer> answerRepository,
            IRepository<QuestionRelation> questionRelationRepository,
            IRepository<QuestionGroup> questionGroupRepository)
        {
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _questionRelationRepository = questionRelationRepository;
            _questionGroupRepository = questionGroupRepository;
        }

        // --- Question Methods ---
        public async Task<List<Question>> GetAllQuestionsAsync() => await _questionRepository.GetAllListAsync();
        public async Task<Question> GetQuestionByIdAsync(int id) => await _questionRepository.FirstOrDefaultAsync(id);
        public int InsertOrUpdateQuestion(Question question) => _questionRepository.InsertOrUpdateAndGetId(question);


        // --- Answer Methods ---
        public async Task<Answer> GetAnswerByIdAsync(int id) => await _answerRepository.FirstOrDefaultAsync(id);
        public async Task<List<Answer>> GetAllAnswersAsync() => await _answerRepository.GetAllListAsync();
        public async Task<List<Answer>> GetAnswersByQuestionIdAsync(int questionId) => await _answerRepository.GetAllListAsync(a => a.QuestionId == questionId);
        public int InsertOrUpdateAnswer(Answer answer) => _answerRepository.InsertOrUpdateAndGetId(answer);
        public async Task DeleteAnswerAsync(Answer answer) => await _answerRepository.DeleteAsync(answer);

        // --- QuestionRelation Methods ---
        public async Task<List<QuestionRelation>> GetAllRelationsAsync() => await _questionRelationRepository.GetAllListAsync();
        public async Task<List<QuestionRelation>> GetRelationsByParentQuestionIdAsync(int parentId) => await _questionRelationRepository.GetAllListAsync(r => r.ParentQuestionId == parentId);
        public async Task<List<QuestionRelation>> GetRelationsByChildQuestionIdAsync(int childId) => await _questionRelationRepository.GetAllListAsync(r => r.ChildQuestionId == childId);
        public int InsertOrUpdateRelation(QuestionRelation relation) => _questionRelationRepository.InsertOrUpdateAndGetId(relation);

        // --- QuestionGroup Methods ---
        public async Task<List<QuestionGroup>> GetAllQuestionGroupsAsync() => await _questionGroupRepository.GetAllListAsync();
        public async Task<QuestionGroup> GetQuestionGroupByIdAsync(int id) => await _questionGroupRepository.FirstOrDefaultAsync(id);
        public async Task<QuestionGroup> GetByGroupNameAsync(string groupName) => await _questionGroupRepository.FirstOrDefaultAsync(qg => qg.Name == groupName);
        public int InsertOrUpdateQuestionGroup(QuestionGroup group) => _questionGroupRepository.InsertOrUpdateAndGetId(group);

        public IQueryable<Question> GetAllQuestions() => _questionRepository.GetAll();

        public IQueryable<Answer> GetAllAnswers() => _answerRepository.GetAll();
        public IQueryable<QuestionRelation> GetAllQuestionRelations() => _questionRelationRepository.GetAll();
        public IQueryable<QuestionGroup> GetAllQuestionGroups() => _questionGroupRepository.GetAll();

        
    }
}
