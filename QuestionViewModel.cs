using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using static Health.Common.HinsuraEnums;

namespace Health.Web.Areas.Products.Models.Question
{
    [AutoMap(typeof(Common.Question.Question))]
    public class QuestionViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Soru Adı", Prompt = "En fazla 255 karakter girilebilir.")]
        public string Name { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(Name = "Soru Cümlesi", Prompt = "En fazla 1000 karakter girilebilir.")]
        public string Text { get; set; }

        [Required]
        [Display(Name = "Soru Türü")]
        public QuestionType QuestionType { get; set; }

        [Required]
        [Display(Name = "Cevap Kaynak Türü")]
        public AnswerSourceType AnswerSourceType { get; set; }

        [Display(Name = "Cevap Kaynak Referans Id Değeri")]
        public int? AnswerSourceId { get; set; }

        [StringLength(1000)]
        [Display(Name = "Cevap Veritabanı Sorgusu")]
        public string AnswerSqlQuery { get; set; }

        [StringLength(1000)]
        [Display(Name = "Varsıyalan Cevap Değeri")]
        public string DefaultAnswerText { get; set; }

        [Display(Name = "Varsıyalan Cevap Id Değeri")]
        public int? DefaultAnswerId { get; set; }

        [Display(Name = "Maksimum Text Uzunluğu")]
        [AdditionalMetadata("maxLength", "3")]
        [AdditionalMetadata("showTouchSpin", "true")]
        [AdditionalMetadata("touchSpinRange", "0-1000")]
        [AdditionalMetadata("touchSpinVerticalBtns", "true")]
        [AdditionalMetadata("touchSpinInitVal", "")]
        [UIHint("ExtTextBox")]
        public int? MaxLength { get; set; }

        [Display(Name = "Minimum Değer")]
        [AdditionalMetadata("maxLength", "10")]
        [AdditionalMetadata("showTouchSpin", "true")]
        [AdditionalMetadata("touchSpinRange", "-2147483648-2147483647")]
        [AdditionalMetadata("touchSpinVerticalBtns", "true")]
        [AdditionalMetadata("touchSpinInitVal", "")]
        [UIHint("ExtTextBox")]
        public decimal? MinValue { get; set; }

        [Display(Name = "Maximum Değer")]
        [AdditionalMetadata("maxLength", "10")]
        [AdditionalMetadata("showTouchSpin", "true")]
        [AdditionalMetadata("touchSpinRange", "-2147483648-2147483647")]
        [AdditionalMetadata("touchSpinVerticalBtns", "true")]
        [AdditionalMetadata("touchSpinInitVal", "")]
        [UIHint("ExtTextBox")]
        public decimal? MaxValue { get; set; }

        public int? QuestionGroupId { get; set; }

        [Required]
        [Display(Name = "Zorunlu Mu")]
        public bool IsRequired { get; set; }

        [Required]
        [Display(Name = "Varsayılan Görünür Mü")]
        public bool IsVisibleByDefault { get; set; }

        [StringLength(1000)]
        [Display(Name = "Soru için yardımcı text")]
        public string HelpText { get; set; }
    }

}