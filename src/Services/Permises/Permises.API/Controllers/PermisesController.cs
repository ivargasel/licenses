using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Permises.API.Model;
using Permises.API.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Permises.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class PermisesController (PermiseContext db, IConfiguration config) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var data = await db.Permises.ToListAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest();

                var data = await db.Permises.SingleOrDefaultAsync(x => x.PermiseGuid == id);
                if (data == null)
                    return NotFound(false);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> OnGetAsync(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    return BadRequest();

                var data = await db.Permises.Where(x => x.Name.Contains(name)).ToListAsync();
                if (data == null)
                    return NotFound();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpGet("select")]
        public async Task<IActionResult> OnSelectAsync()
        {
            try
            {
                var data = await db.Permises.Select(x => new SelectListItem(x.Name, x.PermiseGuid.ToString())).ToListAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync(PermiseModel model)
        {
            try
            {
                var data = new PermiseModel
                {
                    PermiseGuid = Guid.NewGuid(),
                    Name = model.Name,
                    Description = model.Description,
                    Inactive = model.Inactive,
                    CreatedUserId = model.CreatedUserId,
                    CreatedDate = DateTime.Now
                };

                db.Permises.Add(data);
                var response = await db.SaveChangesAsync();

                if (response == 0)
                    return BadRequest();

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> OnPutAsync(Guid id, PermiseModel model)
        {
            try
            {
                var exists = await db.Permises.AnyAsync(x => x.PermiseGuid == id);
                if (!exists)
                    return NotFound();

                var data = new PermiseModel
                {
                    PermiseId = model.PermiseId,
                    PermiseGuid = id,
                    Name = model.Name,
                    Description = model.Description,
                    Inactive = model.Inactive,
                    CreatedUserId = model.CreatedUserId,
                    CreatedDate = model.CreatedDate,
                    UpdatedUserId = model.UpdatedUserId,
                    UpdatedDate = DateTime.Now
                };

                db.Permises.Update(data);
                var response = await db.SaveChangesAsync();

                if (response == 0)
                    return BadRequest();

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> OnDeleteAsync(Guid id)
        {
            try
            {
                var exists = await db.Permises.AnyAsync(x => x.PermiseGuid == id);
                if (!exists)
                    return NotFound();

                var data = await db.Permises.SingleOrDefaultAsync(x => x.PermiseGuid == id);
                db.Permises.Remove(data);
                var response = await db.SaveChangesAsync();

                if (response == 0)
                    return BadRequest(false);

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public string LogIn(string email)
        {
            try
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sid, email),
                    new Claim(JwtRegisteredClaimNames.Email, email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: null,
                    audience: null,
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }            
        }
    }
}
