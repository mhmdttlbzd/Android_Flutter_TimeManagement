using BackEnd_WebApi.Application.Dtos;
using BackEnd_WebApi.Application.Interfaces;
using BackEnd_WebApi.DataAccess.Entities;
using BackEnd_WebApi.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BackEnd_WebApi.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITaskRepository _taskRepository;
        public CategoryService(ICategoryRepository repository, UserManager<ApplicationUser> userManager, ITaskRepository taskRepository)
        {
            _repository = repository;
            _userManager = userManager;
            _taskRepository = taskRepository;
        }
        public async Task<List<CategoryResponceDto>> GetAll(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var categories = await _repository.GetAll(user?.Id ?? string.Empty);
            var res = new List<CategoryResponceDto>();
            foreach (var category in categories)
            {
                var cat = new CategoryResponceDto
                {
                    Name = category.Name,
                    Id = category.Id

                };

                res.Add(cat);
            }
            return res;
        }

        public async Task<ApiResponce> Clear(string userName)
        {
            var user =await _userManager.FindByNameAsync(userName);
            await _repository.Clear(user?.Id.ToString() ?? string.Empty);
            return new ApiResponce { Succeeded = true,Message ="" };
        }
        public async Task<ApiResponce> ClearWithTasks(string userName)
        {
            var user =await _userManager.FindByNameAsync(userName);
            await _repository.Clear(user?.Id.ToString() ?? string.Empty);
            await _taskRepository.Clear(user?.Id.ToString() ?? string.Empty);
            
            return new ApiResponce { Succeeded = true,Message ="" };
        }  
        public async Task<ApiResponce> Delete(int id,string userName)
        {
            var user =await _userManager.FindByNameAsync(userName);
            await _repository.Delete(id,user?.Id.ToString() ?? string.Empty);
            return new ApiResponce { Succeeded = true,Message ="" };
        }
        public async Task<ApiResponce> Edit(int id,string name,string userName)
        {
            var user =await _userManager.FindByNameAsync(userName);
            await _repository.Edit(id,name,user?.Id.ToString() ?? string.Empty);
            return new ApiResponce { Succeeded = true,Message ="" };
        } 
        public async Task<ApiResponce> Create(string name,string userName)
        {
            var user =await _userManager.FindByNameAsync(userName);
            await _repository.Create(new Category { Name = name,UserId = user.Id});
            return new ApiResponce { Succeeded = true,Message ="" };
        }
        
    }
}
