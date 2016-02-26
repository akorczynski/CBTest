using Couchbase;
using Couchbase.Linq;
using Couchbase.Linq.Filters;
using System;
using System.Linq;

namespace cbdocTest
{
    class Program
    {
        static void Main(string[] args)
        {
            const string GUITAR_KEY = "Guitar::";
            const string PIANO_KEY = "Piano::";

            var cluster = new Cluster();
            var bucket = cluster.OpenBucket("TestBB");
            var bucketContext = new BucketContext(bucket);

            // Inser method 1, no document
            var gID = Guid.NewGuid();
            var guitar = new Guitar { ID = gID, Type = "Guitar", Name = "My Guitar" };
            bucket.Insert(GUITAR_KEY + guitar.ID, guitar);

            // Insert method 2, document
            var pID = Guid.NewGuid();
            var piano = new Piano { ID = pID, Type = "Piano", Name = "My Piano" };
            var doc = new Document<Piano> {Id = PIANO_KEY + piano.ID, Content = piano};
            bucket.Insert(doc);

            // Three different ways to return, independent of insert method above
            var g1 = bucket.Get<Guitar>(GUITAR_KEY + gID);
            var g2 = bucket.GetDocument<Guitar>(GUITAR_KEY + gID);
            var g3 = (from g in bucketContext.Query<Guitar>() where g.ID.ToString() == gID.ToString() select g).First();

            var p1 = bucket.Get<Piano>(PIANO_KEY + pID);
            var p2 = bucket.GetDocument<Piano>(PIANO_KEY + pID);
            var p3 = (from p in bucketContext.Query<Piano>() where p.ID.ToString() == pID.ToString() select p).First();
        }
    }

    [DocumentTypeFilter("Guitar")]
    public class Guitar
    {
        public Guid ID { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }

    public class Piano
    {
        public Guid ID { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }
}
