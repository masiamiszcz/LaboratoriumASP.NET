using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Models;

[Table("contacts")]
public class ContactEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(length:20, ErrorMessage = "Max 20 znaków!")]
    public string First_name { get; set; }
    
    
    public string Last_name { get; set; }
    
    
    public string Email { get; set; }
    
    [Column("phone")]
    public string Phonenumber { get; set; }
    
    [DataType(DataType.Date)]
    public DateOnly Date_of_Birth { get; set; }
    
    public Category Category { get; set; }
    
    public DateTime Created { get; set; }
}