using Microsoft.EntityFrameworkCore;

namespace FixitTicket.Models
{
    public class TicketContext : DbContext
    {
        public TicketContext(DbContextOptions<TicketContext> options) : base(options) 
        {
        }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<User> User { get; set; }
    }
}
