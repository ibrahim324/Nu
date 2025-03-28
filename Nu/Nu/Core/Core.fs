﻿// Nu Game Engine.
// Copyright (C) Bryan Edds.

namespace Nu
open System
open System.Diagnostics
open Prime

[<RequireQualifiedAccess>]
module Core =

    let mutable private LastTimeStamp = Stopwatch.GetTimestamp ()
    let private LastTimeStampLock = obj ()

    /// Get a time stamp at the highest-available resolution.
    /// Thread-safe.
    let getTimeStamp () =
        Stopwatch.GetTimestamp ()

    /// Get a unique time stamp, spinning until the time stamp advances if need be.
    /// Thread-safe.
    let getTimeStampUnique () =
        lock LastTimeStampLock $ fun () ->
            let mutable nextStamp = getTimeStamp ()
            while nextStamp = LastTimeStamp do nextStamp <- getTimeStamp ()
            LastTimeStamp <- nextStamp
            nextStamp

[<AutoOpen>]
module CoreOperators =

    /// Sequences two functions like Haskell ($).
    /// Same as the ($) operator found in Prime, but placed here to expose it directly from Nu.
    let inline ($) f g = f g

    /// Test for object equality.
    /// Same as the (===) operator found in Prime, but placed here to expose it directly from Nu.
    let inline (===) (a : obj) (b : obj) = objEq a b

    /// Test for object inequality.
    /// Same as the (=/=) operator found in Prime, but placed here to expose it directly from Nu.
    let inline (=/=) (a : obj) (b : obj) = objNeq a b

    /// Attempt to cast an obj to type 'a, returning 'a option.
    /// TODO: remove this after updating Prime.
    let inline tryCast<'a> (obj : obj) = match obj with :? 'a as a -> Some a | _ -> None

    /// Attempt to cast an obj to type 'a, returning 'a voption.
    /// TODO: remove this after updating Prime.
    let inline tryCast'<'a> (obj : obj) = match obj with :? 'a as a -> ValueSome a | _ -> ValueNone

    /// Force a value to be in between zero and one.
    /// TODO: remove this after updating Prime.
    let inline saturate (a : 'a) = a |> min (one ()) |> max (zero ())