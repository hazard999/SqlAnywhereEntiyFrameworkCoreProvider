using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

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
        public void AddNewBlogPostShouldNotThrow()
        {
            using (var db = new TestDBContext())
            {
                var blog = new Blog { BlogId = 1, Url = "http://blogs.msdn.com/adonet" };
                db.Blogs.Add(blog);
                int count = 0;
                Should.NotThrow(() => count = db.SaveChanges());
                count.ShouldBe(1);
            }
        }
    }
}
