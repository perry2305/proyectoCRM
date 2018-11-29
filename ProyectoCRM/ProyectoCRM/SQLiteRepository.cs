using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using ProyectoCRM.Models;

namespace ProyectoCRM
{
    public class SQLiteRepository : IContactoRepository
    {
        private readonly string connection;

        public SQLiteRepository(string connstr)
        {
            connection = connstr;
        }

        public void Crear(Contacto c)
        {
            var cmd = new SQLiteCommand("INSERT INTO Contactos (Nombre, Colonia, Numero, Email, Tipo, Ciudad) VALUES (@Nombre, @Colonia, @Numero, @Email, @Tipo, @Ciudad)");
            cmd.Parameters.AddWithValue("@Nombre", c.Nombre);
            cmd.Parameters.AddWithValue("@Colonia", c.Colonia);
            cmd.Parameters.AddWithValue("@Numero", c.Numero);
            cmd.Parameters.AddWithValue("@Email", c.Email);
            cmd.Parameters.AddWithValue("@Tipo", c.Tipo);
            cmd.Parameters.AddWithValue("@Ciudad", c.Ciudad);

            ExecuteCMD(cmd);
        }

        public List<Contacto> Leer()
           {
            var cmd = new SQLiteCommand(commandText: selectcont);
            var contactos = new List<Contacto>();
            using (var con = new SQLiteConnection(connection))
            {
                cmd.Connection = con;
                cmd.Connection.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var contacto = ParseContacto(reader);
                        contactos.Add(contacto);
                    }
                }
            }
            return contactos;
        }

        public Contacto LeerPorId(int id)
        {
            var cmd = new SQLiteCommand(selectcont + " WHERE ID = @ID");
            cmd.Parameters.AddWithValue("@ID", id);
            using ( var con = new SQLiteConnection(connection))
            {
                cmd.Connection = con;
                cmd.Connection.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        var Contacto = ParseContacto(reader);
                        return Contacto;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public void Editar(Contacto c)
        {
            var cmd = new SQLiteCommand("UPDATE Contactos SET Nombre = @Nombre, Colonia = @Colonia, Numero = @Numero, Email = @Email, Tipo = @Tipo, Ciudad = @Ciudad WHERE ID = @ID");
             cmd.Parameters.AddWithValue("@ID", c.ID);
            cmd.Parameters.AddWithValue("@Nombre", c.Nombre);
            cmd.Parameters.AddWithValue("@Colonia", c.Colonia);
            cmd.Parameters.AddWithValue("@Numero", c.Numero);
            cmd.Parameters.AddWithValue("@Email", c.Email);
            cmd.Parameters.AddWithValue("@Tipo", c.Tipo);
            cmd.Parameters.AddWithValue("@Ciudad", c.Ciudad);

            ExecuteCMD(cmd);
        }

        public void Borrar(int c )
        {
            var cmd = new SQLiteCommand("DELETE FROM Contactos WHERE ID = @ID");
            cmd.Parameters.AddWithValue("@ID", c);
            ExecuteCMD(cmd);
        }

        private Contacto ParseContacto(SQLiteDataReader reader)
                {
                    var contacto = new Contacto();
                    var i = 0;
                    unchecked
                    {
                        contacto.ID = (int)reader.GetInt64(i++);
                        contacto.Nombre = reader.GetString(i++);
                        contacto.Colonia = reader.GetString(i++);
                        contacto.Numero = reader.GetString(i++);
                        contacto.Email = reader.GetString(i++);
                        contacto.Tipo = reader.GetString(i++);
                        contacto.Ciudad = reader.GetString(i++);
                    }

                    return contacto;
                }

        private string selectcont = "SELECT ID, Nombre, Colonia, Numero, Email, Tipo, Ciudad FROM Contactos";

        public int ExecuteCMD(SQLiteCommand cmd)
        {
            using (var con = new SQLiteConnection(connection))
            {
                cmd.Connection = con;
                cmd.Connection.Open();
                var inserted = cmd.ExecuteNonQuery();
                return inserted;
            }
        }
    }
    public interface IContactoRepository
    {
        void Borrar(int c);

        void Crear(Contacto c);

        void Editar(Contacto c);

        List<Contacto> Leer();

        Contacto LeerPorId(int id);
    }
}
