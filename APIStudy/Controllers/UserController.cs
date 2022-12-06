using APIStudy.Models;
using APIStudy.Repository;
using APIStudy.Validations;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace APIStudy.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        public UserRepository UserRepo { get; set; }
        public UserValidator Validator { get; set; }
        public UserController()
        {
            this.UserRepo = new UserRepository();
            this.Validator = new UserValidator();
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<User> users = UserRepo.GetUsers();
            return Ok(users);
        }

        [HttpPost]
        public IActionResult Cadastrar([FromBody] User user) 
        { 
            ValidationResult results =  Validator.Validate(user);
            if (!results.IsValid)
            {
                return NotFound();
            }
            else
            {
                UserRepo.InsertUser(user);
                return Ok();
            }
            
        }

        [HttpDelete("{id}")]
        public IActionResult Excluir(int id)
        {
            var user = UserRepo.FindUserById(id);

            if (!UserRepo.UserExists(id))
            {
            return NotFound(user.Id);
            }
            else
            {
                UserRepo.DeleteUser(id);
                return Ok();
            }
        }

        [HttpPatch("{id}")]
        public IActionResult Alterar(User user)
        {
            if (!UserRepo.UserExists(user.Id))
            {
                return NotFound("ID não existe na base de dados");
            }
            
            else
            {
                var results = Validator.Validate(user);
                if (!results.IsValid)
                {
                    return BadRequest(results.Errors);
                }
                UserRepo.UpdateUser(user);
                return Ok();
            }

        }

        [HttpGet("Person/pets/{id}")]
        public IActionResult Pets(int id)
        {
            var pets = UserRepo.UserPets(id);
            return Ok(pets);

        }

    }
}
