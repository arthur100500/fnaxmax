module Main

open FiveNightsAtxMax_
open Game
open Textures
open Render
open RenderBasics
open Browser
open Browser.Types

let inline bindButtons () =
    let camSwitch = document.getElementById "camera-up"
    let camLeft = document.getElementById "camera-left"
    let camRight = document.getElementById "camera-right"
    camSwitch.onclick <- fun _ -> globalControls.cameraSwitch <- true
    camRight.onclick <- fun _ -> globalControls.cameraRight <- true
    camLeft.onclick <- fun _ -> globalControls.cameraLeft <- true
    
let gl: WebGLRenderingContext =
    let canvas: Types.HTMLCanvasElement = unbox document.getElementById "game-canvas"
    unbox canvas.getContext "webgl"

let textures =
    [ "img/InOffice00.png"
      "img/LeftCorridorOpen.png"
      "img/RightCorridorOpen.png"
      "img/LeftCorridorClosed.png"
      "img/RightCorridorClosed.png"
      "img/BossOffice.png"
      "img/EntryRoom.png"
      "img/LeftHall.png"
      "img/RightHall.png"
      "img/LeftBackStage.png"
      "img/RightBackStage.png"
      "img/Kitchen.png" ]
    
let inline setCanvasSize () =
    gl.canvas.width <- 1920
    gl.canvas.height <- 1080
    
setCanvasSize ()
bindButtons ()

loadTextures gl textures (fun texturesMap ->
let rData = { gl = gl; textures = texturesMap }
prepare rData
gameLoop initGameState rData draw 0.)
