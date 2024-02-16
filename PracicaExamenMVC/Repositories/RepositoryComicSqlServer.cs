#region PROCEDURES
//CREATE PROCEDURE SP_INSERT_COMIC
//(@NOMBRE NVARCHAR(150), @IMAGEN NVARCHAR(600), @DESCRIPCION NVARCHAR(500))
//AS 
//	DECLARE @IDCOMIC INT;
//SET @IDCOMIC = (SELECT MAX(IDCOMIC) +1 FROM COMICS)
//	INSERT INTO COMICS VALUES(@IDCOMIC, @NOMBRE, @IMAGEN, @DESCRIPCION)
//GO
#endregion
using System.Data.SqlClient;
using System.Data;
using PracicaExamenMVC.Models;
using Oracle.ManagedDataAccess.Client;

namespace PracicaExamenMVC.Repositories
{
    public class RepositoryComicSqlServer:IRepositoryComic
    {
        private DataTable tablaComic;
        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryComicSqlServer()
        {
            string connectionString = @"Data Source=DESKTOP-H7HEH31\SQLEXPRESS;Initial Catalog=Hospital;Persist Security Info=True;User ID=sa;Password=MCSD2023";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            this.tablaComic = new DataTable();
            string sql = "select * from COMICS";
            SqlDataAdapter ad = new SqlDataAdapter(sql, this.cn);
            ad.Fill(this.tablaComic);
        }

        public void DeleteComic(int IdComic)
        {
            string sql = "delete from COMICS where IDCOMIC=@id";
            //OracleParameter pamIdComic =
            //    new OracleParameter(":p_idcomic", id);
            SqlParameter pamId = new SqlParameter("@id", IdComic);
            this.com.Parameters.Add(pamId);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

   

        public Comic FindIdComic(int  id)
        {
            //EL ALIAS datos REPRESENTA CADA OBJETO DENTRO DEL CONJUNTO
            var consulta = from datos in this.tablaComic.AsEnumerable()
                           where datos.Field<int>("IDCOMIC") == id
                           select datos; 
         
            var row = consulta.First();
            Comic comic = new Comic();
            comic.IdComic = row.Field<int>("IDCOMIC");
            comic.Nombre = row.Field<string>("NOMBRE");
            comic.Imagen = row.Field<string>("IMAGEN");
            comic.Descripcion = row.Field<string>("DESCRIPCION");
            return comic;
        }

        public Comic FindIdComicNombre(string comic)
        {
            var consulta = from datos in this.tablaComic.AsEnumerable()
                           where datos.Field<string>("NOMBRE") == comic
                           select datos;

            var row = consulta.First();
            Comic com = new Comic();
            com.IdComic = row.Field<int>("IDCOMIC");
            com.Nombre = row.Field<string>("NOMBRE");
            com.Imagen = row.Field<string>("IMAGEN");
            com.Descripcion = row.Field<string>("DESCRIPCION");
            return com;
        }

        public List<Comic> GetComics()
        {
            var consulta = from datos in this.tablaComic.AsEnumerable()
                           select datos;
            List<Comic> comics = new List<Comic>();
            foreach (var row in consulta)
            {
                Comic com = new Comic
                {
                    IdComic = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION")

                };
                comics.Add(com);
            }
            return comics;

        }
        public List<string> GetComicsSelect()
        {
            var consulta = (from datos in this.tablaComic.AsEnumerable()
                            select datos.Field<string>("NOMBRE")).Distinct();
            List<string> comics = new List<string>();
            foreach (string com in consulta)
            {
                comics.Add(com);
            }
            return comics;
        }


        public void InsertComic(string nombre, string imagen,string descripcion)
        {
            this.com.CommandText = "SP_INSERT_COMIC";
            this.com.CommandType = CommandType.StoredProcedure;
            SqlParameter pamNombre = new SqlParameter("@NOMBRE", nombre);
            this.com.Parameters.Add(pamNombre);
            SqlParameter pamIma = new SqlParameter("@IMAGEN", imagen);
            this.com.Parameters.Add(pamIma);
            SqlParameter pamDes = new SqlParameter("@DESCRIPCION", descripcion);
            this.com.Parameters.Add(pamDes);

            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void InsertComicLambda(string nombre, string imagen, string descripcion)
        {
            string sql = "insert into COMICS values ((SELECT MAX(IDPERSONAJE) +1 FROM PERSONAJES), @P_NOMBRE, @P_IMAGEN, @P_DESCRIPCION)";
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            OracleParameter pamNombre = new OracleParameter("@P_NOMBRE", nombre);
            this.com.Parameters.Add(pamNombre);
            OracleParameter pamImagen = new OracleParameter("@P_IMAGEN", imagen);
            this.com.Parameters.Add(pamImagen);
            OracleParameter pamDescri = new OracleParameter("@P_DESCRIPCION", descripcion);
            this.com.Parameters.Add(pamDescri);
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}
