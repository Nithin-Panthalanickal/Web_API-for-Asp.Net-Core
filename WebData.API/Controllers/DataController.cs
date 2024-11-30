using WebData.API.Models;
using Microsoft.AspNetCore.Mvc;
using WebData.API.Models.Dto;
using WebData.API.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using MySql.Data.MySqlClient;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;


namespace WebData.API.Controllers
{
    [Route("api/Data")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public DataController(IConfiguration configuration)
        {

            _context = new DatabaseContext(configuration.GetConnectionString("DefaultConnection"));
        }
        [HttpGet]
        public IActionResult GetData()
        {
            var dataList = new List<DataDto>();
            using (var connection = _context.GetConnection())
            {
                connection.Open();
                var command = new MySqlCommand("SELECT *FROM Product", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dataList.Add(new DataDto {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Category = reader.GetString("category"),
                            Price = reader.GetInt32("price")

                        });
                    }
                }
            }
            return Ok(dataList);
        }

        [HttpGet("{id}")]
        public IActionResult GetDataById(int id)
        {
            DataDto data = null;
            using (var connection = _context.GetConnection())
            {
                connection.Open();
                var command = new MySqlCommand("SELECT *FROM Product WHERE id=@id", connection);
                command.Parameters.AddWithValue("@id", id);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data = new DataDto
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name"),
                            Category = reader.GetString("category"),
                            Price = reader.GetInt32("price")

                        };
                    }
                }
                if (data == null)
                
                    return NotFound();
                    return Ok(data);
                }
            }
        
        [HttpPost]
        public IActionResult CreateData([FromBody] DataDto DataDto)
        {
            using (var connection = _context.GetConnection())
            {
                connection.Open();
                
                var command = new MySqlCommand("INSERT INTO product(name,category,price) VALUES (@name,@category,@price)", connection);
               
                command.Parameters.AddWithValue("@name", DataDto.Name);
                command.Parameters.AddWithValue("@category", DataDto.Category);
                command.Parameters.AddWithValue("@price", DataDto.Price);
                command.ExecuteNonQuery();
            }
            
            return CreatedAtAction(nameof(GetDataById), new { Id = DataDto.Id }, DataDto);
        }
        

        
        [HttpPut("{id}")]
         public IActionResult UpdateData(int id, [FromBody] DataDto DataDto)
        {
            using (var connection = _context.GetConnection())
            {
                connection.Open();
                var command = new MySqlCommand("UPDATE product SET name=@name,category=@category,price=@price WHERE id=@id", connection);
                command.Parameters.AddWithValue("@id", DataDto.Id);
                command.Parameters.AddWithValue("@name", DataDto.Name);
                command.Parameters.AddWithValue("@category", DataDto.Category);
                command.Parameters.AddWithValue("@price", DataDto.Price);
                command.ExecuteNonQuery();
            }
            return NoContent();
            
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRecord(int id) 
        {
            using (var connection = _context.GetConnection())
            {
                connection.Open();
                var command = new MySqlCommand("DELETE FROM Product WHERE id = @id", connection);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();

            }
            return NoContent();
        }


    }
}
