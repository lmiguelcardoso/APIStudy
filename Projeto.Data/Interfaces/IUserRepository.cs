using Data.DTO;
using Data.Models;
using Microsoft.Data.SqlClient;

namespace Data.Interfaces
{
    public interface IUserRepository
    {
        public string ConnectionString { get; set; }
        public User InsertUser(User newuser);
        public List<User> GetUsers();
        public void DeleteUser(int id);
        public User FindUserById(int id);
        public void UpdateUser(User newUser);
        public List<OwnerPetDTO> UserPets(int id);
        public bool UserExists(int id);
        public void AddParameters(SqlCommand command, User user);
    }
}