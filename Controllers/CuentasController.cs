using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiPelis2023.Models;

namespace WebApiPelis2023.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentasController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;  //poder dar de alta user 
        private readonly IConfiguration _config;                    //usar configs de program
        private readonly SignInManager<IdentityUser> _signInManager;    //login de user

        public CuentasController(UserManager<IdentityUser> userManager, IConfiguration config, 
                                    SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _config = config;
            _signInManager = signInManager;
        }

        private async Task <RespuestaAutenticacion> ConstruirToken(CredencialesUsuario credencialesUsuario)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", credencialesUsuario.Email)
            };

            var usuario = await _userManager.FindByEmailAsync(credencialesUsuario.Email); //obtener user de la bd
            var claimsRoles = await _userManager.GetClaimsAsync(usuario!); //obtener claims del usuario
            //claims es para guardar cosas no sensibles, como si es admin

            //fusionar 
            claims.AddRange(claimsRoles);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["LlaveJWT"]!));
            var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddDays(1);

            //construir token
            var securityToken = new JwtSecurityToken(issuer:null, audience:null, claims: claims, expires: expiracion,
                                                       signingCredentials: credenciales);

            return new RespuestaAutenticacion{
                Token=new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion=expiracion
            };
        }

        [HttpPost("Registrar")]
        public async Task <ActionResult<RespuestaAutenticacion>> Registrar(CredencialesUsuario credencialesUsuario)
        {
            var usuario = new IdentityUser
            {
                UserName = credencialesUsuario.Email, //puede ser diferente al de abajo
                Email=credencialesUsuario.Email
            };
            var resultado = await _userManager.CreateAsync(usuario, credencialesUsuario.Password);
            if (resultado.Succeeded) return await ConstruirToken(credencialesUsuario);
            return BadRequest(resultado.Errors);
        }
    }
}
