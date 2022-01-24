using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using GeneralUnityUtils.PriorityQueue;
using UnityEngine;
using UnityEngine.TestTools;

public class QueueTests
{
    PriorityQueue<int> TestQueue;

    [SetUp]
    public void OnSetUp()
    {
        TestQueue = new PriorityQueue<int>();
    }

    [Test]
    public void Add()
    {
        TestQueue.Enqueue(0, 1.1f);
        Assert.AreEqual(1, TestQueue.Size);
    }

    [Test]
    public void AddPastCapacity()
    {
        for(int i = 0; i < 100; i++)
        {
            TestQueue.Enqueue(i, -i + i * i * 1.1f);
        }
        Assert.AreEqual(100, TestQueue.Size);
    }

    [Test]
    public void Peek()
    {
        TestQueue.Enqueue(0, -1.9f);
        Assert.AreEqual(0, TestQueue.Peek());
    }

    [Test]
    public void Dequeue()
    {
        TestQueue.Enqueue(55, -1.2f);
        Assert.AreEqual(55, TestQueue.Dequeue());
        Assert.AreEqual(0, TestQueue.Size);
    }

    [Test]
    public void QueueProperty()
    {
        for(int i = 0; i < 10; i++)
        {
            TestQueue.Enqueue(i, 10.4f - (float)i);
        }

        Assert.AreEqual(9, TestQueue.Dequeue());
        Assert.AreEqual(8, TestQueue.Dequeue());
        Assert.AreEqual(7, TestQueue.Dequeue());
        Assert.AreEqual(6, TestQueue.Dequeue());
        Assert.AreEqual(5, TestQueue.Dequeue());
        Assert.AreEqual(4, TestQueue.Dequeue());
        Assert.AreEqual(3, TestQueue.Dequeue());
        Assert.AreEqual(2, TestQueue.Dequeue());
        Assert.AreEqual(1, TestQueue.Dequeue());
        Assert.AreEqual(0, TestQueue.Dequeue());
    }

    [Test]
    public void DecreaseKey()
    {
        for (int i = 0; i < 10; i++)
        {
            TestQueue.Enqueue(i, 10.4f - i * 1.0f);
        }

        TestQueue.DecreaseKey(2, -100.354354f);
        Assert.AreEqual(2, TestQueue.Dequeue());
        Assert.AreEqual(9, TestQueue.Dequeue());
        Assert.AreEqual(8, TestQueue.Dequeue());
        Assert.AreEqual(7, TestQueue.Dequeue());
        Assert.AreEqual(6, TestQueue.Dequeue());
        Assert.AreEqual(5, TestQueue.Dequeue());
        Assert.AreEqual(4, TestQueue.Dequeue());
        Assert.AreEqual(3, TestQueue.Dequeue());
        Assert.AreEqual(1, TestQueue.Dequeue());
        Assert.AreEqual(0, TestQueue.Dequeue());
    }
}
