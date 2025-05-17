using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthyU.DAL.Entities;

public class Article
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(100)]
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string ArticleText { get; set; } = string.Empty;

    public int ImageId { get; set; }

    public Image? Image { get; set; }

    public bool IsPublished { get; set; }

    public int? UserId { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = [];
}
