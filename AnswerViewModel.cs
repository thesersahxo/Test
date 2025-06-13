using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Health.Web.Areas.Products.Models.Question
{
    [AutoMap(typeof(Common.Question.Answer))]
    public class AnswerViewModel
    {
        [Display(Name = "Soru Id")]
        public int Id { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [Display(Name = "Soru Metni")]
        public string QuestionText { get; set; } // UI'da kullanılabilir

        [Required(ErrorMessage = "Seçenek metni gereklidir.")]
        [StringLength(500, ErrorMessage = "Seçenek metni en fazla 500 karakter olabilir.")]
        [Display(Name = "Cevap Metni")]
        public string Text { get; set; }

        [Display(Name = "Sıra")]
        public int DisplayOrder { get; set; }
    }

}