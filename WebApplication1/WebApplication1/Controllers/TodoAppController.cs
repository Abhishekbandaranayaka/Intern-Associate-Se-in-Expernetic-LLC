using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TodoAppController : ControllerBase
	{
		private IConfiguration _configuration;

		public TodoAppController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[HttpGet]
		[Route("GetNotes")]
		public JsonResult GetNotes()
		{
			string query = "SELECT * FROM dbo.Notes";
			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("todoAppDBCon");

			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					SqlDataReader myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close(); // Fix the typo here
					myCon.Close();
				}
			}
			return new JsonResult(table);
		}


		[HttpPost]
		[Route("AddNotes")]
		public JsonResult AddNotes([FromForm] String newNotes)
		{
			string query = "Insert into dbo.Notes values (@newNotes)";
			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("todoAppDBCon");

			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@newNotes", newNotes);
					SqlDataReader myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close(); // Fix the typo here
					myCon.Close();
				}
			}
			return new JsonResult("Added Successfully");
		}

		[HttpDelete]
		[Route("DeleteNotes")]
		public JsonResult DeleteNotes(int id)
		{
			string query = "delete from dbo.Notes where id=@id";
			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("todoAppDBCon");

			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@id", id);
					SqlDataReader myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close(); // Fix the typo here
					myCon.Close();
				}
			}
			return new JsonResult("Deleted Successfully");
		}
		[HttpPut]
		[Route("UpdatesNotes")]
		public JsonResult UpdatesNotes(int id, [FromForm] string updatedNotes)
		{
			string query = "UPDATE dbo.Notes SET description = @updatedNotes WHERE id = @id";
			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("todoAppDBCon");

			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@id", id);
					myCommand.Parameters.AddWithValue("@updatedNotes", updatedNotes);
					SqlDataReader myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close(); // Fix the typo here
					myCon.Close();
				}
			}
			return new JsonResult("Updated Successfully");
		}

	}
}
