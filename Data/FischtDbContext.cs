using Fischt.Models;

using Microsoft.EntityFrameworkCore;

    public class FischtDbContext : DbContext
    {
        public FischtDbContext(DbContextOptions<FischtDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Specie> Species { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Invite> Invites { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<Profile>(p => p.UserId)
                .IsRequired();

            // Profile configuration
            modelBuilder.Entity<Profile>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Profile>()
                .HasIndex(p => p.UserId)
                .IsUnique();

            modelBuilder.Entity<Profile>()
                .HasOne(p => p.Specie)
                .WithMany(s => s.Profiles)
                .HasForeignKey(p => p.SpecieId);

            // Specie configuration
            modelBuilder.Entity<Specie>()
                .HasKey(s => s.Id);

            // Contact configuration (Composite Key)
            modelBuilder.Entity<Contact>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Contact>()
                .HasIndex(c => new { c.UserId, c.ContactId })
                .IsUnique();

            modelBuilder.Entity<Contact>()
                .HasOne(c => c.User)
                .WithMany(u => u.UserContacts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Contact>()
                .HasOne(c => c.ContactUser)
                .WithMany(u => u.ContactOfUsers)
                .HasForeignKey(c => c.ContactId)
                .OnDelete(DeleteBehavior.Restrict);

            // Conversation configuration
            modelBuilder.Entity<Conversation>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Conversation>()
                .HasOne(c => c.Contact)
                .WithMany(co => co.Conversations)
                .HasForeignKey(c => c.ContactId);

            // Invite configuration (Composite Key)
            modelBuilder.Entity<Invite>()
                .HasKey(i => new { i.SenderId, i.ReceiverId });

            modelBuilder.Entity<Invite>()
                .HasOne(i => i.Sender)
                .WithMany(u => u.SentInvites)
                .HasForeignKey(i => i.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invite>()
                .HasOne(i => i.Receiver)
                .WithMany(u => u.ReceivedInvites)
                .HasForeignKey(i => i.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            // Message configuration
            modelBuilder.Entity<Message>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationId);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.SenderId);
        }
    }