using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ORM;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly UsersContext _context;

        public UsersController(UsersContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public IEnumerable<User> GetUser()
        {
            return _context.User;
        }

        // GET: api/Users/$Login={login}&$Password={password}
        [HttpGet("$Login={login}&$Password={password}")]
        public async Task<IActionResult> GetUser([FromRoute] string login, [FromRoute] string password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.User.Where(u => u.Login == login).Where(u => u.Password == password).SingleOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // GET: api/Users/$count
        [HttpGet("$count")]
        public long GetUsersAmount()
        {
            return _context.User.Count();
        }

        // GET: api/Users/$DepartmentId={departmentId}
        [HttpGet("$DepartmentId={departmentId}")]
        public IActionResult GetDepartmentUsers([FromRoute] int departmentId)
        {
            var users = _context.User.Where(u => u.DepartmentId == departmentId).ToList();

            return Ok(users);
        }

        // GET: api/Users/Login={login}/$exists
        [HttpGet("Login={login}/$exists")]
        public IActionResult LoginExists([FromRoute] string login)
        {
            var amount = _context.User.Where(u => u.Login == login).Count();

            return Ok(amount > 0);
        }

        // GET: api/Users/$DepartmentId={departmentId}/$select=FullName
        [HttpGet("$DepartmentId={departmentId}/$select=FullName")]
        public IActionResult GetDepartmentUserNames([FromRoute] int departmentId)
        {
            var userNames = _context.User.Where(u => u.DepartmentId == departmentId).Select(u => u.FullName).ToList();

            return Ok(userNames);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}