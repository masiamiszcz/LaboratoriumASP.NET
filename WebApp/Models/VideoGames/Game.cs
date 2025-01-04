using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models.VideoGames
{
    public partial class Game
    {
        [Key]
        public int Id { get; set; }
        
        // Walidacja dla GenreId
        [Required(ErrorMessage = "Wybór nazwy gatunku jest wymagany.")]
        [ForeignKey("Genre")]
        [Display(Name = "Nazwa Gatunku", Order = 1)]
        public int GenreId { get; set; }
        
        // Walidacja dla GameName
        [Required(ErrorMessage = "Nazwa gry jest wymagana.")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Nazwa gry musi zawierać od 2 do 255 znaków.")]
        [Display(Name = "Nazwa gry", Order = 1)] // Dodano atrybut Display
        public string GameName { get; set; }

        // Kolekcja GamePublishers, w tym przypadku nie dodajemy walidacji, ale warto mieć
        public virtual ICollection<GamePublisher> GamePublishers { get; set; } = new List<GamePublisher>();

        // Walidacja dla Genre - jest to właściwość nawigacyjna, nie jest wymagana walidacja, ale możesz dodać opcjonalnie
        public virtual Genre? Genre { get; set; }
    }
}