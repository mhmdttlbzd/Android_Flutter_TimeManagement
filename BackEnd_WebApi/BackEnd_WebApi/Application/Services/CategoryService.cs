using BackEnd_WebApi.Application.Dtos;
using BackEnd_WebApi.Application.Interfaces;
using BackEnd_WebApi.DataAccess.Interfaces;

namespace BackEnd_WebApi.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<CategoryResponceDto>> GetAll()
        {
            var categories = await _repository.GetAll();
            var res = new List<CategoryResponceDto>();
            foreach (var category in categories)
            {
                res.Add(new CategoryResponceDto
                {
                    Name = category.Name,
                    Id = category.Id
                });
            }
            return res;
        }
    }
}
