using System.ComponentModel.DataAnnotations;

public enum Category
{
    [Display(Name = "Rodzina")]  //  0
    Family,

    [Display(Name = "Biznes")]   //  1
    Business,

    [Display(Name = "Przyjaciel")]  //  2
    Friend,

    [Display(Name = "Inne")]    //  3
    Other,
    
    [Display(Name = "NIEDZWONIC")]    //  4
    Notcall
}
