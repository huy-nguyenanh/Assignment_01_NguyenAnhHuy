using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.DAOs
{
    public class MemberDAO
    {
        private static MemberDAO instance = null;
        private static readonly object instanceLock = new object();
        private MemberDAO() { }

        public static MemberDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MemberDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<Member> GetMemberList()
        {
            List<Member> members;
            try
            {
                var myStoreDB = new FStoreDBContext();
                members = myStoreDB.Members.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return members;
        }

        public IEnumerable<Member> SearchMembers(string search)
        {
            List<Member> members;
            try
            {
                var memberDB = new FStoreDBContext();
                members = memberDB.Members.Where(members => members.Email.Contains(search)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return members;
        }

        public Member GetMember(string email)
        {
            Member member = null;
            try
            {
                var myStoreDB = new FStoreDBContext();
                member = myStoreDB.Members.SingleOrDefault(member => member.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return member;
        }
        public Member GetMemberById(int id)
        {
            Member member = null;
            try
            {
                var myStoreDB = new FStoreDBContext();
                member = myStoreDB.Members.SingleOrDefault(member => member.MemberId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return member;
        }

        public Member CheckLogin(string email, string password)
        {
            Member member = null;
            try
            {
                var myStoreDB = new FStoreDBContext();
                member = myStoreDB.Members.SingleOrDefault(member => member.Email == email && member.Password == password);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return member;
        }

        public void AddMember(Member member)
        {
            try
            {
                Member c = GetMember(member.Email);
                if (c == null)
                {
                    var myStoreDB = new FStoreDBContext();
                    myStoreDB.Members.Add(member);
                    myStoreDB.SaveChanges();
                }
                else
                {
                    throw new Exception("The member has already existed!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateMember(Member member)
        {
            try
            {
                Member c = GetMemberById(member.MemberId);
                if (c != null)
                {
                    var myStoreDB = new FStoreDBContext();
                    myStoreDB.Entry<Member>(member).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    myStoreDB.SaveChanges();
                }
                else
                {
                    throw new Exception("The member has not existed!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void RemoveMember(Member member)
        {
            try
            {
                Member c = GetMember(member.Email);
                if (c != null)
                {
                    var myStoreDB = new FStoreDBContext();
                    myStoreDB.Remove(c);
                    myStoreDB.SaveChanges();
                }
                else
                {
                    throw new Exception("The member has not existed!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
