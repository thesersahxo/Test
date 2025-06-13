using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health.Common.Question
{
    public class QuestionGroup : Entity
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; } // Grup adı

        [StringLength(1000)]
        public string Description { get; set; } // Grup açıklaması

        public virtual ICollection<Question> Questions { get; set; } // Grup içindeki sorular
    }
}
