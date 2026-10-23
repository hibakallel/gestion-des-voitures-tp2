using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gestion_des_voitures_tp2.Models
{
    public class CategoryVoiture
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le libellé de la catégorie est obligatoire.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Le libellé doit comporter entre 2 et 50 caractères.")]
        [Display(Name = "Libellé de la catégorie")]
        public string Libelle { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "La description ne peut pas dépasser 250 caractères.")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "URL de l'image")]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Propriété de transport pour réceptionner le fichier téléversé depuis le formulaire HTML.
        /// L'attribut [NotMapped] indique à EF Core de NE PAS créer de colonne en base de données pour ce champ.
        /// </summary>
        [NotMapped]
        [Display(Name = "Photo illustrative")]
        public IFormFile? ImageFile { get; set; }
    }
}
