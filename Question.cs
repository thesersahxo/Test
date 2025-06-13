using Abp.Domain.Entities;
using Health.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Health.Common.HinsuraEnums;

namespace Health.Common.Question
{
    public class Question : Entity
    {
        [Required]
        [StringLength(1000)]
        public string Text { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        // Ek validasyon alanları (isteğe bağlı)
        [Required]
        public QuestionType QuestionType { get; set; }

        [Required]
        public AnswerSourceType AnswerSourceType { get; set; }

        public int? AnswerSourceId { get; set; }

        [StringLength(1000)]
        public string AnswerSqlQuery { get; set; }

        [StringLength(1000)]
        public string DefaultAnswerText { get; set; }

        public int? DefaultAnswerId { get; set; }

        [Range(0, 1000)]
        public int? MaxLength { get; set; }

        public decimal? MinValue { get; set; }

        public decimal? MaxValue { get; set; }

        public int? DisplayOrder { get; set; }

        public bool IsRequired { get; set; }

        public bool IsVisibleByDefault { get; set; }

        [StringLength(1000)]
        public string HelpText { get; set; }
    }
}
