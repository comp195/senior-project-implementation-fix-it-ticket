using Microsoft.EntityFrameworkCore;

namespace FixitTicket.Models
{
    public class TicketContext : DbContext
    {
        public TicketContext(DbContextOptions<TicketContext> options) : base(options) 
        {
        }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Resident> Residents { get; set; }
    }
}
