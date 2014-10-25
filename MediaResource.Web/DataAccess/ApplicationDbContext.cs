using System.Data.Entity;

using MediaResource.Web.Models;

namespace MediaResource.Web.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("name=MediaResourceConnection")
        {
        }

        public virtual DbSet<ApplyForDownload> ApplyForDownloads
        {
            get;
            set;
        }

        public virtual DbSet<Category> Categorys
        {
            get;
            set;
        }

        public virtual DbSet<Film> Films
        {
            get;
            set;
        }

        public virtual DbSet<Graphic> Graphics
        {
            get;
            set;
        }

        public virtual DbSet<Group> Groups
        {
            get;
            set;
        }

        public virtual DbSet<Log> Logs
        {
            get;
            set;
        }

        public virtual DbSet<Music> Musics
        {
            get;
            set;
        }

        public virtual DbSet<News> Newss
        {
            get;
            set;
        }

        public virtual DbSet<Photo> Photos
        {
            get;
            set;
        }

        public virtual DbSet<Role> Roles
        {
            get;
            set;
        }

        public virtual DbSet<TaskList> TaskLists
        {
            get;
            set;
        }

        public virtual DbSet<TaskListNo> TaskListNos
        {
            get;
            set;
        }

        public virtual DbSet<User> Users
        {
            get;
            set;
        }

        public virtual DbSet<UserAssignment> UserAssignments
        {
            get;
            set;
        }

        public virtual DbSet<UserDownloadFile> UserDownloadFiles
        {
            get;
            set;
        }

        public virtual DbSet<UserPhoto> UserPhotos
        {
            get;
            set;
        }

        public virtual DbSet<UserVideo> UserVideos
        {
            get;
            set;
        }

        public virtual DbSet<Video> Videos
        {
            get;
            set;
        }

        public virtual DbSet<Comment> Comments
        {
            get;
            set;
        }

        public virtual DbSet<Score> Scores
        {
            get;
            set;
        }

        public virtual DbSet<UserFolder> UserFolders
        {
            get;
            set;
        }

        public virtual DbSet<GraphicDesign> GraphicDesigns
        {
            get;
            set;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskList>()
                .Property(e => e.TaskListType)
                .IsUnicode(false);

            modelBuilder.Entity<TaskList>()
                .Property(e => e.Category)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}