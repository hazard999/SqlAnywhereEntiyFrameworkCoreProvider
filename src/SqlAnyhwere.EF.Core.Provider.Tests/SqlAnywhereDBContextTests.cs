using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Provider.Tests
{
    public class SqlAnywhereDBContextTests
    {
        [Theory]
        public void NewSqlAnyhwereDBContectShouldNotThrowOnNew()
        {
            var context = new TestDBContext();
        }

        [Theory]
        public void AddNewBlogPostShouldAffect1Record()
        {
            using (var db = new TestDBContext())
            {
                var cmd = db.GetDBCommand();

                cmd.CommandText = "DELETE FROM BLOG";
                cmd.ExecuteNonQuery();

                var blog = new Blog { BlogId = 1, Url = "http://blogs.msdn.com/adonet" };
                db.Blogs.Add(blog);

                var count = db.SaveChanges();
                Assert.Equal(1, count);
            }
        }

        [Theory]
        public async Task AddNewBlogPostAsyncShouldAffect1Record()
        {
            using (var db = new TestDBContext())
            {
                var blog = new Blog { BlogId = 1000, Url = "http://blogs.msdn.com/adonet" };
                db.Blogs.Add(blog);

                var count = await db.SaveChangesAsync();

                Assert.Equal(1, count);
            }
        }

        [Theory]
        public void AddNewBlogPostAutoincrementingShouldNotThrow()
        {
            using (var db = new TestDBContext())
            {
                var blog = new Blog { Url = "http://blogs.msdn.com/adonet" };
                db.Blogs.Add(blog);
                db.SaveChanges();
            }
        }

        [Theory]
        public void BlogsShouldContainOne()
        {


            using (var db = new TestDBContext())
            {
                Assert.Equal(1, db.Blogs.ToList().Count);

            }
        }
    }
}
