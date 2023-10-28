//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace D20Tek.Shurly.Web.Services.Contracts;

public sealed class UpdateShortenedUrlRequest
{
    [Required]
    [StringLength(32, ErrorMessage = "The {0} must at max {1} characters long.")]
    [Display(Name = "Title")]
    public string Title { get; set; } = default!;

    [Required]
    [StringLength(2048, ErrorMessage = "The {0} must at max {1} characters long.")]
    [Display(Name = "Long Url")]
    public string LongUrl { get; set; } = default!;

    [StringLength(1024, ErrorMessage = "The {0} must at max {1} characters long.")]
    [Display(Name = "Summary")]
    public string? Summary { get; set; }

    [JsonIgnore]
    [StringLength(1024, ErrorMessage = "The {0} must at max {1} characters long.")]
    [Display(Name = "Tags")]
    public string TagsRaw { get; set; } = string.Empty;

    public List<string> Tags { get; set; } = new();

    [JsonIgnore]
    public bool HasPublishDate { get; set; } = false;

    public DateTime? PublishOn { get; set; }

    [JsonIgnore]
    public DateTime? LocalPublishOn { get; set; }
}
