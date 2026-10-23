using gestion_des_voitures_tp2.Data;
using gestion_des_voitures_tp2.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace gestion_des_voitures_tp2.Repositories
{
    public class CategoryVoitureRepository : ICategoryVoitureRepository
    {
        private readonly ApplicationDbContext _context;

        // Injection du DbContext par constructeur
        public CategoryVoitureRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CategoryVoiture> GetAll()
        {
            return _context.CategoriesVoitures.AsNoTracking().ToList();
        }

        public CategoryVoiture? GetById(int id)
        {
            return _context.CategoriesVoitures.Find(id);
        }

        public void Add(CategoryVoiture category)
        {
            _context.CategoriesVoitures.Add(category);
            _context.SaveChanges(); // L'ID auto-incrémenté est généré ici !
        }

        public void Update(CategoryVoiture category)
        {
            _context.CategoriesVoitures.Update(category);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var category = _context.CategoriesVoitures.Find(id);
            if (category != null)
            {
                _context.CategoriesVoitures.Remove(category);
                _context.SaveChanges();
            }
        }

        public bool Exists(int id)
        {
            return _context.CategoriesVoitures.Any(c => c.Id == id);
        }
    }
}