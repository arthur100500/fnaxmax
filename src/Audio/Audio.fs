module Audio

open System
open Browser.Types
open Fable.Core

[<Global("Audio")>]
let audioType: HTMLAudioElementType = jsNative

type audioEntry =
    | Ambient
    | Click
    | DoorClose
    | DoorOpen
    | LightOff
    | LightOn
    | DoorKnock

let getAudioData =
    function
    | Ambient -> "Data/audio/ambient.mp3", 1., 0.2
    | Click -> "Data/audio/click.wav", 1., 0.5
    | DoorClose -> "Data/audio/doorOpen.ogg", 0.7, 0.3
    | DoorOpen -> "Data/audio/doorOpen.ogg", 0.7, 0.3
    | LightOff -> "Data/audio/lightOff.mp3", 1., 0.5
    | LightOn -> "Data/audio/lightOn.mp3", 1., 0.5
    | DoorKnock -> "Data/audio/door.mp3", 1., 1.

type audioLib = Map<audioEntry, HTMLAudioElement>

let createAudioElements () =
    let audioEntries =
        [| Ambient; Click; DoorClose; DoorOpen; LightOff; LightOn; DoorKnock |]

    let createAndAdd dict entry =
        let name, pitch, volume = getAudioData entry
        let res = audioType.Create()
        res.src <- name
        res.playbackRate <- pitch
        res.volume <- volume
        Map.add entry res dict

    Array.fold createAndAdd Map.empty audioEntries

let audio = createAudioElements ()

let playAudio entry = (Map.find entry audio).play ()

let playAudioLoop entry =
    (Map.find entry audio).onended <- fun _ -> (Map.find entry audio).play ()
    (Map.find entry audio).play ()
