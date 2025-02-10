using foroLIS_backend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace foroLIS_backend.Infrastructure.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : IdentityDbContext<Users>(options)
       
    {
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<FieldSurvey> Fields { get; set; }
        public DbSet<UserFieldSurvey> UsersFields { get; set; }
        public DbSet<UserHistorial> UserHistorials { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Mention> Mentions { get; set; }
        public DbSet<MediaFile> MediaFiles { get; set; }
        public DbSet<FilePost> FilePosts { get; set; }

        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            builder.Entity<UserFieldSurvey>()
            .HasKey(ufs => new { ufs.UserId, ufs.FieldSurveyId });

            builder.Entity<Survey>()
                .HasIndex(s => s.PostId)
                .IsUnique();

            builder.Entity<UserFieldSurvey>()
                .HasOne(ufs => ufs.Users)
                .WithMany()  
                .HasForeignKey(ufs => ufs.UserId);

            builder.Entity<UserFieldSurvey>()
                .HasOne(ufs => ufs.FieldSurvey)
                .WithMany()  
                .HasForeignKey(ufs => ufs.FieldSurveyId);

            builder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull); 

            builder.Entity<Comment>()
                .HasOne(c => c.Users)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Survey>()
                .HasOne(s => s.Post)
                .WithMany(p => p.Surveries)
                .HasForeignKey(s => s.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<UserHistorial>()
                .HasOne(uh => uh.Users)
                .WithMany(u => u.UserHistorials)
                .HasForeignKey(uh => uh.UserId)
                .OnDelete(DeleteBehavior.NoAction); 
                
            builder.Entity<UserHistorial>()
                .HasOne(uh => uh.Post)
                .WithMany(p => p.UserHistorials)
                .HasForeignKey(uh => uh.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Reaction>()
                .HasKey(r => new { r.ReactionType, r.PostId, r.CommentId,r.UserId });

            builder.Entity<Reaction>()
                .HasOne(r => r.Post)
                .WithMany(p => p.Reactions)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.Entity<Reaction>()
              .HasOne(r => r.Comment)
              .WithMany(c => c.Reactions)
              .HasForeignKey(r => r.CommentId)
              .OnDelete(DeleteBehavior.ClientSetNull);
            builder.Entity<Reaction>()
                .HasOne(r => r.Users)
                .WithMany(u => u.Reactions)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);


            base.OnModelCreating(builder);
        }
    }
}
