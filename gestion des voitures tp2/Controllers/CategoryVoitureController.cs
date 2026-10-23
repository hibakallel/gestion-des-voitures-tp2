using gestion_des_voitures_tp2.Models;
using gestion_des_voitures_tp2.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace gestion_des_voitures_tp2.Controllers
{
    public class CategoryVoitureController : Controller
    {
        private readonly ICategoryVoitureRepository _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryVoitureController(ICategoryVoitureRepository repository, IWebHostEnvironment webHostEnvironment)
        {
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
        }

        // =========================================================================
        // 1. LISTE DES CATÉGORIES (READ)
        // =========================================================================
        public IActionResult Index()
        {
            var categories = _repository.GetAll();
            return View(categories);
        }

        // =========================================================================
        // 2. DÉTAILS D'UNE CATÉGORIE
        // =========================================================================
        public IActionResult Details(int id)
        {
            var category = _repository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // =========================================================================
        // 3. CRÉATION (CREATE) - GET & POST
        // =========================================================================
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryVoiture category)
        {
            if (ModelState.IsValid)
            {
                // Étape A : Sauvegarder d'abord pour obtenir l'ID auto-généré en base
                _repository.Add(category);

                // Étape B : Si une photo a été téléversée, l'enregistrer sur le disque avec l'ID
                if (category.ImageFile != null && category.ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "categories");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Nom du fichier sur disque basé sur l'ID de la catégorie : ex category_1.jpg
                    string extension = Path.GetExtension(category.ImageFile.FileName);
                    string fileName = $"category_{category.Id}{extension}";
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    // Écriture physique sur le disque du serveur
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        category.ImageFile.CopyTo(fileStream);
                    }

                    // Mise à jour de l'URL relative dans la base de données
                    category.ImageUrl = $"/uploads/categories/{fileName}";
                    _repository.Update(category);
                }

                TempData["SuccessMessage"] = "La catégorie a été créée avec succès !";
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // =========================================================================
        // 4. MODIFICATION (EDIT) - GET & POST
        // =========================================================================
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _repository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CategoryVoiture category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var existingCategory = _repository.GetById(id);
                if (existingCategory == null)
                {
                    return NotFound();
                }

                // Mise à jour des informations textuelles
                existingCategory.Libelle = category.Libelle;
                existingCategory.Description = category.Description;

                // Si un nouveau fichier image est fourni pour remplacer l'ancienne photo
                if (category.ImageFile != null && category.ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "categories");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Supprimer l'ancienne image du disque si elle existe
                    if (!string.IsNullOrEmpty(existingCategory.ImageUrl))
                    {
                        string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, existingCategory.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Enregistrer la nouvelle image avec l'ID
                    string extension = Path.GetExtension(category.ImageFile.FileName);
                    string fileName = $"category_{existingCategory.Id}{extension}";
                    string newFilePath = Path.Combine(uploadsFolder, fileName);

                    using (var fileStream = new FileStream(newFilePath, FileMode.Create))
                    {
                        category.ImageFile.CopyTo(fileStream);
                    }

                    existingCategory.ImageUrl = $"/uploads/categories/{fileName}";
                }

                _repository.Update(existingCategory);
                TempData["SuccessMessage"] = "La catégorie a été modifiée avec succès !";
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // =========================================================================
        // 5. SUPPRESSION (DELETE) - GET & POST
        // =========================================================================
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var category = _repository.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _repository.GetById(id);
            if (category != null)
            {
                // Supprimer le fichier image associé sur le disque s'il existe
                if (!string.IsNullOrEmpty(category.ImageUrl))
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, category.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _repository.Delete(id);
                TempData["SuccessMessage"] = "La catégorie et son fichier image ont été supprimés avec succès.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}