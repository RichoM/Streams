# Streams
######*Smalltalk-like streams implemented in C#*

A Stream is a sequence of objects, similar to an IEnumerator. The big difference is that while the enumerator points to an element, a stream contains a pointer **between** elements. Also, the interface differs quite a bit.

You can create a Stream by wrapping an IEnumerable, like:
```c#
Stream<string> stream = new Stream<int>(new string[] { "A", "B", "C" });
```

To access the elements, you use the *Next()* message, which returns the next element in the sequence and advances the pointer. If the stream is at the end of the sequence, it returns *null* (or the default value for the element's type).
```c#
stream.Next(); // "A"
stream.Next(); // "B"
stream.Next(); // "C"
stream.Next(); // null
```

You can also use *Peek()* to retrieve the next element without advancing the stream.
```c#
stream.Peek(); // "A"
stream.Peek(); // "A"
stream.Next(); // "A"
stream.Next(); // "B"
```

To check whether the stream is at the end, you use the *AtEnd* property.
```c#
while (!stream.AtEnd)
{
    // Do something with the stream...
}
```

The stream class also provides a couple other useful methods, check the source code.
