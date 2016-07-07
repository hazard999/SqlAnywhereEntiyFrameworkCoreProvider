using System.Linq;
using System.Threading.Tasks;

namespace Provider.Tests
{
    public class SqlAnywhereDBContextTests
    {
        public void NewSqlAnyhwereDBContectShouldNotThrowOnNew()
        {
            var context = new TestDBContext();
        }

        public int AddNewBlogPostShouldAffect1Record()
        {
            using (var db = new TestDBContext())
            {
                var cmd = db.GetDBCommand();

                cmd.CommandText = "DELETE FROM BLOG";
                cmd.ExecuteNonQuery();

                var blog = new Blog { BlogId = 1, Url = "http://blogs.msdn.com/adonet" };
                db.Blogs.Add(blog);

                return db.SaveChanges();
            }
        }

        public async Task AddNewBlogPostAsyncShouldAffect1Record()
        {
            using (var db = new TestDBContext())
            {
                var blog = new Blog { BlogId = 1000, Url = "http://blogs.msdn.com/adonet" };
                db.Blogs.Add(blog);

                var count = await db.SaveChangesAsync();
            }
        }

        public void AddNewBlogPostAutoincrementingShouldNotThrow()
        {
            using (var db = new TestDBContext())
            {
                var blog = new Blog { Url = "http://blogs.msdn.com/adonet" };
                db.Blogs.Add(blog);
                db.SaveChanges();
            }
        }

        public int BlogsShouldContainOne()
        {
            using (var db = new TestDBContext())
            {
                return db.Blogs.ToList().Count;
            }
        }
    }
}
