using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Health.Web.Controllers;
using Health.Web.Web.Framework;
using Abp.Web.Mvc.Authorization;
using Health.Authorization;
using System.Data;
using Health.Common.Question;
using DataTables.AspNet.Core;
using DataTables.AspNet.Mvc5;
using Abp.Authorization;
using Abp.UI;
using AutoMapper;
using Health.Web.Helpers;
using System.Collections.Generic;
using System;
using Health.Web.Areas.Products.Models.Question;
using Health.Products;
using Health.Web.Areas.Products.Models;

namespace Health.Web.Areas.Products.Controllers
{
    [RouteArea("Products")]
    [AbpMvcAuthorize(PermissionNames.Product)]
    public class QuestionGroupController : HealthControllerBase
    {
        private readonly IQuestionManager _questionManager;

        public QuestionGroupController(
            IQuestionManager questionManager)
        {
            _questionManager = questionManager;
        }

        private class QuestionStepsMenuStatusData
        {
            public int? QuestionGroupId { get; set; }
            public int StepNo { get; set; }
        }
        private async Task SetStepsMenuStatusData(QuestionStepsMenuStatusData stepsMenuStatusData)
        {
            ViewData["stepNo"] = stepsMenuStatusData.StepNo;

            /* Step 1 */
            if (stepsMenuStatusData.StepNo > 1)
            {
                ViewData["step1Done"] = "done";
            }

            ///* Step 2 */
            //if (await _productManager.AnyProductCoveragesByProductIdAsync(stepsMenuStatusData.QuestionGroupId))
            //{
            //    ViewData["step2Done"] = "done";
            //}

            ///* Step 3 */
            //if (await _productManager.AnyPlansByProductIdAsync(stepsMenuStatusData.QuestionGroupId))
            //{
            //    ViewData["step3Done"] = "done";
            //}

            ///* Step 4 */
            //var q = from pcr in _productManager.GetAllPlanCoverages()
            //        join pr in _productManager.GetAllPlan() on pcr.PlanId equals pr.Id
            //        where pr.ProductId == stepsMenuStatusData.QuestionGroupId
            //        select pcr;
            //if (await q.AnyAsync())
            //{
            //    /* at least 1 coverage must have been added for at least 1 of the plans of the product. */
            //    ViewData["step4Done"] = "done";
            //}
        }


        [Breadcrumb]
        public ActionResult Index()
        {
            return View("QuestionGroupList");
        }

        public ActionResult QuestionGroupList()
        {
            return View();
        }
        public async Task<ActionResult> QuestionGroupListPageData(IDataTablesRequest request)
        {
            var questionGroups = await _questionManager.GetAllQuestionGroupsAsync();
            var q = from p in questionGroups
                    select new
                    {

                        p.Id,
                        p.Name,
                        DisplayName = $"{p.Id} - {p.Name}",
                        p.Description,
                    };

            /* Sort Ordering */
            q = q.OrderBy(p => p.Id);

            var count = q.Count();
            var response = DataTablesResponse.Create(request, count, count, q);
            return new DataTablesJsonResult(response, JsonRequestBehavior.AllowGet);
        }

        [Route("~/Products/QuestionGroup/CreateUpdate/{id?}")]
        [Breadcrumb]
        public async Task<ActionResult> CreateUpdate(int? id = null)
        {
            var stepsMenuStatusData = new QuestionStepsMenuStatusData { StepNo = 1, QuestionGroupId = id };
            await SetStepsMenuStatusData(stepsMenuStatusData);

            QuestionGroup questionGroup = null;
            if (id != null)
            {
                try
                {
                    questionGroup = await _questionManager.GetQuestionGroupByIdAsync(id.Value);
                    ViewBag.PageTitle = questionGroup.Name.ToString();
                }
                catch (Exception)
                {
                    return RedirectToAction("Index");
                }
            }

            if (questionGroup != null)
            {
                return View(Mapper.Map<QuestionGroup, QuestionGroupViewModel>(questionGroup));
            }

            ViewBag.PageTitle = "Yeni Soru Seti Tanımlama";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleException]
        public async Task<ActionResult> CreateOrUpdate(QuestionGroupViewModel questionGroupViewModel, string process)
        {
            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException(MyHelpers.GetModelStateErrorMessage(ModelState));
            }

            QuestionGroup questionGroup;
            questionGroupViewModel.Id = questionGroupViewModel.Id;

            int questionGroupId = 0;
            string msgText = "";
            bool success = true;


            /* create or update database record */
            if (process == "create")
            {
                questionGroup = Mapper.Map(questionGroupViewModel, (QuestionGroup)null);
                questionGroupId = _questionManager.InsertOrUpdateQuestionGroup(questionGroup);
                if (success) { msgText = $"{questionGroupId} idli soru seti başarıyla eklendi."; }
            }
            else if (process == "update")
            {
                var data = await _questionManager.GetQuestionGroupByIdAsync(questionGroupViewModel.Id);
                questionGroup = Mapper.Map(questionGroupViewModel, data);
                questionGroupId = _questionManager.InsertOrUpdateQuestionGroup(questionGroup);
                if (success) { msgText = $"{questionGroupId} idli soru seti başarıyla  güncellendi."; }
            }

            var resultDict = new Dictionary<string, object>
                {
                    { "success", success },
                    { "text", $"{msgText}" },
                    { "id", questionGroupId }
                };

            return Json(resultDict);
        }

        [Route("~/Products/QuestionGroup/QuestionList/{questionGroupId?}")]
        [Breadcrumb]
        public async Task<ActionResult> QuestionList(int? questionGroupId = null)
        {
            var stepsMenuStatusData = new QuestionStepsMenuStatusData { StepNo = 2, QuestionGroupId = questionGroupId };
            await SetStepsMenuStatusData(stepsMenuStatusData);

            if (questionGroupId != null)
            {
                try
                {
                    var questionGroup = await _questionManager.GetQuestionGroupByIdAsync(questionGroupId.Value);
                    ViewData["QuestionGroupName"] = questionGroup.Name;
                    return View();
                }
                catch (Exception)/* send back to list if product id is no longer valid. */
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> QuestionListPageData(IDataTablesRequest request, int questionGroupId)
        {
            var searchVal = request.Search.Value;
            var questionQueryable = _questionManager.GetAllQuestions().Where(x => x.QuestionGroupId == questionGroupId);
            var questionList = await questionQueryable.ToListAsync();
            var questionProjectedList = from question in questionList
                                        select new
                                        {
                                            question.Id,
                                            question.HelpText,
                                            question.Name,
                                            question.Text,
                                            question.DisplayOrder,
                                            question.IsRequired,
                                            question.IsVisibleByDefault,
                                        };
            var count = questionProjectedList.Count();
            var response = DataTablesResponse.Create(request, count, count, questionProjectedList);
            return new DataTablesJsonResult(response, JsonRequestBehavior.AllowGet);
        }

        [Route("~/Products/QuestionGroup/CreateQuestion/{questionGroupId?}")]
        public async Task<ActionResult> CreateQuestion(int? questionGroupId)
        {
            if (questionGroupId == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                var questionGroup = await _questionManager.GetQuestionGroupByIdAsync(questionGroupId.Value);
                ViewData["QuestionGroupName"] = questionGroup.Name;
                var questionViewModel = new QuestionViewModel() { QuestionGroupId = questionGroupId };
                return View("CreateOrUpdateQuestion", questionViewModel);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        [Route("~/Products/QuestionGroup/CreateQuestion/{questionGroupId?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleException]
        public async Task<ActionResult> CreateQuestion(QuestionViewModel vModel, int? questionGroupId = null)
        {
            if (questionGroupId == null)
            {
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException(MyHelpers.GetModelStateErrorMessage(ModelState));
            }

            var question = Mapper.Map(vModel, (Question)null);
            var questionId = _questionManager.InsertOrUpdateQuestion(question);
            var successMsg = $"{questionId} idli soru başarıyla eklendi.";
            var resultDict = new Dictionary<string, string> { { "text", successMsg }, { "id", questionId.ToString() } };
            return Json(resultDict);
        }

        [Route("~/Products/QuestionGroup/UpdateQuestion/{questionGroupId?}/{questionId:int?}")]
        public async Task<ActionResult> UpdateQuestion(int? questionGroupId = null, int questionId = 0)
        {
            if (questionGroupId == null || questionId <= 0)
            {
                return RedirectToAction("Index");
            }

            try
            {
                var questionGroup = await _questionManager.GetQuestionGroupByIdAsync(questionGroupId.Value);
                ViewData["QuestionGroupName"] = questionGroup.Name;

                var question = await _questionManager.GetQuestionByIdAsync(questionId);
                if (question != null)
                {
                    return View("CreateOrUpdateQuestion", Mapper.Map<Question, QuestionViewModel>(question));
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [Route("~/Products/QuestionGroup/UpdateQuestion/{questionGroupId?}/{questionId:int?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleException]
        public async Task<ActionResult> UpdateQuestion(QuestionViewModel vModel)
        {
            if (vModel.Id <= 0)
            {
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException(MyHelpers.GetModelStateErrorMessage(ModelState));
            }

            var question = Mapper.Map(vModel, (Question)null);
            var questionId = _questionManager.InsertOrUpdateQuestion(question);
            var successMsg = $"{questionId} idli soru başarıyla güncellendi.";
            var resultDict = new Dictionary<string, string> { { "text", successMsg }, { "id", questionId.ToString() } };
            return Json(resultDict);
        }


        public async Task<ActionResult> ManageAnswers(int questionId)
        {
            var answers = await _questionManager.GetAnswersByQuestionIdAsync(questionId);
            var answerViewModels = answers.Select(Mapper.Map<Answer, AnswerViewModel>).ToList();
            ViewBag.QuestionId = questionId;
            return PartialView("ManageAnswers", answerViewModels);
        }

        [HttpPost]
        [HandleException]
        [ValidateAntiForgeryToken]
        [Route("~/Products/QuestionGroup/AddAnswer")]
        public JsonResult AddAnswer(string text, int questionId, int displayOrder)
        {
            // Sıra benzersiz mi kontrolü
            var exists = _questionManager.GetAllAnswers()
                .Any(a => a.QuestionId == questionId && a.DisplayOrder == displayOrder);

            if (exists)
            {
                return Json(new { success = false, message = "Bu sıra numarası zaten kullanılıyor." });
            }

            // Add your validation and service logic here
            var answer = new Answer { Text = text, QuestionId = questionId, DisplayOrder = displayOrder };
            _questionManager.InsertOrUpdateAnswer(answer);
            return Json(new { success = true, id = answer.Id });
        }

        [HttpPost]
        [HandleException]
        [ValidateAntiForgeryToken]
        [Route("~/Products/QuestionGroup/EditAnswer")]
        public async Task<JsonResult> EditAnswer(int id, string text, int displayOrder)
        {
            var answer = await _questionManager.GetAnswerByIdAsync(id);
            if (answer == null)
            {
                return Json(new { success = false, message = "Cevap bulunamadı." });
            }

            // Sıra benzersiz mi kontrolü (kendi kaydını hariç tut)
            var exists = _questionManager.GetAllAnswers()
                .Any(a => a.QuestionId == answer.QuestionId && a.DisplayOrder == displayOrder && a.Id != id);

            if (exists)
            {
                return Json(new { success = false, message = "Bu sıra numarası zaten kullanılıyor." });
            }

            answer.Text = text;
            answer.DisplayOrder = displayOrder;
            _questionManager.InsertOrUpdateAnswer(answer);
            return Json(new { success = true });
        }



        [HttpPost]
        [HandleException]
        [ValidateAntiForgeryToken]
        [Route("~/Products/QuestionGroup/DeleteAnswer")]
        public async Task<JsonResult> DeleteAnswer(int id)
        {
            var answer = await _questionManager.GetAnswerByIdAsync(id);
            if (answer == null)
            {
                return Json(new { success = false, message = "Cevap bulunamadı." });
            }

            await _questionManager.DeleteAnswerAsync(answer);
            return Json(new { success = true });
        }
    }
}