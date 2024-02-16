using adonet.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows;
using Microsoft.VisualBasic.ApplicationServices;

namespace adonet.DAL.DAO
{
    internal class UserDao
    {
        public static List<DTO.User> GetAll()
        {
            using SqlCommand cmd = new("SELECT * FROM Users", App.MsSqlConnection);
            try
            {
                using SqlDataReader reader = cmd.ExecuteReader();
                List<DTO.User> users=new();
                while (reader.Read())
                {
                    users.Add(new(reader));
                }
                return users;
            }
            catch (Exception ex)
            {
                return null!;
            }
        }
        public static Boolean AddUser(DTO.User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            using var cmd = new SqlCommand(
                $"INSERT INTO Users(Id, Name, Login, PasswordHash, Birthdate)"+ $"VALUES( NEWID(), @name, @login, @passHash, @birthdate )",
                App.MsSqlConnection);
            cmd.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar, 64)
            {
                Value = user.Name
            });
            cmd.Parameters.Add(new SqlParameter("@login", System.Data.SqlDbType.VarChar, 64)
            {
                Value = user.Login
            });
            cmd.Parameters.Add(new SqlParameter("@passHash", System.Data.SqlDbType.Char, 32)
            {
                Value = user.PasswordHash
            });
            cmd.Parameters.Add(new SqlParameter("@birthdate", System.Data.SqlDbType.DateTime)
            {
                Value = user.Birthdate
            });
            try
            {
                cmd.Prepare();  // підготовка запиту - компіляція без параметрів
                cmd.ExecuteNonQuery();  // виконання - передача даних у скомпільований запит
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static DTO.User? GetUserByCredentials(String login, String password)
        {
            ArgumentNullException.ThrowIfNull(login);
            ArgumentNullException.ThrowIfNull(password);
            using var cmd = new SqlCommand("SELECT * FROM Users u WHERE u.login = @login", App.MsSqlConnection);
            cmd.Parameters.Add(new SqlParameter("@login", System.Data.SqlDbType.VarChar, 64)
            {
                Value = login
            });
            try
            {
                cmd.Prepare();  // підготовка запиту - компіляція без параметрів
                var reader=cmd.ExecuteReader();  // виконання - передача даних у скомпільований запит
                if(reader.Read())
                {
                    DTO.User foundUser = new DTO.User(reader);
                    if(foundUser.PasswordHash!=password)
                    {
                        return null;
                    }
                    else
                    {
                        return new DTO.User(reader);
                    }
                }
                else //User not found
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
