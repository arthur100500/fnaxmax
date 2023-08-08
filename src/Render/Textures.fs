module Textures

open System
open Browser.Types
open Fable.Core
open Browser
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

let getTextureName =
    function
    | LRightCorridorClosed -> "Data/img/RightCorridorClosed.png"
    | LLeftCorridorClosed -> "Data/img/LeftCorridorClosed.png"
    | LRightCorridorOpen -> "Data/img/RightCorridorOpen.png"
    | LLeftCorridorOpen -> "Data/img/LeftCorridorOpen.png"
    | LRightBackStage -> "Data/img/RightBackStage.png"
    | LLeftBackStage -> "Data/img/LeftBackStage.png"
    | LRightHall -> "Data/img/RightHall.png"
    | LEntryRoom -> "Data/img/EntryRoom.png"
    | LLeftHall -> "Data/img/LeftHall.png"
    | LKitchen -> "Data/img/Kitchen.png"
    | ORightDoor1 -> "Data/img/door/RightDoor1.png"
    | ORightDoor2 -> "Data/img/door/RightDoor2.png"
    | ORightDoor3 -> "Data/img/door/RightDoor3.png"
    | OLeftDoor1 -> "Data/img/door/LeftDoor1.png"
    | OLeftDoor2 -> "Data/img/door/LeftDoor2.png"
    | OLeftDoor3 -> "Data/img/door/LeftDoor3.png"
    | LBossOffice -> "Data/img/BossOffice.png"
    | ORightLight -> "Data/img/RightLight.png"
    | OLeftLight -> "Data/img/LeftLight.png"
    | OOffice -> "Data/img/Office.png"
    | XRightBackStage -> "Data/img/xMax/xMaxRightBackStage.png"
    | XRightCorridor -> "Data/img/xMax/xMaxRightCorridor.png"
    | XLeftBackStage -> "Data/img/xMax/xMaxLeftBackStage.png"
    | XLeftCorridor -> "Data/img/xMax/xMaxLeftCorridor.png"
    | XRightStage -> "Data/img/xMax/xMaxRightStage.png"
    | XBossOffice -> "Data/img/xMax/xMaxBossOffice.png"
    | XLeftStage -> "Data/img/xMax/xMaxLeftStage.png"
    | XRightHall -> "Data/img/xMax/xMaxRightHall.png"
    | XRightDoor -> "Data/img/xMax/xMaxRightDoor.png"
    | XEntryRoom -> "Data/img/xMax/xMaxEntryRoom.png"
    | XLeftDoor -> "Data/img/xMax/xMaxLeftDoor.png"
    | XLeftHall -> "Data/img/xMax/xMaxLeftHall.png"
    | XKitchen -> "Data/img/xMax/xMaxKitchen.png"
    | XStare -> "Data/img/xMax/xMaxStare.png"

let imageLoadCanvas: HTMLCanvasElement = unbox document.createElement "canvas"
let imageLoadCanvasContext = imageLoadCanvas.getContext_2d ()

let getImageData src k =
    let image: HTMLImageElement = unbox document.createElement "img"

    let onload _ =
        let width = image.width
        let height = image.height
        imageLoadCanvas.width <- width
        imageLoadCanvas.height <- height
        imageLoadCanvasContext.clearRect (0., 0., width, height)
        imageLoadCanvasContext.drawImage (U3.Case1(image), 0., 0., width, height)
        printfn $"Loaded %s{src}"
        imageLoadCanvasContext.getImageData (0., 0., width, height) |> k

    image.onload <- onload
    image.crossOrigin <- "anonymous"
    image.src <- src

let loadSingleTexture (gl: GL) tex k =
    let texture = gl.createTexture ()
    gl.activeTexture gl.TEXTURE0
    gl.bindTexture (gl.TEXTURE_2D, texture)

    let level = 0
    let internalFormat = gl.RGBA
    let srcFormat = gl.RGBA
    let srcType = gl.UNSIGNED_BYTE
    let url = getTextureName tex

    getImageData url (fun data ->
        gl.bindTexture (gl.TEXTURE_2D, texture)
        gl.texImage2D (gl.TEXTURE_2D, level, internalFormat, srcFormat, srcType, data)
        gl.texParameteri (gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE)
        gl.texParameteri (gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE)
        gl.texParameteri (gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.NEAREST)

        k texture)


let loadTextures (gl: GL) k =
    let d = FSharp.Collections.Map.empty

    let textures =
        [ LRightCorridorClosed
          LLeftCorridorClosed
          LRightCorridorOpen
          LLeftCorridorOpen
          LRightBackStage
          LLeftBackStage
          ORightLight
          LBossOffice
          LRightHall
          OLeftLight
          LEntryRoom
          LLeftHall
          LKitchen
          OOffice
          ORightDoor1
          ORightDoor2
          ORightDoor3
          OLeftDoor1
          OLeftDoor2
          OLeftDoor3
          XRightCorridor
          XLeftBackStage
          XLeftCorridor
          XRightStage
          XBossOffice
          XLeftStage
          XRightHall
          XRightDoor
          XEntryRoom
          XLeftDoor
          XLeftHall
          XKitchen
          XRightBackStage
          XStare ]

    gl.pixelStorei (gl.UNPACK_FLIP_Y_WEBGL, 1)

    let rec iLoadTextures (gl: GL) textures (d: FSharp.Collections.Map<texture, WebGLTexture>) k =
        match textures with
        | h :: tl ->
            loadSingleTexture gl h (fun tex ->
                let d = FSharp.Collections.Map.add h tex d
                iLoadTextures gl tl d k)
        | [] -> k d

    iLoadTextures gl textures d k
