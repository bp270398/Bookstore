using Rhetos.Dom.DefaultConcepts;

namespace Bookstore.Service
{
    public class RandomBookGenerator
    {
        public static void InsertBooks(Common.DomRepository repository, int? numberOfBooks)
        {
            var books = Enumerable.Range(0, numberOfBooks.Value)
                .Select(x => new Book { Title = Guid.NewGuid().ToString() }); // Random title.
            repository.Bookstore.Book.Insert(books);
        }
    }
}
