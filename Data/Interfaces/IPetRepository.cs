using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IPetRepository
    {
        public string ConnectionString { get; set; }
        public Pet InsertPet([FromBody] Pet newPet);
        public List<Pet> GetPets();
        public void DeletePet(int id);
        public User FindPetById(int id);
        public void UpdatePet(Pet petToupdate);
        public bool PetExists(int id);
        public void AddParameters(SqlCommand command, Pet pet);
    }
}
