using Microsoft.AspNetCore.Http.HttpResults;
using Oracle.ManagedDataAccess.Client;
using PracicaExamenMVC.Models;
using System.Data;
#region PROCEDURES
//CREATE OR REPLACE PROCEDURE SP_INSERT_COMIC
//    (P_NOMBRE COMICS.NOMBRE%TYPE, P_IMAGEN COMICS.IMAGEN%TYPE, P_DESCRIPCION COMICS.DESCRIPCION%TYPE)
//    AS P_COMIC_COD INT;
//BEGIN
//  SELECT MAX(IDCOMIC)+1 INTO P_COMIC_COD FROM COMICS;
//INSERT INTO COMICS VALUES(P_COMIC_COD, P_NOMBRE, P_IMAGEN, P_DESCRIPCION);
//COMMIT;
//END;


//DELETE
//create or replace procedure sp_delete_comic
//(p_idcomic COMICS.IDCOMIC%TYPE)
//as
//begin
// delete from COMICS where IDCOMIC=p_idcomic;
//commit;
//end;
#endregion 
namespace PracicaExamenMVC.Repositories
{
    public class RepositoryComicOracle : IRepositoryComic
    {
        private OracleConnection cn;
        private OracleCommand com;
        private OracleDataAdapter adapter;
        private DataTable tablaComic;

        public RepositoryComicOracle()
        {
            string connectionString = @"Data Source=LOCALHOST:1521/XE; Persist Security Info=True; User Id=SYSTEM; Password=oracle";
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;
            string sql = "select * from COMICS";
            OracleDataAdapter ad = new OracleDataAdapter(sql, this.cn);
            this.tablaComic = new DataTable();
            ad.Fill(this.tablaComic);
        }
        public Comic FindIdComic(int id)
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
            //EL ALIAS datos REPRESENTA CADA OBJETO DENTRO DEL CONJUNTO
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
            var consulta = from datos in this.tablaComic.AsEnumerable() select datos;
            List<Comic> comics = new List<Comic>();
            foreach (var row in consulta)
            {
                Comic per = new Comic
                {
                    IdComic = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION")

                };
                comics.Add(per);
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

        public void InsertComic(string nombre, string imagen, string descripcion)
        {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERT_COMIC";
          
            OracleParameter pamNombre = new OracleParameter(":P_NOMBRE", nombre);
            this.com.Parameters.Add(pamNombre);
            OracleParameter pamImagen = new OracleParameter(":P_IMAGEN", imagen);
            this.com.Parameters.Add(pamImagen);
            OracleParameter pamDescri = new OracleParameter(":P_DESCRIPCION", imagen);
            this.com.Parameters.Add(pamDescri);
            this.cn.Open();
            this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        //insert con lambda
        public void InsertComicLambda(string nombre, string imagen, string descripcion)
        {
            string sql = "insert into COMICS values ((SELECT MAX(IDPERSONAJE) +1 FROM PERSONAJES), :P_NOMBRE, :P_IMAGEN, :P_DESCRIPCION)";
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            OracleParameter pamNombre = new OracleParameter(":P_NOMBRE", nombre);
            this.com.Parameters.Add(pamNombre);
            OracleParameter pamImagen = new OracleParameter(":P_IMAGEN", imagen);
            this.com.Parameters.Add(pamImagen);
            OracleParameter pamDescri = new OracleParameter(":P_DESCRIPCION", descripcion);
            this.com.Parameters.Add(pamDescri);
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void DeleteComic(int IdComic)
        {
            string sql = "delete from COMICS where IDCOMIC=:id";
            //OracleParameter pamIdComic =
            //    new OracleParameter(":p_idcomic", id);
            OracleParameter pamId = new OracleParameter(":id", IdComic);
            this.com.Parameters.Add(pamId);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}
