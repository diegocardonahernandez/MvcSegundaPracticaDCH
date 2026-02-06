using Microsoft.Data.SqlClient;
using MvcSegundaPracticaDCH.Models;
using System.Data;

namespace MvcSegundaPracticaDCH.Repositories
{
    public class RepositoryComics
    {
        private DataTable tablaComics;
        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryComics()
        {
            string connectionString = @"Data Source=LOCALHOST\DEVELOPER;Initial Catalog=COMICS;User ID=SA;Encrypt=True;Trust Server Certificate=True";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;

            string sql = "SELECT * FROM COMICS";
            SqlDataAdapter ad = new SqlDataAdapter(sql, connectionString);
            this.tablaComics = new DataTable();
            ad.Fill(this.tablaComics);
        }

        public List<Comic> getComics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable() select datos;
            List<Comic> comics = new List<Comic>();

                foreach(var row in consulta)
            {
                Comic comic = new Comic
                {
                    IdComic = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION")
                };
                comics.Add(comic);
            }
            return comics;
        }

        public Comic GetComicdata(int id) {

            var consulta = from datos in this.tablaComics.AsEnumerable()
                           where datos.Field<int>("IDCOMIC") == id select datos;
            var row = consulta.First();

            Comic comic = new Comic
            {
                IdComic = row.Field<int>("IDCOMIC"),
                Nombre = row.Field<string>("NOMBRE"),
                Imagen = row.Field<string>("IMAGEN"),
                Descripcion = row.Field<string>("DESCRIPCION")
            };
            return comic;
        }

        private int GetMaxId()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable() select datos;
            int max = consulta.Max(x => x.Field<int>("IDCOMIC")) + 1;
            return max;
                
        }

        public async Task CrearComic(string nombre, string imagen, string descrpcion)
        {
            string sql = "INSERT INTO COMICS VALUES(@id,@nombre,@imagen,@descripcion)";
            int idcomic = GetMaxId();

            this.com.Parameters.AddWithValue("@id", idcomic);
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@imagen", imagen);
            this.com.Parameters.AddWithValue("@descripcion", descrpcion);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

    }
}
