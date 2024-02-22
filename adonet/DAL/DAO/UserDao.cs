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
        public static bool UpdateUser(DTO.User user)
        {
            ArgumentNullException.ThrowIfNull(user);
            if (user.Id == default) //можна покращити і перевірять наявність у БД
            {
                throw new ArgumentException("Id field value must not be default","user.Id");
            }
            using var cmd = new SqlCommand(
            $"UPDATE Users SET Name=@name, Login=@login, PasswordHash=@passHash, Birthdate=@birthdate " + $" WHERE Id=@id ",
            App.MsSqlConnection);
            cmd.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.UniqueIdentifier)
            {
                Value = user.Id
            });
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
                Value =(object?)user.Birthdate ?? DBNull.Value
            });
            try
            {
                cmd.Prepare();  // підготовка запиту - компіляція без параметрів
                cmd.ExecuteNonQuery();  // виконання - передача даних у скомпільований запит
                return true;
            }
            catch (Exception ex)
            {
                App.LogError(ex.Message);
                return false;
            }
/*            return true;*/
        }

        public static bool DeleteUser(DTO.User user, bool hardMode=false)
        {
            ArgumentNullException.ThrowIfNull(user);
            if (user.Id == default) //можна покращити і перевірять наявність у БД
            {
                throw new ArgumentException("Id field value must not be default", "user.Id");
            }
            using var cmd = new SqlCommand(null, App.MsSqlConnection);
            if (hardMode)
            {
                cmd.CommandText = $"DELETE FROM Users WHERE Id='{user.Id}' ";
            }
            else
            {
                cmd.CommandText = $"UPDATE Users SET DeleteDt=CURRENT_TIMESTAMP, Name='', Birthdate=NULL WHERE Id='{user.Id}' ";
            }
            try
            {
                cmd.ExecuteNonQuery();  // виконання - передача даних у скомпільований запит
                return true;
            }
            catch (Exception ex)
            {
                App.LogError(ex.Message);
                return false;
            }
        }
        public static List<DTO.User> GetAll(bool showDeleted=false)
        {
            using SqlCommand cmd = new("SELECT * FROM Users " + (showDeleted? "" : " WHERE DeleteDt IS NULL"), App.MsSqlConnection);
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
                App.LogError(ex.Message);
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
                App.LogError(ex.Message);
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
                App.LogError(ex.Message);
                return null;
            }
        }
    }
}
