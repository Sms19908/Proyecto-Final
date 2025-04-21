using Microsoft.EntityFrameworkCore;
using SC_701_PAW_Lunes.Data;

namespace SC_701_PAW_Lunes.Services
{
    public class SessionInvalidationService
    {
        private readonly PAWDbContext _context;
        private readonly IConfiguration _configuration;

        public SessionInvalidationService(PAWDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task InvalidateAllSessionsAsync()
        {
            var users = await _context.Users.ToListAsync();
            foreach (var user in users)
            {
                user.SecurityStamp = Guid.NewGuid().ToString();
            }
            await _context.SaveChangesAsync();
        }
    }
}
