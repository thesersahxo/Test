using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Health.Web.Areas.Products.Models.Question
{
    public class QuestionGroupViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Grup adı gereklidir.")]
        [StringLength(255, ErrorMessage = "Grup adı en fazla 255 karakter olabilir.")]
        public string Name { get; set; }

        [StringLength(1000, ErrorMessage = "Grup açıklaması en fazla 1000 karakter olabilir.")]
        [Display(Name = "Açıklama Metni", Prompt = "En fazla 1000 karakter girilebilir.")]
        [AdditionalMetadata("maxlength", "1000")]
        [AdditionalMetadata("style", "height: 300px")]
        [AdditionalMetadata("showMaxLengthCounter", "true")]
        [UIHint("StdTextArea")]
        public string Description { get; set; }

        public List<QuestionViewModel> Questions { get; set; } = new List<QuestionViewModel>(); // Grup içindeki sorular
    }

}