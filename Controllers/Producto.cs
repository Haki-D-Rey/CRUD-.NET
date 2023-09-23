using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SCELL.Model;
using System.Data;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SCELL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Producto : ControllerBase
    {

        private static DBConection DBModel =  new DBConection();
        private static SqlConnection connection = new SqlConnection(DBModel.GetConnectionString());

        internal class ProductoModel : IProducto
        {
            public int? idProducto { get; set; }
            public string descripcion { get; set; } = string.Empty;
            public int cantidad { get; set; }
            public string? precio { get; set; }
            public DateTime fechaCreacion { get; set; }
            public DateTime? fechaModificacion { get; set; }
            public bool estadoActivo { get; set; }
        }

        // GET: api/<Producto>
        [HttpGet]
        public IEnumerable<IProducto> Get()
        {
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM dbo.Producto"; // Remove "@" before "SELECT"
            var dataReader = cmd.ExecuteReader();
            List<IProducto> listaProducto = new List<IProducto>();

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    // Crear un objeto anónimo para almacenar los datos
                    ProductoModel dataObject = new ProductoModel {
                        idProducto = (int)dataReader["IdProducto"],
                        descripcion = (string)dataReader["Descripcion"],
                        cantidad = (int)dataReader["Cantidad"],
                        precio = Convert.ToString((decimal)dataReader["Precio"]),
                        fechaCreacion = (DateTime)dataReader["FechaCreacion"],
                        fechaModificacion = dataReader["FechaModificacion"] != DBNull.Value  ? (DateTime)dataReader["FechaModificacion"] : null,
                        estadoActivo = (bool)dataReader["EstadoActivo"]
                    // Agrega más columnas según sea necesario
                    };

                    listaProducto.Add(dataObject);
                }
            }

                connection.Close(); // Close the connection when you're done
            return listaProducto;
        }

        // GET api/<Producto>/5
        [HttpGet("{id}")]
        public IEnumerable<IProducto> Get(int id)
        {
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM dbo.Producto WHERE IdProducto="+ id +""; // Remove "@" before "SELECT"
            var dataReader = cmd.ExecuteReader();

            List<IProducto> listaProducto = new List<IProducto>();

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    // Crear un objeto anónimo para almacenar los datos
                    ProductoModel dataObject = new ProductoModel
                    {
                        idProducto = (int)dataReader["IdProducto"],
                        descripcion = (string)dataReader["Descripcion"],
                        cantidad = (int)dataReader["Cantidad"],
                        precio = Convert.ToString((decimal)dataReader["Precio"]),
                        fechaCreacion = (DateTime)dataReader["FechaCreacion"],
                        fechaModificacion = dataReader["FechaModificacion"] != DBNull.Value ? (DateTime)dataReader["FechaModificacion"] : null,
                        estadoActivo = (bool)dataReader["EstadoActivo"]
                        // Agrega más columnas según sea necesario
                    };

                    listaProducto.Add(dataObject);
                }
            }

            connection.Close(); // Close the connection when you're done
            return listaProducto;
        }

        // POST api/<Producto>
        [HttpPost]
        public IEnumerable<IProducto> Post([FromBody] string value)
        {
            List<dynamic> arregloJson = JsonConvert.DeserializeObject<List<dynamic>>(value);

            connection.Open();

            Console.WriteLine(arregloJson);

            foreach (var item in arregloJson) { 
            
                Console.WriteLine(item.Descripcion);
                Console.WriteLine(item.Precio);
                Console.WriteLine(item.Cantidad);
            }

            SqlCommand cmd = new SqlCommand("prAgregarProducto", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Clear();
            cmd.Parameters.Add("@JsonProducto", SqlDbType.VarChar, 8000).Value = value;
            var dataReader = cmd.ExecuteReader();

            Console.WriteLine(dataReader);

            List<IProducto> listaProducto = new List<IProducto>();

            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    // Crear un objeto anónimo para almacenar los datos
                    ProductoModel dataObject = new ProductoModel
                    {
                        idProducto = (int)dataReader["IdProducto"],
                        descripcion = (string)dataReader["Descripcion"],
                        cantidad = (int)dataReader["Cantidad"],
                        precio = Convert.ToString((decimal)dataReader["Precio"]),
                        fechaCreacion = (DateTime)dataReader["FechaCreacion"],
                        fechaModificacion = dataReader["FechaModificacion"] != DBNull.Value ? (DateTime)dataReader["FechaModificacion"] : null,
                        estadoActivo = (bool)dataReader["EstadoActivo"]
                        // Agrega más columnas según sea necesario
                    };

                    listaProducto.Add(dataObject);
                }
            }

            connection.Close(); // Close the connection when you're done
            return listaProducto;
        }

        // PUT api/<Producto>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Producto>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
