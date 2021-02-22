using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SystemGroup.Data.Repository.IRepository;
using SystemGroup.Models;
using SystemGroup.Models.Requests;

namespace SystemGroup.MicroblogService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BlogController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [Route("all")]
        [HttpGet]
        public IActionResult All()
        {

            return Ok(_unitOfWork.Posts.GetAll());
        }

        [Route("publish")]
        [HttpPost]
        public IActionResult Post([FromBody] PostRequest postRequest)
        {
            var userId = User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var user = _unitOfWork.Users.GetFirstOrDefault(u => u.Id == userId);

            var post = new Post()
            {
                Title = postRequest.Title,
                Text = postRequest.Text,
                CreatedAt = postRequest.CreatedAt,
                IdentityUserId = user.Id,
                IdentityUser = user
            };


            _unitOfWork.Posts.Add(post);
            _unitOfWork.Save();
            return Ok("Success");
        }
    }
}
