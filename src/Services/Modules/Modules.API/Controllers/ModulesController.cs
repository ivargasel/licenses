using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Modules.API.Model;
using Modules.API.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Modules.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ModulesController(ModuleContext db, IConfiguration config) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var data = await db.Modules.ToListAsync();
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

                var data = await db.Modules.SingleOrDefaultAsync(x => x.ModuleGuid == id);
                data.Childs = await db.Modules.OrderBy(x => x.Name).Where(x => x.ParentModuleId == id).ToListAsync();
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

                var data = await db.Modules.Where(x => x.Name.Contains(name)).ToListAsync();
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
                var data = await db.Modules.Select(x => new SelectListItem(x.Name, x.ModuleGuid.ToString())).ToListAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync(ModuleModel model)
        {
            try
            {
                var data = new ModuleModel
                {
                    ModuleGuid = Guid.NewGuid(),
                    Cod = model.Cod,
                    Order = model.Order,
                    Name = model.Name,
                    Description = model.Description,
                    Url = model.Url,
                    IsParent = model.IsParent,
                    ParentModuleId = model.ParentModuleId,                    
                    Inactive = model.Inactive,
                    CreatedUserId = model.CreatedUserId,
                    CreatedDate = DateTime.Now
                };

                db.Modules.Add(data);
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
        public async Task<IActionResult> OnPutAsync(Guid id, ModuleModel model)
        {
            try
            {
                var exists = await db.Modules.AnyAsync(x => x.ModuleGuid == id);
                if (!exists)
                    return NotFound();

                var data = new ModuleModel
                {
                    ModuleId = model.ModuleId,
                    ModuleGuid = id,
                    Cod = model.Cod,
                    Order = model.Order,
                    Name = model.Name,
                    Description = model.Description,
                    Url = model.Url,
                    IsParent = model.IsParent,
                    ParentModuleId = model.ParentModuleId,
                    Inactive = model.Inactive,
                    CreatedUserId = model.CreatedUserId,
                    CreatedDate = model.CreatedDate,
                    UpdatedUserId = model.UpdatedUserId,
                    UpdatedDate = DateTime.Now
                };

                db.Modules.Update(data);
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
                var exists = await db.Modules.AnyAsync(x => x.ModuleGuid == id);
                if (!exists)
                    return NotFound();

                var data = await db.Modules.SingleOrDefaultAsync(x => x.ModuleGuid == id);
                db.Modules.Remove(data);
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
