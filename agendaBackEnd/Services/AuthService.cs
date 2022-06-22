using agendaBackEnd.Context;
using agendaBackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace agendaBackEnd.Services
{
    public interface IAuthService
    {
        Task SignUp(UsersModel user);
        UsersModel GetUserForSignIn(string email);
    }
    public class AuthService: IAuthService
    {
        readonly DbSet<UsersModel> _dbSet;
        readonly ApplicationDbContext _context;
        public AuthService(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<UsersModel>();
        }

        public async Task SignUp(UsersModel user)
        {
            _dbSet.Add(user);
            await _context.SaveChangesAsync();
        }

        public UsersModel GetUserForSignIn(string email)
        {
            var userResult = _context.UsersModels.Where(x => x.email == email).FirstOrDefault();
            return userResult;
        }

    }
}
