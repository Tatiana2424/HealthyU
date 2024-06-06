using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HealthyU.DAL.Entities;

public class Image
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(100)]
    public string? Title { get; set; }

    [MaxLength(100)]
    public string? Alt { get; set; }

    [Required]
    public string Url { get; set; }

}
