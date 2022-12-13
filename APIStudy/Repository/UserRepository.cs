using APIStudy.DTO;
using APIStudy.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace APIStudy.Repository
{
    public class UserRepository
    {
        public string ConnectionString { get; set; }
        public UserRepository()
        {
            this.ConnectionString = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build().GetConnectionString("userDB");
        }

        public User InsertUser(User newuser)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                try
                {
                    var query = "insert into users(name,role,telephone) values(@name, @role, @telephone)";
                    SqlCommand command = new SqlCommand(query, connection);
                    AddParameters(command, newuser);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return newuser;
        }

        public List<User> GetUsers()
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                List<User> list = new List<User>();
                var query = "select * from users";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    User newuser = new User();
                    newuser.Id = (int)dr["id"];
                    newuser.Name = (string)dr["name"];
                    newuser.Role = (string)dr["role"];
                    newuser.Telephone = (string)dr["telephone"];
                    list.Add(newuser);
                }
                connection.Close();
                return list;
            }
        }

        public void DeleteUser(int id)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                try
                {
                    var query = "delete from users where id = @id ";
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

        public User FindUserById(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    var query = "select * from users where id = @id";
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

        public void UpdateUser(User newUser)
        {

            var query = @"UPDATE users
            SET name = @name, role = @role, telephone = @telephone
            WHERE id = @id";

            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                AddParameters(command, newUser);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

        }

        public List<OwnerPetDTO> UserPets(int id)
        {
            using (SqlConnection connection = new SqlConnection(this.ConnectionString))
            {
                var petList = new List<OwnerPetDTO>();
                var query = @"SELECT 
            [dbo].[pets].idpet,
	        [dbo].[pets].name,
	        [dbo].[pets].animal,
	        [dbo].[pets].race,
	        [dbo].[users].name as ownerName,
	        [dbo].[users].role as ownerRole,
	        [dbo].[users].telephone
        FROM [dbo].[pets]
        INNER JOIN [dbo].[users] ON [dbo].[pets].idowner = [dbo].[users].id
        WHERE [dbo].[pets].idowner = @id";
                SqlCommand command = new SqlCommand(query,connection);
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                connection.Open();
                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    petList.Add(new OwnerPetDTO
                    {
                        IdPet = (int)dr["idpet"],
                        Name = (string)dr["name"],
                        Animal = (string)dr["animal"],
                        Race = (string)dr["race"],
                        OwnerName = (string)dr["ownerName"],
                        OwnerRole = (string)dr["ownerRole"],
                        Telephone = (string)dr["telephone"],

                    }) ;
                }
                connection.Close();
                return petList;
            }

        }

        public bool UserExists(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnectionString))
                {
                    var query = "select * from users where id = @id";
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

        public void AddParameters(SqlCommand command, User user)
        {
            command.Parameters.Add("@name", SqlDbType.VarChar).Value = user.Name;
            command.Parameters.Add("@role", SqlDbType.VarChar).Value = user.Role;
            command.Parameters.Add("@telephone", SqlDbType.VarChar).Value = user.Telephone;
            command.Parameters.Add("@id", SqlDbType.Int).Value = user.Id;
        }
    }
}

