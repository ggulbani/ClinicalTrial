using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicalTrialsAPI.Domain.Entities
{
    public class ClinicalTrial
    {
        public int Id { get; set; }
        public string TrialId { get; set; } = null!;
        public string Title { get; set; } = null!;
        [Column(TypeName = "timestamp with time zone")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "timestamp with time zone")]
        public DateTime? EndDate { get; set; }
        public int Participants { get; set; }
        public string Status { get; set; } = null!;        
        public int Duration { get; set; }
    }
}
