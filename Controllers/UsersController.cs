using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventusaBackend.Models.Users;
using EventusaBackend.Models;

namespace EventusaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserContext _context;

        public UsersController(UserContext context)
        {
            _context = context;
        }



        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            return await _context.Users.ToListAsync();
        }


        // GET: api/login
        [HttpPost("login")]
        public async Task<ActionResult<User>> loginUser([FromBody] User user)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }

            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Name == user.Name && u.Pass == user.Pass);

            if (dbUser == null)
            {
                return NotFound();
            }

            if(user.Pass != dbUser.Pass)
            {
                return Unauthorized();
            }

            return Ok(dbUser);
        }

    }
}
