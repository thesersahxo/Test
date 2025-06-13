using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health.Common.Question
{
    public class Answer : Entity
    {
        [Required]
        [Index("IX_Question_DisplayOrder", 0)]
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }

        [Required(ErrorMessage = "Seçenek metni gereklidir.")]
        [StringLength(500, ErrorMessage = "Seçenek metni en fazla 500 karakter olabilir.")]
        public string Text { get; set; }

        [Index("IX_Question_DisplayOrder", 1, IsUnique = true)]
        [Required]
        public int DisplayOrder { get; set; }
    }
}
