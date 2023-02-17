using ExamenNetCoreMvcInyection.Models;
using System.Data.SqlClient;
using System.Data;
using Oracle.ManagedDataAccess.Client;

#region
/*
 create or replace procedure SP_CREATE_COMIC
(p_idComic out comics.idcomic%type
, p_nombre comics.nombre%type
, p_imagen comics.imagen%type
, p_descripcion comics.descripcion%type)
as
begin
  SELECT MAX(idcomic)+1 INTO p_idComic FROM comics;
  insert into comics values (p_idComic, p_nombre, p_imagen, p_descripcion);
commit;
end;
 */
#endregion

namespace ExamenNetCoreMvcInyection.Repositories
{
    public class RepositoryComicOracle : IRepositoryComic
    {

        OracleConnection connection;
        OracleCommand command;

        OracleDataAdapter adapterComic;
        DataTable tablaComic;

        public RepositoryComicOracle()
        {
            string connectionString = @"Data Source=LOCALHOST:1521/XE; Persist Security Info=True;User Id=system;Password=ORACLE";

            //Sql
            this.connection = new OracleConnection(connectionString);
            this.command = new OracleCommand();
            this.command.Connection = this.connection;

            //Linq
            string consultaDoctor = "select * from comics";
            this.adapterComic = new OracleDataAdapter(consultaDoctor, connectionString);
            this.tablaComic = new DataTable();
            this.adapterComic.Fill(this.tablaComic);
        }

        public List<Comic> GetComic()
        {
            //CONSULTA PRA EXTRAER LOS DATOS DE LA BBDD
            var consulta = from datos in this.tablaComic.AsEnumerable()
                           select datos;

            //GENERAR LISTA DONDE SE GUARDARANN LOS COMICS
            List<Comic> comics = new List<Comic>();

            //RECORREMOS LOS DATOS DE LA CONSULTA Y LOS AÑADIMOS A LA LISTA
            foreach (var row in consulta)
            {
                Comic comic = new Comic
                {
                    IdComic = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION"),
                };
                comics.Add(comic);
            }
            //DEVOLVEMOS LOS COMICS
            return comics;
        }

        //METODO PARA CREAR COMICS
        public void CreateComic(string nombre, string imagen, string descripcion)
        {
            //DECLARACION PROCEDIMIENTO 
            this.command.CommandType = CommandType.StoredProcedure;
            this.command.CommandText = "SP_CREATE_COMIC";

            //DECLARACION E IMPLEMENTACION DE LAS VARIABLES DEL PROCEDURE
            OracleParameter paramIdComic = new OracleParameter();
            paramIdComic.ParameterName = "p_idComic";
            paramIdComic.DbType = DbType.Int32;
            paramIdComic.Direction = ParameterDirection.Output;

            OracleParameter paramNombre = new OracleParameter("p_nombre", nombre);
            OracleParameter paramImagen = new OracleParameter("p_imagen", imagen);
            OracleParameter paramDescripcion = new OracleParameter("p_descripcion", descripcion);

            this.command.Parameters.Add(paramIdComic);
            this.command.Parameters.Add(paramNombre);
            this.command.Parameters.Add(paramImagen);
            this.command.Parameters.Add(paramDescripcion);

            this.connection.Open();
            this.command.ExecuteNonQuery();

            this.connection.Close();

            int res = int.Parse(paramIdComic.Value.ToString());

            this.command.Parameters.Clear();
        }
    }
}
