using Gateway.API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.API.Services
{
    [Route("[controller]")]
    [ApiController]
    public class ModulesController (IConfiguration config, HttpClient client) : ControllerBase
    {
        private new readonly string Url = String.Format(config["APIs:Modules"]);

        private async Task<string> Token(string email)
        {
            if (email == null)
                return "No email added. Error generate token";
            
            string url = String.Format($"{Url}/login?email={email}");
            HttpResponseMessage request = await client.PostAsync(url, null);            
            if (request.IsSuccessStatusCode)
                return await request.Content.ReadAsStringAsync();

            return null;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromHeader]string Email)
        {
            if (Email == null)
                return BadRequest("No Email Added");

            var token = await Token(Email);
            if (token == null)
                return NotFound("Token Invalid");

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            HttpResponseMessage response = await client.GetAsync(Url);
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync();
                return Content(content.Result, "application/json");
            }

            throw new Exception(response.ReasonPhrase);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Index(Guid id, [FromHeader] string Email)
        {
            var token = await Token(Email);
            if (token == null)
                return NotFound("Token Invalid");

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            HttpResponseMessage response = await client.GetAsync($"{Url}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync();
                return Content(content.Result, "application/json");
            }

            throw new Exception(response.ReasonPhrase);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Index(string name, [FromHeader] string Email)
        {
            var token = await Token(Email);
            if (token == null)
                return NotFound("Token Invalid");

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            HttpResponseMessage response = await client.GetAsync($"{Url}/{name}");
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync();
                return Content(content.Result, "application/json");
            }

            throw new Exception(response.ReasonPhrase);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ModuleDTO model, [FromHeader] string Email)
        {
            var token = await Token(Email);
            if (token == null)
                return NotFound("Token Invalid");

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            HttpResponseMessage response = await client.PostAsJsonAsync<ModuleDTO>($"{Url}", model);
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync();
                return Content(result.Result, "application/json");
            }

            throw new Exception(response.ReasonPhrase);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] ModuleDTO model, [FromHeader] string Email)
        {
            var token = await Token(Email);
            if (token == null)
                return NotFound("Token Invalid");

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            HttpResponseMessage response = await client.PutAsJsonAsync<ModuleDTO>($"{Url}/{id}", model);
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync();
                return Content(result.Result, "application/json");
            }

            throw new Exception(response.ReasonPhrase);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, [FromHeader] string Email)
        {
            var token = await Token(Email);
            if (token == null)
                return NotFound("Token Invalid");

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            HttpResponseMessage response = await client.DeleteAsync($"{Url}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync();
                return Content(result.Result, "application/json");
            }

            throw new Exception(response.ReasonPhrase);
        }
    }
}