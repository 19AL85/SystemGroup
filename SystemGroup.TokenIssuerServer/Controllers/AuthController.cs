using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SystemGroup.Data.Repository.IRepository;
using SystemGroup.Models.Requests;
using SystemGroup.Models.Responses;
using SystemGroup.Models;
using SystemGroup.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace SystemGroup.TokenIssuerServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly JwtTokenValidator _jwtTokenValidator;
        private readonly IUnitOfWork _unitOfWork;


        public AuthController(UserManager<IdentityUser> userManager, JwtTokenGenerator jwtTokenGenerator,
                                JwtTokenValidator jwtTokenValidator, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _jwtTokenValidator = jwtTokenValidator;
            _unitOfWork = unitOfWork;
        }


        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                var accessToken = _jwtTokenGenerator.GenerateAccessToken(user);
                var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

                RefreshToken token = _unitOfWork.RefreshTokens.GetFirstOrDefault(t => t.IdentityUserId == user.Id);

                if (token != null)
                    _unitOfWork.RefreshTokens.Remove(token);

                token = new RefreshToken
                {
                    Token = refreshToken,
                    IdentityUserId = user.Id,
                    IdentityUser = user
                };

                _unitOfWork.RefreshTokens.Add(token);
                _unitOfWork.Save();

                return Ok(new AuthenticatedUserResponse() { AccessToken = accessToken, RefreshToken = refreshToken });
            }
            return Unauthorized();
        }


        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not valid");


            var existsUser = await _userManager.FindByEmailAsync(registerRequest.Email);
            if (existsUser != null)
                return BadRequest("User already exist!");

            var user = new IdentityUser() { UserName = registerRequest.Email, Email = registerRequest.Email };

            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (result.Succeeded)
            {
                return Ok("Success! You can login");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Status = "Error", Message = "Registration failed! Please try again leter" });
            }


        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh([FromBody] RefreshRequest refreshRequest)
        {

            bool isValid = _jwtTokenValidator.IsRefreshTokenValid(refreshRequest.RefreshToken);

            if (!isValid)
                return BadRequest("Invalid refresh token");

            RefreshToken token = _unitOfWork.RefreshTokens.
                GetFirstOrDefault(t => t.Token == refreshRequest.RefreshToken, includeProperties: "IdentityUser");

            if (token == null || token.IdentityUser == null)
                return NotFound("Not found");

            var accessToken = _jwtTokenGenerator.GenerateAccessToken(token.IdentityUser);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            var user = token.IdentityUser;

            _unitOfWork.RefreshTokens.Remove(token);

            token = new RefreshToken
            {
                Token = refreshToken,
                IdentityUserId = user.Id,
                IdentityUser = user
            };

            _unitOfWork.RefreshTokens.Add(token);
            _unitOfWork.Save();

            return Ok(new AuthenticatedUserResponse() { AccessToken = accessToken, RefreshToken = refreshToken });
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [Route("logout")]
        [HttpDelete]
        public IActionResult Logout()
        {
            var userId = User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var token = _unitOfWork.RefreshTokens.GetFirstOrDefault(t => t.IdentityUserId == userId);

            if (token != null)
            {
                _unitOfWork.RefreshTokens.Remove(token);
                _unitOfWork.Save();
            }

            return Ok();
        }

      

    }
}
