using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Models;

public class ContactModel
{
    [HiddenInput]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Musisz wpisać imię!")]
    [MaxLength(length:20, ErrorMessage = "Max 20 znaków!")]
    [MinLength(length:2, ErrorMessage = "Minimum 2 znaki!")]
    public string First_name { get; set; }
    [MaxLength(length:50,ErrorMessage = "Nazwisko")]
    [MinLength(length:2, ErrorMessage = "Minimum 2 znaki!")]
    public string Last_name { get; set; }
    [EmailAddress(ErrorMessage = "Wpisz poprawny adres Email!")]
    public string Email { get; set; }
    [Phone(ErrorMessage = "Wprowadz poprawny numer telefonu!")]
    [RegularExpression(pattern:"\\d{9}", ErrorMessage = "Dlugosc numeru telefon to 9 cyfr!")]
    public string Phonenumber { get; set; }
    [DataType(DataType.Date)]
    public DateOnly Date_of_Birth { get; set; }
}
