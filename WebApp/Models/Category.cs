using System.ComponentModel.DataAnnotations;

public enum Category
{
    [Display(Name = "Rodzina")]
    Family,

    [Display(Name = "Biznes")]
    Business,

    [Display(Name = "Przyjaciel")]
    Friend,

    [Display(Name = "Inne")]
    Other
}
