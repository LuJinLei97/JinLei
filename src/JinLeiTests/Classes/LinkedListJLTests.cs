using JinLei.Classes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JinLeiTests.Classes;

[TestClass()]
public class LinkedListJLTests
{
    [TestMethod()]
    public void TryGetLinkedListNodeTest()
    {

    }

    [TestMethod()]
    public void TryInsertChildTest()
    {

    }

    [TestMethod()]
    public void TryInsertTest()
    {

    }

    [TestMethod()]
    public void AddTest()
    {
        var l = new LinkedListJL<int>() { BucketCapacity = 8 };
        for(var i = 1; i <= 100; i++)
        {
            l.Add(i);
        }
    }

    [TestMethod()]
    public void ClearTest()
    {

    }

    [TestMethod()]
    public void ContainsTest()
    {

    }

    [TestMethod()]
    public void CopyToTest()
    {

    }

    [TestMethod()]
    public void GetEnumeratorTest()
    {

    }

    [TestMethod()]
    public void IndexOfTest()
    {

    }

    [TestMethod()]
    public void InsertTest()
    {

    }

    [TestMethod()]
    public void RemoveTest()
    {

    }

    [TestMethod()]
    public void RemoveAtTest()
    {

    }
}