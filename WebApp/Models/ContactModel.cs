    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using Microsoft.AspNetCore.Mvc.Rendering;

    namespace WebApp.Models;

    public class ContactModel
    {
        [HiddenInput]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Musisz wpisać imię!")]
        [MaxLength(length:20, ErrorMessage = "Max 20 znaków!")]
        [MinLength(length:2, ErrorMessage = "Minimum 2 znaki!")]
        [Display(Name = "Imię",Order = 1)]
        public string First_name { get; set; }
        [MaxLength(length:50,ErrorMessage = "Nazwisko")]
        [MinLength(length:2, ErrorMessage = "Minimum 2 znaki!")]
        [Display(Name = "Nazwisko", Order = 2)]
        public string Last_name { get; set; }
        [EmailAddress(ErrorMessage = "Wpisz poprawny adres Email!")]
        [Display(Name = "Adres Email", Order = 3)]
        public string Email { get; set; }
        [DisplayFormat(DataFormatString = "{0:###-###-###}", ApplyFormatInEditMode = true)]
        [Phone(ErrorMessage = "Wprowadz poprawny numer telefonu!")]
        [RegularExpression(pattern:"\\d{9}", ErrorMessage = "Dlugosc numeru telefon to 9 cyfr!")]
        public string Phonenumber { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Data urodzenia")]
        public DateOnly Date_of_Birth { get; set; }

        [Display(Name = "Kategoria")]
        public Category Category { get; set; }  
        
        [HiddenInput]
        public int OrganizationId { get; set; }
        
        [Display(Name = "Organizacja")]
        public OrganizationEntity? Organization { get; set; }
        
        [ValidateNever]
        public List<SelectListItem> Organizations { get; set; }
    }
