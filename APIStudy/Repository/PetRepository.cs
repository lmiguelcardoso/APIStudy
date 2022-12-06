using APIStudy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace APIStudy.Repository
{
    public class PetRepository
    {
        public string ConnectionString { get; set; }
        public PetRepository()
        {
            this.ConnectionString = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build().GetConnectionString("userDB");
        }

        public Pet InsertPet([FromBody] Pet newPet)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                try
                {
                    var query = "insert into pets(name,animal,race,idowner) values(@name, @animal, @race, @idowner)";
                    SqlCommand command = new SqlCommand(query, connection);
                    AddParameters(command, newPet);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return newPet;
        }

        public List<Pet> GetPets()
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                List<Pet> list = new List<Pet>();
                var query = "select * from pets";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    Pet newpet = new Pet();
                    newpet.IdPet = (int)dr["idpet"];
                    newpet.IdOwner = (int)dr["idowner"];
                    newpet.Race = (string)dr["race"];
                    newpet.Name = (string)dr["name"];
                    newpet.Animal = (string)dr["animal"];
                    list.Add(newpet);
                }
                connection.Close();
                return list;
            }
        }

        public void DeletePet(int id)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                try
                {
                    var query = "delete from pets where id = @id ";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public User FindPetById(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    var query = "select * from pets where id = @id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    connection.Open();
                    SqlDataReader dr = command.ExecuteReader();
                    dr.Read();
                    User userxpto = new User();
                    userxpto.Id = (int)dr["id"];
                    userxpto.Name = (string)dr["name"];
                    userxpto.Role = (string)dr["role"];
                    userxpto.Telephone = (string)dr["telephone"];

                    connection.Close();
                    return userxpto;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new User();
            }
        }

        public void UpdatePet(Pet petToupdate)
        {

            var query = @"UPDATE pets
            SET name = @name, race = @race, animal = @animal, idowner = @idowner
            WHERE id = @idpet";

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                AddParameters(command, petToupdate);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

        }

        public bool PetExists(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    var query = "select * from pets where id = @id";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    connection.Open();
                    SqlDataReader dr = command.ExecuteReader();

                    if (!dr.Read())
                    {
                        return false;
                    }
                    connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        public void AddParameters(SqlCommand command, Pet pet)
        {
            command.Parameters.Add("@name", SqlDbType.VarChar).Value = pet.Name;
            command.Parameters.Add("@animal", SqlDbType.VarChar).Value = pet.Animal;
            command.Parameters.Add("@race", SqlDbType.VarChar).Value = pet.Race;
            command.Parameters.Add("@idpet", SqlDbType.Int).Value = pet.IdPet;
            command.Parameters.Add("@idowner", SqlDbType.Int).Value = pet.IdOwner;
        }
    }
}
