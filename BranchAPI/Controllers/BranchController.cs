using BranchAPI.Context;
using BranchAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BranchAPI.Controllers
{
    public class BranchController : Controller
    {
        private DataContext _context;
        private BranchModel _model;
        public BranchController(BranchModel model, DataContext context)
        {
            _context = context;
            _model = model;
        }

        //get all branchdetails

        [HttpGet("GetAllBranchesDetails", Name = "GetAllBranchesDetails")]
        public async Task<IActionResult> GetAllBranchesDetails()
        {
            var cmd = "SELECT * FROM branch";
            var branches = new List<BranchModel>();
            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();
                using var command = new SqlCommand(cmd, connection);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var branch = new BranchModel
                        {
                            brid = reader.GetInt32(reader.GetOrdinal("brid")),
                            brname = reader.GetString(reader.GetOrdinal("brname")),
                            head = reader.GetString(reader.GetOrdinal("head")),
                            noofemployee = reader.GetInt32(reader.GetOrdinal("noofemployee"))
                        };
                        branches.Add(branch);
                    }
                }
            }
            return Ok(branches);
        }

        //get all branch id and name

        [HttpGet("SearchBranches", Name = "SearchBranches")]
        public async Task<IActionResult> SearchBranches()
        {
            var cmd = "SELECT brid, brname FROM branch";
            var branches = new List<BranchModel>();
            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();
                using var command = new SqlCommand(cmd, connection);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var branch = new BranchModel
                        {
                            brid = reader.GetInt32(reader.GetOrdinal("brid")),
                            brname = reader.GetString(reader.GetOrdinal("brname")),
                        };
                        branches.Add(branch);
                    }
                }
            }
            return Ok(branches);
        }

        // search branch by id

        [HttpGet("SearchById/{id}", Name = "SearchById")]
        public async Task<IActionResult> SearchById([FromRoute] int id)
        {
            var cmd = "SELECT * FROM branch WHERE brid=@id";
            var branches = new List<BranchModel>();
            using (var connection = _context.CreateConnection())
            {
                await connection.OpenAsync();
                using var command = new SqlCommand(cmd, connection);
                //add the parameter to the command
                command.Parameters.AddWithValue("@id", id);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var branch = new BranchModel
                        {
                            brid = reader.GetInt32(reader.GetOrdinal("brid")),
                            brname = reader.GetString(reader.GetOrdinal("brname")),
                            head = reader.GetString(reader.GetOrdinal("head")),
                            noofemployee = reader.GetInt32(reader.GetOrdinal("noofemployee"))

                        };
                        branches.Add(branch);
                    }
                }
            }
            return Ok(branches);
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
