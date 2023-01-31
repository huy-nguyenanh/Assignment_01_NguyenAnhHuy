using BussinessLayer.DAOs;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Repos
{
    public interface IMemberRepository
    {
        IEnumerable<Member> GetMembers();
        IEnumerable<Member> SearchMembers(string search);
        Member GetMember(string email);
        Member CheckLogin(string email, string password);
        void AddMember(Member member);
        void UpdateMember(Member member);
        void RemoveMember(Member member);
        Member GetMemberById(int id);
    }

    public class MemberRepository : IMemberRepository
    {
        public void AddMember(Member member) => MemberDAO.Instance.AddMember(member);

        public Member CheckLogin(string email, string password) => MemberDAO.Instance.CheckLogin(email, password);

        public Member GetMember(string email) => MemberDAO.Instance.GetMember(email);

        public IEnumerable<Member> GetMembers() => MemberDAO.Instance.GetMemberList();

        public IEnumerable<Member> SearchMembers(string search) => MemberDAO.Instance.SearchMembers(search);

        public void RemoveMember(Member member) => MemberDAO.Instance.RemoveMember(member);

        public void UpdateMember(Member member) => MemberDAO.Instance.UpdateMember(member);
        public Member GetMemberById(int id) => MemberDAO.Instance.GetMemberById(id);
    }
}
