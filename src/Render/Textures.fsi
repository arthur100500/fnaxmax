module Textures
open Browser.Types
open Fable.Core.JS

type GL = WebGLRenderingContext

type texture =
     | LRightCorridorClosed
     | LLeftCorridorClosed
     | LRightCorridorOpen
     | LLeftCorridorOpen
     | LRightBackStage
     | LLeftBackStage
     | ORightLight
     | LBossOffice
     | LRightHall
     | OLeftLight
     | LEntryRoom
     | LLeftHall
     | LKitchen
     | OOffice
     | ORightDoor1
     | ORightDoor2
     | ORightDoor3
     | OLeftDoor1
     | OLeftDoor2
     | OLeftDoor3
     | XRightCorridor
     | XLeftBackStage
     | XLeftCorridor
     | XRightStage
     | XBossOffice
     | XLeftStage
     | XRightHall
     | XRightDoor
     | XEntryRoom
     | XLeftDoor
     | XLeftHall
     | XKitchen
     | XRightBackStage
     | XStare
     
val loadTextures: GL -> (FSharp.Collections.Map<texture,WebGLTexture> -> unit) -> unit
