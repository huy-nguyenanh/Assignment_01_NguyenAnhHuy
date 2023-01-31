using AutoMapper;
using BussinessLayer.Repos;
using DataLayer.Models;
using DataLayer.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace eStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberRepository _memberRepo;
        private readonly IMapper _mapper;

        public MemberController(IMemberRepository memberRepo, IMapper mapper)
        {
            _memberRepo = memberRepo;
            _mapper = mapper;
        }

        [HttpGet("list")]
        public IActionResult Gets()
        {
            var members = _memberRepo.GetMembers();
            if (members == null || members.Count() == 0)
            {
                return NotFound();
            }
            return Ok(members);
        }
        [HttpGet("{email}")]
        public IActionResult Get(string email)
        {
            var member = _memberRepo.GetMember(email);
            if (member == null)
            {
                return NotFound();
            }
            return Ok(member);
        }
        [HttpPost("create")]
        public IActionResult Post([FromBody] MemberViewModel member)
        {
            if (string.IsNullOrEmpty(member.Email) || string.IsNullOrEmpty(member.Password))
            {
                return BadRequest("Must be fill email and password");
            }
            var createMember = new Member();
            createMember.Email = member.Email;
            createMember.Password = member.Password;
            createMember.CompanyName = member.CompanyName;
            createMember.City = member.City;
            createMember.Country = member.Country;
            _memberRepo.AddMember(createMember);
            return Created("", member);
        }
        [HttpPut("update")]
        public IActionResult Put([FromBody] MemberViewModel member)
        {
            var existedMember = new Member();
            if (member.MemberId != 0)
            {
                existedMember = _memberRepo.GetMemberById(member.MemberId);
            }
            if (existedMember == null)
            {
                return NotFound("Member doesn't exist");
            }
            else
            {
                _mapper.Map<MemberViewModel, Member>(member, existedMember);
                _memberRepo.UpdateMember(existedMember);

                return Ok(member);
            }
        }
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var deleteMember = _memberRepo.GetMemberById(id);
            if (deleteMember == null)
            {
                return NotFound();
            }
            _memberRepo.RemoveMember(deleteMember);
            return Ok();
        }
    }
}
