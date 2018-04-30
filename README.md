
# AirSimApp

Demonstration WPF app for connecting to a [Microsoft AirSim](https://github.com/Microsoft/AirSim) RPC server and displaying vehicle information.

## AirSimApp

Project containing the actual WPF app.

## AirSimRpc

Library for accessing [Microsoft AirSim](https://github.com/Microsoft/AirSim) RPC methods from C#.

## MsgPackRpc

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

## DotSpatialExtensions

Extension methods for the [DotSpatial](https://github.com/DotSpatial/DotSpatial) library, filling in some gaps that I think it has and that are useful for these projects.

## DotSpatialExtensions.Tests

Unit tests for the DotSpatialExtensions project to give some confidence that the math shakes out.

## WpfBBQWinRTXamlToolkit

A place to store WPF-ized versions of the controls contained in the [Win RT XAML Toolkit](https://github.com/xyzzer/WinRTXamlToolkit). See that project's readme for more details.
