using Autofac;
using Common.Queryable;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhetos.Dom.DefaultConcepts;
using Rhetos;
using Rhetos.TestCommon;

namespace Bookstore.Service.Test
{
    [TestClass]
    public class BookTest
    {

        [TestMethod]
        public void AutomaticallyUpdateNumberOfComments()
        {
            using (var scope = TestScope.Create())
            {
                var repository = scope.Resolve<Common.DomRepository>();

                var book = new Book { Title = Guid.NewGuid().ToString() };
                repository.Bookstore.Book.Insert(book);

                int? readNumberOfComments() => repository.Bookstore.BookInfo
                    .Query(bi => bi.ID == book.ID)
                    .Select(bi => bi.NumberOfComments)
                    .Single();

                Assert.AreEqual(0, readNumberOfComments());

                var c1 = new Comment { BookID = book.ID, Text = "c1" };
                var c2 = new Comment { BookID = book.ID, Text = "c2" };
                var c3 = new Comment { BookID = book.ID, Text = "c3" };

                repository.Bookstore.Comment.Insert(c1);
                Assert.AreEqual(1, readNumberOfComments());

                repository.Bookstore.Comment.Insert(c2, c3);
                Assert.AreEqual(3, readNumberOfComments());

                repository.Bookstore.Comment.Delete(c1);
                Assert.AreEqual(2, readNumberOfComments());

                repository.Bookstore.Comment.Delete(c2, c3);
                Assert.AreEqual(0, readNumberOfComments());
            }
        }

        [TestMethod]
        public void CommonMisspellingValidation()
        {
            using (var scope = TestScope.Create())
            {
                var repository = scope.Resolve<Common.DomRepository>();

                var book = new Book { Title = "x curiousity y" };

                TestUtility.ShouldFail<UserException>(
                    () => repository.Bookstore.Book.Insert(book),
                    "It is not allowed to enter misspelled word");
            }
        }

        [TestMethod]
        public void CommonMisspellingValidation_DirectFilter()
        {
            using (var scope = TestScope.Create())
            {
                var repository = scope.Resolve<Common.DomRepository>();

                var books = new[]
                {
                    new Bookstore_Book { Title = "spirit" },
                    new Bookstore_Book { Title = "opportunity" },
                    new Bookstore_Book { Title = "curiousity" },
                    new Bookstore_Book { Title = "curiousity2" }
                }.AsQueryable();

                var invalidBooks = repository.Bookstore.Book.Filter(books, new CommonMisspelling());

                Assert.AreEqual("curiousity, curiousity2", TestUtility.DumpSorted(invalidBooks, book => book.Title));
            }
        }

        [TestMethod]
        public void ParallelCodeGeneration()
        {
            DeleteUnitTestBooks(); 

            var books = new[]
            {
                new Book { Code = $"{UnitTestBookCodePrefix}+++", Title = Guid.NewGuid().ToString() },
                new Book { Code = $"{UnitTestBookCodePrefix}+++", Title = Guid.NewGuid().ToString() },
                new Book { Code = $"{UnitTestBookCodePrefix}ABC+", Title = Guid.NewGuid().ToString() },
                new Book { Code = $"{UnitTestBookCodePrefix}ABC+", Title = Guid.NewGuid().ToString() }
            };


            for (int retry = 0; retry < 3; retry++)  {
                Parallel.ForEach(books, book =>
                {
                    using (var scope = TestScope.Create())
                    {
                        var repository = scope.Resolve<Common.DomRepository>();
                        repository.Bookstore.Book.Insert(book);
                        scope.CommitAndClose(); 
                    }
                });


                using (var scope = TestScope.Create())
                {
                    var repository = scope.Resolve<Common.DomRepository>();
                    var booksFromDb = repository.Bookstore.Book.Load(book => book.Code.StartsWith(UnitTestBookCodePrefix));
                    Assert.AreEqual(
                        $"{UnitTestBookCodePrefix}001, {UnitTestBookCodePrefix}002, {UnitTestBookCodePrefix}ABC1, {UnitTestBookCodePrefix}ABC2",
                        TestUtility.DumpSorted(booksFromDb, book => book.Code));
                }

                DeleteUnitTestBooks();
            }
        }

        private const string UnitTestBookCodePrefix = "UnitTestBooks";

        private void DeleteUnitTestBooks()
        {
            using (var scope = TestScope.Create())
            {
                var repository = scope.Resolve<Common.DomRepository>();
                var testBooks = repository.Bookstore.Book.Load(book => book.Code.StartsWith(UnitTestBookCodePrefix));
                repository.Bookstore.Book.Delete(testBooks);
                scope.CommitAndClose();
            }
        }
    }
}
