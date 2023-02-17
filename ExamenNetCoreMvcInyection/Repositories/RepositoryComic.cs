using ExamenNetCoreMvcInyection.Models;
using System.Data.SqlClient;
using System.Data;


#region
/*
 CREATE PROCEDURE SP_CREATE_COMIC (@IDCOMIC INT OUT, @NOMBRE NVARCHAR(100), @IMAGEN NVARCHAR(100), @DESCRIPCION NVARCHAR(100))
AS
	SELECT @IDCOMIC = MAX(IDCOMIC) + 1 FROM COMICS
	INSERT INTO COMICS VALUES (@IDCOMIC, @NOMBRE, @IMAGEN, @DESCRIPCION)
GO
 */
#endregion

namespace ExamenNetCoreMvcInyection.Repositories
{
    public class RepositoryComic : IRepositoryComic
    {
        //sql connection
        SqlConnection connection;
        SqlCommand command;

        //linq connection
        SqlDataAdapter adapterComic;
        DataTable tablaComic;

        public RepositoryComic()
        {
            string connectionString = @"Data Source=LOCALHOST\DESARROLLO;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Password=MCSD2022";

            //Sql connection
            this.connection = new SqlConnection(connectionString);
            this.command = new SqlCommand();
            this.command.Connection = this.connection;

            //Linq connection
            string consultaDoctor = "SELECT * FROM COMICS";
            this.adapterComic = new SqlDataAdapter(consultaDoctor, connectionString);
            this.tablaComic = new DataTable();
            this.adapterComic.Fill(this.tablaComic);
        }

        //METODO PARA EXTRAER TODOS COMICS
        public List<Comic> GetComic()
        {
            //CONSULTA PRA EXTRAER LOS DATOS DE LA BBDD
            var consulta = from datos in this.tablaComic.AsEnumerable()
                           select datos;

            //GENERAR LISTA DONDE SE GUARDARANN LOS COMICS
            List<Comic> comics = new List<Comic>();

            //RECORREMOS LOS DATOS DE LA CONSULTA Y LOS AÑADIMOS A LA LISTA
            foreach(var row in consulta)
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
            SqlParameter paramIdComic = new SqlParameter("@IDCOMIC", 0);
            paramIdComic.Direction = ParameterDirection.Output;
            this.command.Parameters.Add(paramIdComic);

            SqlParameter paramNombre = new SqlParameter("@NOMBRE", nombre);
            SqlParameter paramImagen = new SqlParameter("@IMAGEN", imagen);
            SqlParameter paramDescripcion = new SqlParameter("@DESCRIPCION", descripcion);

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
