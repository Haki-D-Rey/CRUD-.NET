using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        private static DBConection DBModel = new DBConection();
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

        // GET api/<Producto>/5
        [HttpGet("{id}")]
        public IEnumerable<IProducto> Get(int id)
        {
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM dbo.Producto WHERE IdProducto=" + id + ""; // Remove "@" before "SELECT"
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

            //Console.WriteLine(arregloJson);
            List<IProducto> listaProducto = new List<IProducto>();
            foreach (var item in arregloJson)
            {
                string json = "";
                json = JsonConvert.SerializeObject(item);
                Console.WriteLine(item.Descripcion);
                Console.WriteLine(item.Precio);
                Console.WriteLine(item.Cantidad);

                SqlCommand cmd = new SqlCommand("prAgregarProducto", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Clear();
                cmd.Parameters.Add("@JsonProducto", SqlDbType.VarChar, 8000).Value = json;
                var dataReader = cmd.ExecuteReader();

                Console.WriteLine(dataReader);

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
                dataReader.Close();
            }
            connection.Close(); // Close the connection when you're done
            return listaProducto;
        }

        // PUT api/<Producto>/5
        [HttpPut("{id}")]
        public IEnumerable<IProducto> Put(int id, [FromBody] string value)
        {
            List<IProducto> listaProducto = new List<IProducto>();

            List<dynamic> arregloJson = JsonConvert.DeserializeObject<List<dynamic>>(value);

            IProducto productoEncontrado = Get(id).FirstOrDefault();

            foreach (JObject data in arregloJson)
            {

                if (productoEncontrado != null)
                {
                    connection.Open();
                    // Recorre las propiedades del objeto JSON y actualiza los valores nulos o vacíos
                    foreach (var propiedad in typeof(ProductoModel).GetProperties())
                    {
                        var valorJson = ((JObject)data)[propiedad.Name];
                        var valorActual = propiedad.GetValue(productoEncontrado);

                        // Si el valor en el JSON no es nulo o vacío, actualiza el valor actual
                        if (valorJson != null && !string.IsNullOrEmpty(valorJson.ToString()))
                        {
                            propiedad.SetValue(productoEncontrado, Convert.ChangeType(valorJson, propiedad.PropertyType));
                        }
                    }

                    Console.WriteLine(productoEncontrado);

                    SqlCommand cmd = new SqlCommand("prActualizarProducto", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new SqlParameter("@IdProducto", SqlDbType.Int)).Value = productoEncontrado.idProducto;
                    cmd.Parameters.Add(new SqlParameter("@Descripcion", SqlDbType.VarChar, 8000)).Value = productoEncontrado.descripcion.ToString();
                    cmd.Parameters.Add(new SqlParameter("@Cantidad", SqlDbType.Int)).Value = productoEncontrado.cantidad;
                    cmd.Parameters.Add(new SqlParameter("@Precio", SqlDbType.VarChar, 8000)).Value = productoEncontrado.precio.ToString();
                    cmd.Parameters.Add(new SqlParameter("@EstadoActivo", SqlDbType.Bit)).Value = Convert.ToBoolean(productoEncontrado.estadoActivo);
                    var dataReader = cmd.ExecuteReader();

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
                    dataReader.Close();
                    connection.Close();
                }
            }
            return listaProducto;
        }

        // DELETE api/<Producto>/5
        [HttpDelete("{id}")]
        public object Delete(int id)
        {
            bool resultado = false;

            connection.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "UPDATE [dbo].[Producto] SET EstadoActivo = @EstadoActivo, fechaModificacion = GETDATE() WHERE IdProducto = @IdProducto AND EstadoActivo = 1";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@EstadoActivo", 0);
            cmd.Parameters.AddWithValue("@IdProducto", id);
            var rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                resultado = true;
            }

            connection.Close();

            List<object> responseList = new List<object>
            {
                new { response = resultado, message = resultado ? $"Se ha eliminado con éxito el producto {id}" : $"Producto ya fue eliminado{id}" }
            };

            return responseList.FirstOrDefault();
        }
    }
}
