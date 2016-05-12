using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;

namespace Provider.Tests
{
    [TestClass]
    public class SqlAnywhereDBContextTests
    {
        [TestMethod]
        public void NewSqlAnyhwereDBContectShouldNotThrowOnNew()
        {
            Shouldly.Should.NotThrow(() => new TestDBContext());
        }

        [TestMethod]
        public void AddNewBlogPostShouldAffect1Record()
        {
            using (var db = new TestDBContext())
            {
                var cmd = db.GetDBCommand();

                cmd.CommandText = "DELTE FROM BLOG";
                cmd.ExecuteNonQuery();

                var blog = new Blog { BlogId = 1, Url = "http://blogs.msdn.com/adonet" };
                db.Blogs.Add(blog);

                var count = db.SaveChanges();

                count.ShouldBe(1);
            }
        }

        [TestMethod]
        public async Task AddNewBlogPostAsyncShouldAffect1Record()
        {
            using (var db = new TestDBContext())
            {
                var blog = new Blog { BlogId = 1000, Url = "http://blogs.msdn.com/adonet" };
                db.Blogs.Add(blog);

                var count = await db.SaveChangesAsync();

                count.ShouldBe(1);
            }
        }

        [TestMethod]
        public void AddNewBlogPostAutoincrementingShouldNotThrow()
        {
            using (var db = new TestDBContext())
            {
                var blog = new Blog { Url = "http://blogs.msdn.com/adonet" };
                db.Blogs.Add(blog);
                int count = 0;
                Should.NotThrow(() => count = db.SaveChanges());
            }
        }

        [TestMethod]
        public void BlogsShouldContainOne()
        {
            using (var db = new TestDBContext())
            {
                var cmd = db.GetDBCommand();

                cmd.CommandText = "DELETE FROM BLOG";
                cmd.ExecuteNonQuery();

                var blog = new Blog { BlogId = 1, Url = "http://blogs.msdn.com/adonet" };
                db.Blogs.Add(blog);

                var count = db.SaveChanges();

                count.ShouldBe(1);
            }

            using (var db = new TestDBContext())
            {
                foreach (var blog in db.Blogs)
                    blog.BlogId.ShouldBe(1);
            }
        }
    }
}
