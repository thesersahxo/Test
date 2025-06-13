using Abp.Domain.Entities;
using Health.Common.Question;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Health.Common.HinsuraEnums;

namespace Health.Common.Question
{
    public class QuestionRelation : Entity
    {

        [Required(ErrorMessage = "Ana soru gereklidir.")]
        [ForeignKey("ParentQuestion")]
        public int ParentQuestionId { get; set; }

        public virtual Question ParentQuestion { get; set; }

        [Required(ErrorMessage = "Bağlı soru gereklidir.")]
        [ForeignKey("ChildQuestion")]
        public int ChildQuestionId { get; set; }

        public virtual Question ChildQuestion { get; set; }

        public QuestionRelationAction RelationAction { get; set; }  // Ör: "VisibleIf", "RequiredIf" vb.

        [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir.")]
        public string Description { get; set; }
    }
}
