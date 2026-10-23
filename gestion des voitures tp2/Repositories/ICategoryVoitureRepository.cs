using gestion_des_voitures_tp2.Models;

namespace gestion_des_voitures_tp2.Repositories
{
    public interface ICategoryVoitureRepository
    {
        IEnumerable<CategoryVoiture> GetAll();
        CategoryVoiture? GetById(int id);
        void Add(CategoryVoiture category);
        void Update(CategoryVoiture category);
        void Delete(int id);
        bool Exists(int id);
    }
}