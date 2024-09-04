using Domain.Common.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Dto;

namespace Domain.Entities;

    public class Question : BaseEntity
    {
        public string QuestionText { get; set; }
        
        // Foreign key for the correct option
        public int? CorrectOptionId { get; set; } 

        [ForeignKey(nameof(CorrectOptionId))]
        public Answer CorrectOption { get; set; }

        // Navigation property for the related options
        public ICollection<Answer> Options { get; set; } = new List<Answer>();

        // Foreign key for Quiz
        public int QuizId { get; set; }

        [ForeignKey(nameof(QuizId))]
        public Quiz Quiz { get; set; }
    }