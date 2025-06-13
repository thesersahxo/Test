using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Health.Common.HinsuraEnums;

namespace Health.Web.Areas.Products.Models.Question
{
    public class QuestionRelationViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ana soru gereklidir.")]
        public int ParentQuestionId { get; set; }

        public string ParentQuestionText { get; set; } // UI için ek bilgi

        [Required(ErrorMessage = "Bağlı soru gereklidir.")]
        public int ChildQuestionId { get; set; }

        public string ChildQuestionText { get; set; } // UI için ek bilgi

        [Required]
        public QuestionRelationAction RelationAction { get; set; }

        [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir.")]
        public string Description { get; set; }
    }

}