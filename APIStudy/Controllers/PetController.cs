using APIStudy.Models;
using APIStudy.Repository;
using APIStudy.Validations;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIStudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : ControllerBase
    {
        public PetRepository PetRepo { get; set; }
        public PetValidator Validator { get; set; }
        public PetController()
        {
            this.PetRepo = new PetRepository();
            this.Validator = new PetValidator();
        }

        [HttpGet]
        public IActionResult Index()
        {
            var users = PetRepo.GetPets();
            return Ok(users);
        }

        [HttpPost]
        public IActionResult Cadastrar([FromBody] Pet newpet)
        {
            ValidationResult results = Validator.Validate(newpet);
            if (!results.IsValid)
            {
                return BadRequest(results.Errors);
            }
            else
            {
                var pet = PetRepo.InsertPet(newpet);
                return Ok(pet);
            }

        }

        [HttpDelete("{id}")]
        public IActionResult Excluir(int id)
        {
            var user = PetRepo.FindPetById(id);

            if (!PetRepo.PetExists(id))
            {
                return NotFound("ID não existe na base dados");
            }
            else
            {
                PetRepo.DeletePet(id);
                return Ok();
            }
        }

        [HttpPatch("{id}")]
        public IActionResult Alterar(Pet pet)
        {
            if (!PetRepo.PetExists(pet.IdPet))
            {
                return NotFound("ID não existe na base de dados");
            }

            else
            {
                var results = Validator.Validate(pet);
                if (!results.IsValid)
                {
                    return BadRequest(results.Errors);
                }
                PetRepo.UpdatePet(pet);
                return Ok();
            }

        }
    }
}
