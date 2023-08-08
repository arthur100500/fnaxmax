module Audio

type audioEntry =
    | Ambient
    | Click
    | DoorClose
    | DoorOpen
    | LightOff
    | LightOn
    | DoorKnock
    
val playAudio: audioEntry -> unit
val playAudioLoop: audioEntry -> unit