# MsgPackRpc

Library for calling [msgpack](https://msgpack.org/) RPCs. I wasn't able to find a supported and current library for doing this. I know this version isn't feature complete (doesn't support notification at the moment), and I'm sure it's not the most efficient, given that it works by taking the encoded JSON in the packed message, decoding that first to actual JSON, then deserializing _that_ into the actual object format, but I wasn't able to find a library that:

 1. Worked in a way that I could understand
 2. Worked reliably (found some that crashed pretty easily/consistently)
 3. Was able to deal with the hierarchical objects the AirSim RPCs use (objects containing objects)

This implementation attempts to have a dead simple interface and to rely on [Json.NET](https://www.newtonsoft.com/json) for object serialization/deserialization, and the [MessagePack
](https://github.com/neuecc/MessagePack-CSharp) library for the msgpack en/decoding.

The interface exposes two important methods:

 1. `ConnectAsync(/*...*/)`
 2. `CallAsync<T>(/*...*/)`

The methods do what you'd expect, and are intended to interact well with C#'s Task Parallel Library.
