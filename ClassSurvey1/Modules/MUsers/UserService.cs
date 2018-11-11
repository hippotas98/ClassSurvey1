
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ClassSurvey1.Models;

namespace ClassSurvey1.Modules
{
    public interface IUserService : ITransientService, ICommonService
    {
        long Count(SearchUserEntity SearchUserEntity);
        List<UserEntity> Get(SearchUserEntity SearchUserEntity);
        UserEntity Get(Guid UserId);
        bool ChangePassword(Guid UserId, PasswordChangeEntity passwordEntity);
        UserEntity Create(UserEntity UserEntity);
        //UserEntity Update(Guid UserId, UserEntity UserEntity);
        bool Delete(Guid UserId);
        string Login(UserEntity UserEntity);
    }
    public class UserService : CommonService, IUserService
    {
        private IJWTHandler JWTHandler;
        private SecurePasswordHasher SecurePasswordHasher;
        public UserService(IJWTHandler JWTHandler) : base()
        {
            this.JWTHandler = JWTHandler;
            this.SecurePasswordHasher = new SecurePasswordHasher();
        }
        public long Count(SearchUserEntity SearchUserEntity)
        {
            if (SearchUserEntity == null) SearchUserEntity = new SearchUserEntity();
            IQueryable<User> Users = context.Users;
            Apply(Users, SearchUserEntity);
            return Users.Count();
        }
        public List<UserEntity> Get(SearchUserEntity SearchUserEntity)
        {
            if (SearchUserEntity == null) SearchUserEntity = new SearchUserEntity();
            IQueryable<User> Users = context.Users
                .Include(u => u.Admin)
                .Include(u => u.Student)
                .Include(u => u.Lecturer);
                
            Apply(Users, SearchUserEntity);
            Users = SearchUserEntity.SkipAndTake(Users);
            return Users.ToList().Select(u => new UserEntity(u)).ToList();
        }


        public UserEntity Get(Guid UserId)
        {
            User User =  context.Users
                .Include(u => u.Admin)
                .Include(u => u.Student)
                .Include(u => u.Lecturer)
                .Where(u => u.Id == UserId).FirstOrDefault();
            if (User == null)
                throw new BadRequestException("User không tồn tại");
            return new UserEntity(User);
        }

        public UserEntity Create(UserEntity UserEntity)
        {
            if (string.IsNullOrEmpty(UserEntity.Username))
                throw new BadRequestException("Bạn chưa điền Username");
            if (string.IsNullOrEmpty(UserEntity.Password))
                throw new BadRequestException("Bạn chưa điền Password");
            User User = context.Users.Where(u => u.Name.ToLower().Equals(UserEntity.Username.ToLower())).FirstOrDefault();
            if (User == null)
            {
                User = new User();
                User.Id = Guid.NewGuid();
                User.Name = UserEntity.Username;
                context.Users.Add(User);
            }
            User.Password = SecurePasswordHasher.Hash(UserEntity.Password);
            context.SaveChanges();
            UserEntity.Id = User.Id;
            UserEntity.Password = User.Password;
            return UserEntity;

        }
        public bool ChangePassword(Guid userId, PasswordChangeEntity passwordEntity)
        {
            
            User User = context.Users.FirstOrDefault(u => u.Id.Equals(userId));
            if (User == null) return false;
            if (SecurePasswordHasher.Verify(passwordEntity.OldPassword, User.Password))
            {
                User newPasswordUser = new User(passwordEntity.UserEntity);
                User.Password = SecurePasswordHasher.Hash(passwordEntity.UserEntity.Password);
                context.SaveChanges();
                return true;
            }
            return false;
        }
//        public UserEntity Update(Guid UserId, UserEntity UserEntity)
//        {
//            User User = context.Users.Where(u => u.Id.Equals(UserEntity.Id)).FirstOrDefault();
//            if (User == null)
//                throw new BadRequestException("User không tồn tại.");
//            UserEntity.ToModel(User);
//            User.Password = GetHashString(UserEntity.Password);
//            IMSContext.SaveChanges();
//            return new UserEntity(User);
//        }
        public bool Delete(Guid UserId)
        {
            User User = context.Users.Where(u => u.Id == UserId).FirstOrDefault();
            if (User == null)
                throw new BadRequestException("User không tồn tại.");
            context.Users.Remove(User);
            context.SaveChanges();
            return true;
        }

        public string Login(UserEntity UserEntity)
        {
            if (string.IsNullOrEmpty(UserEntity.Username))
                throw new BadRequestException("Bạn chưa điền Username");
            if (string.IsNullOrEmpty(UserEntity.Password))
                throw new BadRequestException("Bạn chưa điền Password");

            //User User = IMSContext.Users
            //   .Include(u => u.Admin)
            //   .Include(u => u.Student)
            //   .Include(u => u.Lecturer)
            //   .Include(u => u.HrEmployee)
            //   .Where(u => u.Username.ToLower().Equals(UserEntity.Username.ToLower())).FirstOrDefault();

            //if (User == null)
            //    throw new BadRequestException("User không tồn tại.");
            string hashed1234 = SecurePasswordHasher.Hash("1234"); //hashed password 
            //string hashPassword = (UserEntity.Password);
            if (!SecurePasswordHasher.Verify(UserEntity.Password, hashed1234))
                throw new BadRequestException("Bạn nhập sai password.");
            UserEntity = new UserEntity();
            UserEntity.Password = "1234";
            UserEntity.Username = "ABC";
            UserEntity.Roles = new List<string>() { "1", "4" };
            return JWTHandler.CreateToken(UserEntity);
        }

        private string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            HashAlgorithm algorithm = SHA256.Create();
            byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            foreach (byte b in hash)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        private void Apply(IEnumerable<User> source, SearchUserEntity SearchUserEntity)
        {
            if (SearchUserEntity.Username != null)
            {
                source = source.Where(u =>
                    u.Name.Contains(SearchUserEntity.Username) || SearchUserEntity.Username.Contains(u.Name));
            }
        }
    }
}
